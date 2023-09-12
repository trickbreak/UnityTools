using System;
using System.Collections.Generic;
using UnityEngine;

namespace MessageManagement
{
    public sealed class MessageManager : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Every time you broadcast it retrieves all its children.")]
        private bool isDynamicBroadcast = false;

        private Dictionary<Type, List<IMessage>> messageList = new Dictionary<Type, List<IMessage>>();
        
        //private SimpleDictionary<List<IMessage>> messageList = new SimpleDictionary<List<IMessage>>();

        private MessageManager[] messageManagers = null;

        private MessageManager[] MessageManagers
        {
            get
            {
                if (isDynamicBroadcast)
                {
                    messageManagers = GetComponentsInChildren<MessageManager>();
                }
                else
                {
                    if (messageManagers == null)
                    {
                        messageManagers = GetComponentsInChildren<MessageManager>();
                    }
                }

                return messageManagers;
            }
        }

        private bool isEnable = false;



        private void OnEnable()
        {
            isEnable = true;
        }

        private void OnDisable()
        {
            isEnable = false;
        }



        /// <summary>
        /// Receiver의 메시지를 등록 합니다.
        /// </summary>
        public void Register(Type messageType, IMessage message)
        {
            if (messageList.TryGetValue(messageType, out List<IMessage> messages))
            {
                messages.Add(message);
            }
            else
            {
                messageList.Add(messageType, new List<IMessage> { message });
            }
        }
        
        /// <summary>
        /// Receiver의 메시지를 해제 합니다.
        /// </summary>
        public void Unregister(Type messageType, IMessage message)
        {
            if (messageList.TryGetValue(messageType, out List<IMessage> messages))
            {
                if (messages.Remove(message))
                {
                    if (messages.Count == 0)
                    {
                        messageList.Remove(messageType);
                    }
                }
            }
        }

        /// <summary>
        /// 자기 자신에게만 메시지를 보냅니다. (자식 제외)
        /// 비활성화 된 오브젝트는 메시지를 받지 않습니다.
        /// </summary>
        public void SendMessage<TMessage>(Action<TMessage> message) where TMessage : class, IMessage
        {
            if (!isEnable)
            {
                return;
            }

            Type messageType = typeof(TMessage);

            if (messageList.TryGetValue(messageType, out List<IMessage> messages))
            {
                for (int i = 0; i < messages.Count; i++)
                {
                    message.Invoke(messages[i] as TMessage);
                }
            }
        }

        /// <summary>
        /// 자신을 포함한 모든 자식에게 메시지를 보냅니다.
        /// 비활성화 된 오브젝트는 메시지를 받지 않습니다.
        /// </summary>
        public void BroadcastMessage<TMessage>(Action<TMessage> message) where TMessage : class, IMessage
        {
            if (!isEnable)
            {
                return;
            }

            MessageManager[] messageManagers = MessageManagers;

            if (messageManagers == null)
            {
                return;
            }

            for (int i = 0; i < messageManagers.Length; i++)
            {
                messageManagers[i].SendMessage(message);
            }
        }
    }
}
