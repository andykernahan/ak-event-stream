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
using System.Diagnostics;
using System.Reactive;

namespace AK.EventStream.Support
{
    internal sealed class AsyncObserver<T> : ObserverBase<T>
    {
        private readonly IObserver<T> _observer;
        private readonly Mailbox<Message> _mailbox;

        public AsyncObserver(IObserver<T> observer)
        {
            Debug.Assert(observer != null);

            _observer = observer;
            _mailbox = new Mailbox<Message>(Deliver);
        }

        protected override void OnNextCore(T value)
        {
            _mailbox.Post(Message.OnNext(value));
        }

        protected override void OnErrorCore(Exception error)
        {
            _mailbox.Post(Message.OnError(error));
        }

        protected override void OnCompletedCore()
        {
            _mailbox.Post(Message.OnCompleted());
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _mailbox.Empty();
            }
        }

        private void Deliver(Message message)
        {
            try
            {
                message.DeliverTo(_observer);
            }
            catch
            {
                Dispose();
            }
        }

        private struct Message
        {
            private readonly Type _type;
            private readonly T _value;
            private readonly Exception _error;

            private Message(Type type, T value, Exception error)
            {
                _type = type;
                _value = value;
                _error = error;
            }

            public void DeliverTo(IObserver<T> observer)
            {
                switch (_type)
                {
                    case Type.OnNext:
                        observer.OnNext(_value);
                        break;
                    case Type.OnCompleted:
                        observer.OnCompleted();
                        break;
                    case Type.OnError:
                        observer.OnError(_error);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(_type));
                }
            }

            public static Message OnNext(T value) => new Message(Type.OnNext, value, null);

            public static Message OnCompleted() => new Message(Type.OnCompleted, default(T), null);

            public static Message OnError(Exception error) => new Message(Type.OnError, default(T), error);

            private enum Type
            {
                OnNext,
                OnCompleted,
                OnError
            }
        }
    }
}