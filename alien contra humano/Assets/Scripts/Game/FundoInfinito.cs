using UnityEngine;

public class FundoInfinito : MonoBehaviour
{
    [Header("Configuração")]
    public float velocidade = 3f;

    private float larguraDaImagem;
    private Vector3 posicaoInicial;

    void Start()
    {
        // Salva de onde a imagem começou
        posicaoInicial = transform.position;

        // Calcula a largura exata da imagem automaticamente
        larguraDaImagem = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        // Cria um loop matemático que vai de 0 até a largura da imagem
        float deslocamento = Mathf.Repeat(Time.time * velocidade, larguraDaImagem);

        // Move o cenário para a esquerda com base no loop
        transform.position = posicaoInicial + Vector3.left * deslocamento;
    }
}