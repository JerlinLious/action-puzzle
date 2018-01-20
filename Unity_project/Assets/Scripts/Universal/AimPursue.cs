using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimPursue : MonoBehaviour
{

    public float checkRadius = 12f;

    public float deathDistance = 0.75f;

    public float checkInterval = 0.25f;

    public float maxSpeed = 5f;

    public float aimTime = 2.4f;



    private Transform target = null;
    private float restTime = 0f;
    private float sqrDistance;
    private int aimCnt = 0;
    private int maxAimCnt;

    private Vector2 currVelocity = Vector2.zero;


    // Use this for initialization
    void Start() {
        restTime = checkInterval;
        sqrDistance = deathDistance * deathDistance;
        maxAimCnt = (int)(aimTime / Time.deltaTime);
    }

    // Update is called once per frame
    void Update() {
        if (target != null) {
            //Pursue the target
            transform.position = Vector2.SmoothDamp(transform.position, target.position, ref currVelocity, 0.3f, maxSpeed, Time.deltaTime);

            if (Vector2.SqrMagnitude(transform.position - target.position) < sqrDistance) {
                aimCnt++;
            }
            else aimCnt = 0;

            if (aimCnt > maxAimCnt) {
                target.GetComponent<PlayerDeath>().OnDeath();
            }
        }


        if (restTime > 0f) {
            restTime -= Time.deltaTime;
            return;
        }

        restTime = checkInterval;

        bool hasTarget = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, checkRadius);
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].tag == "Player") {
                target = colliders[i].transform;
                hasTarget = true;
                break;
            }
        }

        if (hasTarget == false) target = null;

    }
}
