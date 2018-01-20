using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShanghaiWindy.AssetLoader;

public class DemoDynamicLoading : MonoBehaviour {
    public float distance = 5;

    private SceneAssetPrefab[] allSceneAssets;

	void Start () {
        allSceneAssets = FindObjectsOfType<SceneAssetPrefab>();

        InvokeRepeating("check",1,1);
	}

    void check(){
        for (int i = 0; i < allSceneAssets.Length;i++){
            if (allSceneAssets[i].inLoading || allSceneAssets[i].LoadingDone)
                continue;
            
            if(Vector3.Distance(transform.position,allSceneAssets[i].transform.position)<distance){
                allSceneAssets[i].LoadAsset();
            }
        }
    }
}
