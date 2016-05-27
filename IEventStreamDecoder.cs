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
    /// Decodes events from a stream.
    /// </summary>
    /// <typeparam name="T">The type of the decoded event.</typeparam>
    public interface IEventStreamDecoder<T>
    {
        /// <summary>
        /// Decodes an event from the given stream.
        /// </summary>
        /// <param name="input">The input stream.</param>
        /// <returns>The decoded instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="input"/> is <see langword="null"/>.</exception>
        T Decode(Stream input);
    }
}