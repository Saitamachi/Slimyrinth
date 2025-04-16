using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{

    public GameObject levelSelectorButtonPanelSwitch;



    public void Start()
    {
    }
    public void OnResumePressed()
    {
        Debug.Log("Resume button pressed!");
        CanvasManager.Instance.uiManager.GetComponent<UIToggler>().ClosePanels();
    }


    public void OnRestartPressed()
    {
        Debug.Log("Restart button pressed!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        CanvasManager.Instance.uiManager.GetComponent<UIToggler>().ClosePanels();
    }
    public void OnLevelSelectPressed()
    {
        Debug.Log("Level Select button pressed!");
        CanvasManager.Instance.uiManager.GetComponent<UIToggler>().SelectPanel(levelSelectorButtonPanelSwitch);
    }
}
