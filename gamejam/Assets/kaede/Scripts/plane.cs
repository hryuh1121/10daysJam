using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plane : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rig;
    private SpriteRenderer sprR;
    private BoxCollider2D col2d;
    //public GameObject gameObject;

    public float speed = 4;

    public GameObject particle;

    bool Spflag = true;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        sprR = GetComponent<SpriteRenderer>();
        col2d = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Spflag == true)
        {
            rig.velocity = new Vector2(speed, rig.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.name == "Typhoon")
        //{
        //    particle.SetActive(true);
        //    col2d.enabled = false;
        //    sprR.enabled = false;
        //}
    }
}
