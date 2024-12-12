using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] Transform player;
    Vector3 currentPositon;

    // Start is called before the first frame update
    void Start()
    {
        currentPositon = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(currentPositon.x + player.position.x, 
                        currentPositon.y + player.position.y, transform.position.z);    
    }
}
