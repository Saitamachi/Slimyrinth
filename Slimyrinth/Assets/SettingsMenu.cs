using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{

    public GameObject levelSelectorButtonPanelSwitch;
    public GameObject achievementButtonPanelSwitch;



    public void Start()
    {
    }
    public void OnResumePressed()
    {
        CanvasManager.Instance.uiManager.GetComponent<UIToggler>().ClosePanels();
    }


    public void OnRestartPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        CanvasManager.Instance.uiManager.GetComponent<UIToggler>().ClosePanels();
    }
    public void OnLevelSelectPressed()
    {
        CanvasManager.Instance.uiManager.GetComponent<UIToggler>().SelectPanel(levelSelectorButtonPanelSwitch);
    }

    public void OnAchievementPressed()
    {
        CanvasManager.Instance.uiManager.GetComponent<UIToggler>().SelectPanel(achievementButtonPanelSwitch);
    }
}
