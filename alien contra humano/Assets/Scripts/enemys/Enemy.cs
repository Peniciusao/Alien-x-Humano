using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float velocidade = 2f;
    public int vida = 3;
    public int dano = 1;

    private Transform jogador;
    private WaveManager waveManager;
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
        if (jogador != null)
        {
            Vector2 direcao =
                jogador.position - transform.position;

            transform.position +=
                (Vector3)direcao.normalized *
                velocidade *
                Time.deltaTime;
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