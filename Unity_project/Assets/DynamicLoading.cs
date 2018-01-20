using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ShanghaiWindy.AssetLoader;

public class DynamicLoading : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<SceneAssetPrefab>().LoadAsset();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
