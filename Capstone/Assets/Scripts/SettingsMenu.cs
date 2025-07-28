using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Button settingsButton, closeButton;
    bool isMenuActive = false;
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isMenuActive)
            {
                closeButton.onClick.Invoke();
            }
            else
            {
                settingsButton.onClick.Invoke();
            }

            isMenuActive = !isMenuActive;
            Time.timeScale = isMenuActive ? 0f : 1f;
        }
    }
}
