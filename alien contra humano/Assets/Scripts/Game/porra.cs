using UnityEngine;

public class porra : MonoBehaviour
{
    public GameObject barreiraEsquerda;
    public GameObject barreiraDireita;

    [Header("UI do Boss")]
    public GameObject barraDeVidaBoss;

    public WaveManager waveManager;

    private bool areaAtivada = false;
    private bool areaTerminou = false;

    void Start()
    {
        barreiraEsquerda.SetActive(false);
        barreiraDireita.SetActive(false);

        if (barraDeVidaBoss != null)
            barraDeVidaBoss.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D outro)
    {
        if (areaAtivada)
            return;

        if (outro.CompareTag("Player"))
        {
            areaAtivada = true;

            barreiraEsquerda.SetActive(true);
            barreiraDireita.SetActive(true);

            if (barraDeVidaBoss != null)
                barraDeVidaBoss.SetActive(true);

            waveManager.ComecarWaves();
        }
    }

    void Update()
    {
        if (!areaAtivada || areaTerminou)
            return;

        if (waveManager.TerminouTodasAsWaves())
        {
            AbrirArea();
        }
    }

    void AbrirArea()
    {
        areaTerminou = true;

        barreiraEsquerda.SetActive(false);
        barreiraDireita.SetActive(false);

        if (barraDeVidaBoss != null)
            barraDeVidaBoss.SetActive(false);

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            PlayerHealth vida = player.GetComponent<PlayerHealth>();

            if (vida != null)
            {
                vida.RecuperarVidaTotal();
                Debug.Log("PLAYER RECUPEROU TODA A VIDA!");
            }
        }

        Debug.Log("Área concluída!");
    }
}