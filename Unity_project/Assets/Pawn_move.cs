using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn_move : MonoBehaviour {
    private Rigidbody2D R2D;
    public float speed;
    public bool jumping = false;
    public bool can_move = true;
    // Use this for initialization
    private void Awake()
    {
        R2D = GetComponent<Rigidbody2D>();
        can_move = true;
        tag = "Player";
    }
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (can_move)
        {
            R2D.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, R2D.velocity.y);
            if (Input.GetAxis("Jump") == 1)
                if (!jumping)
                {
                    R2D.AddForce(new Vector2(0, 50));
                    jumping = true;

                }
        }
	}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        jumping = false;
    }

    public void Death()
    {
        tag = "Ghost";
        can_move = false;
        Debug.Log("success");
    }
}
