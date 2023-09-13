using UnityEngine;
using UnityEngine.Pool;

namespace Trickbreak.AddressableUtility
{
    internal class AddressablePool
    {
        private IObjectPool<PoolingTarget> pool;

        private Transform root;

        private string addressableName;



        public IObjectPool<PoolingTarget> Pool => pool;

        public AddressablePool(Transform root, string addressableName, int maxSize)
        {
            this.root = root;
            this.addressableName = addressableName;

            pool = new ObjectPool<PoolingTarget>
            (
                    createFunc:         Create,
                    actionOnGet:        OnGet,
                    actionOnRelease:    OnRelease,
                    actionOnDestroy:    OnDestroy,
                    maxSize:            maxSize
            );
        }



        private PoolingTarget Create()
        {
            GameObject addressableObject = AddressableMaker.Make(addressableName);

            addressableObject.name = addressableName;
            // addressableObject.hideFlags = HideFlags.HideInHierarchy;

            if (addressableObject == null)
            {
                return null;
            }

            PoolingTarget poolingTarget = addressableObject.GetComponent<PoolingTarget>();

            if (poolingTarget == null)
            {
                Object.Destroy(addressableObject);
                return null;
            }

            poolingTarget.OnCreate(this);
            return poolingTarget;
        }

        private void OnGet(PoolingTarget poolingTarget)
        {
            poolingTarget.gameObject.SetActive(true);
            poolingTarget.transform.SetParent(null);
            poolingTarget.OnGet();
        }

        private void OnRelease(PoolingTarget poolingTarget)
        {
            // 씬 전환 등으로 Pools가 파괴된 경우 동작하지 않도록 처리 합니다.
            if (root == null)
            {
                return;
            }

            poolingTarget.gameObject.SetActive(false);
            poolingTarget.transform.SetParent(root);
            poolingTarget.OnRelease();
        }

        private void OnDestroy(PoolingTarget poolingTarget)
        {
            Object.Destroy(poolingTarget.gameObject);
        }
    }
}
