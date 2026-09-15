using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int vida = 10;
    public int vidatotaldoplayer = 20;

    public PlayerHealthBar healthBar;

    void Start()
    { 
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(vidatotaldoplayer);
        }
    }

    public void TomarDano(int dano)
    {
        vida -= dano;

        Debug.Log("Vida do jogador: " + vida);

        if (healthBar != null)
        {
            healthBar.SetHealth(vida);
        }

        if (vida <= 0)
        {
            Morrer();
        }
    }

    public void RecuperarVidaTotal()
    {
        vida = vidatotaldoplayer;

        Debug.Log("Jogador recuperou toda a vida!");
    }

    void Morrer()
    {
        Debug.Log("Jogador morreu");
        SceneManager.LoadScene("MOrte");
        Destroy(gameObject);
    }
}