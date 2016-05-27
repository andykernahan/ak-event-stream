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
    /// Hosts streams of events.
    /// </summary>
    /// <typeparam name="T">The type of the events in the stream.</typeparam>
    public interface IEventStreamHost<T>
    {
        /// <summary>
        /// Begins an asynchronous operation to add a stream with the given identifier.
        /// </summary>        
        /// <param name="id">The stream identifier.</param>
        /// <returns>A task which represents the asynchronous operation and the new stream.</returns>
        /// <exception cref="ArgumentException"><paramref name="id"/> is default.</exception>
        /// <exception cref="EventStreamIOException">An IO level error occurs.</exception>
        /// <exception cref="EventStreamAlreadyExistsException">A stream with the given identifier already exists.</exception>
        Task<IEventStream<T>> AddAsync(EventStreamId id);

        /// <summary>
        /// Begins an asynchronous operation to get an existing stream with the given identifier.
        /// </summary>        
        /// <param name="id">The stream identifier.</param>
        /// <returns>A task which represents the asynchronous operation and the existing stream.</returns>
        /// <exception cref="ArgumentException"><paramref name="id"/> is default.</exception>
        /// <exception cref="EventStreamIOException">An IO level error occurs.</exception>
        /// <exception cref="EventStreamNotFoundException">A stream with the given identifier was not found.</exception>
        Task<IEventStream<T>> GetAsync(EventStreamId id);

        /// <summary>
        /// Begins an asynchronous operation to list the hosted streams.
        /// </summary>        
        /// <returns>A task which represents the asynchronous operation and stream info.</returns>
        /// <exception cref="EventStreamIOException">An IO level error occurs.</exception>
        Task<ImmutableArray<EventStreamInfo>> ListAsync();
    }
}