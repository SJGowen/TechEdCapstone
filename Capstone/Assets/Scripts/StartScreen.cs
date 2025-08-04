using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button quitButton;

    private void Awake()
    {
        if (playButton == null || quitButton == null)
        {
            Debug.LogError("Buttons are not assigned in the StartScreen script.");
        }
        else
        {
            playButton.onClick.AddListener(PlayGame);
            quitButton.onClick.AddListener(QuitGame);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PlayGame()
    {
        Debug.Log("Play game button clicked");
        SceneManager.LoadScene("Opening level");
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
