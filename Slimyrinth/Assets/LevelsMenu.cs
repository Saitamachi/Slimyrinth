using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsMenu : MonoBehaviour
{

    public GameObject backButtonPanelSwitch;
    

    public void Start()
    {
    }
    public void OnBackPressed()
    {
        CanvasManager.Instance.uiManager.GetComponent<UIToggler>().SelectPanel(backButtonPanelSwitch);
    }
}
