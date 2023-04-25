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
            if (!dead && !invincible && !hasMovedThisTurn)
            {
                // Generar un ángulo aleatorio en grados
                float angle = Random.Range(0f, 360f);
                // Convertir el ángulo a una dirección en el plano XZ
                Vector2 direction = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
                // Calcular la posición objetivo
                Vector3 targetPos = transform.position + new Vector3(direction.x, 0f, direction.y) * jumpDistance;
                // Asegurarse de que el enemigo no salga del área del mapa
                targetPos.x = Mathf.Clamp(targetPos.x, -mapSize + 0.5f, mapSize - 0.5f);
                targetPos.z = Mathf.Clamp(targetPos.z, -mapSize + 0.5f, mapSize - 0.5f);
                // Mover el enemigo progresivamente al punto de destino
                float t = 0f;
                while (t < 1f)
                {
                    t += Time.deltaTime * moveSpeed;
                    transform.position = Vector3.Lerp(transform.position, targetPos, t);
                    yield return null;
                }
                // Asegurarse de que el enemigo no haya chocado con ningún objeto en el camino
                Collider[] hits = Physics.OverlapSphere(transform.position, 0.5f);
                foreach (Collider hit in hits)
                {
                    if (hit.gameObject != gameObject && hit.isTrigger)
                    {
                        transform.position = transform.position - new Vector3(direction.x, 0f, direction.y) * jumpDistance;
                        break;
                    }
                }
                // Actualizar la variable que indica si el enemigo ya se movió en el turno actual
                hasMovedThisTurn = true;
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
