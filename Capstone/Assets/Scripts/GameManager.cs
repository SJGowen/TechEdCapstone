using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button quitButton;

    public string transitionedFromScene;

    public static GameManager Instance { get; private set; }

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

    void Update()
    {

    }

    public void PlayGame()
    {
        Debug.Log("Play game button clicked");
        SceneManager.LoadScene("GameScene");
    }

    public void AudioSettings()
    {
        // Debug.Log("Audio settings button clicked");
        // Called from the OnClick event of the Audio Settings button in the UI
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
