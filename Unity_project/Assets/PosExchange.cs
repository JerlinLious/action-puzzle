using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosExchange : MonoBehaviour {
    public GameObject target1;
    public GameObject target2;
    public float time;
    float distance;
	// Use this for initialization
	void Start () {
        distance = (target2.transform.position - transform.position).magnitude;
    }
	
	// Update is called once per frame
	void Update () {

        transform.position = Vector3.MoveTowards(transform.position, target2.transform.position, distance*(Time.deltaTime/time));
	}
}
