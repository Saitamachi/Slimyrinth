using UnityEngine;

public class FireHazardManager : MonoBehaviour
{
    private BoxCollider2D hitbox;
    public GameObject slime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitbox = GetComponent<BoxCollider2D>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (slime.GetComponent<SpriteRenderer>().sprite.name.ToLower().Contains("fire"))
        {
            hitbox.isTrigger = true;
        }
        else if (slime.GetComponent<SpriteRenderer>().sprite.name.ToLower().Contains("water"))
        {
            hitbox.isTrigger = true;
        }
        else
        {
            hitbox.isTrigger = false;

        }
    }
}
