using System;
using UnityEngine;
using UnityEngine.Events;

namespace Trickbreak.AddressableUtility
{
    public class PoolingTarget : MonoBehaviour
    {
        private AddressablePool parentPool;


        [Header("Events")]
        public UnityEvent OnGetUnityEvent; 
        
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

