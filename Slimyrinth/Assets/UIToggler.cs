using System.Collections.Generic;
using UnityEngine;

public class UIToggler : MonoBehaviour
{
    public List<GameObject> panels;

    public GameObject slime;
    private SlimeMovement slimeMovement;

    void Start()
    {
        Debug.Log("slime movement");
        slimeMovement = slime.GetComponent<SlimeMovement>();
        Debug.Log("slime movement: "+ slimeMovement);
        if (panels == null)
        {
            panels = new List<GameObject>();
        }

    }

    public GameObject PanelOpen()
    {
        foreach (GameObject panel in panels)
        {
            if (panel.activeSelf)
            {
                return panel;
            }
        }
        return null;
    }
    public void SelectPanel(GameObject newPanel)
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
        newPanel.SetActive(true);
    }

    public void ClosePanels()
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool anyActive = false;

        foreach (GameObject panel in panels)
        {
            if (panel.activeSelf)
            {
                anyActive = true;
                slimeMovement.isPaused = true;
                break;
            }
        }

        if (!anyActive)
        {
            slimeMovement.isPaused = false;
        }

        OpenPanel();
    }

    private void OpenPanel()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (!PanelOpen())
                SelectPanel(panels[0]);
            else if (PanelOpen())
                ClosePanels();
        }
    }
}
