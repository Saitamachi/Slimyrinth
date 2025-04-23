using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementMenu : MonoBehaviour
{
    public GameObject backButtonPanelSwitch;
    public GameObject achievementPrefab;
    public Transform achievementParent;

    void Start()
    {
        BuildAchievementCards();
    }

    public void BuildAchievementCards()
    {
        // Clear previous achievement cards except back button
        foreach (Transform child in achievementParent)
        {
            if (child.name == "Back" || child.name == "Global Volume")
                continue;

            Destroy(child.gameObject);
        }

        float spacing = 14f; // Adjust based on prefab height + desired gap

        for (int i = 0; i < AchievementManager.Instance.achievements.Count; i++)
        {
            var achievement = AchievementManager.Instance.achievements[i];
            GameObject card = Instantiate(achievementPrefab, achievementParent);

            RectTransform rt = card.GetComponent<RectTransform>();

            float buttonWidth = rt.rect.width * rt.localScale.x;
            float buttonHeight = rt.rect.height * rt.localScale.y;
            rt.anchoredPosition = new Vector2(rt.localPosition.x, -i * (spacing + buttonHeight) + spacing + buttonHeight + 300f);

            Transform nameTransform = card.transform.Find("Name");
            Transform descriptionTransform = card.transform.Find("Description");

            if (nameTransform != null && descriptionTransform != null)
            {
                TextMeshProUGUI nameText = nameTransform.GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI descriptionText = descriptionTransform.GetComponent<TextMeshProUGUI>();

                if (nameText != null) nameText.text = achievement.unlocked ? achievement.name : "????";
                if (descriptionText != null) descriptionText.text = achievement.unlocked ? achievement.description : "????";
            }

            // 🔽 Image handling (similar to your LevelData card logic)
            Transform backgroundTransform = card.transform.Find("Background"); // Assuming this is the container for the background
            if (backgroundTransform != null)
            {
                Image image = card.GetComponent<Image>();
                Image background = backgroundTransform.GetComponent<Image>();
                if (image != null && background != null)
                {
                    image.sprite =achievement.sprite; // ← Uncomment when sprites ready

                    Color color = achievement.unlocked ? Color.white : new Color(0.2f, 0.2f, 0.2f, 1f); // Dimmed gray if locked
                    image.color = color;
                    Color color2 = new Color(0f, 0f, 0f, .2f); // Dimmed gray if locked
                    background.color = color2;
                }
            }
        }

    }

    public void UnlockAchievement(string achievementName)
    {
        
        //AchievementManager.Instance.UnlockAchievement(achievementName);
        BuildAchievementCards(); // Refresh UI
    }

    public void OnBackPressed()
    {
        CanvasManager.Instance.uiManager.GetComponent<UIToggler>().SelectPanel(backButtonPanelSwitch);
    }
}
