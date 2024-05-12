using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Water_Spring : MonoBehaviour
{
    private float force = 0;

    public float velocity = 0;
    public float height = 0f;

    [SerializeField]private float target_height = 0f;
     
    public void WaveSpringUpdate(float springStiffness, float dampening)
    {
        height = transform.localPosition.y;

        var x = height - target_height;
        var loss = -dampening * velocity;

        force = -springStiffness * x + loss;

        velocity += force; //* Time.fixedDeltaTime;
        velocity = Mathf.Clamp(velocity, -0.3f, 0.3f);
        var y = transform.localPosition.y;
        y = Mathf.Clamp(y + velocity, -target_height, target_height);
        transform.localPosition = new Vector3(transform.localPosition.x , y, transform.localPosition.z);
        
    }
    public void WaveSpringUpdate(float springStiffness)
    {
        height = transform.localPosition.y;
        height = Mathf.Clamp(height, -target_height, target_height);

        var x = height - target_height;

        force = -springStiffness * x;

        velocity += force * Time.deltaTime;
        velocity = Mathf.Clamp(velocity, -0.22f, 0.22f);
        var y = transform.localPosition.y;
        transform.localPosition = new Vector3(transform.localPosition.x, y + velocity, transform.localPosition.z);
    }
}
