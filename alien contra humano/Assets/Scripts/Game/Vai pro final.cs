using UnityEngine;
using UnityEngine.SceneManagement;

public class Vaiprofinal : MonoBehaviour
{
        [Header("Configuração da Cena")]
        [Tooltip("Digite exatamente o nome da cena que você quer carregar")]
        public string nomeDaProximaCena;

    // Detecta quando algo entra no Trigger (para jogos 2D)
    private void OnTriggerEnter2D(Collider2D outro)
    {
        // Verifica se o objeto que tocou tem a tag "Player"
        if (outro.CompareTag("Player"))
        {
            Debug.Log("Player tocou no portal! Carregando cena: " + nomeDaProximaCena);

            // Carrega a nova cena
            SceneManager.LoadScene(nomeDaProximaCena);
        }
    }
}

