using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class DoorController : MonoBehaviour
{
    void Start()
    {

    }

    public void Activate(bool status)
    {
        if (status)
            Open();
        else
            Close();
    }



    public void Open()
    {
        StartCoroutine(Animation(3f * -transform.right));
        Debug.Log(transform.right);
    }

    public void Close()
    {
        StartCoroutine(Animation(3f * transform.right));

    }

    IEnumerator Animation(Vector3 offset)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + offset;

        float elapsedTime = 0;
        while (elapsedTime < 1f)
        {
            float fraction = elapsedTime / 1f;
            transform.position = Vector3.Lerp(startPosition, targetPosition, fraction);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }
}
