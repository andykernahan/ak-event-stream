#pragma warning disable 1998
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
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using AK.EventStream.InMemory.Support;
using AK.EventStream.Support;

namespace AK.EventStream.InMemory
{
    internal sealed class InMemoryEventStream<T> : IEventStream<T>
    {
        private readonly object _syncRoot = new object();
        private readonly EventInfoStorage<T> _events = new EventInfoStorage<T>();
        private readonly EventStreamObserverStorage<T> _observers = new EventStreamObserverStorage<T>();
        private State _state;

        private enum State
        {
            Default,
            Sealed,
            Deleted
        }

        public event Action<InMemoryEventStream<T>> Deleted;

        public InMemoryEventStream(EventStreamId id)
        {
            Requires.NotDefault(id, nameof(id));

            Id = id;
        }

        public EventStreamId Id { get; }

        public IObservable<EventStreamSegment<T>> OpenAtStart()
        {
            return MakeObservable(observer =>
            {
                lock (_syncRoot)
                {
                    switch (_state)
                    {
                        case State.Default:
                            var subscription = Add(observer);
                            observer.OnNext(_events.Snapshot());
                            return subscription;
                        case State.Sealed:
                            observer.OnFinal(_events.Snapshot());
                            return Disposable.Empty;
                        case State.Deleted:
                            observer.OnError(new EventStreamDeletedException(Id));
                            return Disposable.Empty;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            });
        }

        public IObservable<EventStreamSegment<T>> OpenAtEnd()
        {
            return MakeObservable(observer =>
            {
                lock (_syncRoot)
                {
                    switch (_state)
                    {
                        case State.Default:
                            return Add(observer);
                        case State.Sealed:
                            observer.OnCompleted();
                            return Disposable.Empty;
                        case State.Deleted:
                            observer.OnError(new EventStreamDeletedException(Id));
                            return Disposable.Empty;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            });
        }

        public IObservable<EventStreamSegment<T>> OpenAt(long sequence)
        {
            return MakeObservable(observer =>
            {
                lock (_syncRoot)
                {
                    if (_state == State.Deleted)
                    {
                        observer.OnError(new EventStreamDeletedException(Id));
                        return Disposable.Empty;
                    }
                    var snapshot = _events.SnapshotFrom(sequence);
                    if (snapshot.IsEmpty)
                    {
                        observer.OnError(new EventStreamSequenceNotFoundException(Id, sequence));
                        return Disposable.Empty;
                    }
                    switch (_state)
                    {
                        case State.Default:
                            var subscription = Add(observer);
                            observer.OnNext(snapshot);
                            return subscription;
                        case State.Sealed:
                            observer.OnFinal(snapshot);
                            return Disposable.Empty;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            });
        }

        public async Task<EventStreamSegment<T>> WriteAsync(ImmutableArray<T> events)
        {
            Requires.NotDefault(events, nameof(events));

            lock (_syncRoot)
            {
                VerifyNotDeleted();
                VerifyNotSealed();

                if (events.IsEmpty)
                {
                    return EventStreamSegment<T>.Empty;
                }
                var segment = _events.Add(events);
                _observers.Snapshot().OnNext(segment);
                return segment;
            }
        }

        public async Task SealAsync()
        {
            lock (_syncRoot)
            {
                VerifyNotDeleted();
                VerifyNotSealed();

                _state = State.Sealed;
                _events.TrimExcess();
                var observers = _observers.Snapshot();
                _observers.Clear();
                observers.OnCompleted();
            }
        }

        public async Task DeleteAsync()
        {
            lock (_syncRoot)
            {
                VerifyNotDeleted();

                _state = State.Deleted;
                _events.Clear();
                var observers = _observers.Snapshot();
                _observers.Clear();
                observers.OnError(new EventStreamDeletedException(Id));
            }
            Deleted?.Invoke(this);
        }

        public async Task<EventStreamInfo> GetInfoAsync() => GetInfo();

        internal EventStreamInfo GetInfo()
        {
            lock (_syncRoot)
            {
                VerifyNotDeleted();

                var sequence = -1L;
                var count = 0L;
                var events = _events.Snapshot();
                if (!events.IsEmpty)
                {
                    var lastEvent = events[events.Count - 1];
                    sequence = lastEvent.Sequence;
                    count = events.Count;
                }
                return new EventStreamInfo(Id, sequence, count, @sealed: _state == State.Sealed);
            }
        }

        private static IObservable<EventStreamSegment<T>> MakeObservable(
            Func<AsyncObserver<EventStreamSegment<T>>, IDisposable> subscribe)
        {
            return Observable.Create<EventStreamSegment<T>>(inner => subscribe(inner.AsAsync()));
        }

        private IDisposable Add(AsyncObserver<EventStreamSegment<T>> observer)
        {
            Debug.Assert(Monitor.IsEntered(_syncRoot));
            Debug.Assert(_state == State.Default);

            _observers.Add(observer);
            return Disposable.Create(() =>
            {
                observer.Dispose();
                lock (_syncRoot)
                {
                    _observers.Remove(observer);
                }
            });
        }

        private void VerifyNotSealed()
        {
            Debug.Assert(Monitor.IsEntered(_syncRoot));

            if (_state == State.Sealed)
            {
                throw new EventStreamSealedException(Id);
            }
        }

        private void VerifyNotDeleted()
        {
            Debug.Assert(Monitor.IsEntered(_syncRoot));

            if (_state == State.Deleted)
            {
                throw new EventStreamDeletedException(Id);
            }
        }
    }
}