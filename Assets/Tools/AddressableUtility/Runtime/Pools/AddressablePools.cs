using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Trickbreak.AddressableUtility
{
    public partial class AddressablePools : MonoBehaviour
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



        public IPoolingTarget Get(string addressableName)
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

        public void Release(IPoolingTarget poolingTargetObject)
        {
            try
            {
                string addressableName = poolingTargetObject.gameObject.name;

                if (pools.TryGetValue(addressableName, out AddressablePool addressableObjectPool))
                {
                    addressableObjectPool.Pool.Release(poolingTargetObject);
                }    
            }
            catch (MissingReferenceException)
            {
                return;
            }
        }
    }
    
    
    
    public partial class AddressablePools
    {
        private class AddressablePool
        {
            private IObjectPool<IPoolingTarget> pool;

            private Transform root;

            private string addressableName;



            public IObjectPool<IPoolingTarget> Pool => pool;

            public AddressablePool(Transform root, string addressableName, int maxSize)
            {
                this.root = root;
                this.addressableName = addressableName;

                pool = new ObjectPool<IPoolingTarget>
                (
                        createFunc:         Create,
                        actionOnGet:        OnGet,
                        actionOnRelease:    OnRelease,
                        actionOnDestroy:    OnDestroy,
                        maxSize:            maxSize
                );
            }



            private IPoolingTarget Create()
            {
                GameObject addressableObject = AddressableMaker.Make(addressableName);

                addressableObject.name = addressableName;
                // addressableObject.hideFlags = HideFlags.HideInHierarchy;

                if (addressableObject == null)
                {
                    return null;
                }

                IPoolingTarget poolingTarget = addressableObject.GetComponent<IPoolingTarget>();

                if (poolingTarget == null)
                {
                    Destroy(addressableObject);
                    return null;
                }

                return poolingTarget;
            }

            private void OnGet(IPoolingTarget poolingTarget)
            {
                poolingTarget.gameObject.SetActive(true);
                poolingTarget.transform.SetParent(null);
            }

            private void OnRelease(IPoolingTarget poolingTarget)
            {
                // 씬 전환 등으로 Pools가 파괴된 경우 동작하지 않도록 처리 합니다.
                if (root == null)
                {
                    return;
                }

                poolingTarget.gameObject.SetActive(false);
                poolingTarget.transform.SetParent(root);
            }

            private void OnDestroy(IPoolingTarget poolingTarget)
            {
                Destroy(poolingTarget.gameObject);
            }
        }    
    }
    
    
    
    public interface IPoolingTarget
    {
        public GameObject gameObject { get; }

        public Transform transform { get; }
    }

    //public static class PoolableAddressableObjectExtension
    //{
    //    public static void Release(this IPoolableAddressableObject poolableObject)
    //    {
    //        try
    //        {
    //            AddressableObjectPools.Instance.Release(poolableObject);
    //        }
    //        catch (System.NullReferenceException)
    //        {
    //            return;
    //        }
    //    }
    //}
}
