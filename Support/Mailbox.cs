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
using System.Diagnostics;
using System.Threading;

namespace AK.EventStream.Support
{
    internal sealed class Mailbox<TMessage>
    {
        private readonly Action<TMessage> _recipient;
        private readonly Queue<TMessage> _messages = new Queue<TMessage>();
        private bool _delivering;

        public Mailbox(Action<TMessage> recipient)
        {
            Debug.Assert(recipient != null);

            _recipient = recipient;
        }

        public void Post(TMessage message)
        {
            Debug.Assert(message != null);

            lock (_messages)
            {
                _messages.Enqueue(message);
                MaybeDeliverAsync();
            }
        }

        public void Empty()
        {
            lock (_messages)
            {
                _messages.Clear();
            }
        }

        private void MaybeDeliverAsync()
        {
            Debug.Assert(Monitor.IsEntered(_messages));

            if (_delivering)
            {
                return;
            }
            _delivering = true;
            ThreadPool.QueueUserWorkItem(Deliver);
        }

        private void Deliver(object _)
        {
            while (true)
            {
                TMessage message;
                lock (_messages)
                {
                    if (_messages.Count == 0)
                    {
                        _delivering = false;
                        return;
                    }
                    message = _messages.Dequeue();
                }
                _recipient(message);
            }
        }
    }
}