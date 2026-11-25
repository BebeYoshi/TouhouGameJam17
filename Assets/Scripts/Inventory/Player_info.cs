using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Player_info", menuName = "Scriptable Objects/Player_info")]
public class Player_info : ScriptableObject
{

    private const string SaveKey = "player_save";

    private PlayerSave data;
    [SerializeField] CharacterDatabase charact;
    [SerializeField] PartyLoadout fullRoster;
    [SerializeField] private int debugPoint;

    public event Action CurrencyChanged;

    private PlayerSave Data
    {
        get 
        {
            if (data == null)
            {
                Load();
            }

            return data;
        }
    }


    public int ClearedStagesCount => Data.clearedStages.Count;
    public int Power => Data.power;
    public int Point => Data.point;

    public void HabilitaCharBanner(string id){
        charact.EnablePullable(id);
        Save();
    }

    public void AddPower(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        Data.power += amount;
        CurrencyChanged?.Invoke();
        SaveQuick();
    }

    public void ResetProgress()
{
    Data.power = 0;
    Data.point = 0;
    Data.clearedStages.Clear();
    Data.pullableCharacterIds.Clear();
    Data.rosterMembers.Clear();

    Save();

    CurrencyChanged?.Invoke();

    Debug.Log("Player progress reset!");
}

    public void AddPoint(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        Data.point += amount;
        CurrencyChanged?.Invoke();
        SaveQuick();
    }

    public bool SpendPower(int amount)
    {
        if (amount < 0 || Data.power < amount)
        {
            return false;
        }

        Data.power -= amount;
        CurrencyChanged?.Invoke();
        SaveQuick();
        return true;
    }

    public bool SpendPoint(int amount)
    {
        if (amount < 0 || Data.point < amount)
        {
            return false;
        }

        Data.point -= amount;
        CurrencyChanged?.Invoke();
        SaveQuick();
        return true;
    }

    public bool IsCleared(string stageId)
    {
        return Data.clearedStages.Contains(stageId);
    }

    public void MarkCleared(string stageId)
    {
        if (!IsCleared(stageId))
        {
            Data.clearedStages.Add(stageId);
            Save();
        }
    }

    private void SaveQuick()
    {
        PlayerPrefs.SetString(SaveKey, JsonUtility.ToJson(Data));
    }

    public void Save()
    {
        Data.pullableCharacterIds.Clear();
        if (charact != null)
        {
            foreach (var c in charact.characters)
            {
                if (c.isPullable)
                {
                    Data.pullableCharacterIds.Add(c.id);
                }
            }
        }

        if (fullRoster != null)
        {
            Data.rosterMembers = new List<UnitEntry>(fullRoster.Members);
        }
        
        PlayerPrefs.SetString(SaveKey, JsonUtility.ToJson(Data));
        PlayerPrefs.Save();
    }

    public void Load()
    {
        string json = PlayerPrefs.GetString(SaveKey, "");
        if (string.IsNullOrEmpty(json))
        {
            data = new PlayerSave();
            return;
        }

        data = JsonUtility.FromJson<PlayerSave>(json);

        if (charact != null && data.pullableCharacterIds != null)
        {
            foreach (var c in charact.characters)
            {
                if (data.pullableCharacterIds.Contains(c.id))
                {
                    c.isPullable = true;
                }
            }
        }

        if (fullRoster != null && data.rosterMembers != null)
        {
            fullRoster.SetParty(data.rosterMembers);
        }
    }
    [ContextMenu("Reset Player Progress")]
    private void ResetPlayerProgress()
    {
    ResetProgress();
    }

    [ContextMenu("Reset Save")]
    public void ResetSave()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        data = new PlayerSave();
        CurrencyChanged?.Invoke();
    }
}

