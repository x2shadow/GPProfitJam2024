using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : MonoBehaviour
{
    Vector3 newPosition;
    int direction = 1;
    public float speed = 2f;

    void Update()
    {
        if(transform.position.x > 2.5f)  direction *= -1;
        if(transform.position.x < -2.5f) direction *= -1;

        newPosition = Vector3.right * direction * speed * Time.deltaTime;
        transform.position += newPosition;
    }
}
