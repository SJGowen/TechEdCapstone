using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] Button playButton;
    //[SerializeField] Button settingsButton;
    [SerializeField] Button quitButton;

    public static GameOverManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        playButton.onClick.AddListener(PlayGame);
        //settingsButton.onClick.AddListener(AudioSettings);
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
