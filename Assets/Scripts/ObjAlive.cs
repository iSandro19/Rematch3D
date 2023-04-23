using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjAlive : MonoBehaviour
{
    public int life;
    public int maxLife;
    protected float dmgTime;
    public float dmgCooldown;
    private bool _invicible;

    public bool dead
    {
        get { return life <= 0; }
    }

    public bool invincible
    {
        get
        {
            return _invicible || Time.time < dmgTime + dmgCooldown;
        }

        set
        {
            _invicible = value;
        }
    }

    protected virtual void OnAtk(int dmg)
    {
        if (!dead || !_invicible)
        {
            float time = Time.time;

            if (time > dmgTime + dmgCooldown)
            {
                dmgTime = time;
                life -= dmg;
            }
        }
    }
}
