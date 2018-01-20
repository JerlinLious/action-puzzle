using UnityEngine;

using UScene = UnityEngine.SceneManagement;

namespace ShanghaiWindy.AssetLoader {
    class DemoLoader : MonoBehaviour {
        public GameObject canvasObj;

        void Awake() {
            SceneLoader.OnLoadingNewScenePrefab += OnLoadingNewScenePrefab;
            SceneLoader.OnStartLoadingScenePrefabs += OnStartLoadingScenePrefabs;

        }
        void OnDestroy() {
            SceneLoader.OnLoadingNewScenePrefab -= OnLoadingNewScenePrefab;
            SceneLoader.OnStartLoadingScenePrefabs -= OnStartLoadingScenePrefabs;
        }

        void OnStartLoadingScenePrefabs(int AssetsCount) {
            Debug.Log("You have to load" + AssetsCount + "Assets to open the scene");
        }
        void OnLoadingNewScenePrefab(string currentLoadingAssetName, int currentLoadingAssetOrder) {
            Debug.Log("Loading:" + currentLoadingAssetName + "Order:" + currentLoadingAssetOrder);
        }

        void Start() {
            DontDestroyOnLoad(this.gameObject); //Coroutine need this gameobject to keep active! 

            //Set the object which coroutine runs on.
            SubBuild_AssetBundleManager.Init(this);


            StartCoroutine(SubBuild_AssetBundleManager.AssetBundleLoop());

            //Get Dependence AssetBundle Info


        }

        public void ToSimpleDemo(){
            StartCoroutine(SubBuild_AssetBundleManager.GetAssetBundleInfos(() => {
                StartCoroutine(SceneLoader.RequestScene("DemoSimple", (onLoaded) => {
                    //On loading is finish,the following scripts will run.
                    Debug.Log("Scene is Loaded!");
                    canvasObj.SetActive(false);
                }));
            }));
        }

        public void ToComplexAdventure(){
            StartCoroutine(SubBuild_AssetBundleManager.GetAssetBundleInfos(() => {
                UScene.SceneManager.LoadScene("Adventure_Cooked");
                SubBuild_AssetBundleManager.LoadAssetFromAssetBundle(new AssetRequestTask(){
                    currentAssetName = "player.actor",
                    onAssetLoaded = (myCharacter)=>{
                        GameObject player = Instantiate<GameObject>((GameObject)myCharacter);
                        player.SetActive(false);

                        GameObject.Find("Sets/Ground_CookedResource").GetComponent<SceneAssetPrefab>().onFinish = () => {
                            player.SetActive(true);
                        };

                        GameObject.Find("Sets/Ground_CookedResource").GetComponent<SceneAssetPrefab>().LoadAsset();

                        canvasObj.SetActive(false);
                    }
                });
            }));
        }
    }
}
