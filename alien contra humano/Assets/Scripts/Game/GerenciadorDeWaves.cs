using UnityEngine;
using TMPro; // Importante para controlar o TextMeshPro da UI

public class GerenciadorDeWaves : MonoBehaviour
{
    [Header("Referências da UI")]
    [Tooltip("Arraste aqui o texto do Canvas que mostra o nome da área")]
    public TextMeshProUGUI textoArea;

    [Tooltip("Arraste aqui o texto do Canvas que mostra o número da wave")]
    public TextMeshProUGUI textoWave;

    [Header("Configurações da Área Atual")]
    public string nomeDaArea = "Setor 1 - Espaço Profundo";
    public int totalDeWaves = 5;

    private int waveAtual = 1;

    void Start()
    {
        AtualizarUI();
    }

    // Chama esta função para avançar de wave
    public void ProximaWave()
    {
        if (waveAtual < totalDeWaves)
        {
            waveAtual++;
            AtualizarUI();
        }
        else
        {
            Debug.Log("Área concluída! Hora do Boss ou Mudança de Área.");
        }
    }

    // Chama esta função quando o jogador entrar em uma nova área
    public void NovaArea(string novoNome, int quantidadeDeWaves)
    {
        nomeDaArea = novoNome;
        totalDeWaves = quantidadeDeWaves;
        waveAtual = 1;
        AtualizarUI();
    }

    // Atualiza os componentes de texto na tela
    private void AtualizarUI()
    {
        if (textoArea != null)
        {
            textoArea.text = nomeDaArea;
        }

        if (textoWave != null)
        {
            textoWave.text = $"WAVE {waveAtual}/{totalDeWaves}";
        }
    }
}