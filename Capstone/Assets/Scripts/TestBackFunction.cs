using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestBackFunction : MonoBehaviour
{
    [SerializeField] Button welcomeButton;

    void Start()
    {
        welcomeButton.onClick.AddListener(Welcome);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Welcome()
    {
        SceneManager.LoadScene("Welcome");
    }
}
