using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField]GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player.transform.position = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
