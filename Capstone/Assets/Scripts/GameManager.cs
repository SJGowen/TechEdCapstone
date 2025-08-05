using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public string transitionedFromScene;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
    }

    void Update()
    {
        if (PlayerController.Instance != null)
        {
            if (SceneManager.GetActiveScene().name == "Game Over" ||
                SceneManager.GetActiveScene().name == "Welcome")
            {
                Destroy(PlayerController.Instance.gameObject);
                PlayerController.Instance = null;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
