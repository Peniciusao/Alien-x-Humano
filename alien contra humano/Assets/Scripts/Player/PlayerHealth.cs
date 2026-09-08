using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int vida = 10;
    public int vidatotaldoplayer = 20;

    public void TomarDano(int dano)
    {
        vida -= dano;

        Debug.Log("Vida do jogador: " + vida);

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
        Destroy(gameObject);
    }
}