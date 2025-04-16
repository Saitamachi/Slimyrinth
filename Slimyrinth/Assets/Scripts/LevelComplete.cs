using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{

    public GameObject levelsPanel;
    private LoadLevels loadLevels;


    public void Start()
    {
        if(levelsPanel){
            loadLevels = levelsPanel.GetComponent<LoadLevels>();
        }   
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(!loadLevels)
                return;


            loadLevels.NextLevel(SceneManager.GetActiveScene().name);
        }
    }
}
