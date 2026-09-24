using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Cria um HUD independente, ancorado no canto inferior esquerdo da área segura.
public class AmmoDisplay : MonoBehaviour
{
    private Shoot arma;
    private GameObject painel;
    private RectTransform areaSegura;
    private TextMeshProUGUI texto;

    public void DefinirArma(Shoot origem)
    {
        arma = origem;
        painel = new GameObject("Ammo HUD", typeof(Canvas), typeof(CanvasScaler));
        Canvas canvas = painel.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 20;
        CanvasScaler escala = painel.GetComponent<CanvasScaler>();
        escala.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        escala.referenceResolution = new Vector2(1920f, 1080f);
        escala.matchWidthOrHeight = 0.5f;
        areaSegura = new GameObject("Safe Area", typeof(RectTransform)).GetComponent<RectTransform>();
        areaSegura.SetParent(painel.transform, false);
        texto = new GameObject("Ammo", typeof(RectTransform), typeof(TextMeshProUGUI)).GetComponent<TextMeshProUGUI>();
        texto.transform.SetParent(areaSegura, false);
        RectTransform rect = texto.rectTransform;
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = new Vector2(1f, 0.3f);
        rect.offsetMin = new Vector2(24f, 24f);
        rect.offsetMax = new Vector2(-24f, -24f);
        texto.fontSizeMin = 12f;
        texto.fontSizeMax = 32f;
        texto.enableAutoSizing = true;
        texto.alignment = TextAlignmentOptions.BottomLeft;
        texto.color = Color.white;
        texto.outlineWidth = 0.2f;
        texto.outlineColor = Color.black;
        texto.raycastTarget = false;
    }

    void LateUpdate()
    {
        if (arma == null || texto == null) return;
        Rect segura = Screen.safeArea;
        areaSegura.anchorMin = new Vector2(segura.xMin / Mathf.Max(1, Screen.width), segura.yMin / Mathf.Max(1, Screen.height));
        areaSegura.anchorMax = new Vector2(segura.xMax / Mathf.Max(1, Screen.width), segura.yMax / Mathf.Max(1, Screen.height));
        areaSegura.offsetMin = areaSegura.offsetMax = Vector2.zero;
        texto.text = arma.NomeArma + "  " + arma.MunicaoAtual + " / " + arma.ReservaAtual + "\n" +
            (arma.Recarregando ? "recarregando... " + arma.TempoRestanteRecarga.ToString("0.0") + "s" :
             arma.ReservaAtual == 0 ? "acabo tua munição" : "R - recarregar");
    }

    void OnEnable() { if (painel != null) painel.SetActive(true); }
    void OnDisable() { if (painel != null) painel.SetActive(false); }
    void OnDestroy() { if (painel != null) Destroy(painel); }
}
