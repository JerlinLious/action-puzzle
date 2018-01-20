using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    //public GameObject bar;
    //private UpdateBar barClass;

    public float revivedDelay = 2f;


    // Use this for initialization
    void Start() {
        //barClass = bar.GetComponent<UpdateBar>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Platform")) {

            OnDeath();
        }
    }
    public void OnDeath() {
        this.gameObject.SetActive(false);

        P.PUBLIC.Revive(this.gameObject, revivedDelay);
    }

}
