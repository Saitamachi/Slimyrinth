using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LevelData
{
    public String sceneName;
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
            levels = new List<LevelData>();

        BuildLevelButtons();
    }
    public void BuildLevelButtons()
    {
        // Clear previous buttons
        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }

        int columns = 3;

        for (int i = 0; i < levels.Count; i++)
        {
            LevelData level = levels[i];
            GameObject buttonObj = Instantiate(buttonPrefab, buttonParent);

            Button button = buttonObj.GetComponent<Button>();
            Image buttonImage = buttonObj.GetComponent<Image>();
            button.interactable = level.unlocked;

            if (buttonImage != null && level.previewImage != null)
            {
                buttonImage.sprite = level.previewImage;
            }

            RectTransform rt = buttonObj.GetComponent<RectTransform>();
            int row = i / columns;
            int col = i % columns;

            float buttonWidth = rt.rect.width * rt.localScale.x;
            float buttonHeight = rt.rect.height * rt.localScale.y;
            float spacing = 60f;

            rt.anchoredPosition = new Vector2(
                col * (buttonWidth + spacing) - 1 * (buttonWidth + spacing),
                -row * (buttonHeight + spacing) + 1 * (buttonHeight + spacing)
            );

            button.onClick.AddListener(() => LoadScene(level.sceneName));
        }
    }



    public void NextLevel(String currentLevel)
    {
        // You can load the scene here using Unity's SceneManager (ensure your scene is in Build Settings)
        int currentIndex = levels.FindIndex(level => level.sceneName == currentLevel);
        if (currentIndex != -1 && currentIndex + 1 < levels.Count)
        {
            levels[currentIndex + 1].unlocked = true;
            Debug.Log(currentIndex);
            BuildLevelButtons();
            LoadScene(levels[currentIndex + 1].sceneName);
        }
        else
        {
            Debug.LogWarning("Current level not found in level list!");
        }
    }
    void LoadScene(String sceneName)
    {
        // You can load the scene here using Unity's SceneManager (ensure your scene is in Build Settings)
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        CanvasManager.Instance.uiManager.GetComponent<UIToggler>().ClosePanels();
    }
}
