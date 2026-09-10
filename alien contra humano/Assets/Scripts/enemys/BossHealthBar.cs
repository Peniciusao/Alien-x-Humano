using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [Header("Componentes UI")]
    public Slider barraDeVida;

    [Header("Referência ao Boss")]
    public Boss boss;

    void Start()
    {
        if (boss != null && barraDeVida != null)
        {
            // Configura o valor máximo com a vida do Boss
            barraDeVida.maxValue = boss.vidaMaxima;
            barraDeVida.value = boss.vidaMaxima;
        }
    }

    public void AtualizarVida(int vidaAtual)
    {
        if (barraDeVida != null)
        {
            barraDeVida.value = vidaAtual;
        }
    }

    public void OcultarBarra()
    {
        gameObject.SetActive(false);
    }
}