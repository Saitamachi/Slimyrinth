using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    public GameObject uiManager;
    private UIToggler uiToggler;
    public GameObject levelSelectorButtonPanelSwitch;



    public void Start()
    {
        uiToggler = uiManager.GetComponent<UIToggler>();
    }
    public void OnResumePressed()
    {
        Debug.Log("Resume button pressed!");
        uiToggler.ClosePanels();
    }


    public void OnRestartPressed()
    {
        Debug.Log("Restart button pressed!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        uiToggler.ClosePanels();
    }
    public void OnLevelSelectPressed()
    {
        Debug.Log("Level Select button pressed!");
        uiToggler.SelectPanel(levelSelectorButtonPanelSwitch);
    }
}
