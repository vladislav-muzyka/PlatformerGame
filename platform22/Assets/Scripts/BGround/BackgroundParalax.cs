using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParalax : MonoBehaviour
{
    [SerializeField] Vector2 parallaxEffect;
    [SerializeField] bool infiniteHorizontal;
    [SerializeField] bool infiniteVertical;
    Transform camTransform;
    Vector3 lastCamPos;
    float spriteSizeX;
    float spriteSizeY;

    void Start()
    {
        camTransform = Camera.main.transform;
        lastCamPos = camTransform.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture2D = sprite.texture;
        spriteSizeX = texture2D.width / sprite.pixelsPerUnit;
        spriteSizeY = texture2D.height / sprite.pixelsPerUnit;
    }

    void FixedUpdate()
    {
        Vector3 deltaMovement = camTransform.position - lastCamPos;
        transform.position += new Vector3(deltaMovement.x * parallaxEffect.x, deltaMovement.y * parallaxEffect.y);
        lastCamPos = camTransform.position;
        if (infiniteHorizontal)
        {
            if (Mathf.Abs(camTransform.position.x - transform.position.x) >= spriteSizeX)
            {
                transform.position = new Vector3(camTransform.position.x , transform.position.y);
            }
        }
        if (infiniteVertical)
        {
            if (Mathf.Abs(camTransform.position.y - transform.position.y) >= spriteSizeY)
            {
                transform.position = new Vector3(camTransform.position.x, transform.position.y);
            }
        }

    }

}
