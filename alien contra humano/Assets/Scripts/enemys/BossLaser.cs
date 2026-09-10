using UnityEngine;
using System.Collections;

public class BossLaser : MonoBehaviour
{
    [Header("Player")]
    public Transform jogador;

    [Header("Componentes do Laser")]
    public SpriteRenderer laserSprite;
    public BoxCollider2D hitbox;

    [Header("Configurações do Ataque")]
    public int dano = 15;
    public float tempoAviso = 1.0f;
    public float duracaoAtaque = 0.5f;

    [Header("Cores do Laser")]
    public Color corAviso = new Color(1f, 0f, 0f, 0.25f);
    public Color corAtaque = Color.red;

    private bool atacando = false;
    private bool playerAtingido = false;

    void Start()
    {
        if (jogador == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                jogador = player.transform;
        }
        DesativarLaser();
    }

    public void Atacar()
    {
        gameObject.SetActive(true);

        if (atacando)
            return;

        StartCoroutine(DispararLaser());
    }

    IEnumerator DispararLaser()
    {
        atacando = true;
        playerAtingido = false;

        if (jogador == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                jogador = player.transform;
        }

        if (jogador == null)
        {
            atacando = false;
            yield break;
        }

        if (laserSprite != null)
        {
            laserSprite.color = corAviso;
            laserSprite.gameObject.SetActive(true);
        }

        if (hitbox != null)
            hitbox.enabled = false;

        float tempoFimAviso = Time.time + tempoAviso;

        while (Time.time < tempoFimAviso)
        {
            if (jogador == null) break;
            AtualizarDirecao();
            yield return null;
        }

        if (laserSprite != null)
        {
            laserSprite.color = corAtaque;
        }

        if (hitbox != null)
            hitbox.enabled = true;

        float tempoFimAtaque = Time.time + duracaoAtaque;

        while (Time.time < tempoFimAtaque)
        {
            if (jogador == null) break;
            AtualizarDirecao();
            VerificarHitbox();
            yield return null;
        }

        DesativarLaser();
        atacando = false;
    }

    void DesativarLaser()
    {
        if (laserSprite != null)
            laserSprite.gameObject.SetActive(false);

        if (hitbox != null)
            hitbox.enabled = false;

        gameObject.SetActive(false);
    }

    void AtualizarDirecao()
    {
        Vector2 direcao = (Vector2)jogador.position - (Vector2)transform.position;
        if (direcao.sqrMagnitude <= 0.001f) return;
        float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angulo - 90f);
    }

    void VerificarHitbox()
    {
        if (hitbox == null || playerAtingido) return;

        Vector2 centro = hitbox.transform.TransformPoint(hitbox.offset);
        Vector2 tamanho = Vector2.Scale(hitbox.size, hitbox.transform.lossyScale);

        Collider2D[] objetos = Physics2D.OverlapBoxAll(centro, tamanho, hitbox.transform.eulerAngles.z);

        foreach (Collider2D objeto in objetos)
        {
            PlayerHealth vida = objeto.GetComponentInParent<PlayerHealth>();
            if (vida != null)
            {
                vida.TomarDano(dano);
                playerAtingido = true;
                break;
            }
        }
    }
}