using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hud : MonoBehaviour
{
    private LineRenderer line;
    private player01 player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player01").GetComponent<player01>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
