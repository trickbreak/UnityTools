using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Trickbreak.AddressableUtility
{
    public class Pools : MonoBehaviour
    {
        private bool isDestroyObject = false;

        private bool isApplicationQuit = false;

        private const int MaxSize = 100;

        private Dictionary<string, AddressableObjectPool> pools = new Dictionary<string, AddressableObjectPool>();
        


        private void OnDestroy() 
        {
            isDestroyObject = true;
        }

        private void OnApplicationQuit() 
        {
            isApplicationQuit = true;
        }



        public IPoolableAddressableObject Get(string addressableName)
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
                AddressableObjectPool addressableObjectPool = new AddressableObjectPool(transform, addressableName, MaxSize);
                pools.Add(addressableName, addressableObjectPool);
            }

            return pools[addressableName].Pool.Get();
        }

        public void Release(IPoolableAddressableObject poolableObject)
        {
            try
            {
                string addressableName = poolableObject.gameObject.name;

                if (pools.TryGetValue(addressableName, out AddressableObjectPool addressableObjectPool))
                {
                    addressableObjectPool.Pool.Release(poolableObject);
                }    
            }
            catch (MissingReferenceException)
            {
                return;
            }
        }
    }

    public class AddressableObjectPool
    {
        private IObjectPool<IPoolableAddressableObject> pool;

        private Transform root;

        private string addressableName;



        public IObjectPool<IPoolableAddressableObject> Pool => pool;

        public AddressableObjectPool(Transform root, string addressableName, int maxSize)
        {
            this.root = root;
            this.addressableName = addressableName;

            pool = new ObjectPool<IPoolableAddressableObject>
            (
                    createFunc:         Create,
                    actionOnGet:        OnGet,
                    actionOnRelease:    OnRelease,
                    actionOnDestroy:    OnDestroy,
                    maxSize:            maxSize
            );
        }



        private IPoolableAddressableObject Create()
        {
            GameObject addressableObject = AddressableAssetsMaker.Make(addressableName);

            addressableObject.name = addressableName;
            // addressableObject.hideFlags = HideFlags.HideInHierarchy;

            if (addressableObject == null)
            {
                return null;
            }

            IPoolableAddressableObject poolableObject = addressableObject.GetComponent<IPoolableAddressableObject>();

            if (poolableObject == null)
            {
                GameObject.Destroy(addressableObject);
                return null;
            }

            return poolableObject;
        }

        private void OnGet(IPoolableAddressableObject poolableObject)
        {
            poolableObject.gameObject.SetActive(true);
            poolableObject.transform.SetParent(null);
        }

        private void OnRelease(IPoolableAddressableObject poolableObject)
        {
            // 씬 전환 등으로 Pools가 파괴된 경우 동작하지 않도록 처리 합니다.
            if (root == null)
            {
                return;
            }

            poolableObject.gameObject.SetActive(false);
            poolableObject.transform.SetParent(root);
        }

        private void OnDestroy(IPoolableAddressableObject poolableObject)
        {
            GameObject.Destroy(poolableObject.gameObject);
        }
    }

    public interface IPoolableAddressableObject
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
