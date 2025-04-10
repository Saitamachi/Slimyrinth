using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    public GameObject uiManager;
    private SettingsMenuToggle settingsMenuToggle;
    

    public void Start()
    {
        settingsMenuToggle = uiManager.GetComponent<SettingsMenuToggle>();
    }
    public void OnResumePressed()
    {
        Debug.Log("Resume button pressed!");
        // Add your logic here, e.g., hide the settings panel
        gameObject.SetActive(false);
    }


    public void OnRestartPressed()
    {
        Debug.Log("Restart button pressed!");
        // Add your logic here, e.g., hide the settings panel
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        settingsMenuToggle.isOpen = false;
    }

}
