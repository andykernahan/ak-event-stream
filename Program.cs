﻿// Copyright 2016 Andy Kernahan
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using AK.EventStream.InMemory;

namespace AK.EventStream
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Exercise(MakeHost()).Wait();
            Console.ReadLine();
        }

        private static async Task Exercise(IEventStreamHost<int> host)
        {
            IEventStream<int> streamA = await host.AddAsync("a");

            // OpenAtStart returns an observable sequence that produces events from the start of the stream until the stream is sealed.
            IObservable<EventStreamSegment<int>> openAtStart = streamA.OpenAtStart();
            (from segment in openAtStart from e in segment where e.Data.IsEven() select e).Subscribe(O("a.OpenAtStart(Even)"));

            // Schedule a periodic WriteAsync to the stream.
            var writer = Observable.Interval(TimeSpan.FromSeconds(0.5)).Subscribe(i =>
            {
                // WriteAsync writes one-or-more events (automically) to the stream.
                Task<EventStreamSegment<int>> written = streamA.WriteAsync((int)i);
            });
            await Task.Delay(TimeSpan.FromSeconds(3));

            // As the stream now contains events, OpenAtStart will produce those events before any other.
            (from segment in openAtStart from e in segment where e.Data.IsOdd() select e).Subscribe(O("a.OpenAtStart(Odd)"));

            await Task.Delay(TimeSpan.FromSeconds(3));

            // OpenAtEnd returns an observable sequence that produces events until the stream is sealed.
            IObservable<EventStreamSegment<int>> openAtEnd = streamA.OpenAtEnd();
            (from segment in openAtEnd from e in segment select e).Subscribe(O("a.OpenAtEnd(*)"));

            await Task.Delay(TimeSpan.FromSeconds(3));

            // GetInfoAsync returns a snapshot of information about the stream:
            //     Sequence: The current sequence number.
            //     Count:    The current number of events.
            //     Sealed:   Indicates if the stream is sealed.
            EventStreamInfo info = await streamA.GetInfoAsync();

            // OpenAt returns an observable sequence that produces events from the given sequence (inclusive) until the stream is sealed.
            long openAtSequence = info.Sequence / 2;
            IObservable<EventStreamSegment<int>> openAt = streamA.OpenAt(openAtSequence);
            (from segment in openAt from e in segment where e.Data.IsEven() select e).Subscribe(O($"a.OpenAt({openAtSequence},Even)"));

            await Task.Delay(TimeSpan.FromSeconds(3));
            writer.Dispose();

            // Seal the stream, thus notifying observers of completion and preventing further writes.
            // The behaviour of the *Open methods is now as follows:
            //     OpenAtStart: Produces events from the start of the stream and then completes.
            //     OpenAtEnd:   Immediately completes.
            //     OpenAt:      Produces events from the given sequence and then completes.
            await streamA.SealAsync();

            var streamB = await host.AddAsync("b");
            (from segment in streamB.OpenAtEnd() from e in segment select e).Subscribe(O("b.OpenAtEnd(*)"));

            // Delete the stream, thus notifying observers of the _error_.
            // The behaviour of the *Open methods is now as follows:
            //     OpenAtStart: Immediately notifies of error.
            //     OpenAtEnd:   Immediately notifies of error.
            //     OpenAt:      Immediately notifies of error.
            await streamB.DeleteAsync();
        }

        private static IEventStreamHost<int> MakeHost()
        {
            return new InMemoryEventStreamHost<int>();
        }

        private static IObserver<EventInfo<int>> O(string id)
        {
            return Observer.Create<EventInfo<int>>(
                value => Console.WriteLine($"{id,-20}[{Thread.CurrentThread.ManagedThreadId}]: OnNext({value})"),
                error => Console.WriteLine($"{id,-20}[{Thread.CurrentThread.ManagedThreadId}]: OnError({error})"),
                () => Console.WriteLine($"{id,-20}[{Thread.CurrentThread.ManagedThreadId}]: OnCompleted()"));
        }
    }

    internal static class Int32Extensions
    {
        public static bool IsEven(this int i)
        {
            return (i & 1) == 0;
        }

        public static bool IsOdd(this int i)
        {
            return !i.IsEven();
        }
    }
}