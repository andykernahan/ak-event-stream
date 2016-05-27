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

namespace AK.EventStream.InMemory.Support
{
    internal sealed class EventStreamObserverStorage<T>
    {
        private ImmutableArray<IObserver<EventStreamSegment<T>>> _observers =
            ImmutableArray<IObserver<EventStreamSegment<T>>>.Empty;

        public void Add(IObserver<EventStreamSegment<T>> observer)
        {
            Debug.Assert(observer != null);

            _observers = _observers.Add(observer);
        }

        public void Remove(IObserver<EventStreamSegment<T>> observer)
        {
            Debug.Assert(observer != null);

            _observers = _observers.Remove(observer);
        }

        public void Clear() => _observers = _observers.Clear();

        public ImmutableArray<IObserver<EventStreamSegment<T>>> Snapshot() => _observers;
    }
}