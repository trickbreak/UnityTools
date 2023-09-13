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

    public static class AddressableMaker
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

        public static TComponent Make<TComponent>(string key, Transform parent = null) where TComponent : Component
        {
            var addressableAsset = Make(key, parent);
            
            if (addressableAsset != null)
            {
                return addressableAsset.GetComponent<TComponent>();
            }
            else
            {
                return null;
            }
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
    
        public static TComponent Make<TComponent>(AssetReference assetReference, Transform parent = null) where TComponent : Component
        {
            var addressableAsset = Make(assetReference, parent);
            if (addressableAsset != null)
            {
                return addressableAsset.GetComponent<TComponent>();     
            }
            else
            {
                return null;
            }
        }
    }
}