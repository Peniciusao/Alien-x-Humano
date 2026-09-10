using UnityEngine;

public class GatilhoArenaBoss : MonoBehaviour
{
    [Header("Referências")]
    public GameObject barraDeVidaBoss;

    [Header("Configurações")]
    public bool destruirAposAtivar = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se quem entrou na arena foi o Jogador
        if (collision.CompareTag("Player"))
        {
            if (barraDeVidaBoss != null)
            {
                barraDeVidaBoss.SetActive(true);
                Debug.Log("<color=yellow>[ARENA]</color> Início da batalha contra o Boss!");
            }

            // Destrói o gatilho para não ativar repetidamente
            if (destruirAposAtivar)
            {
                Destroy(gameObject);
            }
        }
    }
}