using System.Collections.Generic;
using UnityEngine;

public class GerenciadorDeChao : MonoBehaviour
{
    [Header("Prefabs dos Pedaços de Chão")]
    public GameObject[] prefabsDeChao;

    [Header("Configurações do Movimento")]
    public float velocidade = 5f;
    public float larguraDoPedaco = 10f; // Largura no eixo X de cada bloco
    public int blocosVisiveis = 3;     // Quantidade de blocos mantida na tela

    [Header("Destruição")]
    public float limiteEsquerdo = -15f; // Posição X onde o bloco é apagado

    private List<GameObject> blocosAtivos = new List<GameObject>();
    private float proximaPosicaoX = 0f;

    void Start()
    {
        // Spawna os blocos iniciais para preencher a tela
        for (int i = 0; i < blocosVisiveis; i++)
        {
            SpawnarBloco();
        }
    }

    void Update()
    {
        // Move todos os blocos para a esquerda
        for (int i = blocosAtivos.Count - 1; i >= 0; i--)
        {
            GameObject bloco = blocosAtivos[i];
            bloco.transform.Translate(Vector3.left * velocidade * Time.deltaTime);

            // Quando o bloco sai da tela pela esquerda
            if (bloco.transform.position.x <= limiteEsquerdo)
            {
                // Ajusta a posição onde o próximo bloco vai nascer para não deixar buracos
                proximaPosicaoX -= larguraDoPedaco;

                blocosAtivos.RemoveAt(i);
                Destroy(bloco);

                // Cria um novo bloco lá na frente à direita
                SpawnarBloco();
            }
        }
    }

    void SpawnarBloco()
    {
        if (prefabsDeChao.Length == 0) return;

        // Escolhe um pedaço aleatório do array
        int indexAleatorio = Random.Range(0, prefabsDeChao.Length);

        // Instancia o bloco na posição correta
        GameObject novoBloco = Instantiate(prefabsDeChao[indexAleatorio]);
        novoBloco.transform.position = new Vector3(proximaPosicaoX, transform.position.y, 0f);

        // Prepara o X do próximo bloco
        proximaPosicaoX += larguraDoPedaco;

        blocosAtivos.Add(novoBloco);
    }
}