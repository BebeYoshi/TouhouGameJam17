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

        Debug.Log($"Reward {encounter.StageId}: +{power} P, +{point} Point (first clear: {firstClear}) | total {playerInfo.Power} P, {playerInfo.Point} Point");
    }
}
