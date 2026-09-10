using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    private enum Estado { Perseguindo, Mirando }

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

    [Header("Mira (aiming)")]
    [Tooltip("Tempo que o inimigo fica parado mirando antes de atirar.")]
    public float tempoMira = 0.6f;

    [Header("Visual / Sprite")]
    public SpriteRenderer spriteRenderer;
    [Tooltip("Sprites placeholder do ciclo de caminhada (opcional). Se vazio, não anima.")]
    public Sprite[] spritesCaminhada;
    public float fpsCaminhada = 6f;

    private Transform jogador;
    private WaveManager waveManager;
    private float proximoTiro = 0f;
    private bool morreu = false;

    private Estado estado = Estado.Perseguindo;
    private float tempoMiraRestante = 0f;

    private int frameCaminhadaAtual = 0;
    private float tempoProximoFrame = 0f;

    void Start()
    {
        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            jogador = playerObject.transform;

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // Garante que a rotação começa neutra (sem inclinar o sprite)
        transform.rotation = Quaternion.identity;
    }

    void Update()
    {
        if (jogador == null || morreu)
            return;

        switch (estado)
        {
            case Estado.Perseguindo:
                AtualizarPerseguindo();
                break;

            case Estado.Mirando:
                AtualizarMirando();
                break;
        }
    }

    void AtualizarPerseguindo()
    {
        Vector2 direcao = jogador.position - transform.position;
        float distancia = direcao.magnitude;

        Vector2 movimento = Vector2.zero;

        if (distancia < distanciaFuga)
        {
            movimento = -direcao.normalized;
        }
        else if (distancia > distanciaIdeal)
        {
            movimento = direcao.normalized;
        }

        if (movimento != Vector2.zero)
        {
            transform.position += (Vector3)movimento * velocidade * Time.deltaTime;
            AnimarCaminhada();
        }

        // Vira o sprite pra esquerda/direita em vez de girar o objeto todo
        AtualizarFlip(direcao.x);

        // Quando estiver perto da hora de atirar, entra no estado de mira
        if (Time.time >= proximoTiro - tempoMira)
        {
            estado = Estado.Mirando;
            tempoMiraRestante = tempoMira;
        }
    }

    void AtualizarMirando()
    {
        // Fica parado enquanto mira
        Vector2 direcao = jogador.position - transform.position;

        if (direcao != Vector2.zero)
        {
            float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angulo);
        }

        tempoMiraRestante -= Time.deltaTime;

        if (tempoMiraRestante <= 0f)
        {
            Atirar();

            proximoTiro = Time.time + cooldownTiro;

            // Volta pro modo de caminhada, desfazendo a rotação de mira
            transform.rotation = Quaternion.identity;
            estado = Estado.Perseguindo;
        }
    }

    void AnimarCaminhada()
    {
        if (spritesCaminhada == null || spritesCaminhada.Length == 0 || spriteRenderer == null)
            return;

        if (Time.time >= tempoProximoFrame)
        {
            frameCaminhadaAtual = (frameCaminhadaAtual + 1) % spritesCaminhada.Length;
            spriteRenderer.sprite = spritesCaminhada[frameCaminhadaAtual];
            tempoProximoFrame = Time.time + (1f / Mathf.Max(0.01f, fpsCaminhada));
        }
    }

    void AtualizarFlip(float direcaoX)
    {
        if (spriteRenderer == null)
            return;

        if (Mathf.Abs(direcaoX) > 0.01f)
        {
            spriteRenderer.flipX = direcaoX < 0f;
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