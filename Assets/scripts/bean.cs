using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bean : MonoBehaviour
{
    private LineRenderer line;
    private Vector3 start_pos;
    private int pos_count, pos_time, direction;
    private GameObject end_pos;
    private player01 player;

    public int pos_max;
    public float damage_done;
    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        player = GameObject.Find("player01").GetComponent<player01>();
    }

    // Update is called once per frame
    void Update()
    {
        start_pos = GameObject.Find("kamehameha pos").transform.position;
        end_pos = GameObject.Find("kamehameha_bean_end");
        pos_time++;
        if (player.left) direction = 1; else direction = -1;
        if (pos_time >= pos_max)
        {
            pos_count++;
            line.positionCount = pos_count;
            pos_time = 0;
        }
        for (int i = 0; i < pos_count; i++)
        {
            line.enabled = true;
            line.SetPosition(i, new Vector3(start_pos.x + i * direction, start_pos.y, start_pos.z));
            if (i > 2)
            {
                if (player.left) end_pos.transform.position = new Vector3(start_pos.x + i * direction - 0.5f, start_pos.y, start_pos.z);
                else end_pos.transform.position = new Vector3(start_pos.x + i * direction + 0.5f, start_pos.y, start_pos.z);
            }
        }
        Destroy(this.gameObject, 0.8f);
    }
}
