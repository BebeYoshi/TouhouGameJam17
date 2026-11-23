using UnityEngine;

[CreateAssetMenu(fileName = "TeamSelectionScreen", menuName = "Scriptable Objects/TeamSelectionScreen")]
public class TeamSelectionScreen : ScriptableObject
{
    public string levelName;
    public string sceneName;

    public GameObject[] enemies;
    public GameObject[] team;

}
