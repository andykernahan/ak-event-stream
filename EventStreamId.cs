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
    /// Defines the identifier of a stream.
    /// </summary>
    public struct EventStreamId : IEquatable<EventStreamId>
    {
        private readonly string _id;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventStreamId"/> class.
        /// </summary>
        /// <param name="id">The stream identifier.</param>
        /// <exception cref="ArgumentNullException"><paramref name="id"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="id"/> is consists only of white-space characters.</exception>
        public EventStreamId(string id)
        {
            Requires.NotNullOrWhiteSpace(id, nameof(id));

            _id = id;
        }

        /// <summary>
        /// Gets a value indicating whether this struct was initialized without an identifier.
        /// </summary>
        public bool IsDefault => _id == null;

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj != null && GetType() == obj.GetType() && Equals((EventStreamId)obj);

        /// <inheritdoc/>
        public bool Equals(EventStreamId other) => String.Equals(_id, other._id);

        /// <inheritdoc/>
        public override int GetHashCode() => 37 * _id.GetHashCode() + GetType().GetHashCode();

        /// <inheritdoc/>
        public override string ToString() => _id;

        /// <summary>
        /// Converts <paramref name="value"/> to an <see cref="EventStreamId"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> is consists only of white-space characters.
        /// </exception>
        public static implicit operator EventStreamId(string value)
        {
            return new EventStreamId(value);
        }
    }
}