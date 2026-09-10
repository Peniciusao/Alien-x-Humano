using UnityEngine;
using System.Collections;

public class BossAreaAttack : MonoBehaviour
{
    [Header("Componentes de Área")]
    public SpriteRenderer areaSprite;
    public BoxCollider2D hitboxArea;

    [Header("Configurações do Ataque")]
    public int dano = 20;
    public float tempoAviso = 1.2f;
    public float duracaoAtaque = 0.4f;

    [Header("Cores")]
    public Color corAviso = new Color(1f, 0.64f, 0f, 0.3f);
    public Color corAtaque = new Color(1f, 0.27f, 0f, 0.8f);

    private bool atacando = false;
    private bool playerAtingido = false;

    void Start()
    {
        DesativarArea();
    }

    public void Atacar()
    {
        gameObject.SetActive(true);

        if (atacando)
            return;

        StartCoroutine(DispararArea());
    }

    IEnumerator DispararArea()
    {
        atacando = true;
        playerAtingido = false;

        if (areaSprite != null)
        {
            areaSprite.color = corAviso;
            areaSprite.gameObject.SetActive(true);
        }

        if (hitboxArea != null)
            hitboxArea.enabled = false;

        yield return new WaitForSeconds(tempoAviso);

        if (areaSprite != null)
        {
            areaSprite.color = corAtaque;
        }

        if (hitboxArea != null)
            hitboxArea.enabled = true;

        float tempoFimAtaque = Time.time + duracaoAtaque;

        while (Time.time < tempoFimAtaque)
        {
            VerificarHitboxArea();
            yield return null;
        }

        DesativarArea();
        atacando = false;
    }

    void DesativarArea()
    {
        if (areaSprite != null)
            areaSprite.gameObject.SetActive(false);

        if (hitboxArea != null)
            hitboxArea.enabled = false;

        gameObject.SetActive(false);
    }

    void VerificarHitboxArea()
    {
        if (hitboxArea == null || playerAtingido) return;

        Vector2 centro = hitboxArea.transform.TransformPoint(hitboxArea.offset);
        Vector2 tamanho = Vector2.Scale(hitboxArea.size, hitboxArea.transform.lossyScale);

        Collider2D[] objetos = Physics2D.OverlapBoxAll(centro, tamanho, hitboxArea.transform.eulerAngles.z);

        foreach (Collider2D objeto in objetos)
        {
            PlayerHealth vida = objeto.GetComponentInParent<PlayerHealth>();
            if (vida != null)
            {
                vida.TomarDano(dano);
                playerAtingido = true;
                break;
            }
        }
    }
}