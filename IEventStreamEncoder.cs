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
using System.IO;

namespace AK.EventStream
{
    /// <summary>
    /// Encodes events to a stream.
    /// </summary>
    /// <typeparam name="T">The type of the encoded event.</typeparam>
    public interface IEventStreamEncoder<T>
    {
        /// <summary>
        /// Encodes the given event to the given stream.
        /// </summary>
        /// <param name="e">The event to encode.</param>
        /// <param name="output">The output stream.</param>
        /// <returns>The decoded instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="output"/> or <paramref name="e"/> is <see langword="null"/>.
        /// </exception>
        void Encode(T e, Stream output);
    }
}