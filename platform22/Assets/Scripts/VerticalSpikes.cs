using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalSpikes : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float range;
    Vector2 startPoint;
    int direction = 1;

    void Start()
    {
        startPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y - startPoint.y > range && direction > 0)
        {
            direction *= -1;
        }
        else if (startPoint.y - transform.position.y > range && direction < 0)
        {
            direction *= -1;
        }
        transform.Translate(0, speed * direction * Time.deltaTime, 0);
       
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position,new Vector3(0.5f,range,0));
    }
}
