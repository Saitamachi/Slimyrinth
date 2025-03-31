using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerElementCollision : MonoBehaviour
{
    public Tilemap tilemap; 
    public string elementTag = "Elements"; 

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(elementTag)) // if collides with elements layer tile map
        {
            GetSlimeSprite script = collision.gameObject.GetComponent<GetSlimeSprite>(); // gets the script with the sprite
            if (script != null)
            {
                SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = script.GetSprite();
                Destroy(collision.gameObject);
            }
        }
    }
}
