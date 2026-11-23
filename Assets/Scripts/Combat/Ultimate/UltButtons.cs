
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartyUltButtons : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private StageSetup stageSetup;
    [SerializeField] private PartyLoadout partyLoadout;
    [SerializeField] private Image[] imagens;
    [SerializeField] private CharacterDatabase characterDB;
    private CharacterData character;

    [Header("Buttons")]
    [SerializeField] private Button[] ultButtons = new Button[3];

    private CombatStats[] partyCombatants = new CombatStats[3];

    private void Start()
    {
        LoadUltButtons();
        RefreshButtons();
    }

    private void Update()
    {
        RefreshButtons();
    }

    private void LoadUltButtons()
    {
        for (int i = 0; i < partyLoadout.Members.Count; i++)
        {
            Debug.Log(partyLoadout.Members[i].CharacterId);
            character = characterDB.FindById(partyLoadout.Members[i].CharacterId);
            ultButtons[i].image.sprite = character.sprite;
            ultButtons[i].gameObject.SetActive(true);
            if (i >= partyLoadout.Members.Count || i >= stageSetup.AllySlots.Count)
            {
                ultButtons[i].gameObject.SetActive(false);
                continue;
            }


            partyCombatants[i] = stageSetup.AllySlots[i];

        }
    }

    private void RefreshButtons()
    {
        for (int i = 0; i < 3; i++)
        {
            if (partyCombatants[i] == null)
                continue;

            ultButtons[i].interactable =
                partyCombatants[i].IsAlive &&
                partyCombatants[i].CanAct &&
                partyCombatants[i].HasUltAvailable &&
                partyCombatants[i].IsUltReady;
        }
    }

    public void UseUlt1()
    {
        UseUltimate(0);
    }

    public void UseUlt2()
    {
        UseUltimate(1);
    }

    public void UseUlt3()
    {
        UseUltimate(2);
    }

    private void UseUltimate(int index)
    {
        CombatStats combat = partyCombatants[index];

        if (combat == null || !combat.IsUltReady || !combat.CanAct)
            return;

        bool success = combat.UseUlt(null);

        RefreshButtons();
    }
}
