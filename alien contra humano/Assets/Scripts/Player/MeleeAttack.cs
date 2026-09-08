using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class MeleeAttack : MonoBehaviour
{
    public GameObject hitbox;

    public float distancia = 1.2f;
    public float duracaoAtaque = 0.15f;
    public float cooldown = 0.3f;

    private bool podeAtacar = true;

    void Start()
    {
        // Garante que começa desligado
        hitbox.SetActive(false);
    }

    void Update()
    {
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame &&
            podeAtacar)
        {
            StartCoroutine(Atacar());
        }
    }

    IEnumerator Atacar()
    {
        podeAtacar = false;

        // GARANTE que o hitbox está desligado
        hitbox.SetActive(false);

        // Espera 1 frame para a física reconhecer a saída do trigger
        yield return null;

        Vector2 mouseTela = Mouse.current.position.ReadValue();

        Vector3 mouseMundo = Camera.main.ScreenToWorldPoint(
            new Vector3(
                mouseTela.x,
                mouseTela.y,
                -Camera.main.transform.position.z
            )
        );

        // Jogador -> mouse
        Vector2 direcao =
            ((Vector2)mouseMundo - (Vector2)transform.position).normalized;

        // Posiciona o hitbox
        hitbox.transform.position =
            transform.position +
            (Vector3)(direcao * distancia);

        // Rotaciona para o mouse
        float angulo =
            Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;

        hitbox.transform.rotation =
            Quaternion.Euler(0f, 0f, angulo);

        // Ativa o ataque
        hitbox.SetActive(true);

        // Duração do golpe
        yield return new WaitForSeconds(duracaoAtaque);

        // Desliga o ataque
        hitbox.SetActive(false);

        // Cooldown
        yield return new WaitForSeconds(cooldown);

        podeAtacar = true;
    }
}