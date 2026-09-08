using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    public int dano = 1;

    void OnTriggerEnter2D(Collider2D outro)
    {
        // Ignora inimigos
        if (outro.GetComponentInParent<Enemy>() != null)
            return;

        if (outro.GetComponentInParent<RangedEnemy>() != null)
            return;

        // Procura o Player
        PlayerHealth player = outro.GetComponentInParent<PlayerHealth>();

        if (player != null)
        {
            player.TomarDano(dano);
            Destroy(gameObject);
        }
    }
}