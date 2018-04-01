using UnityEngine;
using System.Collections;

public class BackgroundController : MonoBehaviour
{
    public bool spawnObjects = true;                                  // flag lets to spawning objects on background

    [Header("Objects")]
    public GameObject[] galaxyObjects;                                // Planets, whirls, small nebulas, supernovas
    public GameObject[] nebulas;                                      // big nebula
    public GameObject[] asteroids;                                    // asteroids rain
    public GameObject[] spaceRockets;

    [Header("Spawn Points")]
    public Vector3 galaxyObjectsSpawn;                                // position where objects will spawn
    public Vector3 nebulaSpawn;                                       // Big nebula and rain asteroids position
    public Vector3 asteroidSpawn;                                     // asteroid rand spawn position
    public Vector3 spaceRocketSpawn;                                  // space rocket rand span position

    [Header("Time between spawn objects")]
    public float objectsTime;
    public float asteroidsTime;
    public float nebulasTime;
    public float spaceRocketsTime;

    private Boundry boundryMap;                                       // boundry where player can move
    private float objectsTimeLeft, asteroidsTimeLeft, nebulasTimeLeft, spaceRocketsTimeLeft;   // how many times left to spawn objects

    void Start()
    {
        if (!spawnObjects) return;               // if there is no spawnObjets, do nothing

        boundryMap = GameMaster.instance.boundry;
        CreateObjectOnStart(2);                  // create objets that will appeare on screen from begining
        StartCoroutine(SpawnObjectUpdate());     // start loop that create objects on background
    }


    IEnumerator SpawnObjectUpdate()
    {
        while (spawnObjects)                                                     // if command spawn objects is true create all background objects
        {
            if (CheckTimeLeft(ref objectsTime, ref objectsTimeLeft))             // galaxy objects
                CreateRandomObject(ref galaxyObjects, galaxyObjectsSpawn, true, false);

            if (CheckTimeLeft(ref asteroidsTime, ref asteroidsTimeLeft))         // asteroids
                CreateRandomObject(ref asteroids, asteroidSpawn, false, false);

            if (CheckTimeLeft(ref nebulasTime, ref nebulasTimeLeft))             // nebulas
                CreateRandomObject(ref nebulas, nebulaSpawn, false, false);

            if (CheckTimeLeft(ref spaceRocketsTime, ref spaceRocketsTimeLeft))   // space rocket
                CreateRandomObject(ref spaceRockets, spaceRocketSpawn, false, true);

            yield return null;
        }
    }


    bool CheckTimeLeft(ref float maxTime, ref float timeLeft)
    {
        if (timeLeft > 0)                   // if time didn't elapse
        {
            timeLeft -= Time.deltaTime;
            return false;
        }

        timeLeft = maxTime;                 // if time eplised, reset timer
        return true;
    }


    void CreateRandomObject(ref GameObject[] objects, Vector3 position, bool isRandPositionX, bool isRandPositionZ)
    {
        int randIndex = Random.Range(0, objects.Length);                                                                            // choose random index in array
        GameObject randObject = objects[randIndex];                                                                                 // choose GameObject in array

        if (isRandPositionX)                                                                                                        // if is nessesery to set random position
            position = new Vector3(Random.Range(boundryMap.left, boundryMap.right), position.y, position.z);                                  // choose random position on X axis


        if (isRandPositionZ)                                                                                                        // if is nessesery to set random position
            position = new Vector3(position.x, position.y, Random.Range(1, boundryMap.up));                                         // choose random position on Y axis

        GameObject newObject = Instantiate(randObject, position, randObject.transform.rotation) as GameObject;                      // create new GameObject
        newObject.transform.SetParent(GameMaster.instance.hierarchyGuard);                                                          // set parent to hierarchy guard
    }


    void CreateObjectOnStart(int howMany)                                                                                           // create few galaxy objets on start
    {
        for (int i = 0; i < howMany; i++)                                                                                           
            CreateRandomObject(ref galaxyObjects, new Vector3(0, galaxyObjectsSpawn.y, 0), true, true);
    }
}  // Karol Sobanski
