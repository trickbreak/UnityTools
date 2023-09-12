using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace MessageManagement.Tests
{
    public class SpeedTest
    {
        private interface ITest_001_Message : IMessage
        {
            void Test_001();
        }

        private interface ITest_002_Message : IMessage
        {
            void Test_002(int a);
        }

        private class TestMessageReceiver : MonoBehaviourMessagePlus, ITest_001_Message, ITest_002_Message
        {
            private int count = 0;

            public void Test_001()
            {
                count++;
            }

            public void Test_002(int a)
            {
                count += a;
            }
        }

        private class TestMessageSender : MonoBehaviourMessagePlus
        {

        }

        private class SendMessageTester : MonoBehaviour
        {
            private int count = 0;

            public void Test()
            {
                count++;
            }

            public void Test1(int a)
            {
                count += a;
            }
        }

        private const int TestCount = 10000000;

        private int count = 0;

        [Test]
        public void Test1_EmptyFuncCall()
        {
            for (int i = 0; i < TestCount; i++)
            {
                EmptyFunc();
            }
        }
        
        [Test]
        public void Test2_AddCountFuncCall()
        {
            for (int i = 0; i < TestCount; i++)
            {
                AddCountFunc();
            }
        }

        // [Test]
        // public void FindInterfaceTest()
        // {
        //     GameObject gameObject = new GameObject();
        //     MessageReceiver component = gameObject.AddComponent<TestMessageReceiver>();

        //     for (int i = 0; i < TestCount; i++)
        //     {
        //         FindInterfaceFunc(component);
        //     }
        // }

        [Test]
        public void Test3_SendMessageCall()
        {
            GameObject gameObject = new GameObject();
            gameObject.AddComponent<SendMessageTester>();

            for (int i = 0; i < TestCount; i++)
            {
                SendMessageFunc(gameObject);
            }
        }

        [Test]
        public void Test4_SendMessageBoxingCall()
        {
            GameObject gameObject = new GameObject();
            gameObject.AddComponent<SendMessageTester>();

            for (int i = 0; i < TestCount; i++)
            {
                SendMessageBoxingFunc(gameObject);
            }
        }

        [Test]
        public void Test5_CustomSendMessageCall()
        {
            GameObject gameObject = new GameObject();
            TestMessageSender testMessageSender = gameObject.AddComponent<TestMessageSender>();
            TestMessageReceiver testMessageReceiver = gameObject.AddComponent<TestMessageReceiver>();

            for (int i = 0; i < TestCount; i++)
            {
                testMessageSender.SendMessage((ITest_001_Message test1)=>test1.Test_001());
            }
        }

        [Test]
        public void Test6_CustomSendMessageParameterCall()
        {
            GameObject gameObject = new GameObject();
            TestMessageSender testMessageSender = gameObject.AddComponent<TestMessageSender>();
            TestMessageReceiver testMessageReceiver = gameObject.AddComponent<TestMessageReceiver>();

            for (int i = 0; i < TestCount; i++)
            {
                testMessageSender.SendMessage((ITest_002_Message test2)=>test2.Test_002(1));
            }
        }

        private void EmptyFunc()
        {

        }

        private void AddCountFunc()
        {
            count++;
        }

        private void FindInterfaceFunc(MonoBehaviourMessagePlus messageReceiver)
        {
            Type thisType = messageReceiver.GetType();

            Type[] messagesType = thisType.FindInterfaces(IsTypeCheker, nameof(IMessage));

            bool IsTypeCheker(Type targetType, object filterCriteria)
            {
                var message = targetType.GetInterface(filterCriteria.ToString());
                return message != null;
            }
        }

        private void SendMessageFunc(GameObject target)
        {
            target.SendMessage("Test");
        }

        private void SendMessageBoxingFunc(GameObject target)
        {
            target.SendMessage("Test1", 1);
        }
    }
}
