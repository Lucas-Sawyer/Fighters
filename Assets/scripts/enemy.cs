using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemy : MonoBehaviour
{
    public float heath = 100, mana = 100, gravity_speed, push_max, combo_t_max;
    public LayerMask damage_mask;

    private Slider life_bar, mana_bar;
    private GameObject player_ob, col_check_player;
    private bool left, right, facing_right = true, grounded, pushed, take_damage;
    private CharacterController controller;
    private Vector2 direction;
    private float push_time, push_x, push_y, combo, combo_t_time;
    private Text combo_t;
    // Start is called before the first frame update
    void Start()
    {
        player_ob = GameObject.Find("player01");
        controller = GetComponent<CharacterController>();
        combo_t = GameObject.Find("combo (1)").GetComponent<Text>();
        take_damage = Physics2D.OverlapCircle(transform.position, 0, damage_mask);
    }

    // Update is called once per frame
    void Update()
    {
        //Damage===============================================================Damage
        if (take_damage)
        {
        }
        //Damage===============================================================Damage

        //HUD=====================================================================HUD
        life_bar = GameObject.Find("life bar enemy").GetComponent<Slider>();
        mana_bar = GameObject.Find("mana bar enemy").GetComponent<Slider>();
        life_bar.value = heath;
        mana_bar.value = mana;
        if (combo != 0)
        {
            combo_t.text = "" + combo;
            combo_t_time++;
            if (combo_t_time >= combo_t_max)
            {
                combo = 0;
                combo_t_time = 0;
            }
        }
        else
        {
            combo_t.text = null;
        }
        //HUD=====================================================================HUD

        //Flip check=======================================================Flip check
        if (player_ob.transform.position.x > transform.position.x)
        {
            left = true;
            right = false;
        }
        else
        {
            left = false;
            right = true;
        }
        if (left && !facing_right)
        {
            flip();
        }
        if (right && facing_right)
        {
            flip();
        }
        //Flip check=======================================================Flip check

        //Gravity/Jump===================================================Gravity/Jump
        grounded = Physics2D.OverlapCircle(GameObject.Find("ground_check_enemy").transform.position, 0);
        if (!grounded)
        {
            direction.y -= gravity_speed * Time.deltaTime;
        }
        else
        {
            direction.y = 0;
        }
        //Gravity/Jump===================================================Gravity/Jump

        //Push controller=============================================Push controller
        if (pushed)
        {
            push_time++;
            if (player_ob.GetComponent<player01>().left)
            {
                direction.x = push_x;
                direction.y = push_y;
            }
            else
            {
                direction.x = push_x * -1;
            }
            if (push_time >= push_max)
            {
                pushed = false;
                push_x = 0;
                push_y = 0;
                direction.y = 0;
                direction.x = 0;
                push_time = 0;

            }
        }
        //Push controller=============================================Push controller

        //Movement===========================================================Movement
        controller.Move(direction * Time.deltaTime);
        //Movement===========================================================Movement

    }
    public void Take_damage(float damage_taken, float distance_x, float distance_y)
    {
        heath -= damage_taken;
        if (distance_x != 0 || distance_y != 0)
        {
            print(distance_x);
            print(distance_y);
            pushed = true;
            push_x = distance_x;
            push_y = distance_y;
        }
    }

    public void flip()
    {
        facing_right = !facing_right;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }
}
