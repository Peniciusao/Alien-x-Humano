using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransicaoDeCena : MonoBehaviour
{
    [Header("Configuração da Cena")]
    [Tooltip("Nome exato da cena que será carregada")]
    public string nomeDaProximaCena;

    [Header("Configuração da Animação")]
    [Tooltip("Objeto que possui o componente Animator (ex: Tela de Fade/Transição)")]
    public Animator animator;

    [Tooltip("Nome do gatilho (Trigger) criado no Animator")]
    public string triggerAnimacao = "IniciarTransicao";

    [Tooltip("Tempo (em segundos) que a animação dura até trocar a cena")]
    public float tempoDeEspera = 1.5f;

    private bool jaFoiAcionado = false;

    private void OnTriggerEnter2D(Collider2D outro)
    {
        // Garante que apenas o Player ativa a transição e que ela roda apenas uma vez
        if (outro.CompareTag("Player") && !jaFoiAcionado)
        {
            jaFoiAcionado = true;
            StartCoroutine(MudarCenaComAnimacao());
        }
    }

    private IEnumerator MudarCenaComAnimacao()
    {
        // 1. Toca a animação se um Animator foi passado
        if (animator != null)
        {
            animator.SetTrigger(triggerAnimacao);
        }

        // 2. Aguarda a duração da animação
        yield return new WaitForSeconds(tempoDeEspera);

        // 3. Carrega a cena escolhida no Inspector
        SceneManager.LoadScene(nomeDaProximaCena);
    }
}