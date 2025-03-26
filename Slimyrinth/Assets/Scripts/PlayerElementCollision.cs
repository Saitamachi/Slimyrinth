using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerElementCollision : MonoBehaviour
{
    public Tilemap tilemap;  // Reference to your Tilemap
    public string elementTag = "Elements";  // Tag of the Tilemap

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(elementTag))
        {
        Debug.Log(collision.name);
            Destroy(collision.gameObject);
            
        }
    }
}
