using UnityEngine;
using System.Collections.Generic;

public class Hitbox : MonoBehaviour
{
    public int dano = 1;

    // Guarda quem já foi atingido neste ataque
    private List<GameObject> atingidos = new List<GameObject>();

    void OnEnable()
    {
        // Novo ataque = limpa a lista
        atingidos.Clear();
    }

    void OnTriggerEnter2D(Collider2D outro)
    {
        // Procura o objeto principal do inimigo
        Enemy inimigo = outro.GetComponentInParent<Enemy>();

        if (inimigo != null)
        {
            GameObject alvo = inimigo.gameObject;

            if (!atingidos.Contains(alvo))
            {
                inimigo.TomarDano(dano);
                atingidos.Add(alvo);
            }

            return;
        }

        // Procura o RangedEnemy
        RangedEnemy ranged = outro.GetComponentInParent<RangedEnemy>();

        if (ranged != null)
        {
            GameObject alvo = ranged.gameObject;

            if (!atingidos.Contains(alvo))
            {
                ranged.TomarDano(dano);
                atingidos.Add(alvo);
            }

            return;
        }

        // Procura o Boss
        Boss boss = outro.GetComponentInParent<Boss>();

        if (boss != null)
        {
            GameObject alvo = boss.gameObject;

            if (!atingidos.Contains(alvo))
            {
                Debug.Log("BOSS ATINGIDO!");
                boss.TomarDano(dano);
                atingidos.Add(alvo);
            }
        }
    }
}