using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ShanghaiWindy.AssetBuilider{
    [InitializeOnLoad]
    public static class ToolGuide{
        static ToolGuide() {
            Debug.Log("Up and running");
            bool isTutorial = PlayerPrefs.HasKey("ToolGuide");

            if(!isTutorial){

            }
        }

        [MenuItem("Tools/ShanghaiWindy/Tutorial")]
        static void PopTutorial() {
            Rect wr = new Rect(0, 0, 250, 500);
            ToolGuideGUI window = (ToolGuideGUI)EditorWindow.GetWindowWithRect(typeof(ToolGuideGUI), wr, false, "ToolGuideGUI");
            window.Show();
        }

        public static void BuildTutorial(BuildTarget platform){
            SceneBuilder mySceneBuilder = new SceneBuilder() {
                runtimePlatform = platform
            };

            if (!EditorUtility.DisplayDialog("[Important]Confirm", "Compile Asset on Target Platform:" + mySceneBuilder.runtimePlatform.ToString(), "Yes", "No")) {
                return;
            }

            mySceneBuilder.BuildAssets();
            EditorBuildSettingsScene[] scenes = new EditorBuildSettingsScene[]{
                new EditorBuildSettingsScene(){
                    path = "Assets/SceneBuilderSubAssets/Res/Cooked/Scene/Adventure_Cooked.unity",
                    enabled = true
                },
                new EditorBuildSettingsScene(){
                    path = "Assets/SceneBuilderSubAssets/Res/Cooked/Scene/DemoSimple_Cooked.unity",
                    enabled = true
                }
            };
            EditorBuildSettings.scenes = scenes;
        }
    }

    public class ToolGuideGUI:EditorWindow {
        BuildTarget targetPlatform = BuildTarget.StandaloneWindows;

        private void OnGUI() {
            EditorGUILayout.LabelField("Thanks for purchasing");

            if(!System.IO.Directory.Exists("Assets/StreamingAssets/TWRPackages")){
                EditorGUILayout.HelpBox("You haven't build the assets now",MessageType.Error);
                EditorGUILayout.LabelField("So,do you want us to show you the demo?");
                targetPlatform = (BuildTarget)EditorGUILayout.EnumPopup("Target Platform", targetPlatform);
                if (GUILayout.Button("Yes.Show it to me now!")) {
                    ToolGuide.BuildTutorial(targetPlatform);
                }


            }


        }
    }
}