using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerFireCollision : MonoBehaviour
{
    public Tilemap tilemap;
    public string elementTag = "FireHazard";

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(elementTag))
        {
            if (GetComponent<SpriteRenderer>().sprite.name.ToLower().Contains("water") && collision.name == "FireHazard")
            {
                Destroy(collision.transform.gameObject);
                GetComponent<PlayerElementCollision>().DeleteObject();
                AchievementManager.Instance.GetComponent<AchievementManager>().UnlockAchievement("Extinguisher");
            }
        }



    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(elementTag))
        {
            if (GetComponent<SpriteRenderer>().sprite.name.ToLower().Contains("wind") && collision.name == "WindHitbox")
            {
                BoxCollider2D ignitionHitbox = collision.transform.Find("IgnitionHitbox").GetComponent<BoxCollider2D>();

                Vector2 originalSize = ignitionHitbox.size;
                Vector2 originalOffset = ignitionHitbox.offset;

                ignitionHitbox.size = new Vector2(4, 3);

                if (transform.position.x < collision.transform.position.x)
                    ignitionHitbox.offset = new Vector2(1.5f, 0);
                if (transform.position.x > collision.transform.position.x)
                    ignitionHitbox.offset = new Vector2(-1.5f, 0);




                AchievementManager.Instance.GetComponent<AchievementManager>().UnlockAchievement("Fire Blower");
                //StartCoroutine(ResetHitboxAfterSeconds(ignitionHitbox, originalSize, originalOffset, .5f)); // 1.5s for example
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(elementTag))
        {
            if (GetComponent<SpriteRenderer>().sprite.name.ToLower().Contains("wind") && collision.name == "WindHitbox")
            {
                BoxCollider2D ignitionHitbox = collision.transform.Find("IgnitionHitbox").GetComponent<BoxCollider2D>();

                Vector2 originalSize = new Vector2(1, 3);
                Vector2 originalOffset = new Vector2(0, 0);

                AchievementManager.Instance.GetComponent<AchievementManager>().UnlockAchievement("Fire Blower");
                StartCoroutine(ResetHitboxAfterSeconds(ignitionHitbox, originalSize, originalOffset, .5f)); // 1.5s for example
            }
        }
    }


    IEnumerator ResetHitboxAfterSeconds(BoxCollider2D hitbox, Vector2 originalSize, Vector2 originalOffset, float delay)
    {
        yield return new WaitForSeconds(delay);
        hitbox.size = originalSize;
        hitbox.offset = originalOffset;
    }

}
