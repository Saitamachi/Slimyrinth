using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public GameObject uiManager;
    private UIToggler uiToggler;
    public GameObject levelSelectorButtonPanelSwitch;



    public void Start()
    {
        uiToggler = uiManager.GetComponent<UIToggler>();
    }


    public void OnPlayPressed()
    {
        Debug.Log("Play button pressed!");
        uiToggler.SelectPanel(levelSelectorButtonPanelSwitch);
    }
}
