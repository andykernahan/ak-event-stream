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
using System.Threading.Tasks;

namespace AK.EventStream
{
    /// <summary>
    /// Provides a read-only stream events.
    /// </summary>
    /// <typeparam name="T">The type of the events in the stream.</typeparam>
    public interface IReadOnlyEventStream<T>
    {
        /// <summary>
        /// Gets the identifier the stream.
        /// </summary>
        EventStreamId Id { get; }

        /// <summary>
        /// Begins an asynchronous operation to retrieve the stream information.
        /// </summary>
        /// <returns>A task which represents the asynchronous operation and the stream information.</returns>
        /// <exception cref="EventStreamIOException">An IO level error occurs.</exception>
        /// <exception cref="EventStreamDeletedException">The stream has been deleted.</exception>
        Task<EventStreamInfo> GetInfoAsync();

        /// <summary>
        /// Returns an observable sequence that produces events from the start of the stream until the stream is sealed.
        /// </summary>
        /// <returns>An observable sequence that produces events.</returns>
        IObservable<EventStreamSegment<T>> OpenAtStart();

        /// <summary>
        /// Returns an observable sequence that produces events until the stream is sealed.
        /// </summary>
        /// <returns>An observable sequence that produces events.</returns>
        IObservable<EventStreamSegment<T>> OpenAtEnd();

        /// <summary>
        /// Returns an observable sequence that produces events from the given <paramref name="sequence"/> (inclusive)
        /// until the stream is sealed.
        /// </summary>
        /// <param name="sequence">The exact sequence that events should be produced from (inclusive).</param>
        /// <returns>An observable sequence that produces events.</returns>
        IObservable<EventStreamSegment<T>> OpenAt(long sequence);
    }
}