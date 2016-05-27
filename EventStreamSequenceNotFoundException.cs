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
using System.Runtime.Serialization;

namespace AK.EventStream
{
    /// <summary>
    /// The exception that is thrown when an operation fails as the sequence was not found in the target stream.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class EventStreamSequenceNotFoundException : EventStreamException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventStreamSequenceNotFoundException"/> class.
        /// </summary>
        /// <param name="id">The identifier of the stream.</param>
        /// <param name="sequence">The sequence.</param>
        /// <exception cref="ArgumentException"><paramref name="id"/> is default.</exception>
        public EventStreamSequenceNotFoundException(EventStreamId id, long sequence)
            : base($"The sequence {sequence} was not found in the stream with the identifier '{id}'.")
        {
            Requires.NotDefault(id, nameof(id));
        }

        private EventStreamSequenceNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}