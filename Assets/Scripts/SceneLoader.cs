using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    [SerializeField] TeamSelectionScreen levelSelecionado;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CarregaCena(string nome){
        RewardUIManager.Instance?.ClearReward();
        RewardUIManager.Instance?.ClearNotification();

        SceneManager.LoadScene(nome);
    }

    public void PreparaCenaTime(TeamSelectionScreen team){
        SelectedLevel.selectedLevel = team;
    }

    public void CarregaTime(){
        SceneManager.LoadScene("TimeSelectionScreen");
    }

    public void CarregaLevelCorreto(){
        SceneManager.LoadScene(SelectedLevel.selectedLevel.sceneName);
    }
}
