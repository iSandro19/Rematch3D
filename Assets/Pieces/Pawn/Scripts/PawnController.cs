using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnController : ObjEnemy
{
    public MeshRenderer rend;
    public float moveSpeed = 2f; // Velocidad de movimiento del enemigo
    public float jumpDistance = 2f; // Distancia del salto del enemigo
    public int mapSize = 10; // Tamaño del mapa
    private bool hasMovedThisTurn = false; // Variable para llevar registro si el enemigo ya se movió en el turno actual

    void Start()
    {
        life = 3;
        invincible = false;
        dmgCooldown = 1f;
        dmgTime = 0f;
        rend = GetComponent<MeshRenderer>();

        // Iniciar la corrutina para mover el objeto cada 3 segundos
        StartCoroutine(MoveObjectEvery3Seconds());
    }

    IEnumerator MoveObjectEvery3Seconds()
    {
        while (true)
        {
            // Esperar 3 segundos
            yield return new WaitForSeconds(3f);

            // Si el objeto no está muerto ni invencible, moverlo a una posición aleatoria
            if (!dead && !invincible)
            {
                // Generar una dirección aleatoria perpendicular a la posición actual del enemigo
                Vector2 perpendicularDirection = Random.insideUnitCircle.normalized;
                Vector3 currentDirection = transform.forward;
                perpendicularDirection = new Vector2(currentDirection.z, -currentDirection.x);
                // Mover el enemigo en la dirección aleatoria
                Vector3 newPos = transform.position + new Vector3(perpendicularDirection.x, 0f, perpendicularDirection.y) * jumpDistance;
                // Asegurarse de que el enemigo no salga del área del mapa
                newPos.x = Mathf.Clamp(newPos.x, -mapSize + 0.5f, mapSize - 0.5f);
                newPos.z = Mathf.Clamp(newPos.z, -mapSize + 0.5f, mapSize - 0.5f);
                transform.position = newPos;
            }
        }
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
        else
        {
            rend.enabled = true;
        }
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

    // Evento que se ejecuta al inicio de cada turno
    public void OnTurnStart()
    {
        hasMovedThisTurn = true;
    }

    // Evento que se ejecuta al final de cada turno
    public void OnTurnEnd()
    {
        hasMovedThisTurn = false;
    }
}
