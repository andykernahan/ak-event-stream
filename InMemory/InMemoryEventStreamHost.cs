#pragma warning disable 1998
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

using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Threading.Tasks;
using AK.EventStream.InMemory.Support;

namespace AK.EventStream.InMemory
{
    /// <summary>
    /// An in-memory <see cref="IEventStreamHost{T}"/>. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="T">The type of the events in the stream.</typeparam>
    public sealed class InMemoryEventStreamHost<T> : IEventStreamHost<T>
    {
        private readonly ConcurrentDictionary<EventStreamId, InMemoryEventStream<T>> _streams =
            new ConcurrentDictionary<EventStreamId, InMemoryEventStream<T>>();

        /// <inheritdoc/>
        public async Task<IEventStream<T>> AddAsync(EventStreamId id)
        {
            Requires.NotDefault(id, nameof(id));

            var stream = new InMemoryEventStream<T>(id);
            if (!_streams.TryAdd(id, stream))
            {
                throw new EventStreamAlreadyExistsException(id);
            }
            stream.Deleted += OnStreamDeleted;
            return stream;
        }

        /// <inheritdoc/>
        public async Task<IEventStream<T>> GetAsync(EventStreamId id)
        {
            Requires.NotDefault(id, nameof(id));

            InMemoryEventStream<T> stream;
            if (!_streams.TryGetValue(id, out stream))
            {
                throw new EventStreamNotFoundException(id);
            }
            return stream;
        }

        /// <inheritdoc/>
        public async Task<ImmutableArray<EventStreamInfo>> ListAsync()
        {
            var builder = ImmutableArray.CreateBuilder<EventStreamInfo>(_streams.Count);
            foreach (var value in _streams.Values)
            {
                try
                {
                    builder.Add(value.GetInfo());
                }
                catch (EventStreamDeletedException)
                {
                }
            }
            return builder.PreferMoveToImmutable();
        }

        private void OnStreamDeleted(InMemoryEventStream<T> stream)
        {
            InMemoryEventStream<T> removed;
            _streams.TryRemove(stream.Id, out removed);
            Debug.Assert(ReferenceEquals(removed, stream));
            stream.Deleted -= OnStreamDeleted;
        }
    }
}