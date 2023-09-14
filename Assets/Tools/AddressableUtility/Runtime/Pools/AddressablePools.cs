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
        /// <param name="position">풀링 타겟이 활성화(Active - True) 되기 직전에 월드 좌표를 세팅할 수 있습니다.
        /// <remarks>[주의사항]<br/> 풀링 타겟이 처음 만들어질때(Create) 원본 프리팹의 상태에 따라 gameObject가 켜져있는 상태로 생성될 수 있습니다.<br/>
        /// 리지드바디가 붙어 있는 오브젝트처럼 꺼져 있는 상태에서 풀링 타겟의 위치를 세팅하고 싶으면 원본 프리펩의 활성상태를 false로 설정해야 합니다.<br/>
        /// 생성 이후 재활용시에는 Release 처리에서 활성상태를 false 처리하기 때문에 상관 없습니다.</remarks>
        /// </param>
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
