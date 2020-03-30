using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class puff : MonoBehaviour
{
    private Rigidbody2D puff_rb;
    private player01 player;

    public float speed, direction;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player01").GetComponent<player01>();
    }

    // Update is called once per frame
    void Update()
    {
        player.strong = false;
        transform.parent = null;
        if (player.left) direction = 1; else direction = -1; //transform.localScale = new Vector2(transform.localScale.x * direction, transform.localScale.y);
        puff_rb = GetComponent<Rigidbody2D>();
        puff_rb.velocity = new Vector2(speed * direction, 0);
        Destroy(this.gameObject, 0.40f);
    }
}
