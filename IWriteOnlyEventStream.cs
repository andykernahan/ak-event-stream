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
using System.Threading.Tasks;

namespace AK.EventStream
{
    /// <summary>
    /// Provides a write-only stream of events.
    /// </summary>
    /// <typeparam name="T">The type of the events in the stream.</typeparam>
    public interface IWriteOnlyEventStream<T>
    {
        /// <summary>
        /// Gets the identifier the stream.
        /// </summary>
        EventStreamId Id { get; }

        /// <summary>
        /// Begins an asynchronous operation to write to the stream.
        /// </summary>
        /// <param name="events">The events to write.</param>
        /// <returns>A task which represents the asynchronous write operation.</returns>
        /// <exception cref="ArgumentException"><paramref name="events"/> is default or empty.</exception>
        /// <exception cref="EventStreamIOException">An IO level error occurs.</exception>
        /// <exception cref="EventStreamDeletedException">The stream has been deleted.</exception>
        /// <exception cref="EventStreamSealedException">The stream has been sealed.</exception>
        Task<EventStreamSegment<T>> WriteAsync(ImmutableArray<T> events);

        /// <summary>
        /// Begins an asynchronous operation to seal the stream.
        /// </summary>        
        /// <returns>A task which represents the asynchronous seal operation.</returns>
        /// <exception cref="EventStreamIOException">An IO level error occurs.</exception>
        /// <exception cref="EventStreamDeletedException">The stream has been deleted.</exception>
        /// <exception cref="EventStreamSealedException">The stream has already been sealed.</exception>
        Task SealAsync();

        /// <summary>
        /// Begins an asynchronous operation to delete the stream.
        /// </summary>        
        /// <returns>A task which represents the asynchronous delete operation.</returns>
        /// <exception cref="EventStreamIOException">An IO level error occurs.</exception>
        /// <exception cref="EventStreamDeletedException">The stream has already been deleted.</exception>
        Task DeleteAsync();
    }
}