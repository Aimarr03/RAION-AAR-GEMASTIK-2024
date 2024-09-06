using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyFollowEffect : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;
    public float parallaxEffect;

    private SpriteRenderer spriteRenderer;
    void Start()
    {
        startpos = transform.position.x;
        spriteRenderer = GetComponent<SpriteRenderer>();
        length = spriteRenderer.bounds.size.x;
        Vector3 thisPosition = transform.position;
        Vector3 newPositionEast = new Vector3(transform.position.x + length, thisPosition.y, thisPosition.z);
        Vector3 newPositionWest = new Vector3(transform.position.x - length, thisPosition.y, thisPosition.z);

        GameObject newObjectEast = Instantiate(transform.GetChild(0).gameObject, transform);
        newObjectEast.gameObject.SetActive(true);
        newObjectEast.transform.position = newPositionEast;

        GameObject newObjectWest = Instantiate(transform.GetChild(0).gameObject, transform);
        newObjectWest.gameObject.SetActive(true);
        newObjectWest.transform.position = newPositionWest;
    }

    void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}
