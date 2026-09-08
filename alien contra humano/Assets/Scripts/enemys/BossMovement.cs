using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [Header("Player")]
    public Transform jogador;

    [Header("Movimento")]
    public float velocidade = 2f;
    public float distanciaParar = 2f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (jogador == null)
        {
            GameObject player =
                GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                jogador = player.transform;
            }
        }
    }

    void FixedUpdate()
    {
        if (jogador == null)
            return;

        Vector2 distancia =
            (Vector2)jogador.position - rb.position;

        float distanciaAtual = distancia.magnitude;

        // Se estiver longe, persegue
        if (distanciaAtual > distanciaParar)
        {
            Vector2 direcao = distancia.normalized;

            rb.MovePosition(
                rb.position +
                direcao * velocidade * Time.fixedDeltaTime
            );
        }
        else
        {
            // Está perto o suficiente, então para
            rb.linearVelocity = Vector2.zero;
        }
    }
}