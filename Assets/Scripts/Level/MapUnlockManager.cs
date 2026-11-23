using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MapUnlockManager : MonoBehaviour
{
    [SerializeField] private Player_info playerInfo;
    [SerializeField] private List<Button> stageButtons;

    private void Start()
    {
        RefreshButtons();
    }

    public void RefreshButtons()
    {
        int cleared = playerInfo.ClearedStagesCount;
        Debug.Log(cleared);

        for (int i = 0; i < stageButtons.Count; i++)
        {
            stageButtons[i].interactable = i <= cleared;
            CanvasGroup group = stageButtons[i].GetComponent<CanvasGroup>();

            if (group == null)
                group = stageButtons[i].gameObject.AddComponent<CanvasGroup>();

            group.alpha = i <= cleared ? 1f : 0.35f;
        }
    }
}