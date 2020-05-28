using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolve : MonoBehaviour
{
    Material material;

    bool isDissolving = false;
    bool positiveDissolve = false;
    float fade = 1f;

    void Start()
    {
        // Get a reference to the material
        material = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            isDissolving = true;
            positiveDissolve = !positiveDissolve;
        }

        if (isDissolving)
        {
            if (positiveDissolve)
            {
                fade -= Time.deltaTime;

                if (fade <= 0f)
                {
                    fade = 0f;
                    isDissolving = false;
                }
            } else
            {
                fade += Time.deltaTime;

                if (fade >= 1f)
                {
                    fade = 1f;
                    isDissolving = false;
                }
            }

            // Set the property
            material.SetFloat("_Fade", fade);
        }
    }
}