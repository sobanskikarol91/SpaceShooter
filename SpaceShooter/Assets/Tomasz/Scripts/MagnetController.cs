using UnityEngine;
using System.Collections;

public class MagnetController : MonoBehaviour
{

    public GameObject spherePub;
    public float radiusMultipler;
    public float lastingSphereTime;


    private GameObject sphere;
    private GameObject magnetingSphere;
    private float startingDistance;
    private Transform shipTransform;


    // Use this for initialization
    void Start()
    {
        radiusMultipler = 1;
        shipTransform = transform;
    }


    public float getStartingdDistance()
    {
        return startingDistance;
    }


    public void setStartingDistance(float distance)
    {
        startingDistance = distance;
    }


    public void UseMagnet()
    {
        PickUpGUIController.instance.ActiveForWhile(PickUpGUIController.instance.magnet, lastingSphereTime);   // light HUD magnet Icone    

        magnetingSphere = new GameObject();
        magnetingSphere.gameObject.transform.SetParent(shipTransform);
        magnetingSphere.name = "MagnetingSphere";
        magnetingSphere.AddComponent<SphereCollider>();
        SphereCollider colliderReference = magnetingSphere.GetComponent<SphereCollider>();
        colliderReference.radius = 3F * radiusMultipler;
        magnetingSphere.transform.position = shipTransform.position;
        magnetingSphere.tag = "PickUp"; // To avoid triggering shots
        sphere = Instantiate(spherePub);
        sphere.name = "MagnetingSphereClone";

        if (!sphere) Debug.Log("Error with sphere reference");
        sphere.transform.SetParent(shipTransform);
        sphere.transform.position = magnetingSphere.transform.position;
        sphere.transform.localScale = new Vector3(6f * radiusMultipler, 6f * radiusMultipler, 6f * radiusMultipler);
        sphere.GetComponent<MeshRenderer>().enabled = true;

        StartCoroutine(DecreaseSphereTime());
    }


    IEnumerator DecreaseSphereTime()
    {
        float lastingTime = lastingSphereTime;
        while (lastingTime > 0)
        {

            lastingTime -= Time.deltaTime;
            yield return null;
        }

        sphere.GetComponent<MeshRenderer>().enabled = false;
        Destroy(sphere);
        Destroy(magnetingSphere);
    }

}
