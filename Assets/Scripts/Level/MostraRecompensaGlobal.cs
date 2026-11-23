using UnityEngine;
using TMPro;

public class RewardUIManager : MonoBehaviour
{
    public static RewardUIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI pointText;
    [SerializeField] private TextMeshProUGUI notificationText;  

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ShowReward(int power, int point)
    {
        powerText.gameObject.SetActive(true);
        pointText.gameObject.SetActive(true);
        if (powerText != null)
            powerText.text = "+" + power;

        if (pointText != null)
            pointText.text = "+" + point;
    }

    public void ShowNotification(string message)
    {
        notificationText.text = message;
        notificationText.gameObject.SetActive(true);

    }

    public void ClearReward()
    {
        powerText.gameObject.SetActive(false);
        pointText.gameObject.SetActive(false);

        powerText.text = "";
        pointText.text = "";
    }

    public void ClearNotification()
    {
        pointText.gameObject.SetActive(false);

        notificationText.text = "";
    }
}