using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnController : ObjEnemy
{
    public MeshRenderer rend;
    public float moveSpeed = 2f; // Velocidad de movimiento del enemigo
    public float jumpDistance = 2f; // Distancia del salto del enemigo

    void Start()
    {
        life = 3;
        invincible = false;
        dmgCooldown = 0.2f;
        dmgTime = 0.2f;
        rend = GetComponent<MeshRenderer>();

        // Iniciar la corrutina para mover el objeto cada 1 segundo
        StartCoroutine(MoveObjectEvery1Second());
    }

    IEnumerator MoveObjectEvery1Second()
    {
        float stepSize = 1f; // Tamaño de cada paso

        while (true)
        {
            // Seleccionar una dirección aleatoria (0, 90, 180 o 270 grados)
            float angle = Random.Range(0, 4) * 90f;

            // Calcular la dirección del movimiento en función del ángulo
            Vector3 dir = Quaternion.Euler(0, angle, 0) * transform.forward;

            // Comprobar si la dirección elegida es válida (no sale del mapa y no colisiona con ningún objeto)
            RaycastHit hit;
            float newX = transform.position.x + dir.x * stepSize;
            float newZ = transform.position.z + dir.z * stepSize;
            if (!Physics.Raycast(transform.position, dir, out hit, stepSize))
            {
                // Mover gradualmente hacia la nueva posición
                Vector3 targetPos = transform.position + dir * stepSize;
                float t = 0f;
                while (t < 1f)
                {
                    t += Time.deltaTime * moveSpeed;
                    transform.position = Vector3.Lerp(transform.position, targetPos, t);
                    yield return null;
                }
            }

            // Esperar 1 segundo antes de volver a mover el objeto
            yield return new WaitForSeconds(1f);
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
}
