using UnityEngine;

/// <summary>Shared WASD movement, visual facing, and optional walk animation.</summary>
public abstract class PlayerMovementAnimation : MonoBehaviour
{
    [Header("Sprite and animation")]
    [Tooltip("A child containing the sprite, separate from the camera and gameplay objects.")]
    [SerializeField] private Transform spriteVisual;
    [Tooltip("Optional Animator on the visual child. Add a bool parameter named IsMoving.")]
    [SerializeField] private Animator animator;
    [Tooltip("Enable if the original sprite artwork faces right.")]
    [SerializeField] private bool spriteFacesRight = true;

    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    private RuntimeAnimatorController cachedController;
    private bool hasMovingParameter;
    private bool facingLeft;
    private Quaternion originalRotation;

    protected virtual void Awake()
    {
        if (spriteVisual == null)
        {
            SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();
            if (sprite != null && sprite.transform != transform)
                spriteVisual = sprite.transform;
        }

        if (spriteVisual != null)
        {
            originalRotation = spriteVisual.localRotation;
            facingLeft = !spriteFacesRight;
            if (animator == null)
                animator = spriteVisual.GetComponentInChildren<Animator>();
        }
    }

    protected virtual void Update()
    {
        Vector2 input = new Vector2(
            (Input.GetKey(KeyCode.D) ? 1f : 0f) - (Input.GetKey(KeyCode.A) ? 1f : 0f),
            (Input.GetKey(KeyCode.W) ? 1f : 0f) - (Input.GetKey(KeyCode.S) ? 1f : 0f));
        Vector2 velocity = GetVelocity(input);
        // World-space movement keeps WASD independent of sprite rotation.
        transform.position += (Vector3)velocity * Time.deltaTime;
        if (velocity.x != 0f)
            facingLeft = velocity.x < 0f;

        SetMoving(velocity.sqrMagnitude > 0f);
    }

    protected virtual void LateUpdate()
    {
        if (spriteVisual == null || spriteVisual == transform)
            return;

        // Apply after Animator updates so walk frames retain their facing direction.
        bool mirrored = facingLeft == spriteFacesRight;
        spriteVisual.localRotation = originalRotation * Quaternion.Euler(0f, mirrored ? 180f : 0f, 0f);
    }

    protected virtual void OnDisable()
    {
        SetMoving(false);
    }

    private void SetMoving(bool moving)
    {
        if (animator == null || animator.runtimeAnimatorController == null)
            return;

        if (cachedController != animator.runtimeAnimatorController)
        {
            cachedController = animator.runtimeAnimatorController;
            hasMovingParameter = false;
            foreach (AnimatorControllerParameter parameter in animator.parameters)
            {
                if (parameter.nameHash == IsMoving && parameter.type == AnimatorControllerParameterType.Bool)
                {
                    hasMovingParameter = true;
                    break;
                }
            }
        }

        // Static sprites and controllers without walk animation remain supported.
        if (hasMovingParameter)
            animator.SetBool(IsMoving, moving);
    }

    protected abstract Vector2 GetVelocity(Vector2 input);
}
