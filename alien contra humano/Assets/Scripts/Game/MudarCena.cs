using UnityEngine;
using UnityEngine.SceneManagement; // Essencial para gerenciar cenas

public class MudarCena : MonoBehaviour
{
    // Forma recomendada e corrigida:
    public void CarregarCena(string nomeDaCena)
    {
        SceneManager.LoadScene(nomeDaCena);
    }
}