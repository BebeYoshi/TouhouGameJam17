
using UnityEngine;

public class TentarFuncionar : MonoBehaviour
{
    private static TentarFuncionar instance;

    [SerializeField] private PartyLoadout fullRoster;
    [SerializeField] private PartyLoadout partyLoadout;
    [SerializeField] private Player_info playerInfo;
    [SerializeField] private GachaPull gachaPull;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        Debug.Log("[GAMEDATA] Mantendo referências dos assets!");
    }
}
