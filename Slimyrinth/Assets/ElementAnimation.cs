using System.Collections;
using UnityEngine;

public class ElementAnimation : MonoBehaviour
{

    public bool enabledAnimation = true;
    public float moveAmount = .1f;
    private Coroutine animationCoroutine;

    void Update()
    {
        if (enabledAnimation && animationCoroutine == null)
        {
            Debug.Log("Starting Animation");
            animationCoroutine = StartCoroutine(AnimationLoop());
        }
        else if (!enabledAnimation && animationCoroutine != null)
        {
            Debug.Log("Stopping Animation");
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
    }

    IEnumerator AnimationLoop()
    {
        while (enabledAnimation)
        {
            yield return MoveObject(-moveAmount, 1f); // Move down
            yield return MoveObject(moveAmount, 1f);  // Move up
        }
        animationCoroutine = null; // Reset so it can restart if enabled again
    }

    IEnumerator MoveObject(float moveAmount, float duration)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + new Vector3(0, moveAmount, 0);

        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }
}
