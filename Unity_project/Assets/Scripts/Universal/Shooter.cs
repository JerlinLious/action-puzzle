using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{

    public float startTime = 2f;
    public float timeInterval = 2f;

    public List<Vector2> speeds = new List<Vector2> { new Vector2(0, -50f) };

    public bool isWaiting = false;

    private float restTime = 0f;
    private bool clockFlag = true;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        //开始之前
        if (startTime > Mathf.Epsilon) {
            startTime -= Time.deltaTime;
            return;
        }

        if (isWaiting) {
            //间隔
            if (clockFlag) {
                restTime = timeInterval;
                clockFlag = false;
            }

            if (restTime > 0f) {
                restTime -= Time.deltaTime;
                return;
            }
            isWaiting = false;
            clockFlag = true;
        }
        else {

            for (int i = 0; i < speeds.Count; i++) {
                GameObject arrow = Instantiate(P.Objs["arrow"], transform.position, Quaternion.FromToRotation(Vector3.down, speeds[i]), transform);
                arrow.GetComponent<Rigidbody2D>().velocity = speeds[i];
            }

            isWaiting = true;
        }




    }
}
