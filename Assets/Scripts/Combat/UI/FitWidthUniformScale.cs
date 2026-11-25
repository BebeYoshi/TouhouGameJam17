using UnityEngine;

public class FitWidthUniformScale : MonoBehaviour
{
    public RectTransform mapArea;

    void Update()
    {
        if (mapArea == null) return;
        float scale = mapArea.rect.width / 1920f;
        transform.localScale = new Vector3(scale, scale, 1f);

        float scaledHeight = 1080f * scale;
        float mapAreaHeight = mapArea.rect.height;
        float yOffset = (scaledHeight - mapAreaHeight) * 0.5f;

        RectTransform rt = (RectTransform)transform;
        rt.anchoredPosition = new Vector2(0f, yOffset);
    }
}