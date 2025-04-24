using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnCollisionHandler : MonoBehaviour
{
    public float timeToIgnite = 2f;
    private HashSet<Collider2D> alreadyBurning = new HashSet<Collider2D>();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wood") && !alreadyBurning.Contains(other))
        {
            Debug.Log("burning");
            StartCoroutine(BurnCoroutine(other));
        }
    }

    IEnumerator BurnCoroutine(Collider2D target)
    {
        alreadyBurning.Add(target);
        float timer = 0f;

        while (timer < timeToIgnite)
        {
            Debug.Log("flame: target: " + target);
            Debug.Log("flame: bounds: " + GetComponent<Collider2D>().IsTouching(target));
            if (!target || !GetComponent<Collider2D>().IsTouching(target))
            {
                Debug.Log("flame: break");
                alreadyBurning.Remove(target);
                yield break;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        if (target)
        {
            Debug.Log("flame: destroy");
            Destroy(target.transform.parent.gameObject);
            //gameObject.transform.parent.gameObject.GetComponent<SlimeMovement>().


            if (gameObject.transform.parent.CompareTag("Player"))
            {
                Physics2D.gravity = Vector2.down * 9.81f;
                AchievementManager.Instance.GetComponent<AchievementManager>().UnlockAchievement("Fire Skin");
            }
            else
            {
                AchievementManager.Instance.GetComponent<AchievementManager>().UnlockAchievement("Fire Blower");
            }
        }

        alreadyBurning.Remove(target);
    }


}
