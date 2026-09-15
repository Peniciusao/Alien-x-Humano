using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 2f;
    public int vida = 3;
    public int dano = 1;

    [Header("Visual / Sprite")]
    public SpriteRenderer spriteRenderer;
    [Tooltip("dps pega sprite de walk cycle, se n tiver deixa nada")]
    public Sprite[] spritesCaminhada;
    public float fpsCaminhada = 6f;

    private Transform jogador;
    private WaveManager waveManager;
    private bool morreu = false;

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

        // Garante que o objeto não fica com rotação estranha herdada
        transform.rotation = Quaternion.identity;
    }

    void Update()
    {
        if (jogador == null || morreu)
            return;

        Vector2 direcao =
            jogador.position - transform.position;

        if (direcao != Vector2.zero)
        {
            transform.position +=
                (Vector3)direcao.normalized *
                velocidade *
                Time.deltaTime;

            AnimarCaminhada();
            AtualizarFlip(direcao.x);
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
            "Enemy tomou dano: " +
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
            player.TomarDano(dano);
        }
    }

    void OnTriggerEnter2D(Collider2D outro)
    {
        PlayerHealth player =
            outro.GetComponent<PlayerHealth>();

        if (player != null)
        {
            player.TomarDano(dano);
        }
    }
}