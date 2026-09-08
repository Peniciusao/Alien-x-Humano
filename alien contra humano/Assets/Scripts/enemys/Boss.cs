using UnityEngine;

public class Boss : MonoBehaviour
{
    // Enumeração para identificar o tipo de ataque
    public enum TipoDano { Melee, Projetil }

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

    [Header("Componentes")]
    public BossEscudoRefletor escudoRefletor; // Referência ao script do escudo

    void Start()
    {
        vidaAtual = vidaMaxima;
        Debug.Log("Boss apareceu com " + vidaAtual + " de vida.");
    }

    // Sobrecarga mantida para compatibilidade (assume Melee por padrão)
    public void TomarDano(int dano)
    {
        TomarDano(dano, TipoDano.Melee);
    }

    public void TomarDano(int dano, TipoDano tipo)
    {
        if (morreu) return;

        // 🛡️ Se for projétil e o Boss estiver imune, cancela o dano
        if (tipo == TipoDano.Projetil && imuneAProjeteis)
        {
            Debug.Log("<color=cyan>[ESCUDO]</color> Boss bloqueou o projétil!");
            return;
        }

        vidaAtual -= dano;

        if (vidaAtual < 0)
            vidaAtual = 0;

        Debug.Log($"<color=red>[DANO]</color> Boss tomou {dano} de dano ({tipo}). Vida restante: {vidaAtual}");

        VerificarEventosDeVida();

        if (vidaAtual <= 0)
        {
            Morrer();
        }
    }

    void VerificarEventosDeVida()
    {
        float porcentagemVida = (float)vidaAtual / vidaMaxima * 100f;

        if (evento75 && porcentagemVida <= 75f)
        {
            evento75 = false;
            AtivarEvento();
        }

        if (evento50 && porcentagemVida <= 50f)
        {
            evento50 = false;
            AtivarEvento();
        }

        if (evento25 && porcentagemVida <= 25f)
        {
            evento25 = false;
            AtivarEvento();
        }
    }

    void AtivarEvento()
    {
        Debug.Log("<color=purple>[EVENTO ATIVADO]</color> Boss ativou o escudo refletor!");

        Parar();
        AtivarImunidadeProjeteis();

        // Ativa o escudo visual e a reflexão
        if (escudoRefletor != null)
        {
            escudoRefletor.AtivarEscudo();
        }
    }

    public void Parar()
    {
        parado = true;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;
        Debug.Log("BOSS PAROU!");
    }

    public void Continuar()
    {
        parado = false;
        Debug.Log("BOSS VOLTOU A ANDAR!");
    }

    public void AtivarImunidadeProjeteis()
    {
        imuneAProjeteis = true;
        Debug.Log("BOSS FICOU IMUNE A PROJÉTEIS!");
    }

    public void DesativarImunidadeProjeteis()
    {
        imuneAProjeteis = false;
        Debug.Log("BOSS VOLTOU A RECEBER PROJÉTEIS!");
    }

    public bool EstaImuneAProjeteis()
    {
        return imuneAProjeteis;
    }

    void Morrer()
    {
        if (morreu) return;
        morreu = true;
        Debug.Log("BOSS DERROTADO!");
        Destroy(gameObject);
    }
}