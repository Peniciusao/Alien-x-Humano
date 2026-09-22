using System.Collections;
using UnityEngine;

public class GeradorDeObstaculos : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] obstaculos; // Arraste seus asteroides/inimigos aqui

    [Header("Configurações de Spawn")]
    public float tempoEntreSpawns = 2f;

    [Tooltip("Altura máxima e mínima onde o obstáculo pode nascer na tela")]
    public float alturaMinima = -4f;
    public float alturaMaxima = 4f;

    void Start()
    {
        // Inicia a rotina de criar obstáculos infinitamente
        StartCoroutine(SpawnarRotina());
    }

    IEnumerator SpawnarRotina()
    {
        while (true)
        {
            // Espera o tempo definido
            yield return new WaitForSeconds(tempoEntreSpawns);
            Spawnar();
        }
    }

    void Spawnar()
    {
        if (obstaculos.Length == 0) return;

        // Escolhe um obstáculo aleatório da lista
        int indexAleatorio = Random.Range(0, obstaculos.Length);
        GameObject obstaculoEscolhido = obstaculos[indexAleatorio];

        // Sorteia uma altura (eixo Y)
        float altura = Random.Range(alturaMinima, alturaMaxima);

        // Define a posição (X do gerador, Y sorteado, Z zero)
        Vector3 posicaoSpawn = new Vector3(transform.position.x, altura, 0f);

        // Cria o obstáculo na cena
        Instantiate(obstaculoEscolhido, posicaoSpawn, Quaternion.identity);
    }
}