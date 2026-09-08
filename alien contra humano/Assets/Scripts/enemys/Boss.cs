using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Vida do Boss")]
    public int vidaMaxima = 100;

    private int vidaAtual;
    private bool morreu = false;

    [Header("Estado")]
    public bool imuneAProjeteis = false;
    public bool parado = false;

    [Header("Eventos de Vida")]
    public bool evento75 = true;
    public bool evento50 = true;
    public bool evento25 = true;

    [Header("Itens de Cura")]
    public GameObject itemCura;
    public int quantidadeCuras = 2;

    [Header("Inimigos")]
    public GameObject[] inimigos;
    public int quantidadeInimigos = 3;

    [Header("Pontos para Spawn")]
    public Transform[] pontosSpawn;

    void Start()
    {
        vidaAtual = vidaMaxima;

        Debug.Log(
            "Boss apareceu com " +
            vidaAtual +
            " de vida."
        );
    }

    public void TomarDano(int dano)
    {
        if (morreu)
            return;

        vidaAtual -= dano;

        if (vidaAtual < 0)
            vidaAtual = 0;

        Debug.Log(
            "Boss tomou " +
            dano +
            " de dano. Vida: " +
            vidaAtual
        );

        VerificarEventosDeVida();

        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }

    void VerificarEventosDeVida()
    {
        float porcentagemVida =
            (float)vidaAtual / vidaMaxima * 100f;

        // Evento dos 75%
        if (evento75 && porcentagemVida <= 75f)
        {
            evento75 = false;
            AtivarEvento();
        }

        // Evento dos 50%
        if (evento50 && porcentagemVida <= 50f)
        {
            evento50 = false;
            AtivarEvento();
        }

        // Evento dos 25%
        if (evento25 && porcentagemVida <= 25f)
        {
            evento25 = false;
            AtivarEvento();
        }
    }

    void AtivarEvento()
    {
        Debug.Log("EVENTO DO BOSS ATIVADO!");

        // 🛑 Para o Boss
        Parar();

        // 🛡️ Fica imune a projéteis
        AtivarImunidadeProjeteis();
    }

    // ==========================================
    // 🛑 MOVIMENTO
    // ==========================================

    public void Parar()
    {
        parado = true;

        Rigidbody2D rb =
            GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        Debug.Log("BOSS PAROU!");
    }

    public void Continuar()
    {
        parado = false;

        Debug.Log("BOSS VOLTOU A ANDAR!");
    }

    // ==========================================
    // 🛡️ IMUNIDADE
    // ==========================================

    public void AtivarImunidadeProjeteis()
    {
        imuneAProjeteis = true;

        Debug.Log(
            "BOSS FICOU IMUNE A PROJÉTEIS!"
        );
    }

    public void DesativarImunidadeProjeteis()
    {
        imuneAProjeteis = false;

        Debug.Log(
            "BOSS VOLTOU A RECEBER PROJÉTEIS!"
        );
    }

    public bool EstaImuneAProjeteis()
    {
        return imuneAProjeteis;
    }



    // ==========================================
    // 💀 MORTE
    // ==========================================

    void Morrer()
    {
        if (morreu)
            return;

        morreu = true;

        Debug.Log("BOSS DERROTADO!");

        Destroy(gameObject);
    }
}