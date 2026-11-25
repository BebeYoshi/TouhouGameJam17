
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TeamSelectionManager : MonoBehaviour
{
    [SerializeField] private CharacterDatabase database;

    [SerializeField] private List<CharacterButtonHandler> characterButtons;
    [SerializeField] private PowerupHandler powerUp;
    [SerializeField] private Player_info playerInfo;
    [SerializeField] private SceneLoader sceneLoader;

    [SerializeField] private TMP_Text selectionCounter;
    [SerializeField] private Button confirmButton;

    [SerializeField] private PartyLoadout partyLoadout;
    [SerializeField] private PartyLoadout fullRoster;

    [SerializeField] private Image previewSprite;
    [SerializeField] private TMP_Text previewNome;
    [SerializeField] private TMP_Text previewAttack;
    [SerializeField] private TMP_Text previewHealth;
    [SerializeField] private TMP_Text custoTXT;
    [SerializeField] private TMP_Text powerTXT;



    private const int selecaoMaxima = 3;

    private CharacterData inspectedCharacter;

    private List<CharacterData> selectedCharacters = new();

    public IReadOnlyList<CharacterData> SelectedCharacters => selectedCharacters;

    private void Start()
    {
        GeraPersonagens();
        AtualizaUISelecao();
        powerTXT.text = playerInfo.Power.ToString();
    }

    private void OnEnable()
        {
            playerInfo.CurrencyChanged += AtualizaPower;
        }

        private void OnDisable()
        {
            playerInfo.CurrencyChanged -= AtualizaPower;
        }

        private void AtualizaPower()
        {
            powerTXT.text = playerInfo.Power.ToString();
        }

    private void GeraPersonagens()
    {
        List<CharacterData> characters = database.ReturnAll();

        for (int i = 0; i < characterButtons.Count; i++)
        {
            if (i >= characters.Count)
            {
                characterButtons[i].gameObject.SetActive(false);
                continue;
            }

            characterButtons[i].gameObject.SetActive(true);

            bool unlocked = IsUnlocked(characters[i]);

            characterButtons[i].Setup(
                characters[i],
                OnButtonClick,
                unlocked
            );
        }
    }

    private void OnButtonClick(CharacterData character)
    {
        inspectedCharacter = character;

        AtualizaPreview();

        if (selectedCharacters.Contains(character))
        {
            selectedCharacters.Remove(character);
        }
        else if (selectedCharacters.Count < selecaoMaxima)
        {
            selectedCharacters.Add(character);
        }

        AtualizaUISelecao();
    }

    private void AtualizaPreview()
    {
        if (inspectedCharacter == null)
            return;

        previewSprite.sprite = inspectedCharacter.sprite;
        previewNome.text = inspectedCharacter.DisplayName;

        previewAttack.text = inspectedCharacter.Attack.ToString();
        previewHealth.text = inspectedCharacter.MaxHealth.ToString(); 
    }

    private void AtualizaUISelecao()
    {
        foreach (CharacterButtonHandler button in characterButtons)
        {
            if (button.Character == null)
                continue;

            button.SetSelected(
                selectedCharacters.Contains(button.Character)
            );
        }

        selectionCounter.text = SelectedCharacters.Count + " /3";
        if (inspectedCharacter != null)
    {
        var personagemSelecionado = fullRoster.GetMember(inspectedCharacter.id);

        if (personagemSelecionado != null)
        {
            int cost = Mathf.RoundToInt(20 * Mathf.Pow(1.04f, personagemSelecionado.Level));


            custoTXT.text = cost.ToString();
        }
    }

        confirmButton.interactable =
            selectedCharacters.Count >= 1;
    }

    public void ConfirmSelection()
    {
        if (selectedCharacters.Count == 0)
            return;

        List<UnitEntry> newMembers = new();

        foreach (CharacterData character in selectedCharacters)
        {
            var member = fullRoster.GetMember(character.id);

            if (member == null)
                continue;

            newMembers.Add(new UnitEntry(
                character.id.ToString(),
                member.Level
            ));
        }

    partyLoadout.SetParty(newMembers);
    sceneLoader.CarregaLevelCorreto();    
    }

    public void OpenPowerup()
    {
        if (inspectedCharacter == null)
            return;
        var personagemSelecionado = fullRoster.GetMember(inspectedCharacter.id);
        int cost = Mathf.RoundToInt(20 * Mathf.Pow(1.04f, personagemSelecionado.Level));
        var currency = playerInfo.Power;

    if(currency < cost){
        return;
    }

    powerUp.Setup(inspectedCharacter.id.ToString());
    custoTXT.text = cost.ToString();
    }

    private bool IsUnlocked(CharacterData character)
{
    foreach (UnitEntry entry in fullRoster.Members)
    {
        if (entry.CharacterId == character.id.ToString())
            return true;
    }

    return false;
}
}
