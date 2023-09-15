using UnityEngine;
using UnityEngine.Events;

namespace Trickbreak.AddressableUtility
{
    public class PoolingTarget : MonoBehaviour
    {
        private AddressablePool parentPool;

        [Header("Option")]
        [Tooltip("참조 하고 싶은 컴포넌트를 하나 추가할 수 있습니다. GetComponent로 컴포넌트를 가져오지 않기 위해 사용할 수 있습니다. (필수X, 옵션O)")]
        [SerializeField]
        private Component target;

        [Header("Events")]
        [Tooltip("Get 처리시 오브젝트가 활성화(Active - True)되기 직전에 호출되는 이벤트 입니다.")]
        [SerializeField]
        private UnityEvent onGetUnityEvent; 
        
        [Tooltip("Release 처리시 오브젝트가 비활성화(Active - False)되고 바로 호출되는 이벤트 입니다.")]
        [SerializeField]
        private UnityEvent onReleaseUnityEvent;
        
        
        
        internal void OnCreate(AddressablePool parentPool)
        {
            this.parentPool = parentPool;
        }

        internal void OnGet()
        {
            onGetUnityEvent?.Invoke();
        }

        internal void OnRelease()
        {
            onReleaseUnityEvent?.Invoke();
        }



        public void Release()
        {
            parentPool.Pool.Release(this);
        }

        /// <summary>
        /// 옵션으로 Target을 지정한 경우 Target을 가져올 수 있습니다.<br/>
        /// GetComponent를 사용하지 않고 타겟에 접근할 수 있습니다.
        /// </summary>
        public T GetTarget<T>() where T : Component
        {
            return target as T;
        }
    }    
}

