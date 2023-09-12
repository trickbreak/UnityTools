using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Trickbreak.AddressableUtility
{
    public sealed class AutoReleaser : MonoBehaviour
    {
        private void OnDestroy() 
        {
            Addressables.ReleaseInstance(gameObject);
        }
    }

    public static class AddressableAssetsMaker
    {
        public static GameObject Make(string key, Transform parent = null)
        {
            var addressableAsset = Addressables.InstantiateAsync(key, parent).WaitForCompletion();
            if (addressableAsset != null)
            {
                addressableAsset.AddComponent<AutoReleaser>();
            }

            return addressableAsset;
        }

        public static T Make<T>(string key, Transform parent = null)
        {
            var addressableAsset = Addressables.InstantiateAsync(key, parent).WaitForCompletion();
            if (addressableAsset == null)
                return default;
        
            addressableAsset.AddComponent<AutoReleaser>();
        
            return addressableAsset.GetComponent<T>();
        }
    
        public static GameObject Make(AssetReference assetReference, Transform parent = null)
        {
            var addressableAsset = Addressables.InstantiateAsync(assetReference, parent).WaitForCompletion();
            if (addressableAsset != null)
            {
                addressableAsset.AddComponent<AutoReleaser>();
            }
        
            return addressableAsset;
        }
    
        public static T Make<T>(AssetReference assetReference, Transform parent = null)
        {
            var addressableAsset = Addressables.InstantiateAsync(assetReference, parent).WaitForCompletion();
            if (addressableAsset == null)
                return default;
        
            addressableAsset.AddComponent<AutoReleaser>();
        
            return addressableAsset.GetComponent<T>();
        }
    }
}