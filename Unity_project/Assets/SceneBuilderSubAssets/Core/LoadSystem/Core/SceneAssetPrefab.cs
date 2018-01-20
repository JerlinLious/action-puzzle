using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;


namespace ShanghaiWindy.AssetLoader {
    public class SceneAssetPrefab :MonoBehaviour {
        public System.Action onFinish;

        public string assetBundleName, assetBundleVariant, assetName;
        [System.NonSerialized]
        public bool LoadingDone = false, inLoading = false;

        [System.Serializable]
        public class MeshParameter {
            public Vector4 LightingMapTilingOffset;
            public int LightingMapIndex = -1;
            public bool RendererPathinChild = false;
            public string RendererPath = "";
            public ReflectionProbeUsage reflectionusage;
        }
        public MeshParameter[] meshParameters;



        public void LoadAsset() {
            AssetRequestTask assetRequestTask = new AssetRequestTask() {
                onAssetLoaded = (myReturnValue) => {
                    if (myReturnValue == null) {
                        LoadingDone = true;
                        inLoading = false;

                        if (onFinish != null)
                            onFinish();

                        return;
                    }
                    if (onFinish != null)
                        onFinish();

                    GameObject Instance = Instantiate(myReturnValue) as GameObject;

                    Instance.transform.position = transform.position;
                    Instance.transform.rotation = transform.rotation;
                    Instance.transform.SetParent(transform.parent);

                    if (meshParameters.Length > 0) {
                        foreach (MeshParameter meshParameter in meshParameters) {
                            MeshRenderer TargetRenderer = null;
                            if (meshParameter.RendererPathinChild) {
                                if (Instance.transform.Find(meshParameter.RendererPath))
                                    TargetRenderer = Instance.transform.Find(meshParameter.RendererPath).GetComponent<MeshRenderer>();
                            }
                            else {
                                if (Instance.transform)
                                    TargetRenderer = Instance.transform.GetComponent<MeshRenderer>();
                            }


                            if (TargetRenderer) {
                                TargetRenderer.lightmapIndex = meshParameter.LightingMapIndex;
                                TargetRenderer.lightmapScaleOffset = meshParameter.LightingMapTilingOffset;
                                TargetRenderer.reflectionProbeUsage = meshParameter.reflectionusage;
                            }
                        }
                    }

                    LoadingDone = true;
                    inLoading = false;
                }
            };
            assetRequestTask.SetAssetBundleName(assetBundleName, assetBundleVariant);

            SubBuild_AssetBundleManager.LoadAssetFromAssetBundle(assetRequestTask);
        }
    }
}
