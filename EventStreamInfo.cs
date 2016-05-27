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
    /// Provides information about a stream. This class cannot be inherited.
    /// </summary>
    public sealed class EventStreamInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventStreamInfo"/> class.
        /// </summary>
        /// <param name="id">The stream identifier.</param>
        /// <param name="sequence">The current sequence number.</param>
        /// <param name="count">The current number of events.</param>
        /// <param name="sealed">The sealed status of the stream.</param>
        /// <exception cref="ArgumentException"><paramref name="id"/> is default.</exception>
        public EventStreamInfo(EventStreamId id, long sequence, long count, bool @sealed)
        {
            Requires.NotDefault(id, nameof(id));

            Id = id;
            Sequence = sequence;
            Count = count;
            Sealed = @sealed;
        }

        /// <summary>
        /// Gets the stream identifier.
        /// </summary>
        public EventStreamId Id { get; }

        /// <summary>
        /// Gets the current sequence number.
        /// </summary>
        public long Sequence { get; }

        /// <summary>
        /// Gets the current number of events.
        /// </summary>
        public long Count { get; }

        /// <summary>
        /// Gets a value indicating if the stream is sealed.
        /// </summary>
        public bool Sealed { get; }

        /// <inheritdoc/>
        public override string ToString() => $"EventStreamInfo(Id='{Id}', Sequence={Sequence}, Count={Count}, Sealed={Sealed})";
    }
}