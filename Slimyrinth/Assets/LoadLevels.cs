using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LevelData
{
    public SceneAsset scene;
    public Sprite previewImage;
    public bool unlocked;
}


public class LoadLevels : MonoBehaviour
{

    public List<LevelData> levels;
    public Transform buttonParent;
    public GameObject buttonPrefab;
    void Start()
    {
        if (levels == null)
        {
            levels = new List<LevelData>();
        }


        int rows = 2;  // Number of rows
        int columns = 3;  // Number of columns

        // Iterate over all the levels and create buttons dynamically
        for (int i = 0; i < levels.Count; i++)
        {
            LevelData level = levels[i];

            // Instantiate the prefab for each level button
            GameObject buttonObj = Instantiate(buttonPrefab, buttonParent);

            // Get the Button and Image components from the instantiated prefab
            Button button = buttonObj.GetComponent<Button>();
            Image buttonImage = buttonObj.GetComponent<Image>();
            button.interactable = levels[i].unlocked;

            // Set the button's image (preview of the level)
            if (buttonImage != null && level.previewImage != null)
            {
                buttonImage.sprite = level.previewImage; // Set the image to show as the button's preview
            }

            // Optionally set button size and position if needed
            RectTransform rt = buttonObj.GetComponent<RectTransform>();
            //rt.localScale = Vector3.one;  // Reset scale if needed

            // Calculate the position for the button in a 3x2 grid
            int row = i / columns;  // Row is determined by the index divided by the number of columns
            int col = i % columns;  // Column is the remainder of index divided by the number of columns

            // Get button width and height from the prefab's RectTransform
            float buttonWidth = rt.rect.width  * rt.localScale.x;  // Width of the button (from RectTransform)
            float buttonHeight = rt.rect.height  * rt.localScale.y; // Height of the button (from RectTransform)
            float spacing = 60f; // Space between buttons, adjust as necessary

            // Position calculation: 3 columns, 2 rows, starting top-left
            rt.anchoredPosition = new Vector2(col * (buttonWidth + spacing) - 1 * (buttonWidth + spacing), -row * (buttonHeight + spacing) +1 * (buttonHeight + spacing));

            // Add a listener to load the scene when the button is clicked
            button.onClick.AddListener(() => LoadScene(level.scene));
        }
    }

    void LoadScene(SceneAsset sceneAsset)
    {
        // Convert SceneAsset to a string (path of the scene)
        string sceneName = sceneAsset.name;

        // You can load the scene here using Unity's SceneManager (ensure your scene is in Build Settings)
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
