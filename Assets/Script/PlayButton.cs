using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public string playSceneName = "LUDO_BOARD";

    public void PlayGame()
    {
        SceneManager.LoadScene(playSceneName);
    }

    public void ExitGame()
    {
        
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
