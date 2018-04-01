using UnityEngine;
using System.Collections;

public class MoveDroidLaser : MonoBehaviour {

    public float moveSpeed = 500;
    public Transform target;


    private Rigidbody rigidbody;
   

    // Use this for initialization
    void Start () {

        rigidbody = GetComponent<Rigidbody>();
        transform.LookAt(target);
       
    }
	

	// Update is called once per frame
	void Update () {
        rigidbody = GetComponent<Rigidbody>();

        if (target)
        {
           rigidbody.velocity = transform.forward * moveSpeed * Time.deltaTime;
        }
    }
}
