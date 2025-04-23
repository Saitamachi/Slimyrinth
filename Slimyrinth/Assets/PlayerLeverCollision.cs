using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerLeverCollision : MonoBehaviour
{
    public Tilemap tilemap;
    public string leverTag = "Lever";
    private bool canToggle = true;  // Flag to check if the lever can be toggled

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(leverTag) && canToggle)
        {
            LeverController leverController = collision.gameObject.GetComponent<LeverController>(); // gets the script with the sprite
            if (leverController != null)
            {
                AchievementManager.Instance.GetComponent<AchievementManager>().UnlockAchievement(collision.name);

                leverController.Toggle();
                StartCoroutine(ToggleLeverDelay());

            }
        }
    }
    IEnumerator ToggleLeverDelay()
    {
        canToggle = false; // Prevent further toggling until the delay is over
        yield return new WaitForSeconds(1f); // Wait for 1 second (or any other time you want)
        canToggle = true; // Allow toggling again
    }

}
