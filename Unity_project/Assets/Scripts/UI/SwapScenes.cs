using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapScenes : MonoBehaviour {

    public GameObject s1, s2;

    private void Start() {
        s1 = GameObject.Find("left");
        s2 = GameObject.Find("right");

        if (s1 == null || s2 == null) {
            P.WarningPrint(transform, this.GetType().ToString());
            return;
        }
    }


    public void Swap() {
        

        Vector3 t = s1.transform.position;
        s1.transform.position = s2.transform.position;
        s2.transform.position = t;
    }
}
