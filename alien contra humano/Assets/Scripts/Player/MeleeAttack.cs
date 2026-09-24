using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class MeleeAttack : MonoBehaviour
{
    public GameObject hitbox;
    public float distancia = 1.2f;
    [Min(0.01f)] public float duracaoAtaque = 0.15f;
    [Min(0f)] public float cooldown = 0.3f;

    [Header("Animação do ataque")]
    [Tooltip("Animator do visual do jogador. Se estiver vazio, procura nos filhos.")]
    public Animator animator;
    [Tooltip("Parâmetro do tipo Trigger usado para iniciar a animação de ataque.")]
    public string gatilhoAtaque = "MeleeAttack";
    [Tooltip("Tempo entre o início da animação e a ativação do dano. Ajuste ao momento do impacto.")]
    [Min(0f)] public float atrasoImpacto = 0f;

    private bool podeAtacar = true;

    void Awake()
    {
        if (hitbox != null) hitbox.SetActive(false);
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Time.timeScale > 0f && Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame &&
            podeAtacar && hitbox != null && Camera.main != null)
            StartCoroutine(Atacar());
    }

    IEnumerator Atacar()
    {
        podeAtacar = false;
        hitbox.SetActive(false);
        Camera cameraPrincipal = Camera.main;
        Vector2 mouseTela = Mouse.current.position.ReadValue();
        Vector3 mouseMundo = cameraPrincipal.ScreenToWorldPoint(new Vector3(mouseTela.x, mouseTela.y, -cameraPrincipal.transform.position.z));
        Vector2 direcao = ((Vector2)mouseMundo - (Vector2)transform.position).normalized;
        if (direcao == Vector2.zero) direcao = Vector2.right;

        // A direção é fixada no início do golpe para acompanhar a animação.
        DispararAnimacao();
        // Mantém a hitbox desativada por pelo menos um ciclo de física entre golpes.
        yield return new WaitForFixedUpdate();
        if (atrasoImpacto > 0f) yield return new WaitForSeconds(atrasoImpacto);
        if (hitbox == null) { podeAtacar = true; yield break; }
        hitbox.transform.position = transform.position + (Vector3)(direcao * distancia);
        hitbox.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg);
        hitbox.SetActive(true);
        yield return new WaitForSeconds(Mathf.Max(0.01f, duracaoAtaque));
        if (hitbox != null) hitbox.SetActive(false);
        yield return new WaitForSeconds(Mathf.Max(0f, cooldown));
        podeAtacar = true;
    }

    void DispararAnimacao()
    {
        if (animator == null || animator.runtimeAnimatorController == null || string.IsNullOrEmpty(gatilhoAtaque)) return;
        int identificador = Animator.StringToHash(gatilhoAtaque);
        foreach (AnimatorControllerParameter parametro in animator.parameters)
        {
            if (parametro.nameHash == identificador && parametro.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(identificador);
                animator.SetTrigger(identificador);
                break;
            }
        }
    }

    void OnDisable()
    {
        // Impede que a hitbox continue causando dano ao desativar o jogador.
        StopAllCoroutines();
        if (hitbox != null) hitbox.SetActive(false);
        podeAtacar = true;
    }
}
