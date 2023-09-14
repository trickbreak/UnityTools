using System.Collections.Generic;
using UnityEngine;

namespace Trickbreak.AddressableUtility
{
    public class AddressablePools : MonoBehaviour
    {
        /// <summary>
        /// 하나의 Pool 에서 최대로 가질 수 있는 오브젝트의 갯수 입니다.
        /// </summary>
        private const int MaxSize = 100;
        
        
        
        private bool isDestroyObject = false;

        private bool isApplicationQuit = false;

        private Dictionary<string, AddressablePool> pools = new Dictionary<string, AddressablePool>();
        


        private void OnDestroy() 
        {
            isDestroyObject = true;
        }

        private void OnApplicationQuit() 
        {
            isApplicationQuit = true;
        }



        /// <summary>
        /// 풀링 타겟 오브젝트를 가져 옵니다.
        /// </summary>
        /// <param name="addressableName">풀링 타겟의 어트레서블 키값 입니다.</param>
        /// <param name="position">풀링 타겟이 활성화(Active - True) 되기 직전에 월드 좌표를 세팅할 수 있습니다.</param>
        /// <param name="parent">풀링 타겟이 활성화(Active - True) 되기 직전에 부모를 세팅할 수 있습니다.</param>
        public PoolingTarget Get(string addressableName, Vector3? position = null, Transform parent = null)
        {
            if (isDestroyObject)
            {
                return null;
            }

            if (isApplicationQuit)
            {
                return null;
            }

            if (!pools.ContainsKey(addressableName))
            {
                AddressablePool addressablePool = new AddressablePool(transform, addressableName, MaxSize);
                pools.Add(addressableName, addressablePool);
            }

            PoolingTarget poolingTarget = pools[addressableName].Pool.Get();

            if (poolingTarget == null)
            {
                return null;
            }

            Transform poolingTargetTransform = poolingTarget.transform;
            
            poolingTargetTransform.SetParent(parent);

            if (position.HasValue)
            {
                poolingTargetTransform.position = position.Value;
            }
            
            poolingTarget.OnGet();
            
            poolingTarget.gameObject.SetActive(true);
            
            return poolingTarget;
        }
    }
}
