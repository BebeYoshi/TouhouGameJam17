using UnityEngine;
using TMPro;

public class PowerupHandler : MonoBehaviour
{
    [SerializeField] private GameObject powerupPanel;
    [SerializeField] private PartyLoadout fullRoster;
    [SerializeField] private CharacterDatabase database;
    [SerializeField] private Player_info playerInfo;


    [SerializeField] private TMP_Text currentLevelTXT;
    [SerializeField] private TMP_Text nextLevelTXT;

    [SerializeField] private TMP_Text attackTotal;
    [SerializeField] private TMP_Text healthTotal;
    [SerializeField] private TMP_Text newAttack;
    [SerializeField] private TMP_Text newHealth;

    private UnitEntry selectedCharacter;

    private float baseAttack;
    private float baseHealth;

    private void Awake()
    {
        powerupPanel.SetActive(false);
    }

    public void Setup(string characterId)
    {
        if (string.IsNullOrEmpty(characterId))
            return;

        Debug.Log(characterId);
        selectedCharacter = fullRoster.GetMember(characterId);

        if (selectedCharacter == null)
            return;

        CharacterData character = database.FindById(characterId);

        if (character == null)
            return;

        baseAttack = character.Attack;
        baseHealth = character.MaxHealth;

        AtualizaPreview();

        powerupPanel.SetActive(true);
    }

    private void AtualizaPreview()
    {
    if (selectedCharacter == null)
        return;

    int level = selectedCharacter.Level;
    int nextLevel = level + 1;

    float currentAttack = baseAttack * Mathf.Pow(1.04f, level - 1);
    float currentHealth = baseHealth * Mathf.Pow(1.04f, level - 1);

    float nextAttack = baseAttack * Mathf.Pow(1.04f, nextLevel - 1);
    float nextHealth = baseHealth * Mathf.Pow(1.04f, nextLevel - 1);

    attackTotal.text = currentAttack.ToString("0");
    healthTotal.text = currentHealth.ToString("0");

    currentLevelTXT.text = level.ToString();
    nextLevelTXT.text = nextLevel.ToString();

    newAttack.text = nextAttack.ToString("0");
    newHealth.text = nextHealth.ToString("0");
    }

    public void ConfirmPowerup()
    {
        if (selectedCharacter.Level >= 40)
            return;

       int cost = Mathf.RoundToInt(20 * Mathf.Pow(1.04f, selectedCharacter.Level));
        if (!playerInfo.SpendPower(cost))
        {
            Debug.Log("Power insuficiente!");
            return;
        }

        selectedCharacter.LevelUp();

        ClosePowerup();
    }


    public void ClosePowerup()
    {
        powerupPanel.SetActive(false);
        selectedCharacter = null;
    }
}
