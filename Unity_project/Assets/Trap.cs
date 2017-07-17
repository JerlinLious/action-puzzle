using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {
    public GameObject spawnPoint;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("success");
            Pawn_move pawn = new Pawn_move();
            collision.gameObject.GetComponent<Pawn_move>().Death();
            GameObject Resapwn = Instantiate(collision.gameObject);
            Resapwn.transform.localPosition = spawnPoint.transform.localPosition;
            
        }


    }
}
