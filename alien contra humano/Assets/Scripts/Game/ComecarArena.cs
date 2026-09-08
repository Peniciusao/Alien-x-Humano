using UnityEngine;

public class ComecarArena : MonoBehaviour
{
    public WaveManager waveManager;

    private bool ativou = false;

    private void OnTriggerEnter2D(Collider2D outro)
    {
        if (ativou)
            return;

        if (outro.CompareTag("Player"))
        {
            ativou = true;

            Debug.Log("PLAYER ENTROU NA ARENA 2!");

            if (waveManager != null)
            {
                waveManager.ComecarWaves();
            }
        }
    }
}