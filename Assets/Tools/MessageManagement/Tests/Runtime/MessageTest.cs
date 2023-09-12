using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System;
using System.Collections;

namespace MessageManagement.Tests
{
    public class MessageTest
    {
        private class TestMessageSender : MonoBehaviourMessagePlus
        {

        }



        private interface IMoveMessage : IMessage
        {
            void OnMove(Vector2 position);
        }

        private interface IAttackMessage : IMessage
        {
            void OnAttack();
        }

        private interface IJumpMessage : IMessage
        {
            void OnJump(Action<MonoBehaviour> endEvent);
        }



        private class MoveMessageReceiver : MonoBehaviourMessagePlus, IMoveMessage
        {
            public void OnMove(Vector2 position)
            {
                Debug.Log("MoveMessageReceiver -> OnMove");
            }
        }

        private class AttackMessageReceiver : MonoBehaviourMessagePlus, IAttackMessage
        {
            public void OnAttack()
            {
                Debug.Log("AttackMessageReceiver -> OnAttack");
            }
        }

        private class MoveAndAttackMessageReceiver : MonoBehaviourMessagePlus, IMoveMessage, IAttackMessage
        {
            public void OnAttack()
            {
                Debug.Log("MoveAndAttackMessageReceiver -> OnAttack");
            }

            public void OnMove(Vector2 position)
            {
                Debug.Log("MoveAndAttackMessageReceiver -> OnMove");
            }
        }

        private class JumpMessageReceiver : MonoBehaviourMessagePlus, IJumpMessage
        {
            private Action<MonoBehaviour> endEvent = null;

            private void JumpEnd()
            {
                endEvent?.Invoke(this);
            }



            public const float JumpTime = 0.5f;

            public void OnJump(Action<MonoBehaviour> endEvent)
            {
                this.endEvent = endEvent;
                
                Debug.Log($"{GetType().Name} -> OnJump");
                Invoke("JumpEnd", JumpTime);
            }
        }

        private class MoveAndJumpMessageReceiver : JumpMessageReceiver, IMoveMessage
        {
            public void OnMove(Vector2 position)
            {
                Debug.Log("MoveAndJumpMessageReceiver -> OnMove");
            }
        }



        [Test]
        public void 메시지_전송_테스트_001()
        {
            GameObject gameObject = new GameObject();

            TestMessageSender messageManager = gameObject.AddComponent<TestMessageSender>();

            MonoBehaviourMessagePlus component1 = gameObject.AddComponent<MoveMessageReceiver>();
            MonoBehaviourMessagePlus component2 = gameObject.AddComponent<AttackMessageReceiver>();
            MonoBehaviourMessagePlus component3 = gameObject.AddComponent<MoveAndAttackMessageReceiver>();

            messageManager.SendMessage((IMoveMessage onMove) => onMove.OnMove(Vector2.one));

            LogAssert.Expect(LogType.Log, "MoveMessageReceiver -> OnMove");
            LogAssert.Expect(LogType.Log, "MoveAndAttackMessageReceiver -> OnMove");
        }

        [Test]
        public void 메시지_전송_테스트_002()
        {
            GameObject gameObject = new GameObject();

            TestMessageSender messageManager = gameObject.AddComponent<TestMessageSender>();

            MonoBehaviourMessagePlus component1 = gameObject.AddComponent<MoveMessageReceiver>();
            MonoBehaviourMessagePlus component2 = gameObject.AddComponent<AttackMessageReceiver>();
            MonoBehaviourMessagePlus component3 = gameObject.AddComponent<MoveAndAttackMessageReceiver>();

            messageManager.SendMessage((IAttackMessage onAttack) => onAttack.OnAttack());

            LogAssert.Expect(LogType.Log, "AttackMessageReceiver -> OnAttack");
            LogAssert.Expect(LogType.Log, "MoveAndAttackMessageReceiver -> OnAttack");
        }

        [Test]
        public void 리시버_제거_후_메시지_전송_테스트()
        {
            GameObject gameObject = new GameObject();

            TestMessageSender messageManager = gameObject.AddComponent<TestMessageSender>();

            MonoBehaviourMessagePlus component1 = gameObject.AddComponent<MoveMessageReceiver>();
            MonoBehaviourMessagePlus component2 = gameObject.AddComponent<AttackMessageReceiver>();
            MonoBehaviourMessagePlus component3 = gameObject.AddComponent<MoveAndAttackMessageReceiver>();

            GameObject.DestroyImmediate(component2);

            messageManager.SendMessage((IAttackMessage onAttack) => onAttack.OnAttack());

            LogAssert.Expect(LogType.Log, "MoveAndAttackMessageReceiver -> OnAttack");
        }

        [Test]
        public void 리시버_추가_후_메시지_전송_테스트()
        {
            GameObject gameObject = new GameObject();

            TestMessageSender messageManager = gameObject.AddComponent<TestMessageSender>();

            MonoBehaviourMessagePlus component1 = gameObject.AddComponent<MoveMessageReceiver>();
            MonoBehaviourMessagePlus component2 = gameObject.AddComponent<AttackMessageReceiver>();
            
            messageManager.SendMessage((IAttackMessage onAttack) => onAttack.OnAttack());

            MonoBehaviourMessagePlus component3 = gameObject.AddComponent<MoveAndAttackMessageReceiver>();

            messageManager.SendMessage((IAttackMessage onAttack) => onAttack.OnAttack());

            LogAssert.Expect(LogType.Log, "AttackMessageReceiver -> OnAttack");
            LogAssert.Expect(LogType.Log, "AttackMessageReceiver -> OnAttack");
            LogAssert.Expect(LogType.Log, "MoveAndAttackMessageReceiver -> OnAttack");
        }
    
        [UnityTest]
        public IEnumerator 비동기_메시지_전송_테스트()
        {
            GameObject gameObject = new GameObject();

            TestMessageSender messageManager = gameObject.AddComponent<TestMessageSender>();

            MonoBehaviourMessagePlus component1 = gameObject.AddComponent<JumpMessageReceiver>();
            MonoBehaviourMessagePlus component2 = gameObject.AddComponent<MoveAndJumpMessageReceiver>();

            messageManager.SendMessage((IJumpMessage onJump) => onJump.OnJump((target) => Debug.Log($"{target.GetType().Name} -> Jump End")));

            yield return new WaitForSeconds(JumpMessageReceiver.JumpTime + 0.5f);

            LogAssert.Expect(LogType.Log, $"{nameof(JumpMessageReceiver)} -> OnJump");
            LogAssert.Expect(LogType.Log, $"{nameof(MoveAndJumpMessageReceiver)} -> OnJump");
            LogAssert.Expect(LogType.Log, $"{nameof(JumpMessageReceiver)} -> Jump End");
            LogAssert.Expect(LogType.Log, $"{nameof(MoveAndJumpMessageReceiver)} -> Jump End");
        }
    }
}
