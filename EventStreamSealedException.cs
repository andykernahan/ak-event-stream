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
    /// The exception that is thrown when an operation fails as the target stream has been sealed.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class EventStreamSealedException : EventStreamException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventStreamSealedException"/> class.
        /// </summary>
        /// <param name="id">The identifier of the stream.</param>
        /// <exception cref="ArgumentException"><paramref name="id"/> is default.</exception>
        public EventStreamSealedException(EventStreamId id)
            : base($"The stream with the identifier '{id}' has been sealed.")
        {
            Requires.NotDefault(id, nameof(id));
        }

        private EventStreamSealedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}