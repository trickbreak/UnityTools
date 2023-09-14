using System;
using UnityEngine;
using UnityEngine.Events;

namespace Trickbreak.AddressableUtility
{
    public class PoolingTarget : MonoBehaviour
    {
        private AddressablePool parentPool;


        [Header("Events")]
        [Tooltip("Get 처리시 오브젝트가 활성화(Active - True)되기 직전에 호출되는 이벤트 입니다.")]
        public UnityEvent OnGetUnityEvent; 
        
        [Tooltip("Release 처리시 오브젝트가 비활성화(Active - False)되고 바로 호출되는 이벤트 입니다.")]
        public UnityEvent OnReleaseUnityEvent;
        
        
        
        internal void OnCreate(AddressablePool parentPool)
        {
            this.parentPool = parentPool;
        }

        internal void OnGet()
        {
            OnGetUnityEvent?.Invoke();
        }

        internal void OnRelease()
        {
            OnReleaseUnityEvent?.Invoke();
        }



        public void Release()
        {
            parentPool.Pool.Release(this);
        }
    }    
}

