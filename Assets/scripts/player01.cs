using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class player01 : MonoBehaviour
{
    public float walk_speed, skill_max, gravity_speed, jump_force, jump_max, dash_force, aura_time, combo_max,
    damage_range, combo_t_max,
    combo04_max, strong_up_max, sequence_max, combo_kaioken_max, combo_kaioken_radius, combo_kaioken_speed, regen_mana,
    heath = 100, mana = 100;
    public GameObject charge_aura, blast_ob, puff_ob, kamehameha_ob, enemy_ob, col_check_enemy;
    public bool can_walk, strong, skill, left, right;
    public LayerMask enemy_mask;

    private Animator animator;
    private CharacterController controller;
    private Vector2 direction;
    private float skill_time, jump_time, dash_direction, charge_time, combo_count, combo_time, damage_done, combo_t_time,
    combo04_time, strong_up_time, up_count, front_count, back_count, down_count, push_y, push_x, sequence_time, combo_kaioken_time,
    combo;
    private string key_name, last_key, skill_name, attack;
    private bool grounded, walking, jumping, falling, dashing, charging, combing, combo04, blast, puff, strong_up, kamehameha,
    combo_kaioken, hit_combo_kaioken, facing_right = true, deal_damage, hit, kamehameha_kaioken;
    private Slider life_bar, mana_bar;
    private Text combo_t;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        col_check_enemy = GameObject.Find("col check enemy");
        combo_t = GameObject.Find("combo").GetComponent<Text>();
    }

    // Update*****************************************************************Update
    void Update()
    {
        enemy_ob = GameObject.Find("enemy");
        //HUD=====================================================================HUD
        life_bar = GameObject.Find("life bar").GetComponent<Slider>();
        mana_bar = GameObject.Find("mana bar").GetComponent<Slider>();
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
        if (enemy_ob.transform.position.x > transform.position.x)
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

        //Key Check=========================================================Key Check
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (can_walk || charging) dash_direction = -1;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (can_walk || charging) dash_direction = 1;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            sequence_time = 0;
            switch (last_key)
            {
                case "back":
                    skill_name = "kamehameha";
                    break;
            }
            if (right)
            {
                front_count++;
                last_key = "front";
                if (down_count == 2)
                {
                    skill_name = "combo kaioken";
                }
            }
            if (left)
            {
                back_count++;
                last_key = "back";
            }
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            sequence_time = 0;
            switch (last_key)
            {
                case "back":
                    skill_name = "kamehameha";
                    break;
            }
            if (right)
            {
                back_count++;
                last_key = "back";
            }
            if (left)
            {
                front_count++;
                last_key = "front";
                if (down_count == 2)
                {
                    skill_name = "combo kaioken";
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            last_key = "up";
            up_count++;
            sequence_time = 0;
            if (down_count == 2 && back_count == 2 && front_count == 2)
            {
                skill_name = "transformation";
            }
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            last_key = "down";
            down_count++;
            sequence_time = 0;
        }
        //Key Check=========================================================Key Check

        //Gravity/Jump===================================================Gravity/Jump
        grounded = Physics2D.OverlapCircle(GameObject.Find("ground_check").transform.position, 0.2f);
        if (!grounded)
        {
            if (!combing) direction.y -= gravity_speed * Time.deltaTime;
            if (!jumping && !strong_up)
            {
                falling = true;
            }
        }
        else
        {
            direction.y = 0;
            falling = false;
            if (Input.GetKeyUp(KeyCode.Space))
            {
                direction.y = jump_force;
                if (!charging) jumping = true;
            }
        }
        //Jump time---------------------------------------------------------Jump time
        if (jumping)
        {
            jump_time++;
            if (jump_time >= jump_max)
            {
                jumping = false;
                jump_time = 0;
            }
        }
        //Jump time---------------------------------------------------------Jump time
        //Gravity/Jump===================================================Gravity/Jump

        //Damage controller=========================================Damage controller
        if (deal_damage)
        {
            hit = Physics2D.OverlapCircle(GameObject.Find("range player").transform.position, damage_range, enemy_mask);
            if (hit)
            {
                combo++;
                combo_t_time = 0;
                enemy_ob.GetComponent<enemy>().Take_damage(damage_done, push_x, push_y);
            }
            deal_damage = false;
            push_x = 0;
            push_y = 0;
        }
        else hit = false;
        //Damage controller=========================================Damage controller

        //Skill Controller===========================================Skill Controller
        if (skill_name != null)
        {
            skill_time++;
            if (skill_time >= skill_max)
            {
                skill_time = 0;
                skill_name = null;
            }
        }
        if (back_count != 0 || front_count != 0 || down_count != 0 || up_count != 0)
        {
            sequence_time++;
            if (sequence_time >= sequence_max)
            {
                sequence_time = 0;
                back_count = 0;
                front_count = 0;
                up_count = 0;
                down_count = 0;
                last_key = null;
            }
        }
        //Skill Controller===========================================Skill Controller

        //Combo=================================================================Combo
        if (Input.GetKeyUp(KeyCode.Z))
        {
            combo_count++;
            combo_time = 0;
        }
        if (combo_count != 0)
        {
            jumping = false;
            falling = false;
            combing = true;
            combo_time++;
            if (combo_time >= combo_max)
            {
                combo_time = 0;
                combing = false;
                combo_count = 0;
            }
        }
        if (combo04)
        {
            combo04_time++;
            if (combo04_time >= combo04_max)
            {
                combo04_time = 0;
                combo04 = false;
            }
            if (left) controller.Move(new Vector3(10, 0, 0) * Time.deltaTime);
            if (right) controller.Move(new Vector3(-10, 0, 0) * Time.deltaTime);
        }
        //Combo=================================================================Combo

        //Strong===============================================================Strong
        if (Input.GetKeyUp(KeyCode.X))
        {
            strong = true;
            switch (last_key)
            {
                case null:
                    animator.Play("strong_neutral");
                    break;
                case "front":
                    animator.Play("strong_side");
                    break;
                case "back":
                    animator.Play("strong_side");
                    break;
                case "up":
                    animator.Play("strong_up");
                    break;
                case "down":
                    animator.Play("strong_down");
                    break;
            }
        }
        if (blast)
        {
            Instantiate(blast_ob, GameObject.Find("blast pos").transform);
            blast = false;
        }
        if (puff)
        {
            Instantiate(puff_ob, GameObject.Find("puff pos").transform);
            puff = false;
        }
        if (strong_up)
        {
            falling = false;
            strong_up_time++;
            if (strong_up_time >= strong_up_max)
            {
                strong_up_time = 0;
                strong_up = false;
                strong = false;
                falling = true;
            }
            controller.Move(new Vector3(0, 3, 0) * Time.deltaTime);
        }
        //Strong===============================================================Strong

        //Charge/Skills==================================================Charge/Skills
        if (Input.GetKeyDown(KeyCode.C))
        {
            int custo;
            switch (skill_name)
            {
                case null:
                    break;
                case "kamehameha":
                    custo = 30;
                    if (custo <= mana)
                    {
                        animator.Play("kamehameha");
                        mana -= custo;
                        skill = true;
                    }
                    else skill = false;
                    break;
                case "combo kaioken":
                    custo = 50;
                    if (custo <= mana)
                    {
                        combo_kaioken = true;
                        mana -= custo;
                    }
                    else skill = false;
                    break;
                case "transformation":
                    animator.Play("transformation");
                    break;
            }
        }
        if (kamehameha)
        {
            Instantiate(kamehameha_ob, GameObject.Find("kamehameha pos").transform);
            kamehameha = false;
        }
        if (kamehameha_kaioken)
        {
            Instantiate(kamehameha_ob, GameObject.Find("kamehameha pos").transform);
            kamehameha_kaioken = false;
        }
        if (combo_kaioken)
        {
            hit_combo_kaioken = Physics2D.OverlapCircle(transform.position, combo_kaioken_radius, enemy_mask);
            if (!hit_combo_kaioken)
            {
                direction.x = combo_kaioken_speed * dash_direction;
                combo_kaioken_time++;
                if (combo_kaioken_time >= combo_kaioken_max)
                {
                    combo_kaioken = false;
                    combo_kaioken_time = 0;
                }
            }
            else
            {
                direction.x = 0;
            }
        }
        if (Input.GetKey(KeyCode.C))
        {
            if (grounded && !skill && !combo_kaioken) charging = true; else charging = false;
            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) && !dashing) dashing = true;
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            charging = false;
            charge_time = 0;
            dashing = false;
            if (GameObject.Find("charge aura(Clone)")) Destroy(GameObject.Find("charge aura(Clone)"));
        }
        if (charging)
        {
            if (!dashing)
            {
                if (mana < 100) mana += regen_mana * Time.deltaTime; else mana = 100;
            }
            charge_time++;
            if (charge_time >= aura_time)
            {
                if (!GameObject.Find("charge aura(Clone)")) Instantiate(charge_aura, GameObject.Find("aura pos").transform);
            }
        }
        //Charge/Skills==================================================Charge/Skills

        //Dash===================================================================Dash
        if (dashing)
        {
            charging = false;
            direction.x = dash_force * dash_direction;
            if (GameObject.Find("charge aura(Clone)")) Destroy(GameObject.Find("charge aura(Clone)"));
            if (left && direction.x < 0) flip();
            if (right && direction.x > 0) flip();
            mana -= regen_mana * Time.deltaTime;
        }
        //Dash===================================================================Dash

        //Movement===========================================================Movement
        if (GameObject.Find("puff(Clone)") || charging || combing || strong || skill) can_walk = false; else can_walk = true;
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            if (!dashing && !combo_kaioken) direction.x = 0;
            walking = false;
        }
        else
        {
            if (!dashing && !combo_kaioken)
            {
                direction.x = Input.GetAxisRaw("Horizontal") * walk_speed;
                walking = true;
                if (left && direction.x < 0) flip();
                if (right && direction.x > 0) flip();
            }
        }
        if (can_walk) controller.Move(direction * Time.deltaTime);
        //Movement===========================================================Movement

        //Check animation clip number===================//Check animation clip number
        /*for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++)
        {
            print(i + ": " + animator.runtimeAnimatorController.animationClips[i]);
        }*/
        //Check animation clip number===================//Check animation clip number

        //Animator controller=====================================Animator controller
        animator.SetFloat("dash_direction", dash_direction);
        animator.SetFloat("combo_count", combo_count);

        animator.SetBool("walking", walking);
        animator.SetBool("falling", falling);
        animator.SetBool("jumping", jumping);
        animator.SetBool("dashing", dashing);
        animator.SetBool("charging", charging);
        animator.SetBool("combo kaioken", combo_kaioken);
        animator.SetBool("hit combo kaioken", hit_combo_kaioken);

        for (int i = 0; i < animator.runtimeAnimatorController.animationClips.Length; i++)
        {
            AnimationEvent e;
            AnimationEvent e2;
            AnimationEvent e3;
            switch (i)
            {
                case 5:
                    e = new AnimationEvent();
                    e.functionName = "Range";
                    e.time = 0.25f;
                    e.stringParameter = "combo01";
                    animator.runtimeAnimatorController.animationClips[i].AddEvent(e);
                    break;
                case 6:
                    e = new AnimationEvent();
                    e.functionName = "Range";
                    e.time = 0.3334f;
                    e.stringParameter = "combo02_1";
                    animator.runtimeAnimatorController.animationClips[i].AddEvent(e);
                    e2 = new AnimationEvent();
                    e2.functionName = "Range";
                    e2.time = 0.5f;
                    e2.stringParameter = "combo02_2";
                    animator.runtimeAnimatorController.animationClips[i].AddEvent(e2);
                    e3 = new AnimationEvent();
                    e3.functionName = "Range";
                    e3.time = 0.66661f;
                    e3.stringParameter = "combo02_3";
                    animator.runtimeAnimatorController.animationClips[i].AddEvent(e3);
                    break;
                case 7:
                    e = new AnimationEvent();
                    e.functionName = "Range";
                    e.time = 0.5f;
                    e.stringParameter = "combo03_1";
                    animator.runtimeAnimatorController.animationClips[i].AddEvent(e);
                    e2 = new AnimationEvent();
                    e2.functionName = "Range";
                    e2.time = 0.833f;
                    e2.stringParameter = "combo03_2";
                    animator.runtimeAnimatorController.animationClips[i].AddEvent(e2);
                    break;
                case 8:
                    e = new AnimationEvent();
                    e.time = 0.40f;
                    e.functionName = "Combo04";
                    animator.runtimeAnimatorController.animationClips[i].AddEvent(e);
                    e2 = new AnimationEvent();
                    e2.time = 0.34f;
                    e2.functionName = "Range";
                    e2.stringParameter = "combo04";
                    animator.runtimeAnimatorController.animationClips[i].AddEvent(e2);
                    break;
                case 9:
                    e = new AnimationEvent();
                    e.time = 0.40f;
                    e.functionName = "Blast";
                    animator.runtimeAnimatorController.animationClips[i].AddEvent(e);
                    e2 = new AnimationEvent();
                    e2.functionName = "Range";
                    e2.stringParameter = "blast";
                    animator.runtimeAnimatorController.animationClips[i].AddEvent(e2);
                    break;
                case 10:
                    e = new AnimationEvent();
                    e.time = 0.40f;
                    e.functionName = "Puff";
                    animator.runtimeAnimatorController.animationClips[i].AddEvent(e);
                    e2 = new AnimationEvent();
                    e2.functionName = "Range";
                    e2.stringParameter = "puff";
                    animator.runtimeAnimatorController.animationClips[i].AddEvent(e2);
                    break;
                case 12:
                    e = new AnimationEvent();
                    e.time = 0.40f;
                    e.functionName = "Strong_down_finish";
                    animator.runtimeAnimatorController.animationClips[i].AddEvent(e);
                    e2 = new AnimationEvent();
                    e2.functionName = "Range";
                    e2.stringParameter = "strong down";
                    animator.runtimeAnimatorController.animationClips[i].AddEvent(e2);
                    break;
                case 13:
                    e = new AnimationEvent();
                    e.time = 0.40f;
                    e.functionName = "Strong_up";
                    animator.runtimeAnimatorController.animationClips[i].AddEvent(e);
                    e2 = new AnimationEvent();
                    e2.functionName = "Range";
                    e2.stringParameter = "strong up";
                    animator.runtimeAnimatorController.animationClips[i].AddEvent(e2);
                    break;
                case 14:
                    e = new AnimationEvent();
                    e.time = 0.75f;
                    e.functionName = "Kamehameha";
                    animator.runtimeAnimatorController.animationClips[i].AddEvent(e);
                    e2 = new AnimationEvent();
                    e2.time = 1.66f;
                    e2.functionName = "Stop_skill";
                    animator.runtimeAnimatorController.animationClips[i].AddEvent(e2);
                    break;
                case 16:
                    e = new AnimationEvent();
                    e.time = 2.8f;
                    e.functionName = "Finish_combo_kaioken";
                    animator.runtimeAnimatorController.animationClips[i].AddEvent(e);
                    e2 = new AnimationEvent();
                    e2.time = 3.58f;
                    e2.functionName = "Kamehameha_kaioken";
                    animator.runtimeAnimatorController.animationClips[i].AddEvent(e2);
                    break;
            }
        }
        //Animator controller=====================================Animator controller
    }
    // Update*****************************************************************Update

    public void Combo04()
    {
        combo04 = true;
    }
    public void Blast()
    {
        blast = true;
    }
    public void Puff()
    {
        puff = true;
    }
    public void Strong_up()
    {
        strong_up = true;
    }
    public void Kamehameha()
    {
        kamehameha = true;
    }
    public void Stop_skill()
    {
        skill = false;
    }
    public void Kamehameha_kaioken()
    {
        kamehameha_kaioken = true;
    }
    public void Finish_combo_kaioken()
    {
        combo_kaioken = false;
        hit_combo_kaioken = false;
    }
    public void Strong_down_finish()
    {
        strong = false;
    }
    public void Deal_damage()
    {
        deal_damage = true;
    }
    public void Range(string attack)
    {
        switch (attack)
        {
            case "combo01":
                if (left) GameObject.Find("range player").transform.position = new Vector2(transform.position.x + 1.549f, transform.position.y + 0.153f);
                if (right) GameObject.Find("range player").transform.position = new Vector2(transform.position.x - 1.549f, transform.position.y + 0.153f);
                damage_done = 0.3f;
                damage_range = 0;
                deal_damage = true;
                break;
            case "combo02_1":
                if (left) GameObject.Find("range player").transform.position = new Vector2(transform.position.x + 1.287f, transform.position.y + 0.033f);
                if (right) GameObject.Find("range player").transform.position = new Vector2(transform.position.x - 1.287f, transform.position.y + 0.033f);
                damage_done = 0.5f;
                damage_range = 0.2f;
                deal_damage = true;
                break;
            case "combo02_2":
                if (left) GameObject.Find("range player").transform.position = new Vector2(transform.position.x + 1.3365f, transform.position.y - 0.6325f);
                if (right) GameObject.Find("range player").transform.position = new Vector2(transform.position.x - 1.3365f, transform.position.y - 0.6325f);
                damage_done = 0.5f;
                damage_range = 0.2f;
                deal_damage = true;
                break;
            case "combo02_3":
                if (left) GameObject.Find("range player").transform.position = new Vector2(transform.position.x + 1.2595f, transform.position.y - 0.8745f);
                if (right) GameObject.Find("range player").transform.position = new Vector2(transform.position.x - 1.2595f, transform.position.y - 0.8745f);
                damage_done = 0.7f;
                damage_range = 0.5f;
                deal_damage = true;
                break;
            case "combo03_1":
                if (left) GameObject.Find("range player").transform.position = new Vector2(transform.position.x + 1.397f, transform.position.y - 0.968f);
                if (right) GameObject.Find("range player").transform.position = new Vector2(transform.position.x - 1.397f, transform.position.y - 0.968f);
                damage_done = 0.75f;
                damage_range = 0.5f;
                deal_damage = true;
                break;
            case "combo03_2":
                if (left) GameObject.Find("range player").transform.position = new Vector2(transform.position.x + 1.397f, transform.position.y - 0.968f);
                if (right) GameObject.Find("range player").transform.position = new Vector2(transform.position.x - 1.397f, transform.position.y - 0.968f);
                damage_done = 0.75f;
                damage_range = 0.5f;
                deal_damage = true;
                break;
            case "combo04":
                if (left) GameObject.Find("range player").transform.position = new Vector2(transform.position.x + 1.4245f, transform.position.y + 0.308f);
                if (right) GameObject.Find("range player").transform.position = new Vector2(transform.position.x - 1.4245f, transform.position.y + 0.308f);
                damage_done = 1.5f;
                damage_range = 0.5f;
                deal_damage = true;
                push_x = 30;
                break;
        }
    }
    public void flip()
    {
        if (can_walk)
        {
            facing_right = !facing_right;
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, combo_kaioken_radius);
    }
}
