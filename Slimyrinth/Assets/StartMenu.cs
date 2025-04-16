using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public GameObject levelSelectorButtonPanelSwitch;



    public void Start()
    {
    }


    public void OnPlayPressed()
    {
        Debug.Log("Play button pressed!");
        CanvasManager.Instance.uiManager.GetComponent<UIToggler>().SelectPanel(levelSelectorButtonPanelSwitch);
    }
}
