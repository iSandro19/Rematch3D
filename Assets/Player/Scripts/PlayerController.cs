using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ObjAlive
{
    public CharacterController charac;
    public Animator anim;
    public Vector3 vel;
    public float velMax = 20f;
    public float atkTime = 0f;
    public float atkCooldown = 0.4f; 

    void Start()
    {
        charac = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        life = 4;
        invincible = false;
        dmgCooldown = 1f;
        dmgTime = 0f;
    }

    void Update()
    {
        if (dead)
        {
            Debug.Log("dead");
            anim.SetTrigger("dead");
            enabled = false;
        }
        else if (atkTime <= 0f)
        {
            vel.x = Input.GetAxis("Horizontal") * velMax;
            vel.z = Input.GetAxis("Vertical") * velMax;

            if (vel == Vector3.zero)
            {
                anim.SetBool("running", false);
            }
            else
            {
                anim.SetBool("running", true);

                transform.rotation = Quaternion.AngleAxis(
                    90f - Mathf.Rad2Deg * Mathf.Atan2(vel.z, vel.x),
                    Vector3.up
                );

                charac.Move(vel * Time.deltaTime);
            }

            if (Input.GetButtonDown("Fire1"))
            {
                atkTime = atkCooldown;

                anim.SetTrigger("attack");
            }
        }
        else
        {
            Collider[] cols = Physics.OverlapSphere(
                (
                    transform.position +
                    transform.rotation * Vector3.forward +
                    Vector3.up * 0.5f
                ),
                0.5f
            );

            foreach (Collider col in cols)
            {
                ObjEnemy enemy = col.gameObject.GetComponent<ObjEnemy>();

                if (enemy != null)
                {
                    enemy.SendMessage("OnAtk", 1);
                }
            }

            atkTime -= Time.deltaTime;
        }
    }

    protected override void OnAtk(int dmg)
    {
        if (!dead && !invincible) anim.SetTrigger("dmg");

        base.OnAtk(dmg);
    }
}
