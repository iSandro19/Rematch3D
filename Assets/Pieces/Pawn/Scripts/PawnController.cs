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
        StartCoroutine(MoveObjectEvery1Second(GameObject.Find("Player")));
    }

    IEnumerator MoveObjectEvery1Second(GameObject player)
    {
        float stepSize = 1f; // Tamaño de cada paso

        while (true)
        {
            // Calcular la dirección hacia el personaje
            Vector3 dirToPlayer = (player.transform.position - transform.position).normalized;

            // Calcular una dirección aleatoria con una ligera desviación hacia el jugador
            float deviationAngle = Random.Range(-20f, 20f);
            Vector3 dir = Quaternion.Euler(0, deviationAngle, 0) * dirToPlayer;

            // Calcular la posición a la que saltar
            Vector3 targetPos = transform.position + dir * stepSize;

            // Comprobar si la dirección elegida es válida (no sale del mapa y no colisiona con ningún objeto)
            RaycastHit hit;
            float newX = transform.position.x + dir.x * stepSize;
            float newZ = transform.position.z + dir.z * stepSize;
            if (!Physics.Raycast(transform.position, dir, out hit, stepSize))
            {
                // Calcular el ángulo de la dirección actual
                float angle = Mathf.Round(transform.eulerAngles.y / 90f) * 90f;

                // Si se mueve en cruz, moverse 1 metro en esa dirección
                if (angle == 0f || angle == 90f || angle == 180f || angle == 270f)
                {
                    targetPos = transform.position + dir * stepSize;
                }
                else // Si se mueve en diagonal, moverse a 45 grados y calcular la hipotenusa para mantenerse en el centro de los tiles
                {
                    float hypotenuse = Mathf.Sqrt(2) * stepSize / 2f;
                    Vector3 offset = new Vector3(dir.x, 0, dir.z).normalized * hypotenuse;
                    targetPos = transform.position + offset;
                }

                // Mover gradualmente hacia la nueva posición
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
