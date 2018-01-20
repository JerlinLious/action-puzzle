using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace ShanghaiWindy.AssetBuilider {
    public class SceneBuilder{
        public BuildTarget runtimePlatform = BuildTarget.StandaloneWindows;

        public List<SceneData> AllSceneData = new List<SceneData>();

        public string SceneDataNames = null;


        public void LoadSceneData() {
            string[] AllSceneDataAssetsInfo = Directory.GetFiles("Assets/SceneBuilderSubAssets/Cooks/Map", "*.asset");
            foreach (string SceneDataAssetPath in AllSceneDataAssetsInfo) {
                SceneData sceneData = AssetDatabase.LoadAssetAtPath(SceneDataAssetPath, typeof(SceneData)) as SceneData;
                if (!AllSceneData.Contains(sceneData)) {
                    SceneDataNames += SceneDataAssetPath + "\n";
                    AllSceneData.Add(sceneData);
                }

            }


            foreach (SceneData sceneData in AllSceneData) {
                Debug.Log(sceneData.SceneObjectReferences.Length);
            }
        }
        public void LabelAssets() {
            List<GameObject> AllCurrentAssets = new List<GameObject>();
            foreach (SceneData sceneData in AllSceneData) {
                foreach (GameObject sceneDataObjects in sceneData.SceneObjectReferences) {
                    AllCurrentAssets.Add(sceneDataObjects);

                }
            }
            foreach (GameObject CurrentAsset in AllCurrentAssets) {
                string Path = AssetDatabase.GetAssetPath(CurrentAsset);
                AssetImporter assetImporter = AssetImporter.GetAtPath(Path);
                string AssetPathToGUID = AssetNameCorretor(AssetDatabase.AssetPathToGUID(Path));
                assetImporter.assetBundleName = AssetPathToGUID;
                assetImporter.assetBundleVariant = "sceneobject";
                assetImporter.SaveAndReimport();
            }
            string[] AssetBundleNames = AssetDatabase.GetAllAssetBundleNames();

            foreach (string AssetBundleName in AssetBundleNames) {
                string[] AllAssetBundlePaths = AssetDatabase.GetAssetPathsFromAssetBundle(AssetBundleName);
                foreach (string AssetBundlePath in AllAssetBundlePaths) {
                    GameObject PreviousAsset = AssetDatabase.LoadAssetAtPath(AssetBundlePath, typeof(GameObject)) as GameObject;
                    if (!AllCurrentAssets.Contains(PreviousAsset)) {
                        AssetImporter assetImporter = AssetImporter.GetAtPath(AssetBundlePath);
                        string AssetPathToGUID = AssetNameCorretor(AssetDatabase.AssetPathToGUID(AssetBundlePath));
                        if (assetImporter.assetBundleVariant != "sceneobject") {
                            continue;
                        }
                        assetImporter.assetBundleName = null;
                        assetImporter.assetBundleVariant = null;
                        assetImporter.SaveAndReimport();
                    }
                }
            }
        }
        public void BuildAssets() {
            if (!EditorUtility.DisplayDialog("[Important]Confirm", "Compile Asset on Target Platform:" + runtimePlatform.ToString(), "Yes", "No")) {
                return;
            }

            DirectoryInfo folder = new DirectoryInfo("Assets/SceneBuilderSubAssets/res/Cooked/.packages/");
            if (!folder.Exists) {
                folder.Create();
            }

            AssetBundleManifest assetbundleMainifest = BuildPipeline.BuildAssetBundles("Assets/SceneBuilderSubAssets/res/Cooked/.packages", BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle, runtimePlatform);
            Hashtable Info = new Hashtable();

            foreach (FileInfo file in folder.GetFiles("*.sceneobject")) {
                List<string> Details = new List<string>();
                Details.Add(GetMD5HashFromFile(file.FullName));
                Info.Add(file.Name, Details);
            }

            StreamWriter streamWriter = new StreamWriter(folder + "md5.checker");
            streamWriter.Write(ShanghaiWindy.AssetLoader.Json.Json.Serialize(Info));
            streamWriter.Close();

            if (!EditorUtility.DisplayDialog("Copy", "Copy cooked assets to Streamassets Folder", "Yes", "No")) {
                return;
            }


            string original_path = "Assets/SceneBuilderSubAssets/res/Cooked/.packages/";
            string search_path = Application.streamingAssetsPath + "/TWRPackages";

            DirectoryInfo cookedDataDircetory = new DirectoryInfo("Assets/SceneBuilderSubAssets/res/Cooked/.packages/");
            DirectoryInfo TargetDir = new DirectoryInfo(Application.streamingAssetsPath + "/TWRPackages");

            if (!TargetDir.Exists) {
                TargetDir.Create();
            }

            AssetDatabase.RemoveUnusedAssetBundleNames();


            for (int i = 0; i < AssetDatabase.GetAllAssetBundleNames().Length; i++) {
                string source = original_path + "/" + AssetDatabase.GetAllAssetBundleNames()[i];
                string target = search_path + "/" + AssetDatabase.GetAllAssetBundleNames()[i];


                if (!File.Exists(target) || (GetMD5HashFromFile(target) != GetMD5HashFromFile(source))) {
                    File.Copy(source, target, true);
                }

            }
            File.Copy(original_path + "/.packages", search_path + "/main.packages", true);
        }
        internal string AssetNameCorretor(string str) {
            return str.Replace(" ", "").ToLower();

        }

        internal string GetMD5HashFromFile(string fileName) {
            try {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++) {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (System.Exception ex) {
                Debug.Log(ex.Source);
                return null;
            }
        }
    }

    class SceneBuilderGUI : EditorWindow {
        public static SceneBuilder sceneBuilder = new SceneBuilder();


        [MenuItem("Tools/ShanghaiWindy/Build/SceneBuilder")]
        static void Init() {
            Rect wr = new Rect(0, 0, 250, 500);
            SceneBuilderGUI window = (SceneBuilderGUI)EditorWindow.GetWindowWithRect(typeof(SceneBuilderGUI), wr, false, "SceneBuilder");
            window.Show();
            if (PlayerPrefs.HasKey("TargetPlatform")) {
                sceneBuilder.runtimePlatform = (BuildTarget)PlayerPrefs.GetInt("TargetPlatform");
            }
            sceneBuilder.LoadSceneData();
        }



        private void OnGUI() {
            if (PlayerPrefs.HasKey("TargetPlatform")) {
                sceneBuilder.runtimePlatform = (BuildTarget)PlayerPrefs.GetInt("TargetPlatform");
            }

            EditorGUILayout.HelpBox("The tool will build all assets serialized into Assetbundles separately. ", MessageType.None);

            EditorGUILayout.HelpBox("[Important]!!! Select target platform you want to build assets! ", MessageType.Error);
            sceneBuilder.runtimePlatform = (BuildTarget)EditorGUILayout.EnumPopup("Target Platform", sceneBuilder.runtimePlatform);
            PlayerPrefs.SetInt("TargetPlatform", (int)sceneBuilder.runtimePlatform);

            EditorGUILayout.HelpBox("Click 1,2,3 in order to build assets!", MessageType.Info);

            EditorGUILayout.HelpBox("AssetBundle Settings ", MessageType.None);

            EditorGUILayout.HelpBox(string.Format("Scene Data Count: {0} Maps:{1}", sceneBuilder.AllSceneData.Count.ToString(), sceneBuilder.SceneDataNames),MessageType.None);


            if (GUILayout.Button("1-Reload  Cooked Scene Data "))
            {
                sceneBuilder.LoadSceneData();
            }
            if (GUILayout.Button("2-Label Assets"))
            {
                sceneBuilder.LabelAssets();
            }
            if (GUILayout.Button("3-Build Sub-Assets")) {
                sceneBuilder.BuildAssets();
            }

            
        }



    }
}
