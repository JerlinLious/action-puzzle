using UnityEngine;
namespace ShanghaiWindy.AssetLoader {
    public class AssetRequestTask {
        public System.Action<Object> onAssetLoaded;

        public string currentAssetName;

        public void SetAssetBundleName(string assetBundleName, string assetBundleVariant) {
            currentAssetName = string.Format("{0}.{1}", assetBundleName.ToLower(), assetBundleVariant.ToLower());
        }

        public string GetABName() {
            return currentAssetName;
        }
    }
}