using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button quitButton;

    public static GameOverScreen Instance;

    void Start()
    {
        playButton.onClick.AddListener(PlayGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    public void PlayGame()
    {
        
        Debug.Log("Start Again button clicked");
        SceneManager.LoadScene("Welcome");
    }

    public void QuitGame()
    {
        Debug.Log("Quit game button clicked");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL   
        Application.OpenURL("https://softmine.itch.io/");
#else
        Application.Quit();
#endif
    }
}
