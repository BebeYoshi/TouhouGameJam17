using UnityEngine;
using TMPro;

public class StageReward : MonoBehaviour
{
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private StageSetup stageSetup;
    [SerializeField] private Player_info playerInfo;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI pointText;

    private void OnEnable()
    {
        battleManager.Victory += GiveReward;
    }

    private void OnDisable()
    {
        battleManager.Victory -= GiveReward;
    }

    private void GiveReward()
    {
        StageEncounter encounter = stageSetup.Encounter;
        bool firstClear = !playerInfo.IsCleared(encounter.StageId);
        int power = firstClear ? encounter.PowerReward : encounter.RerunPowerReward;
        int point = firstClear ? encounter.PointReward : encounter.RerunPointReward;
        
        switch (encounter.StageId)
        {
            case "1-3":
                playerInfo.HabilitaCharBanner("miyoi");
                RewardUIManager.Instance?.ShowNotification("Miyoi is available on banner!");
                break;

            case "1-6":
                playerInfo.HabilitaCharBanner("mamizou");
                RewardUIManager.Instance?.ShowNotification("Mamizou is available on banner!");
                break;

            case "1-9":
                playerInfo.HabilitaCharBanner("reimu");
                RewardUIManager.Instance?.ShowNotification("Reimu is available on banner!");
                break;

            case "2-3":
                playerInfo.HabilitaCharBanner("aya");
                RewardUIManager.Instance?.ShowNotification("Aya is available on banner!");
                break;

            case "2-6":
                playerInfo.HabilitaCharBanner("momiji");
                RewardUIManager.Instance?.ShowNotification("Momiji is available on banner!");
                break;

            case "3-3":
                playerInfo.HabilitaCharBanner("kasen");
                RewardUIManager.Instance?.ShowNotification("Kasen is available on banner!");
                break;

            case "3-9":
                playerInfo.HabilitaCharBanner("yuugi");
                RewardUIManager.Instance?.ShowNotification("Yuugi is available on banner!");
                break;
        }
        playerInfo.AddPower(power);
        playerInfo.AddPoint(point);
        playerInfo.MarkCleared(encounter.StageId);
        playerInfo.Save();

        if (powerText != null)
        {
            powerText.text = "+" + power;
        }

        if (pointText != null)
        {
            pointText.text = "+" + point;
        }
        RewardUIManager.Instance?.ShowReward(power, point);
        Debug.Log($"Reward {encounter.StageId}: +{power} P, +{point} Point (first clear: {firstClear}) | total {playerInfo.Power} P, {playerInfo.Point} Point");
    }
}
