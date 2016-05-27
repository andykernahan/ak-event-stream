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
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;

namespace AK.EventStream.InMemory.Support
{
    internal sealed class EventInfoStorage<T>
    {
        private int _count;
        private long _sequence;
        private EventInfo<T>[] _events = Array.Empty<EventInfo<T>>();

        private static readonly IComparer<EventInfo<T>> SequenceComparer =
            Comparer<EventInfo<T>>.Create((x, y) => x.Sequence.CompareTo(y.Sequence));

        public EventStreamSegment<T> Add(ImmutableArray<T> events)
        {
            Debug.Assert(!events.IsDefault);

            var offset = _count;
            var timestamp = DateTime.UtcNow;
            EnsureCapacity(events.Length);
            foreach (var e in events)
            {
                _events[_count++] = new EventInfo<T>(_sequence++, timestamp, e);
            }
            return new EventStreamSegment<T>(_events, offset, events.Length);
        }

        public void Clear()
        {
            _events = Array.Empty<EventInfo<T>>();
            _count = 0;
        }

        public void TrimExcess()
        {
            if (_count <= (int)(_events.Length * 0.9))
            {
                Array.Resize(ref _events, _count);
            }
        }

        public EventStreamSegment<T> Snapshot()
        {
            return new EventStreamSegment<T>(_events, 0, _count);
        }

        public EventStreamSegment<T> SnapshotFrom(long sequence)
        {
            var i = IndexOf(sequence);
            return i != -1 ? new EventStreamSegment<T>(_events, i, _count - i) : EventStreamSegment<T>.Empty;
        }

        private int IndexOf(long sequence)
        {
            if (_count == 0)
            {
                return -1;
            }
            var template = _events[0];
            var e = new EventInfo<T>(sequence, template.Timestamp, template.Data);
            var i = Array.BinarySearch(_events, 0, _count, e, SequenceComparer);
            return i >= 0 ? i : -1;
        }

        private void EnsureCapacity(int additions)
        {
            var requiredLength = checked(_count + additions);
            if (requiredLength > _events.Length)
            {
                Array.Resize(ref _events, Math.Max(NextPowerOfTwo(requiredLength), 64));
            }
        }

        private static int NextPowerOfTwo(int n)
        {
            checked
            {
                // http://graphics.stanford.edu/~seander/bithacks.html#RoundUpPowerOf2
                var v = n;
                v--;
                v |= v >> 1;
                v |= v >> 2;
                v |= v >> 4;
                v |= v >> 8;
                v |= v >> 16;
                v++;
                return v;
            }
        }
    }
}