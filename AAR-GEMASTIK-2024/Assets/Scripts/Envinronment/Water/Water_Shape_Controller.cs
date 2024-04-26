using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water_Shape_Controller : MonoBehaviour
{
    [SerializeField] private float springStiffness = 0.1f;
    [SerializeField] private float dampening = 0.03f;
    [SerializeField] private List<Water_Spring> springs;
    public float spread = 0.006f;
    private void Awake()
    {
        Splash(3, 0.1f);
    }

    private void FixedUpdate()
    {
        foreach(Water_Spring spring in springs)
        {
            spring.WaveSpringUpdate(springStiffness);
        }
        UpdateSprings();
    }
    private void UpdateSprings() { 
        int count = springs.Count;
        float[] left_deltas = new float[count];
        float[] right_deltas = new float[count];

        for(int i = 0; i < count; i++) {
            if (i > 0) {
                left_deltas[i] = spread * (springs[i].height - springs[i-1].height);
                springs[i-1].velocity += left_deltas[i];
            }
            if (i < springs.Count - 1) {
                right_deltas[i] = spread * (springs[i].height - springs[i+1].height);
                springs[i+1].velocity += right_deltas[i];
            }
        }
    }
    private void Splash(int index, float speed)
    {
        if(index>= 0 && index <springs.Count)
        {
            springs[index].velocity += speed;
        }
    }
}
