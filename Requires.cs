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
using System.Collections.Immutable;
using System.Diagnostics;

namespace AK.EventStream
{
    [DebuggerStepThrough]
    internal static class Requires
    {
        public static void NotNull<T>(T value, string paramName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void NotNullOrWhiteSpace(string value, string paramName)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                if (value == null)
                {
                    throw new ArgumentNullException(paramName);
                }
                throw new ArgumentException("Must not be empty or consist only of white-space characters.", paramName);
            }
        }

        public static void Utc(DateTime value, string paramName)
        {
            if (value.Kind != DateTimeKind.Utc)
            {
                throw new ArgumentOutOfRangeException(paramName, "Must be UTC.");
            }
        }

        public static void NotDefault<T>(ImmutableArray<T> value, string paramName)
        {
            if (value.IsDefault)
            {
                throw new ArgumentException("Must not be default.", paramName);
            }
        }

        public static void NotDefault(EventStreamId value, string paramName)
        {
            if (value.IsDefault)
            {
                throw new ArgumentException("Must not be default.", paramName);
            }
        }

        public static void ValidRange(Array array, int offset, int count, string paramName)
        {
            if (array == null)
            {
                throw new ArgumentNullException(paramName);
            }
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }
            if (array.Length - offset < count)
            {
                throw new ArgumentException("Offset and count must specify a valid range.");
            }
        }
    }
}