using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnController : ObjEnemy
{
    public MeshRenderer rend;

    void Start()
    {
        life = 10;
        invincible = false;
        dmgCooldown = 1f;
        dmgTime = 0f;
        rend = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (invincible)
        {
            rend.enabled = !rend.enabled;
        }
        else if (dead)
        {
            gameObject.SetActive(false);
        }
        else rend.enabled = true;
    }

    void OnTriggerStay(Collider col)
    {
        PlayerController player = col.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            player.SendMessage("OnAtk", 1);
        }
    }

    protected override void OnAtk(int dmg)
    {
        base.OnAtk(dmg);
    }
}
