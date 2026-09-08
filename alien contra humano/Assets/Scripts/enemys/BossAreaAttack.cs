using UnityEngine;
using System.Collections;

public class BossAreaAttack : MonoBehaviour
{
    [Header("Ataque de Área")]
    public float alcance = 2.5f;
    public int dano = 10;
    public float tempoAviso = 0.8f; // Tempo que a área pisca/avisa antes do dano

    [Header("Cores do Aviso")]
    public Color corAviso = new Color(1f, 0.5f, 0f, 0.4f); // Laranja transparente
    public Color corAtaque = Color.red;                    // Vermelho forte

    [Header("Visual da Área")]
    public bool mostrarArea = true;
    public float espessuraCirculo = 0.05f;
    public int quantidadePontos = 64;

    private LineRenderer circulo;
    private bool atacando = false;

    void Start()
    {
        CriarCirculo();
    }

    public void Atacar()
    {
        if (!atacando)
            StartCoroutine(ExecutarAtaqueDeArea());
    }

    IEnumerator ExecutarAtaqueDeArea()
    {
        atacando = true;

        // 1. FASE DE AVISO (Mostra a área piscando/mudando de cor)
        if (circulo != null)
        {
            circulo.enabled = true;
            circulo.startColor = corAviso;
            circulo.endColor = corAviso;
        }

        yield return new WaitForSeconds(tempoAviso);

        // 2. FASE DE IMPACTO (Muda para cor forte e calcula dano)
        if (circulo != null)
        {
            circulo.startColor = corAtaque;
            circulo.endColor = corAtaque;
        }

        VerificarEAplicarDano();

        // Breve pausa para mostrar o impacto visual
        yield return new WaitForSeconds(0.2f);

        // 3. FIM DO ATAQUE
        if (circulo != null && !mostrarArea)
            circulo.enabled = false;

        atacando = false;
    }

    void VerificarEAplicarDano()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null) return;

        float distancia = Vector2.Distance(transform.position, player.transform.position);

        // Só acerta se o Player estiver dentro da área
        if (distancia <= alcance)
        {
            PlayerHealth vida = player.GetComponent<PlayerHealth>();

            if (vida != null)
            {
                vida.TomarDano(dano);

                // 🎯 AVISO NO CONSOLE DA UNITY (Com cor destacada)
                Debug.Log($"<color=orange>[ALERTA]</color> O Boss acertou {player.name} com o Ataque de Área causando {dano} de dano!");
            }
        }
        else
        {
            // Aviso de esquiva no console
            Debug.Log($"<color=yellow>[ESQUIVA]</color> {player.name} ficou fora do alcance da área ({distancia:F1}m / {alcance}m) e não tomou dano!");
        }
    }

    void CriarCirculo()
    {
        GameObject objetoCirculo = new GameObject("AreaVisualBoss");
        objetoCirculo.transform.SetParent(transform);
        objetoCirculo.transform.localPosition = Vector3.zero;

        circulo = objetoCirculo.AddComponent<LineRenderer>();
        circulo.useWorldSpace = false;
        circulo.loop = true;
        circulo.positionCount = quantidadePontos;
        circulo.startWidth = espessuraCirculo;
        circulo.endWidth = espessuraCirculo;

        Shader shader = Shader.Find("Sprites/Default");
        if (shader != null)
            circulo.material = new Material(shader);

        AtualizarCirculo();
        circulo.enabled = mostrarArea;
    }

    void AtualizarCirculo()
    {
        if (circulo == null) return;

        for (int i = 0; i < quantidadePontos; i++)
        {
            float angulo = 2f * Mathf.PI * i / quantidadePontos;
            float x = Mathf.Cos(angulo) * alcance;
            float y = Mathf.Sin(angulo) * alcance;

            circulo.SetPosition(i, new Vector3(x, y, 0f));
        }
    }

    void OnValidate()
    {
        if (quantidadePontos < 8) quantidadePontos = 8;
        if (espessuraCirculo < 0.001f) espessuraCirculo = 0.001f;

        if (circulo != null)
        {
            circulo.positionCount = quantidadePontos;
            AtualizarCirculo();
        }
    }
}