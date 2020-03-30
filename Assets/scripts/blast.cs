using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blast : MonoBehaviour
{
    public Rigidbody2D blast_rb;
    public float speed, direction;
    private player01 player;

    IEnumerator Start()
    {
        player = GameObject.Find("player01").GetComponent<player01>();
        if (player.left) direction = 1; else direction = -1;
        transform.parent = null;
        blast_rb = GetComponent<Rigidbody2D>();
        yield return new WaitForSeconds(0.18f);
        player.strong = false;
        blast_rb.velocity = new Vector2(speed * direction, blast_rb.velocity.y);
        Destroy(this.gameObject, 1);
    }
}
