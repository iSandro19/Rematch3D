using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ObjAlive
{
    public CharacterController charac;
    public Animator anim;

    public float angle = 45f;
    public Vector3 vel;
    public float velMax = 4f;

    public int basicDMG = 1;

    public float atkTime = -0.6f;
    public float atkStart = 0.2f;
    public float atkEnd = 0.3f;
    public float atkDur = 0.6f;
    public float atkCooldown = 0.7f;

    public float dashVel = 16f;
    public float dashTime = -0.1f;
    public float dashDur = 0.1f;
    public float dashCooldown = 1f;
    public GameObject dashEffectPrefab;
    private GameObject dashEffect;
    public bool dashing = false;

    public GameObject swordPrefab;
    public GameObject swordShotPrefab;
    public float swordShotTime = -0.6f;
    public float swordShotDur = 0.6f;

    void Start()
    {
        charac = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        life = 4;
        invincible = false;
        dmgDur = 1f;
        dmgTime = 0f;
    }

    void Update()
    {
        float time = Time.time;
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        
        if (dead)
        {
            anim.SetTrigger("dead");
            enabled = false;
        }
        else if (time < atkTime + atkDur)
        {
            if (atkTime + atkStart < time && time < atkTime + atkEnd)
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
                        enemy.SendMessage("OnAtk", basicDMG);
                    }
                }
            }
        }
        else if (time < dashTime + dashDur)
        {
            charac.Move(transform.rotation * Vector3.forward * dashVel * Time.deltaTime);
        }
        else if (time < swordShotTime + swordShotDur)
        {
        }
        else
        {
            Vector3 offset = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            Vector3 direction = offset.normalized;
            vel = direction.magnitude == 0f? Vector3.zero:
                direction * (Mathf.Clamp01(offset.magnitude / direction.magnitude) * velMax);

            anim.SetFloat("vel", vel.magnitude);

            if (time > dashTime + dashCooldown && dashing)
            {
                dashEffect = GameObject.Instantiate(dashEffectPrefab, transform);

                dashing = false;
            }

            if (swordPrefab.activeSelf && time > atkTime + atkCooldown && Input.GetButtonDown("Fire1"))
            {
                atkTime = Time.time;

                anim.SetTrigger("attack");
            }
            else if (time > dashTime + dashCooldown && Input.GetButtonDown("Fire2"))
            {
                dashTime = Time.time;

                anim.SetTrigger("dash");

                dashing = true;
                Destroy(dashEffect);
            }
            else if (swordPrefab.activeSelf && Input.GetButtonDown("Fire3"))
            {
                GameObject.Instantiate(
                    swordShotPrefab,
                    transform.position + Vector3.up,
                    transform.rotation
                );

                anim.SetTrigger("attack");

                swordPrefab.SetActive(false);
            }
            else if (vel != Vector3.zero)
            {
                transform.rotation = Quaternion.AngleAxis(
                    90f + angle - Mathf.Rad2Deg * Mathf.Atan2(vel.z, vel.x),
                    Vector3.up
                );

                charac.Move(Quaternion.AngleAxis(angle, Vector3.up) * vel * Time.deltaTime);
            }
        }
    }

    protected override void OnAtk(int dmg)
    {
        if (!dead && !invincible) anim.SetTrigger("dmg");

        base.OnAtk(dmg);
    }
}
