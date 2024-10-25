using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundGradientManager : MonoBehaviour
{
    public float cycleDuration = 5f;  // Time in seconds for one color lerp cycle
    private float t;  // Progress tracker
    private SpriteRenderer backGround;
    // Define an array of gradients
    public Gradient[] gradients;
    private int currentGradientIndex;

    void Start()
    {
        if (gradients.Length > 0)
            currentGradientIndex = 0;
        backGround = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (gradients.Length == 0) return;

        // Progress through the current gradient
        t += Time.deltaTime / cycleDuration;
        backGround.color = gradients[currentGradientIndex].Evaluate(t);

        // When one cycle is complete, switch to the next gradient
        if (t >= 1f)
        {
            t = 0f;
            //Check when list reaches last index to restart
            currentGradientIndex = (currentGradientIndex + 1) % gradients.Length;
        }
    }
}
