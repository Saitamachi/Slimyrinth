using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsMenu : MonoBehaviour
{

    public GameObject uiManager;
    private UIToggler uitToggler;

    public GameObject backButtonPanelSwitch;
    

    public void Start()
    {
        uitToggler = uiManager.GetComponent<UIToggler>();
    }
    public void OnBackPressed()
    {
        Debug.Log("Back button pressed!");
        uitToggler.SelectPanel(backButtonPanelSwitch);
    }



}
