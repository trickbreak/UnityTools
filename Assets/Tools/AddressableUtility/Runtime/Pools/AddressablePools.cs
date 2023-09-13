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



        public PoolingTarget Get(string addressableName)
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

            return pools[addressableName].Pool.Get();
        }
    }
}
