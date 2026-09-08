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
    public float tempoAviso = 1.0f;  // Tempo que o laser fica só mirando
    public float duracaoAtaque = 0.5f; // Tempo do disparo real com dano

    [Header("Cores do Laser")]
    public Color corAviso = new Color(1f, 0f, 0f, 0.25f); // Vermelho transparente
    public Color corAtaque = Color.red;                   // Vermelho solido

    [Header("Tamanho do Laser")]
    public float larguraLaser = 1f;
    public float comprimentoLaser = 10f;

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

        // --- FASE 1: AVISO / MIRA (Sem Dano) ---
        if (laserSprite != null)
        {
            laserSprite.color = corAviso;
            laserSprite.gameObject.SetActive(true);
        }

        // Garante que a hitbox fica DESLIGADA na mira
        if (hitbox != null)
            hitbox.enabled = false;

        float tempoFimAviso = Time.time + tempoAviso;

        while (Time.time < tempoFimAviso)
        {
            if (jogador == null) break;
            AtualizarDirecao(); // Acompanha o jogador enquanto avisa
            yield return null;
        }

        // --- FASE 2: DISPARO REAL (Com Dano) ---
        if (laserSprite != null)
        {
            laserSprite.color = corAtaque;
        }

        // Liga a hitbox apenas para o disparo real
        if (hitbox != null)
            hitbox.enabled = true;

        float tempoFimAtaque = Time.time + duracaoAtaque;

        while (Time.time < tempoFimAtaque)
        {
            if (jogador == null) break;

            AtualizarDirecao();
            VerificarHitbox(); // Causa dano

            yield return null;
        }

        // --- FIM DO ATAQUE ---
        DesativarLaser();
        atacando = false;
    }

    void DesativarLaser()
    {
        if (laserSprite != null)
            laserSprite.gameObject.SetActive(false);

        if (hitbox != null)
            hitbox.enabled = false;
    }

    void AtualizarDirecao()
    {
        Vector2 direcao = (Vector2)jogador.position - (Vector2)transform.position;

        if (direcao.sqrMagnitude <= 0.001f)
            return;

        float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angulo - 90f);
    }

    void VerificarHitbox()
    {
        if (hitbox == null || playerAtingido)
            return;

        Vector2 centro = hitbox.transform.TransformPoint(hitbox.offset);
        Vector2 tamanho = Vector2.Scale(hitbox.size, hitbox.transform.lossyScale);

        Collider2D[] objetos = Physics2D.OverlapBoxAll(
            centro,
            tamanho,
            hitbox.transform.eulerAngles.z
        );

        foreach (Collider2D objeto in objetos)
        {
            PlayerHealth vida = objeto.GetComponentInParent<PlayerHealth>();

            if (vida != null)
            {
                Debug.Log("acerto o laser");
                vida.TomarDano(dano);
                playerAtingido = true;
                break;
            }
        }
    }
    void OnDrawGizmos()
    {
        if (hitbox == null) return;

        // Salva a matriz de transformação original da Unity
        Matrix4x4 matrizOriginal = Gizmos.matrix;

        // Aplica a transformação exata de posição, rotação e escala do objeto da Hitbox
        Gizmos.matrix = hitbox.transform.localToWorldMatrix;

        // 1. Desenha o contorno em linha vermelha
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(hitbox.offset, hitbox.size);

        // 2. Desenha um preenchimento vermelho semitransparente
        Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
        Gizmos.DrawCube(hitbox.offset, hitbox.size);

        // Restaura a matriz original dos Gizmos
        Gizmos.matrix = matrizOriginal;
    }
}