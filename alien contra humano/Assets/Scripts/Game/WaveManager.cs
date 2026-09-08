using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    [Header("Inimigos")]
    public GameObject[] inimigos;
    public Transform jogador;
    public Transform[] pontosSpawn;

    [Header("Waves")]
    public int numeroDeWaves = 3;
    public int inimigosPorWave = 5;
    public int aumentoPorWave = 2;

    [Header("Tempo")]
    public float tempoEntreWaves = 5f;
    public float intervaloEntreInimigos = 0.5f;

    private int waveAtual = 0;
    private int inimigosVivos = 0;

    private bool wavesAtivas = false;
    private bool spawnando = false;

    public void ComecarWaves()
    {
        if (wavesAtivas)
            return;

        wavesAtivas = true;

        Debug.Log("WAVES COMEÇARAM!");

        StartCoroutine(IniciarWaves());
    }

    IEnumerator IniciarWaves()
    {
        while (waveAtual < numeroDeWaves)
        {
            waveAtual++;

            Debug.Log("Preparando Wave " + waveAtual);

            yield return new WaitForSeconds(tempoEntreWaves);

            Debug.Log("COMEÇANDO WAVE " + waveAtual);

            yield return StartCoroutine(SpawnInimigos());

            Debug.Log(
                "Wave " + waveAtual +
                " terminou de spawnar. Inimigos vivos: " +
                inimigosVivos
            );

            yield return new WaitUntil(
                () => inimigosVivos <= 0
            );

            Debug.Log(
                "WAVE " + waveAtual +
                " TERMINOU!"
            );

            inimigosPorWave += aumentoPorWave;

            if (waveAtual < numeroDeWaves)
            {
                yield return new WaitForSeconds(1f);
            }
        }

        wavesAtivas = false;

        Debug.Log("TODAS AS WAVES TERMINARAM!");
    }

    IEnumerator SpawnInimigos()
    {
        spawnando = true;

        if (inimigos == null || inimigos.Length == 0)
        {
            Debug.LogError(
                "ERRO: Nenhum inimigo foi colocado!"
            );

            spawnando = false;
            yield break;
        }

        if (pontosSpawn == null || pontosSpawn.Length == 0)
        {
            Debug.LogError(
                "ERRO: Nenhum ponto de spawn foi colocado!"
            );

            spawnando = false;
            yield break;
        }

        for (int i = 0; i < inimigosPorWave; i++)
        {
            Transform ponto =
                pontosSpawn[
                    Random.Range(0, pontosSpawn.Length)
                ];

            GameObject prefab =
                inimigos[
                    Random.Range(0, inimigos.Length)
                ];

            if (ponto != null && prefab != null)
            {
                GameObject novoInimigo = Instantiate(
                    prefab,
                    ponto.position,
                    Quaternion.identity
                );

                inimigosVivos++;

                // Passa ESTE WaveManager para o inimigo
                Enemy enemy =
                    novoInimigo.GetComponent<Enemy>();

                if (enemy != null)
                {
                    enemy.DefinirWaveManager(this);
                }

                RangedEnemy ranged =
                    novoInimigo.GetComponent<RangedEnemy>();

                if (ranged != null)
                {
                    ranged.DefinirWaveManager(this);
                }

                Debug.Log(
                    "Inimigo spawnado. Restam: " +
                    inimigosVivos
                );
            }

            yield return new WaitForSeconds(
                intervaloEntreInimigos
            );
        }

        spawnando = false;
    }

    public void InimigoMorreu()
    {
        inimigosVivos--;

        if (inimigosVivos < 0)
            inimigosVivos = 0;

        Debug.Log(
            "Inimigo morreu. Restam: " +
            inimigosVivos
        );
    }

    public bool TerminouTodasAsWaves()
    {
        return waveAtual >= numeroDeWaves &&
               inimigosVivos <= 0 &&
               !wavesAtivas;
    }
}