using System.Collections;
using UnityEngine;

namespace Trickbreak.AddressableUtility
{
    public class PoolingTargetAutoDestroy : PoolingTarget
    {
        [Header("Etc")]
        [SerializeField]
        private float destroyTime = 1.0f;

        private void OnEnable() 
        {
            StartCoroutine(DelayedDestroy());
        }

        private IEnumerator DelayedDestroy()
        {
            yield return new WaitForSeconds(destroyTime);
            Release();
        }
    }    
}

