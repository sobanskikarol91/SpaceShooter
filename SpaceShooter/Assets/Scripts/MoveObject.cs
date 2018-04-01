using UnityEngine;
using System.Collections;


public class MoveObject : MonoBehaviour
{

    private Vector3 offset;
    public float speed = -0.2f;


    void Start()
    {
        offset = new Vector3(0, 0, speed);
    }


    void Update()
    {
        transform.position += offset * Time.deltaTime;                             // set new position
    }

}  // Karol Sobanski


