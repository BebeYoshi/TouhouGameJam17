using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Player_info", menuName = "Scriptable Objects/Player_info")]
public class Player_info : ScriptableObject
{
    private const string SaveKey = "player_save";

    private PlayerSave data;

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

    public int Power => Data.power;
    public int Point => Data.point;

    public void AddPower(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        Data.power += amount;
        CurrencyChanged?.Invoke();
    }

    public void AddPoint(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        Data.point += amount;
        CurrencyChanged?.Invoke();
    }

    public bool SpendPower(int amount)
    {
        if (amount < 0 || Data.power < amount)
        {
            return false;
        }

        Data.power -= amount;
        CurrencyChanged?.Invoke();
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
        }
    }

    public void Save()
    {
        PlayerPrefs.SetString(SaveKey, JsonUtility.ToJson(Data));
        PlayerPrefs.Save();
    }

    public void Load()
    {
        string json = PlayerPrefs.GetString(SaveKey, "");
        data = string.IsNullOrEmpty(json) ? new PlayerSave() : JsonUtility.FromJson<PlayerSave>(json);
    }

    [ContextMenu("Reset Save")]
    public void ResetSave()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        data = new PlayerSave();
        CurrencyChanged?.Invoke();
    }
}

