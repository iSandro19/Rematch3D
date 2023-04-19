using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    IDLE, RUN
}

public class PlayerController : MonoBehaviour
{
    public Animator anim;

    public float tileW = 1f;
    public float tileH = 1f;
    public float runTime = 0f;
    public float runTimeMax = 0.4f;
    public Vector3 originPos, destPos;
    public Quaternion originAng, destAng;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        switch ((PlayerState)anim.GetInteger("state"))
        {
            case PlayerState.IDLE:

                if (Input.GetAxisRaw("Horizontal") != 0f)
                {
                    float dir = Mathf.Sign(Input.GetAxisRaw("Horizontal"));

                    anim.SetInteger("state", (int)PlayerState.RUN);

                    originPos = transform.position;
                    destPos = transform.position;
                    destPos.x += dir * tileW;

                    originAng = transform.rotation;
                    destAng = Quaternion.AngleAxis(
                        90f * dir,
                        Vector3.up
                    );

                }
                else if (Input.GetAxisRaw("Vertical") != 0f)
                {
                    float dir = Mathf.Sign(Input.GetAxisRaw("Vertical"));

                    anim.SetInteger("state", (int)PlayerState.RUN);
                    originPos = transform.position;
                    destPos = transform.position;
                    destPos.z += dir * tileH;

                    originAng = transform.rotation;
                    destAng = Quaternion.AngleAxis(
                       dir > 0f? 0f: 180f,
                        Vector3.up
                    );
                }
                break;

            case PlayerState.RUN:
                if (runTime < runTimeMax)
                {
                    runTime += Time.deltaTime;
                    transform.position = Vector3.Lerp(originPos, destPos, runTime / runTimeMax);
                    transform.rotation = Quaternion.Lerp(originAng, destAng, runTime / runTimeMax);
                }
                else
                {
                    runTime = 0f;
                    anim.SetInteger("state", (int)PlayerState.IDLE);
                }
                break;
        }
        /*
        vel.x = Input.GetAxisRaw("Horizontal") * maxVelocity;
        vel.z = Input.GetAxis("Vertical") * maxVelocity;
        angle = 90f - Mathf.Rad2Deg * Mathf.Atan2(vel.z, vel.x);
        transform.rotation = Quaternion.AngleAxis(
            angle,
            Vector3.up
        );*/
    }
}
