using UnityEngine;

public class GetSlimeSprite : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Sprite slimeSprite;
    void Start()
    {

    }

    public Sprite GetSprite()
    {
        return slimeSprite;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
