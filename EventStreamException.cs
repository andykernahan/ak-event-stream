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
    /// Defines the base class for exceptions thrown by the stream API. This class is <see langword="abstract"/>.
    /// </summary>
    public abstract class EventStreamException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventStreamException"/> class.
        /// </summary>
        protected EventStreamException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventStreamException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        protected EventStreamException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventStreamException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        protected EventStreamException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventStreamException"/> class.
        /// </summary>
        /// <param name="info">
        /// The container that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        /// The container that contains contextual information about the source or destination.
        /// </param>
        protected EventStreamException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}