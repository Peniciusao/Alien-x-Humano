using UnityEngine;
using System.Collections;

public class BossEscudoRefletor : MonoBehaviour
{
    [Header("Configurações do Escudo")]
    public Boss boss;
    public float duracaoEscudo = 5.0f;          // Tempo que o escudo fica ativo
    public float multiplicadorVelocidade = 1.5f; // Aumenta a velocidade do tiro refletido
    public string tagProjetilPlayer = "ProjetilPlayer";

    [Header("Visual do Escudo")]
    public GameObject objetoEscudoVisual;      // Sprite/Partícula do escudo
    public SpriteRenderer spriteEscudo;
    public Color corEscudo = new Color(0f, 0.8f, 1f, 0.5f); // Azul ciano transparente

    private bool escudoAtivo = false;

    void Start()
    {
        if (boss == null)
            boss = GetComponentInParent<Boss>();

        DesativarEscudo();
    }

    public void AtivarEscudo()
    {
        if (escudoAtivo) return;

        StartCoroutine(RotinaEscudo());
    }

    IEnumerator RotinaEscudo()
    {
        escudoAtivo = true;

        if (objetoEscudoVisual != null)
            objetoEscudoVisual.SetActive(true);

        if (spriteEscudo != null)
            spriteEscudo.color = corEscudo;

        Debug.Log("<color=cyan>[ESCUDO]</color> Escudo Refletor ligado! Refletindo projéteis e vulnerável apenas a Melee.");

        yield return new WaitForSeconds(duracaoEscudo);

        DesativarEscudo();
    }

    public void DesativarEscudo()
    {
        escudoAtivo = false;

        if (objetoEscudoVisual != null)
            objetoEscudoVisual.SetActive(false);

        if (boss != null)
        {
            boss.DesativarImunidadeProjeteis();
            boss.Continuar();
        }

        Debug.Log("<color=cyan>[ESCUDO]</color> Escudo desativado! Boss voltou ao normal.");
    }

    private void OnTriggerEnter2D(Collider2D outro)
    {
        // Só reflete se o escudo estiver ativo e o objeto for um projétil do jogador
        if (!escudoAtivo) return;

        if (outro.CompareTag(tagProjetilPlayer))
        {
            RefletirProjetil(outro);
        }
    }

    void RefletirProjetil(Collider2D projetil)
    {
        Rigidbody2D rb = projetil.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // Inverte a direção da velocidade e aplica o multiplicador
            rb.linearVelocity = -rb.linearVelocity * multiplicadorVelocidade;

            // Gira o sprite do projétil 180 graus para apontar na nova direção
            projetil.transform.Rotate(0f, 0f, 180f);

            // Troca a tag para que o projétil agora possa atingir o Player
            projetil.tag = "ProjetilInimigo";

            Debug.Log($"<color=magenta>[REFLEXÃO]</color> Projétil {projetil.name} foi refletido de volta para o jogador!");
        }
    }
}