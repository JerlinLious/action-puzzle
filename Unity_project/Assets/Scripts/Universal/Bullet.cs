using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collider2D) {
        if (collider2D.gameObject.layer == LayerMask.NameToLayer("Platform")) {
            Destroy(this.gameObject);
        }
        else if (collider2D.tag == "Player") {
            collider2D.gameObject.GetComponent<PlayerDeath>().OnDeath();
        }
    }


}
