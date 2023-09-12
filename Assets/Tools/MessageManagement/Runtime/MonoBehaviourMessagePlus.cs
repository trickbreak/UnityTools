using System;
using System.Collections.Generic;
using UnityEngine;

namespace MessageManagement
{
    [RequireComponent(typeof(MessageManager))]
    public abstract class MonoBehaviourMessagePlus : MonoBehaviour
    {
        private MessageManager messageManager = null;

        private MessageManager MessageManager
        {
            get
            {
                if (messageManager == null)
                {
                    messageManager = GetComponent<MessageManager>();
                }

                return messageManager;
            }
        }

        private Dictionary<Type, IMessage> messages = null;




        protected virtual void Awake() 
        {
            messages = InitializationMessages();

            foreach (var message in messages)
            {
                MessageManager.Register(message.Key, message.Value);
            }



            Dictionary<Type, IMessage> InitializationMessages()
            {
                Type thisType = this.GetType();

                Type[] messagesTypes = thisType.FindInterfaces(IsTypeCheker, nameof(IMessage));

                Dictionary<Type, IMessage> messages = new Dictionary<Type, IMessage>(messagesTypes.Length);

                for (int i = 0; i < messagesTypes.Length; i++)
                {
                    messages.Add(messagesTypes[i], this as IMessage);
                }

                return messages;



                bool IsTypeCheker(Type targetType, object filterCriteria)
                {
                    var message = targetType.GetInterface(filterCriteria.ToString());
                    return message != null;
                }
            }
        }

        protected virtual void OnDestroy() 
        {
            if (messages == null)
            {
                return;
            }

            foreach (var message in messages)
            {
                MessageManager.Unregister(message.Key, message.Value);
            }
        }




        public void SendMessage<TMessage>(Action<TMessage> message) where TMessage : class, IMessage
        {
            MessageManager.SendMessage(message);
        }

        public void BroadcastMessage<TMessage>(Action<TMessage> message) where TMessage : class, IMessage
        {
            MessageManager.BroadcastMessage(message);
        }
    }
}
