using UnityEngine;
using System.Collections;

public class ConstantWeaponRay : Weapon
{
    public float activeEffectTime = 0.1f;                     // how long effect will persist after stop pressing fire button by player
    public GameObject bullet;
    public GameObject wave;
    public float Disturbance = 0;
    public AudioClip endDeathRay;

    private float timeFromLastShot;                           // prevents to invoke many times corutine when player press al the time fire buttin
    private Vector3 scale;
    private GameObject createdBullet;
    private BeamParam beamParametr;
    private bool isRayfinished = true;


    void Start()
    {
        beamParametr = GetComponent<BeamParam>();
        timeFromLastShot = activeEffectTime;
        scale = new Vector3(beamParametr.Scale, beamParametr.Scale, beamParametr.Scale);   // get scale value from script
    }


    public override bool Shot(bool controllShot)
    {
        if (isRayfinished)                                          // it prevents to invoke this method many times
            StartCoroutine(GenerateRay());
        else
            timeFromLastShot = 0;

        if (timeToShot > timeBetweenBullets && Time.timeScale != 0)
        {
            timeToShot = 0;                                                               // reset time
            return true;                                                                  // shot was successful
        }
        else
            return false;
    }


    IEnumerator GenerateRay()
    {
        timeFromLastShot = 0;                                                             // reset counter
        StartRay();
        yield return StartCoroutine(KeepRay());
        EndRay();
    }


    void StartRay()
    {
        GameObject wav = Instantiate(wave, transform.position, transform.rotation) as GameObject;         // create new wave
        wav.transform.Rotate(Vector3.left, 90.0f);                                                        // set wave rotate

        wav.GetComponent<BeamWave>().col = beamParametr.BeamColor;                                        // set color wave like this
        createdBullet = Instantiate(bullet, transform.position, transform.rotation) as GameObject;        // create new bullet

        createdBullet.transform.parent = transform;                                                       // parented it
        createdBullet.transform.localScale = scale;                                                       // set
        createdBullet.GetComponent<BeamParam>().SetBeamParam(beamParametr);
    }


    IEnumerator KeepRay()
    {
        audioSource.Play();

        isRayfinished = false;

        while (timeFromLastShot < activeEffectTime)
        {
            timeFromLastShot += Time.deltaTime;                                                           // increase time from last shoot
            yield return null;
        }
        isRayfinished = true;

        audioSource.Stop();
        audioSource.PlayOneShot(endDeathRay);        
    }


    void EndRay()
    {
        if (createdBullet)
            createdBullet.GetComponent<BeamParam>().bEnd = true;
    }
}
