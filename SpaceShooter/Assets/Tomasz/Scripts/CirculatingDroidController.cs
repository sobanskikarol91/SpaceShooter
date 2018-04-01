using UnityEngine;
using System.Collections.Generic;
using System;

public class CirculatingDroidController : ObjectController
{

    public float r;
    public float t;     // circuit travel time
    public GameObject droidPrefab;
    public List<GameObject> Droids;
    public int droidCount = 0; //for controll initiial droids in inspector
    public int maxDroids = 3;
    public float[] droidSpawnAngles;

    private GameObject circulatingSphere;   //prefab for droid
    private Transform opponentTransform;    //nearest oponent


    void Start()
    {
        for (int i = 0; i < droidCount; i++)
            AddDroid();
    }


    public void AddDroid()
    {
        circulatingSphere = Instantiate(droidPrefab);
        circulatingSphere.transform.SetParent(transform);   // set playership transform as parent transform
        circulatingSphere.transform.localPosition = new Vector3(r, 0f, 0f);
        circulatingSphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        Droids.Add(circulatingSphere);
        Droids[Droids.Count - 1].transform.position = Droids[0].transform.position;
        Droids[Droids.Count - 1].transform.rotation = Droids[0].transform.rotation;
        Droids[Droids.Count - 1].transform.RotateAround(transform.position, Vector3.up, droidSpawnAngles[Droids.Count - 1]);
    }


    Transform findClosestOponent()
    {
        Transform enemyTransform = null;
        float distance;
        float smallestDistance = 10000f;

        foreach (Transform child in GameMaster.instance.hierarchyGuard)
        {
            if (child.tag == "Enemy")
            {
                distance = (transform.position - child.position).magnitude;
                if (distance <= smallestDistance)
                {
                    enemyTransform = child;
                    smallestDistance = distance;
                }
            }
        }

        return enemyTransform;
    }


    public void DestroyAllDroids()
    {
        foreach (GameObject droid in Droids)
        {
            Destroy(droid);
        }
        Droids.Clear();
    }


    void Update()
    {


        foreach (GameObject droid in Droids)
        {
            droid.transform.RotateAround(transform.position, transform.up, 360 * Time.deltaTime / t);
        }

        opponentTransform = findClosestOponent();

        if (isShooting && opponentTransform)
        {

            if (opponentTransform)
            {

                foreach (GameObject droid in Droids)
                {
                    droid.GetComponent<SlowWeapon>().bullet.GetComponent<MoveDroidLaser>().target = opponentTransform;
                    droid.GetComponent<SlowWeapon>().Shot(false);
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.P) && Droids.Count < maxDroids)
        {
            AddDroid();
        }
    }


    public override void TakeDamage(float damage, Vector3 damagePosition)
    {
        throw new NotImplementedException();
    }


    protected override void CheckBoundry()
    {
        throw new NotImplementedException();
    }


    public override void IEConstandDamageByTimeAdditional()
    {
        throw new NotImplementedException();
    }
}
