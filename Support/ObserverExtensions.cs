// Copyright 2016 Andy Kernahan
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
using System.Collections.Immutable;

namespace AK.EventStream.Support
{
    internal static class ObserverExtensions
    {
        public static AsyncObserver<T> AsAsync<T>(this IObserver<T> inner)
        {
            return new AsyncObserver<T>(inner);
        }

        public static void OnFinal<T>(this IObserver<T> observer, T value)
        {
            observer.OnNext(value);
            observer.OnCompleted();
        }

        public static void OnCompleted<T>(this ImmutableArray<IObserver<T>> observers)
        {
            foreach (var observer in observers)
            {
                observer.OnCompleted();
            }
        }

        public static void OnNext<T>(this ImmutableArray<IObserver<T>> observers, T value)
        {
            foreach (var observer in observers)
            {
                observer.OnNext(value);
            }
        }        

        public static void OnError<T>(this ImmutableArray<IObserver<T>> observers, Exception error)
        {
            foreach (var observer in observers)
            {
                observer.OnError(error);
            }
        }
    }
}