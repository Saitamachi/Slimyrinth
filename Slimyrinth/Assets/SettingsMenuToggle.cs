using UnityEngine;

public class SettingsMenuToggle : MonoBehaviour
{
    public GameObject settingsPanel;
    public bool isOpen = false;
    public GameObject slime;
    private SlimeMovement slimeMovement;


    void Start()
    {
        settingsPanel.SetActive(isOpen);
        slimeMovement = slime.GetComponent<SlimeMovement>();
    }
    void Update()
    {
        isOpen = settingsPanel.activeInHierarchy;
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            isOpen = !isOpen;
            
            settingsPanel.SetActive(isOpen);
        }

        slimeMovement.isPaused = isOpen;
    }
}
