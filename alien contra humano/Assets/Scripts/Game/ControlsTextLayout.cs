using TMPro;
using UnityEngine;

/// <summary>Keeps controls text inside the top-left corner of its UI parent.</summary>
[ExecuteAlways]
[DisallowMultipleComponent]
[RequireComponent(typeof(TextMeshProUGUI))]
public class ControlsTextLayout : MonoBehaviour
{
    [SerializeField] private Vector2 padding = new Vector2(24f, 24f);
    [SerializeField] private Vector2 preferredSize = new Vector2(760f, 240f);
    [SerializeField, Min(1f)] private float maximumFontSize = 36f;

    private TextMeshProUGUI controlsText;

    private void OnEnable()
    {
        ApplyLayout();
    }

    private void LateUpdate()
    {
        // Also responds to Game view resizing and CanvasScaler updates.
        ApplyLayout();
    }

    private void ApplyLayout()
    {
        if (controlsText == null)
            controlsText = GetComponent<TextMeshProUGUI>();

        RectTransform textRect = controlsText.rectTransform;
        RectTransform parentRect = textRect.parent as RectTransform;
        if (parentRect == null)
            return;

        Vector2 available = parentRect.rect.size;
        if (available.x <= 0f || available.y <= 0f)
            return;

        Vector2 inset = new Vector2(
            Mathf.Clamp(padding.x, 0f, available.x * 0.25f),
            Mathf.Clamp(padding.y, 0f, available.y * 0.25f));
        Vector2 size = new Vector2(
            Mathf.Min(Mathf.Max(1f, preferredSize.x), available.x - inset.x * 2f),
            Mathf.Min(Mathf.Max(1f, preferredSize.y), available.y - inset.y * 2f));

        textRect.anchorMin = textRect.anchorMax = new Vector2(0f, 1f);
        textRect.pivot = new Vector2(0f, 1f);
        textRect.anchoredPosition3D = new Vector3(inset.x, -inset.y, 0f);
        textRect.sizeDelta = size;
        textRect.localScale = Vector3.one;
        textRect.localRotation = Quaternion.identity;

        // Remove legacy negative margins that let text escape its rectangle.
        controlsText.margin = Vector4.zero;
        controlsText.alignment = TextAlignmentOptions.TopLeft;
        controlsText.textWrappingMode = TextWrappingModes.Normal;
        controlsText.enableAutoSizing = true;
        controlsText.fontSizeMin = 1f;
        controlsText.fontSizeMax = Mathf.Max(1f, maximumFontSize);
        controlsText.overflowMode = TextOverflowModes.Truncate;
        controlsText.raycastTarget = false;
    }
}
