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

using System.Collections.Immutable;
using System.Threading.Tasks;

namespace AK.EventStream
{
    /// <summary>
    /// Provides extension methods for the namespace. This class is <see langword="static"/>.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// A synchronous <see cref="IEventStreamHost{T}.AddAsync"/>.
        /// </summary>
        public static IEventStream<T> Add<T>(this IEventStreamHost<T> host, EventStreamId id)
        {
            Requires.NotNull(host, nameof(host));

            return host.AddAsync(id).GetResult();
        }

        /// <summary>
        /// A synchronous <see cref="IEventStreamHost{T}.GetAsync"/>.
        /// </summary>        
        public static IEventStream<T> Get<T>(this IEventStreamHost<T> host, EventStreamId id)
        {
            Requires.NotNull(host, nameof(host));

            return host.GetAsync(id).GetResult();
        }

        /// <summary>
        /// A synchronous <see cref="IEventStreamHost{T}.ListAsync"/>.
        /// </summary>        
        public static ImmutableArray<EventStreamInfo> List<T>(this IEventStreamHost<T> host)
        {
            Requires.NotNull(host, nameof(host));

            return host.ListAsync().GetResult();
        }

        /// <summary>
        /// A synchronous <see cref="IReadOnlyEventStream{T}.GetInfoAsync"/>.
        /// </summary>        
        public static EventStreamInfo GetInfo<T>(this IReadOnlyEventStream<T> stream)
        {
            Requires.NotNull(stream, nameof(stream));

            return stream.GetInfoAsync().GetResult();
        }

        /// <summary>
        /// A synchronous <see cref="IWriteOnlyEventStream{T}.WriteAsync"/>.
        /// </summary>
        public static EventStreamSegment<T> Write<T>(this IWriteOnlyEventStream<T> stream, ImmutableArray<T> events)
        {
            Requires.NotNull(stream, nameof(stream));

            return stream.WriteAsync(events).GetResult();
        }

        /// <summary>
        /// A single event, synchronous <see cref="IWriteOnlyEventStream{T}.WriteAsync"/>.
        /// </summary>        
        public static EventStreamSegment<T> Write<T>(this IWriteOnlyEventStream<T> stream, T e)
        {
            Requires.NotNull(stream, nameof(stream));

            return stream.WriteAsync(e).GetResult();
        }

        /// <summary>
        /// A single event <see cref="IWriteOnlyEventStream{T}.WriteAsync"/>.
        /// </summary>        
        public static Task<EventStreamSegment<T>> WriteAsync<T>(this IWriteOnlyEventStream<T> stream, T e)
        {
            Requires.NotNull(stream, nameof(stream));

            return stream.WriteAsync(ImmutableArray.Create(e));
        }

        /// <summary>
        /// A synchronous <see cref="IWriteOnlyEventStream{T}.SealAsync"/>.
        /// </summary>        
        public static void Seal<T>(this IWriteOnlyEventStream<T> stream)
        {
            Requires.NotNull(stream, nameof(stream));

            stream.SealAsync().GetResult();
        }

        /// <summary>
        /// A synchronous <see cref="IWriteOnlyEventStream{T}.DeleteAsync"/>.
        /// </summary>        
        public static void Delete<T>(this IWriteOnlyEventStream<T> stream)
        {
            Requires.NotNull(stream, nameof(stream));

            stream.DeleteAsync().GetResult();
        }

        private static T GetResult<T>(this Task<T> task) => task.Result;

        private static void GetResult(this Task task) => task.GetAwaiter().GetResult();        
    }
}