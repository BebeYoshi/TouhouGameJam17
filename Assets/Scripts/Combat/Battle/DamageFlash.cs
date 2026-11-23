using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Color flashColor = new Color(1f, 0.35f, 0.35f, 1f);
    [SerializeField, Min(0.01f)] private float flashDuration = 0.12f;

    private CombatStats stats;
    private SpriteRenderer[] renderers;
    private Color[] originalColors;
    private float flashTimer;

    private void Awake()
    {
        stats = GetComponent<CombatStats>();
        RefreshRenderers();

        if (stats != null)
        {
            stats.Damaged += OnDamaged;
            stats.Died += OnResetState;
            stats.Revived += OnResetState;
        }

        enabled = false;
    }

    private void OnDestroy()
    {
        if (stats != null)
        {
            stats.Damaged -= OnDamaged;
            stats.Died -= OnResetState;
            stats.Revived -= OnResetState;
        }
    }

    public void RefreshRenderers()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
        originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalColors[i] = renderers[i].color;
        }
    }

    private void OnDamaged(float damageAmount)
    {
        if (renderers == null || renderers.Length == 0)
        {
            RefreshRenderers();
        }

        SetColor(flashColor);
        flashTimer = flashDuration;
        enabled = true;
    }

    private void OnResetState(CombatStats unit)
    {
        flashTimer = 0f;
        ResetColors();
        enabled = false;
    }

    private void Update()
    {
        flashTimer -= Time.deltaTime;

        if (flashTimer <= 0f)
        {
            ResetColors();
            enabled = false;
        }
    }

    private void SetColor(Color color)
    {
        if (renderers == null) return;

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null)
            {
                renderers[i].color = color;
            }
        }
    }

    private void ResetColors()
    {
        if (renderers == null) return;

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] != null)
            {
                renderers[i].color = originalColors[i];
            }
        }
    }
}