using UnityEngine;
using UnityEngine.SceneManagement; // Necessário para reiniciar a cena se o player morrer

public class Obstaculo : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 6f;
    public float limiteEsquerdo = -15f;

    [Header("Efeitos Visuais (Opcional)")]
    public GameObject efeitoExplosao;

    void Update()
    {
        // Move o obstáculo para a esquerda
        transform.Translate(Vector3.left * velocidade * Time.deltaTime);

        // Se sair da tela, destrói o objeto
        if (transform.position.x <= limiteEsquerdo)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.CompareTag("Player"))
        {
            // Se você tiver uma partícula de explosão, instancia no lugar do player
            if (efeitoExplosao != null)
            {
                Instantiate(efeitoExplosao, outro.transform.position, Quaternion.identity);
            }

            // Destrói o navinha do player
            Destroy(outro.gameObject);

            // Reinicia a cena atual instantaneamente após o impacto
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            // DICA: Se preferir abrir uma tela de Game Over em vez de reiniciar direto,
            // você pode chamar um script gerenciador aqui (ex: GameManager.instancia.GameOver();)
        }
    }
}