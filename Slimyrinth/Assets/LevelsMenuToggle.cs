using UnityEngine;

public class LevelsMenuToggle : MonoBehaviour
{

    public GameObject levelsPanel;

    public bool isOpen = false;
    public GameObject slime;
    private SlimeMovement slimeMovement;
    void Start()
    {
        levelsPanel.SetActive(isOpen);
        slimeMovement = slime.GetComponent<SlimeMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            isOpen = false;

        }
        levelsPanel.SetActive(isOpen);
        isOpen = levelsPanel.activeInHierarchy;

        slimeMovement.isPaused = isOpen;
    }
}
