using System.Collections;
using UnityEngine;

public class SwimController : MonoBehaviour
{
    public string waterTag = "Water";

    public float xVelo = .1f;
    public float yVelo = .1f;
    public bool wet = false;


    void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Collision: " + collision.tag);
        if (collision.gameObject.CompareTag(waterTag)) // if collides with water tag
        {
            Debug.Log("Collision: Water");
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>(); // gets the current sprite
            Rigidbody2D rb = GetComponent<Rigidbody2D>(); // gets the current rigidbody
            wet = true;
            if (spriteRenderer != null)
            {
                Debug.Log(spriteRenderer.sprite.name);
                if (!spriteRenderer.sprite.name.ToLower().Contains("water"))
                {
                    Debug.Log("Contains water");
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x * xVelo, yVelo);
                }
            }
        }

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(waterTag)) 
        {
            wet = false;
        }
    }
}
