using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace ShanghaiWindy.AssetLoader {
    public class SubBuild_AssetBundleManager : MonoBehaviour {
        public static Dictionary<string, AssetBundle> LoadedAssets = new Dictionary<string, AssetBundle>();

        private static AssetBundleManifest assetBundleManifest;

        private static MonoBehaviour MonoActiveObject;

        private static bool hasTaskToFinish = false;

        private static Queue<AssetRequestTask> assetTaskQueue = new Queue<AssetRequestTask>();


        public static void Init(MonoBehaviour _monoActiveObject) {
            MonoActiveObject = _monoActiveObject;
        }

        public static IEnumerator GetAssetBundleInfos(System.Action onFinish) {
            WWW www = null;

            www = new WWW(GetAssetbundleLoadPath() + "main.packages");

            yield return www;

            assetBundleManifest = (AssetBundleManifest)www.assetBundle.LoadAsset("AssetBundleManifest");

            if (onFinish != null) {
                onFinish();
            }
        }


        public static void LoadAssetFromAssetBundle(AssetRequestTask assetRequestTask) {
            assetTaskQueue.Enqueue(assetRequestTask);
            hasTaskToFinish = true;
        }

        public static IEnumerator AssetBundleLoop() {
            while (true) {
                while (hasTaskToFinish == false) {
                    yield return new WaitForEndOfFrame();
                }
                //处理Task
                while (assetTaskQueue.Count > 0) {
                    AssetRequestTask current = assetTaskQueue.Dequeue();
                    //AssetBundle已经被读取 
                    if (LoadedAssets.ContainsKey(current.GetABName())) {
                        string assetPath = LoadedAssets[current.GetABName()].GetAllAssetNames()[0];

                        AssetBundleRequest abRequest = LoadedAssets[current.GetABName()].LoadAssetAsync(assetPath);

                        yield return abRequest;

                        current.onAssetLoaded(abRequest.asset);
                    }
                    //AssetBundle尚未读取
                    else {
                        //依赖包 Loop
                        string[] DependenciesInfo = assetBundleManifest.GetAllDependencies(current.GetABName());
                        for (int i = 0; i < DependenciesInfo.Length; i++) {
                            if (LoadedAssets.ContainsKey(DependenciesInfo[i])) {
                                continue;
                            }
                            bool isDependenceAssetLoaded = false;

                            MonoActiveObject.StartCoroutine(LoadAsset(DependenciesInfo[i], () => {
                                isDependenceAssetLoaded = true;
                            }));

                            while (!isDependenceAssetLoaded) {
                                yield return new WaitForFixedUpdate();
                            }
                        }
                        //主AB
                        bool isMainLoaded = false;

                        MonoActiveObject.StartCoroutine(LoadAsset(current.GetABName(), () => {
                            isMainLoaded = true;
                        }));

                        while (!isMainLoaded)
                            yield return new WaitForFixedUpdate();

                        //添加回队列 等待队列回调
                        assetTaskQueue.Enqueue(current);
                    }
                }
                hasTaskToFinish = false;
            }
        }

        private static IEnumerator LoadAsset(string assetBundleName, System.Action onFinish) {
            WWW www = new WWW(GetAssetbundleLoadPath() + assetBundleName);

            yield return www;

            if (string.IsNullOrEmpty(www.error)) {
                LoadedAssets.Add(assetBundleName, www.assetBundle);
            }
            else {
                Debug.Log(assetBundleName);
                Debug.LogError(www.error);
            }
            onFinish();
        }

        private static string GetAssetbundleLoadPath() {
            string path = "";
            switch (Application.platform) {
                case RuntimePlatform.Android:
                    path = "jar:file://" + Application.dataPath + "!/assets/TWRPackages/";
                    break;
                case RuntimePlatform.WindowsPlayer:
                    path = "file://" + Application.streamingAssetsPath + "/TWRPackages/";
                    break;
                case RuntimePlatform.WSAPlayerARM:
                    path = "file://" + Application.streamingAssetsPath + "/TWRPackages/";
                    break;
                case RuntimePlatform.WSAPlayerX86:
                    path = "file://" + Application.streamingAssetsPath + "/TWRPackages/";
                    break;
                case RuntimePlatform.WSAPlayerX64:
                    path = "file://" + Application.streamingAssetsPath + "/TWRPackages/";
                    break;
                case RuntimePlatform.WindowsEditor:
                    path = "file://" + Application.streamingAssetsPath + "/TWRPackages/";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    path = "file://" + Application.streamingAssetsPath + "/TWRPackages/";
                    break;
                case RuntimePlatform.OSXPlayer:
                    path = Application.dataPath + "/StreamingAssets/TWRPackages/";
                    break;
                case RuntimePlatform.OSXEditor:
                    path = "file://" + Application.streamingAssetsPath + "/TWRPackages/";
                    break;
            }

            return path;
        }

    }
}