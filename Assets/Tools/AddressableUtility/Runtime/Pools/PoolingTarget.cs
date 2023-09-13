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
        
        public event Action OnGetEvent;

        public event Action OnReleaseEvent;
        
        

        internal void OnCreate(AddressablePool parentPool)
        {
            this.parentPool = parentPool;
        }

        internal void OnGet()
        {
            OnGetEvent?.Invoke();
            OnGetUnityEvent?.Invoke();
        }

        internal void OnRelease()
        {
            OnReleaseEvent?.Invoke();
            OnReleaseUnityEvent?.Invoke();
        }



        public void Release()
        {
            parentPool.Pool.Release(this);
        }
    }    
}

