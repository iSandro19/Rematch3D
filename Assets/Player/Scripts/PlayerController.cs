using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    IDLE, RUN, ATK
}

public class PlayerController : MonoBehaviour
{
    public CharacterController charac;
    public Animator anim;
    public Vector3 vel;
    public float velMax = 20f;
    public float atkTime = 0f;
    public float atkCooldown = 0.4f; 

    void Start()
    {
        charac = GetComponent< CharacterController >();
        anim = GetComponent< Animator >();
    }

    void Update()
    {
        if (atkTime <= 0f)
        {
            vel.x = Input.GetAxis("Horizontal") * velMax;
            vel.z = Input.GetAxis("Vertical") * velMax;

            if (vel == Vector3.zero)
            {
                anim.SetInteger("state", (int)PlayerState.IDLE);
            }
            else
            {
                transform.rotation = Quaternion.AngleAxis(
                    90f - Mathf.Rad2Deg * Mathf.Atan2(vel.z, vel.x),
                    Vector3.up
                );
                anim.SetInteger("state", (int)PlayerState.RUN);

                charac.Move(vel * Time.deltaTime);
            }

            if (Input.GetButtonDown("Fire1"))
            {
                atkTime = atkCooldown;
                anim.Play("ATK", -1, 0);
            }
        }
        else
        {
            atkTime -= Time.deltaTime;
        }
    }
}
