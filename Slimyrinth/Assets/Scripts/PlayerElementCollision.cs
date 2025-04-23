using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerElementCollision : MonoBehaviour
{
    public Tilemap tilemap;
    public string elementTag = "Elements";

    public Sprite defaultSprite;



    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(elementTag)) // if collides with elements layer tile map
        {
            GetSlimeSprite script = collision.gameObject.GetComponent<GetSlimeSprite>(); // gets the script with the sprite
            if (script != null)
            {
                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = script.GetSprite();
                DisableObject(collision.gameObject);

                AchievementManager.Instance.GetComponent<AchievementManager>().UnlockAchievement(collision.name);

            }
        }
    }


    public void DisableObject(GameObject obj)
    {
        foreach (Transform child in tilemap.transform)
        {
            child.gameObject.SetActive(true);
        }
        obj.gameObject.SetActive(false);
    }

    public void DeleteObject()
    {
        foreach (Transform child in tilemap.transform)
        {
            if(!child.gameObject.activeSelf)
                Destroy(child.gameObject);
        }
        GetComponent<SpriteRenderer>().sprite = defaultSprite;
    }
}
