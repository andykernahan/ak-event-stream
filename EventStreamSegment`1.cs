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
using System.Collections;
using System.Collections.Generic;

namespace AK.EventStream
{
    /// <summary>
    /// Contains a segment of a stream.
    /// </summary>
    /// <typeparam name="T">The type of the events in the stream.</typeparam>
    public struct EventStreamSegment<T> : IReadOnlyList<EventInfo<T>>
    {
        private readonly EventInfo<T>[] _events;
        private readonly int _offset;
        private readonly int _count;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="EventStreamSegment{T}"/> structure.
        /// </summary>
        /// <param name="events">The array containing the events.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="events"/> is <see langword="null"/>.
        /// </exception>
        public EventStreamSegment(EventInfo<T>[] events)
        {
            Requires.NotNull(events, nameof(events));

            _events = events;
            _offset = 0;
            _count = events.Length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventStreamSegment{T}"/> structure.
        /// </summary>
        /// <param name="events">The array containing the range of events.</param>
        /// <param name="offset">The zero-based index of the first event in the range.</param>
        /// <param name="count">The number of events in the range.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="events"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> or <paramref name="count"/> is less than 0.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="offset"/> and <paramref name="count"/> do not specify a valid range in <paramref name="events"/>.
        /// </exception>
        public EventStreamSegment(EventInfo<T>[] events, int offset, int count)
        {
            Requires.ValidRange(events, offset, count, nameof(events));

            _events = events;
            _offset = offset;
            _count = count;
        }

        /// <inheritdoc/>        
        public EventInfo<T> this[int index] => _events[index + _offset];

        /// <inheritdoc/>
        public int Count => _count;

        /// <summary>
        /// Gets a value indicating whether the range is empty.
        /// </summary>
        public bool IsEmpty => _count == 0;

        /// <summary>
        /// Gets an empty <see cref="EventStreamSegment{T}"/>.
        /// </summary>
        public static EventStreamSegment<T> Empty => default(EventStreamSegment<T>);

        /// <inheritdoc/>        
        public IEnumerator<EventInfo<T>> GetEnumerator()
        {
            for (int i = 0; i < _count; ++i)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}