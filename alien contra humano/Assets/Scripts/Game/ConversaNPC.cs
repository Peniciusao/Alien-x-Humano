using UnityEngine;
using TMPro;

public class ConversaNPC : MonoBehaviour
{
    public GameObject caixaDeTexto; // O painel da caixa
    public TextMeshProUGUI textoDoDialogo; // O texto da caixa
    public string mensagem = "Olá! Presione 'E' para fechar.";

    private bool jogadorPerto = false;

    void Update()
    {
        // Se o jogador estiver perto E apertar a tecla E
        if (jogadorPerto && Input.GetKeyDown(KeyCode.E))
        {
            // Inverte: se estiver aberto fecha, se estiver fechado abre
            bool estaAtivo = caixaDeTexto.activeSelf;
            caixaDeTexto.SetActive(!estaAtivo);

            if (!estaAtivo)
            {
                textoDoDialogo.text = mensagem;
            }
        }
    }

    // Quando o jogador entra no sensor
    private void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.CompareTag("Player"))
        {
            jogadorPerto = true;
        }
    }

    // Quando o jogador sai do sensor
    private void OnTriggerExit2D(Collider2D outro)
    {
        if (outro.CompareTag("Player"))
        {
            jogadorPerto = false;
            caixaDeTexto.SetActive(false); // Esconde a caixa
        }
    }
}