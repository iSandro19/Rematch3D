using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordShotController : MonoBehaviour
{
    public int DMG = 2;
    private Vector3 vel;
    public float velMax = 8f;
    private bool pinned = false;


    void Start()
    {
        vel = transform.rotation * Vector3.forward * velMax;
    }

    void Update()
    {
        transform.position += vel * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        ObjEnemy enemy = other.gameObject.GetComponent<ObjEnemy>();

        if (enemy == null)
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();

            if (player == null)
            {
                vel = Vector3.zero;
                pinned = true;
            }
            else if (pinned)
            {
                player.swordPrefab.SetActive(true);
                Destroy(this.gameObject);
            }
            
        }
        else
        {
            enemy.SendMessage("OnAtk", DMG);
        }

    }

}
