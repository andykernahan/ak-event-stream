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

namespace AK.EventStream
{
    /// <summary>
    /// Defines an event read from a stream.
    /// </summary>
    /// <typeparam name="T">The type of the events in the stream.</typeparam>
    public struct EventInfo<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventInfo{T}"/> structure.
        /// </summary>
        /// <param name="sequence">The event sequence number.</param>
        /// <param name="timestamp">The event timestamp (at the host), specified in UTC.</param>
        /// <param name="data">The event data.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="timestamp"/> is not of kind <see cref="DateTimeKind.Utc"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is <see langword="null"/>.</exception>
        public EventInfo(long sequence, DateTime timestamp, T data)
        {
            Requires.Utc(timestamp, nameof(timestamp));
            Requires.NotNull(data, nameof(data));

            Sequence = sequence;
            Timestamp = timestamp;
            Data = data;
        }

        /// <summary>
        /// Gets the event sequence number.
        /// </summary>
        public long Sequence { get; }

        /// <summary>
        /// Gets the event timestamp (at the host), specified in UTC.
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        /// Gets the event data.
        /// </summary>
        public T Data { get; }

        /// <inheritdoc/>
        public override string ToString() => $"EventInfo(Sequence={Sequence}, Timestamp='{Timestamp:o}', Data='{Data}')";
    }
}