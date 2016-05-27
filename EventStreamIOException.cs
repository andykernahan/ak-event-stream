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
    /// The exception that is thrown when an operation fails due to an IO level error.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class EventStreamIOException : EventStreamException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventStreamIOException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public EventStreamIOException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventStreamIOException"/> class.
        /// </summary>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public EventStreamIOException(Exception innerException)
            : base(innerException.Message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventStreamIOException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public EventStreamIOException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        private EventStreamIOException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}