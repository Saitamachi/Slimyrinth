using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LeverController : MonoBehaviour
{

    public bool status;
    public GameObject door;

    void Start()
    {
        status = false;
        transform.position += new Vector3(0,0,3);
    }

    public bool Toggle()
    {

        status = !status;
        Debug.Log(status);

        Flip(); // flips the lever
        Activate(); // performs the action

        return status;
    }

    private void Flip()
    {
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 180, 0);
    }

    private void Activate()
    {
        // door.open();
        // door.close();
        // door.activate(true/false)
        DoorController doorController = door.GetComponent<DoorController>();
        if(doorController == null)
            return;
        doorController.Activate(status);

    }

}
