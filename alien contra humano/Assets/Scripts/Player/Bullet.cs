using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int dano = 1;
    public float velocidade = 10f;

    private Vector2 direcao;

    public void DefinirDirecao(Vector2 novaDirecao)
    {
        direcao = novaDirecao.normalized;

        float angulo =
            Mathf.Atan2(
                direcao.y,
                direcao.x
            ) * Mathf.Rad2Deg;

        transform.rotation =
            Quaternion.Euler(
                0f,
                0f,
                angulo
            );
    }

    void Update()
    {
        transform.position +=
            (Vector3)direcao *
            velocidade *
            Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D outro)
    {
        // 👾 Inimigo normal
        Enemy inimigo =
            outro.GetComponentInParent<Enemy>();

        if (inimigo != null)
        {
            inimigo.TomarDano(dano);
            Destroy(gameObject);
            return;
        }

        // 👾 Inimigo ranged
        RangedEnemy ranged =
            outro.GetComponentInParent<RangedEnemy>();

        if (ranged != null)
        {
            ranged.TomarDano(dano);
            Destroy(gameObject);
            return;
        }

        // 👹 Boss
        Boss boss =
            outro.GetComponentInParent<Boss>();

        if (boss != null)
        {
            // 🛡️ Boss está imune a projéteis
            if (boss.EstaImuneAProjeteis())
            {
                Debug.Log(
                    "PROJÉTIL BLOQUEADO! BOSS ESTÁ IMUNE."
                );

                Destroy(gameObject);
                return;
            }

            // 💥 Boss recebe dano normalmente
            boss.TomarDano(dano);

            Destroy(gameObject);
            return;
        }
    }
}