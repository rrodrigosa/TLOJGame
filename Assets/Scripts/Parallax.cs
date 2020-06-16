using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;
    [Tooltip("0 = no effect \n1 = full effect (follows the camera)")]
    [Range(0, 1)]
    public float parallaxEffect;
    [Tooltip("Needs SpriteRenderer component")]
    public bool repeatBg = false;

    void Start()
    {
        startpos = transform.position.x;
        if (repeatBg)
        {
            length = GetComponent<SpriteRenderer>().bounds.size.x;
        }
    }
    void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        RepeatBackground(repeatBg, temp);
    }

    void RepeatBackground(bool value, float temp)
    {
        if (value)
        {
            if (temp > startpos + length)
            {
                startpos += length;
            }
            else if (temp < startpos - length)
            {
                startpos -= length;
            }
        }
    }
}
