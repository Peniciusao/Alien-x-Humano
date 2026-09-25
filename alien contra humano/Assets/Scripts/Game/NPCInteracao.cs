using UnityEngine;

public class NPCInteracao : MonoBehaviour
{
    [Header("Elementos Visuais")]
    [Tooltip("Coloque aqui o objeto que tem o texto 'Pressione E'")]
    public GameObject avisoPressioneE;

    [Tooltip("Coloque aqui o objeto da Caixa de Texto do NPC")]
    public GameObject caixaDeTexto;

    private bool jogadorPorPerto = false;

    void Start()
    {
        // Esconde os textos quando o jogo começa
        if (avisoPressioneE != null) avisoPressioneE.SetActive(false);
        if (caixaDeTexto != null) caixaDeTexto.SetActive(false);
    }

    void Update()
    {
        // Verifica se o jogador está na área do collider E apertou a tecla E
        if (jogadorPorPerto && Input.GetKeyDown(KeyCode.E))
        {
            // Alterna a caixa de texto (se estiver desligada, liga. Se ligada, desliga)
            bool estadoAtual = caixaDeTexto.activeSelf;
            caixaDeTexto.SetActive(!estadoAtual);

            // Esconde o aviso "Pressione E" enquanto a caixa de texto estiver aberta
            avisoPressioneE.SetActive(estadoAtual);
        }
    }

    private void OnTriggerEnter2D(Collider2D outro)
    {
        // Quando o jogador entra na área de contato
        if (outro.CompareTag("Player"))
        {
            jogadorPorPerto = true;
            avisoPressioneE.SetActive(true); // Mostra o aviso
        }
    }

    private void OnTriggerExit2D(Collider2D outro)
    {
        // Quando o jogador sai da área de contato
        if (outro.CompareTag("Player"))
        {
            jogadorPorPerto = false;

            // Esconde tudo automaticamente se o jogador for embora
            avisoPressioneE.SetActive(false);
            caixaDeTexto.SetActive(false);
        }
    }
}