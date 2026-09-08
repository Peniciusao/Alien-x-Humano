using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour
{
    [Header("Player")]
    public Transform jogador;

    [Header("Distâncias")]
    public float distanciaPerto = 3f;

    [Header("Tempo entre decisões")]
    public float tempoMinimo = 1.5f;
    public float tempoMaximo = 3f;

    [Header("Ataques")]
    public bool ataqueArea = true;
    public bool ataqueLaser = true;

    private float proximaDecisao;

    private int ultimoAtaque = -1;
    private int ataquesSeguidos = 0;

    private bool decidindo = false;

    void Start()
    {
        if (jogador == null)
        {
            GameObject player =
                GameObject.FindGameObjectWithTag("Player");

            if (player != null)
                jogador = player.transform;
        }

        proximaDecisao =
            Time.time + Random.Range(1f, 2f);
    }

    void Update()
    {
        if (jogador == null)
            return;

        if (decidindo)
            return;

        if (Time.time >= proximaDecisao)
        {
            StartCoroutine(TomarDecisao());
        }
    }

    IEnumerator TomarDecisao()
    {
        decidindo = true;

        float distancia =
            Vector2.Distance(
                transform.position,
                jogador.position
            );

        int ataqueEscolhido = EscolherAtaque(distancia);

        ExecutarAtaque(ataqueEscolhido);

        // Espera antes de decidir novamente
        float tempo =
            Random.Range(
                tempoMinimo,
                tempoMaximo
            );

        yield return new WaitForSeconds(tempo);

        proximaDecisao = Time.time;

        decidindo = false;
    }

    int EscolherAtaque(float distancia)
    {
        // Se só existe um ataque disponível
        if (ataqueArea && !ataqueLaser)
            return 0;

        if (!ataqueArea && ataqueLaser)
            return 1;

        // Se existem os dois
        if (ataqueArea && ataqueLaser)
        {
            // Se usou o mesmo ataque duas vezes seguidas,
            // força o outro ataque.
            if (ataquesSeguidos >= 2)
            {
                if (ultimoAtaque == 0)
                    return 1;

                return 0;
            }

            // Perto = maior chance de ataque de área
            if (distancia <= distanciaPerto)
            {
                int chance = Random.Range(0, 100);

                if (chance < 70)
                    return 0;
                else
                    return 1;
            }

            // Longe = maior chance de laser,
            // mas ainda pode escolher área.
            else
            {
                int chance = Random.Range(0, 100);

                if (chance < 65)
                    return 1;
                else
                    return 0;
            }
        }

        return -1;
    }

    void ExecutarAtaque(int ataque)
    {
        if (ataque == -1)
            return;

        // Ataque de área
        if (ataque == 0)
        {
            Debug.Log("BOSS ESCOLHEU: ATAQUE DE ÁREA");

            BossAreaAttack area =
                GetComponent<BossAreaAttack>();

            if (area != null)
            {
                area.Atacar();
            }
        }

        // Laser
        else if (ataque == 1)
        {
            Debug.Log("BOSS ESCOLHEU: LASER");

            BossLaser laser =
                GetComponent<BossLaser>();

            if (laser != null)
            {
                laser.Atacar();
            }
        }

        // Guarda o ataque utilizado
        if (ataque == ultimoAtaque)
        {
            ataquesSeguidos++;
        }
        else
        {
            ataquesSeguidos = 1;
        }

        ultimoAtaque = ataque;
    }
}