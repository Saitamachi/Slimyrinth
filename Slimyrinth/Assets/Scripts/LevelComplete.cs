using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{

    private LoadLevels loadLevels;

    public void Start()
    {
        loadLevels = CanvasManager.Instance.levelsPanel.GetComponent<LoadLevels>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!loadLevels)
                return;

            Debug.Log("Next Level");

            loadLevels.NextLevel(SceneManager.GetActiveScene().name);
        }
    }
}
