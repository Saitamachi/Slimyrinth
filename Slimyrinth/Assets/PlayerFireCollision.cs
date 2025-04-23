using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerFireCollision : MonoBehaviour
{
    public Tilemap tilemap;
    public string elementTag = "FireHazard";

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(elementTag)) // if collides with elements layer tile map
        {
            if (GetComponent<SpriteRenderer>().sprite.name.ToLower().Contains("water"))
            {
                Destroy(collision.transform.gameObject);
                GetComponent<PlayerElementCollision>().DeleteObject();
                AchievementManager.Instance.GetComponent<AchievementManager>().UnlockAchievement("Extinguisher");
            }


        }
    }
}
