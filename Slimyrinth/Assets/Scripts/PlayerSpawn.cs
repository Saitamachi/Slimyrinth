using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public Transform spawnPoint; 

    void Start()
    {
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
        }
    }
}
