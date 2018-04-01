using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Rigidbody))]
public class RandomRotator : MonoBehaviour
{
    public float rotateSpeed = 100;


    void Start() // Asteroid random rotator
    {
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * Time.deltaTime * rotateSpeed;
    }

}   // Karol Sobanski
