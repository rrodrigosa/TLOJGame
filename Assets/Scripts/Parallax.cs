using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, horizontalStartPos, verticalStartPos;
    public GameObject cam;
    [Tooltip("0 = no effect \n1 = full effect (follows the camera)")]
    [Range(0, 1)]
    public float horizontalParallaxEffect;
    [Tooltip("0 = no effect \n1 = full effect (follows the camera)")]
    [Range(0, 1)]
    public float verticalParallaxEffect;
    [Tooltip("Needs SpriteRenderer component")]
    public bool repeatBg = false;

    void Start()
    {
        horizontalStartPos = transform.position.x;
        verticalStartPos = transform.position.y;
        if (repeatBg)
        {
            length = GetComponent<SpriteRenderer>().bounds.size.x;
        }
    }
    void Update()
    {
        HorizontalEffect();
        VerticalEffect();
    }

    void HorizontalEffect()
    {
        float temp = (cam.transform.position.x * (1 - horizontalParallaxEffect));
        float dist = (cam.transform.position.x * horizontalParallaxEffect);
        transform.position = new Vector3(horizontalStartPos + dist, transform.position.y, transform.position.z);
        RepeatBackground(repeatBg, temp);
    }

    void VerticalEffect()
    {
        float verticalDist = (cam.transform.position.y * verticalParallaxEffect);
        transform.position = new Vector3(transform.position.x, verticalStartPos + verticalDist, transform.position.z);
    }

    void RepeatBackground(bool value, float temp)
    {
        if (value)
        {
            if (temp > horizontalStartPos + length)
            {
                horizontalStartPos += length;
            }
            else if (temp < horizontalStartPos - length)
            {
                horizontalStartPos -= length;
            }
        }
    }
}
