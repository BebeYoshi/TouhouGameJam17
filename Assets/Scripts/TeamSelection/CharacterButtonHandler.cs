using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

public class CharacterButtonHandler : MonoBehaviour
{
    [SerializeField] private Image sprite;
    [SerializeField] private TMP_Text nomePersonagem;
    [SerializeField] private Button button;

    public CharacterData Character { get; private set; }

    private void Awake()
    {
        button = GetComponent<Button>();
        sprite = GetComponent<Image>();
    }

    public void Setup(CharacterData character, Action<CharacterData> callback, bool unlocked)
    {
        Character = character;

        sprite.sprite = character.sprite;
        // nomePersonagem.text = character.displayName;

        button.interactable = unlocked;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => callback?.Invoke(Character));

        SetSelected(false);
    }

    public void SetSelected(bool selected)
    {
    Color color = sprite.color;
    color.a = selected ? 1f : 0.8f;

    sprite.color = color;
    }
}
