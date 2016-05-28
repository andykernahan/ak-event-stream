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

        private static readonly WaitCallback DeliverMessagesWaitCallback = state => DeliverMessages((Mailbox<TMessage>)state);

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
            ThreadPool.QueueUserWorkItem(DeliverMessagesWaitCallback, this);
        }

        private static void DeliverMessages(Mailbox<TMessage> mailbox)
        {
            var messages = mailbox._messages;
            var recipient = mailbox._recipient;
            while (true)
            {
                TMessage message;
                lock (messages)
                {
                    Debug.Assert(mailbox._delivering);

                    if (messages.Count == 0)
                    {
                        mailbox._delivering = false;
                        return;
                    }
                    message = messages.Dequeue();
                }
                recipient(message);
            }
        }
    }
}