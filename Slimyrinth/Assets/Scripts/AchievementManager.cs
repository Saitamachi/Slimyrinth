using System;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class AchievementData
{
    public Sprite sprite;
    public String name;
    public String description;
    public bool unlocked;
}

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    public List<AchievementData> achievements;

    public AudioClip unlockSound;
    public AudioSource audioSource;

    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UnlockAchievement(string achievementName)
    {
        Debug.Log("name: "+achievementName);
        AchievementData achievement = achievements.Find(a => a.name == achievementName);

        if (achievement != null && !achievement.unlocked)
        {
            achievement.unlocked = true;

            if (audioSource && unlockSound)
                audioSource.PlayOneShot(unlockSound);

        }
    }
}
