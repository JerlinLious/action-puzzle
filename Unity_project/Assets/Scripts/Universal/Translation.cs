using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Translation : MonoBehaviour
{

    public float startTime = 2f;
    public float timeInterval = 2f;
    public float translateTime = 4f;
    public Vector2 targetVector = Vector2.zero;
    public bool isWaiting = false;

    public Ease normal = Ease.Linear;
    public Ease reverse = Ease.Linear;

    private float direction = 1f;
    private float restTime = 0f;
    private bool clockFlag = true;


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    private void FixedUpdate() {

        //开始之前
        if (startTime > 0.0001f) {
            startTime -= Time.fixedDeltaTime;
            return;
        }

        if (isWaiting) {
            //间隔
            if (clockFlag) {
                restTime = timeInterval;
                clockFlag = false;
            }

            if (restTime > 0f) {
                restTime -= Time.fixedDeltaTime;
                return;
            }
            isWaiting = false;
            clockFlag = true;
        }
        else {
            //变换        
            if (clockFlag) {
                restTime = translateTime;
                transform.DOMove(transform.position + (Vector3)targetVector * direction, translateTime).SetEase<Tween>(direction > 0f ? normal : reverse);
                clockFlag = false;
                direction *= -1f;
            }

            if (restTime > 0f) {
                restTime -= Time.fixedDeltaTime;
                return;
            }
            isWaiting = true;
            clockFlag = true;
        }
    }
}
