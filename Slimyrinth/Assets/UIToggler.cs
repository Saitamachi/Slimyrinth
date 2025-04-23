using System.Collections.Generic;
using UnityEngine;

public class UIToggler : MonoBehaviour
{
    public List<GameObject> panels;

    public bool isPaused = false;

    void Start()
    {
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
        AchievementMenu test = newPanel.GetComponent<AchievementMenu>();
        if(test != null)
            test.BuildAchievementCards();
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
        Debug.Log("test100");
        bool anyActive = false;




        foreach (GameObject panel in panels)
        {
            if (panel.activeSelf)
            {
                anyActive = true;
                isPaused = true;
                break;
            }
        }

        if (!anyActive)
        {
            isPaused = false;
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
