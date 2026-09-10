using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public enum TipoDano { Melee, Projetil }

    [Header("UI")]
    public Slider barraDeVidaUI;

    [Header("Vida do Boss")]
    public int vidaMaxima = 100;
    private int vidaAtual;
    private bool morreu = false;

    [Header("Estado")]
    public bool imuneAProjeteis = false; // 🔵 Se true, pisca azul ao levar tiro e ignora dano
    public bool parado = false;

    [Header("Eventos de Vida")]
    public bool evento75 = true;
    public bool evento50 = true;
    public bool evento25 = true;

    [Header("Componentes Visuais")]
    public SpriteRenderer spriteRenderer;
    private Coroutine corrotinaPiscar;
    private Color corOriginal = Color.white;

    void Start()
    {
        // 1. Configura o SpriteRenderer para o efeito de piscar
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
            corOriginal = spriteRenderer.color;

        // 2. Localiza a barra de vida na cena (mesmo desativada)
        if (barraDeVidaUI == null)
        {
            Slider[] slidersNaCena = Resources.FindObjectsOfTypeAll<Slider>();
            foreach (Slider s in slidersNaCena)
            {
                if (s.gameObject.name == "BarraDeVidaBoss" && s.gameObject.scene.isLoaded)
                {
                    barraDeVidaUI = s;
                    break;
                }
            }
        }

        // 3. Inicializa os valores de vida
        vidaAtual = vidaMaxima;

        if (barraDeVidaUI != null)
        {
            barraDeVidaUI.maxValue = vidaMaxima;
            barraDeVidaUI.value = vidaAtual;
        }
    }

    // Sobrecarga mantida para ataques Melee padrão
    public void TomarDano(int dano)
    {
        TomarDano(dano, TipoDano.Melee);
    }

    // Método principal de dano com verificação de imunidade e piscar de cor
    public void TomarDano(int dano, TipoDano tipo)
    {
        if (morreu) return;

        // 🔵 Se for projétil E o Boss estiver invulnerável a tiros:
        if (tipo == TipoDano.Projetil && imuneAProjeteis)
        {
            Debug.Log("<color=cyan>[BLOQUEADO]</color> Boss invulnerável a tiros!");
            PiscarCor(Color.cyan); // Pisca em azul ciano
            return;
        }

        vidaAtual -= dano;
        if (vidaAtual < 0) vidaAtual = 0;

        if (barraDeVidaUI != null)
        {
            barraDeVidaUI.value = vidaAtual;
        }

        Debug.Log($"<color=red>[DANO]</color> Boss tomou {dano} de dano ({tipo}). Vida restante: {vidaAtual}");

        VerificarEventosDeVida();

        if (vidaAtual <= 0)
        {
            if (barraDeVidaUI != null)
                barraDeVidaUI.gameObject.SetActive(false);

            Morrer();
        }
    }

    // 🔵 Lógica do piscar visual
    void PiscarCor(Color cor)
    {
        if (spriteRenderer == null) return;

        if (corrotinaPiscar != null)
            StopCoroutine(corrotinaPiscar);

        corrotinaPiscar = StartCoroutine(EfeitoPiscar(cor));
    }

    IEnumerator EfeitoPiscar(Color cor)
    {
        spriteRenderer.color = cor;
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.color = corOriginal;
    }

    void VerificarEventosDeVida()
    {
        float porcentagemVida = (float)vidaAtual / vidaMaxima * 100f;

        if (evento75 && porcentagemVida <= 75f) { evento75 = false; AtivarEvento(); }
        if (evento50 && porcentagemVida <= 50f) { evento50 = false; AtivarEvento(); }
        if (evento25 && porcentagemVida <= 25f) { evento25 = false; AtivarEvento(); }
    }

    void AtivarEvento()
    {
        Debug.Log("<color=purple>[EVENTO ATIVADO]</color> Boss entrou na fase defensiva!");
        Parar();
        AtivarImunidadeProjeteis();
    }

    // 🔴 MÉTODOS DE CONTROLE RESTAURADOS PARA EVITAR ERROS EM OUTROS SCRIPTS:

    public void Parar()
    {
        parado = true;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }

    public void Continuar()
    {
        parado = false;
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