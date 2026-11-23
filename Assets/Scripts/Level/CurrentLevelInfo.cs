using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrentLevelInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelAtual;
    [SerializeField] private Image textoLevel;
    [SerializeField] private Image imagemMundo1;
    [SerializeField] private Image imagemMundo2;
    [SerializeField] private Image imagemMundo3;

    private void Start()
    {
        UpdateLevelInfo();
    }

    public void UpdateLevelInfo()
    {
        string levelName = GetCurrentLevelName();
        if (string.IsNullOrEmpty(levelName)) return;

        if (imagemMundo1 != null) imagemMundo1.gameObject.SetActive(false);
        if (imagemMundo2 != null) imagemMundo2.gameObject.SetActive(false);
        if (imagemMundo3 != null) imagemMundo3.gameObject.SetActive(false);

        if (levelName.StartsWith("Boss 1-") || levelName.StartsWith("Normal 1-"))
        {
            if (imagemMundo1 != null) imagemMundo1.gameObject.SetActive(true);
        }
        else if (levelName.StartsWith("Boss 2-") || levelName.StartsWith("Normal 2-"))
        {
            if (imagemMundo2 != null) imagemMundo2.gameObject.SetActive(true);
        }
        else if (levelName.StartsWith("Boss 3-") || levelName.StartsWith("Normal 3-"))
        {
            if (imagemMundo3 != null) imagemMundo3.gameObject.SetActive(true);
        }

        // Update level text
        if (levelAtual != null)
        {
            levelAtual.text = levelName;
            textoLevel.gameObject.SetActive(true);
        }
    }

    public string GetCurrentLevelName()
    {
        if (SelectedLevel.selectedLevel == null) return string.Empty;
        return SelectedLevel.selectedLevel.levelName;
    }
}
