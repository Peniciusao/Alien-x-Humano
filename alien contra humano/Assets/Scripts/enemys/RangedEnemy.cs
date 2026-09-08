using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 2f;
    public float distanciaIdeal = 6f;
    public float distanciaFuga = 3f;

    [Header("Vida")]
    public int vida = 3;

    [Header("Tiro")]
    public GameObject bala;
    public Transform pontoTiro;
    public float cooldownTiro = 2f;
    public float velocidadeBala = 8f;

    private Transform jogador;
    private WaveManager waveManager;
    private float proximoTiro = 0f;
    private bool morreu = false;

    void Start()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            jogador = playerObject.transform;
    }

    void Update()
    {
        if (jogador == null)
            return;

        Vector2 direcao =
            jogador.position - transform.position;

        float distancia = direcao.magnitude;

        if (direcao != Vector2.zero)
        {
            float angulo =
                Mathf.Atan2(direcao.y, direcao.x) *
                Mathf.Rad2Deg;

            transform.rotation =
                Quaternion.Euler(0f, 0f, angulo);
        }

        if (distancia < distanciaFuga)
        {
            Vector2 fugir =
                -direcao.normalized;

            transform.position +=
                (Vector3)fugir *
                velocidade *
                Time.deltaTime;
        }
        else if (distancia > distanciaIdeal)
        {
            transform.position +=
                (Vector3)direcao.normalized *
                velocidade *
                Time.deltaTime;
        }

        if (Time.time >= proximoTiro)
        {
            Atirar();

            proximoTiro =
                Time.time + cooldownTiro;
        }
    }

    void Atirar()
    {
        if (bala == null || pontoTiro == null)
            return;

        GameObject novaBala =
            Instantiate(
                bala,
                pontoTiro.position,
                Quaternion.identity
            );

        Collider2D balaCollider =
            novaBala.GetComponent<Collider2D>();

        if (balaCollider != null)
        {
            Collider2D[] coliders =
                FindObjectsByType<Collider2D>(
                    FindObjectsSortMode.None
                );

            foreach (Collider2D col in coliders)
            {
                if (col.GetComponentInParent<Enemy>() != null ||
                    col.GetComponentInParent<RangedEnemy>() != null)
                {
                    Physics2D.IgnoreCollision(
                        balaCollider,
                        col
                    );
                }
            }
        }

        Vector2 direcao =
            (jogador.position -
             pontoTiro.position).normalized;

        float angulo =
            Mathf.Atan2(direcao.y, direcao.x) *
            Mathf.Rad2Deg;

        novaBala.transform.rotation =
            Quaternion.Euler(
                0f,
                0f,
                angulo
            );

        Rigidbody2D rb =
            novaBala.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity =
                direcao * velocidadeBala;
        }
    }

    public void DefinirWaveManager(WaveManager manager)
    {
        waveManager = manager;
    }

    public void TomarDano(int danoRecebido)
    {
        if (morreu)
            return;

        vida -= danoRecebido;

        Debug.Log(
            "RangedEnemy tomou dano: " +
            danoRecebido
        );

        if (vida <= 0)
        {
            morreu = true;

            if (waveManager != null)
            {
                waveManager.InimigoMorreu();
            }

            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D colisao)
    {
        PlayerHealth player =
            colisao.gameObject.GetComponent<PlayerHealth>();

        if (player != null)
        {
            player.TomarDano(1);
        }
    }

    void OnTriggerEnter2D(Collider2D outro)
    {
        PlayerHealth player =
            outro.GetComponent<PlayerHealth>();

        if (player != null)
        {
            player.TomarDano(1);
        }
    }
}