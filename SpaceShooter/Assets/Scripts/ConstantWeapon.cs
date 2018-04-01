using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LightningBolt))]
public class ConstantWeapon : Weapon
{
    [Tooltip("How far weapon detects enemies")]
    public float radius;
    //[Tooltip("area where weapon can detects enemies")]      // TODO: degrees
    //public float degree;

    public float damage = 0.05f;
    public LayerMask shootableMask;                           // layermask so the raycast only hits things on the shootable layer"  
    public AudioClip weaponEndSnd;
    public float activeEffectTime = 0.1f;                     // how long effect will persist after stop pressing fire button by player


    private LightningBolt lightingBoltScript;                 // reference to lighting bolt script to enable/disable it        
    private Collider[] colliders;                             // references to all objects colliders that will be detect by this gameobject
    private Transform targetTransform;                        // reference to nearest target transform
    private float detectorExitArea = 2;                       // prevents to exit and enter in detector area many times;
    private float timeFromLastShot;                           // prevents to invoke many times corutine when player press al the time fire buttin
    private bool isEnemyInSight;                              // boolen to know that Corutine is active in this moment


    void Start()
    {
        lightingBoltScript = GetComponent<LightningBolt>();                                                 // get reference to lightingBolt script;
    }


    public override bool Shot(bool controllShot)
    {
        if (timeFromLastShot >= activeEffectTime || !isEnemyInSight)                                          // it prevents to invoke this method many times
            Detector();
        else
            timeFromLastShot = 0;

        if (timeToShot > timeBetweenBullets && Time.timeScale != 0  && isEnemyInSight)                     
        {
            timeToShot = 0;                                                               // reset time
            return true;                                                                  // shot was successful
        }
        else
            return false;
    }


    void Detector()
    {
        timeFromLastShot = 0;                                                                           // reset counter
        colliders = Physics.OverlapSphere(transform.position, radius, shootableMask);                   // array to keep all object in raycast area with shotable mask

        if (colliders.Length > 0)                                                                       // if there is any object in array
        {

            targetTransform = colliders[0].transform;                                                   // transform to nearest enemy default first object in array                                                              

            if (colliders.Length > 1)                                                                   // if there is more objects
                targetTransform = NearestEnemy();                                                       // find nearest enemy by shortest distance to gameobject

            if (!lightingBoltScript.enabled)                                                            // if script is already disable
                lightingBoltScript.enabled = true;                                                      // turn on script to show weapon visual effect


            targetTransform.GetComponent<ObjectController>().ConstantDamageByTime(damage);              // take damage enemy
            lightingBoltScript.target = targetTransform;                                                // assign target where lighting will hit to nearest enemy in sight
            audioSource.Play();                                                                         // play weapon sound
            StartCoroutine(CheckIfTargetIsInSight());                                                   // check if is the reason to interrupt weapon lighting
        }
    }


    Transform NearestEnemy()
    {
        float nearestdistance = Mathf.Abs((transform.position - colliders[0].transform.position).magnitude);   // shortest distance to object - assign first object from array
        Collider nearestCollider = colliders[0];                                                               // reference to nearest collider in area, set by default first in array

        for (int i = 0; i < colliders.Length; i++)
        {
            float distance = Mathf.Abs((transform.position - colliders[i].transform.position).magnitude);      // count distance to gameobject colliders

            if (distance < nearestdistance)                  // if distance is shooter that current 
            {
                nearestdistance = distance;                  // set new nearest distance
                nearestCollider = colliders[i];              // remember nearest collider
            }
        }

        return nearestCollider.transform;                    // return nearest transform;
    }


    IEnumerator CheckIfTargetIsInSight()
    {
        isEnemyInSight = true;
        while (targetTransform)                                                                         // if opponent is alive
        {
            timeFromLastShot += Time.deltaTime;

            if (Mathf.Abs((transform.position - targetTransform.position).magnitude) > radius + detectorExitArea || timeFromLastShot > activeEffectTime)   // if target isn't in sight and time from last press fire button by player was to long
                StopOpponentAtack();

            yield return null;
        }
        isEnemyInSight = false;
        StopPlayingEffects();
        timeFromLastShot = activeEffectTime;                                                                // now player can shot in case when opponent die and he holding fire button
    }


    void StopOpponentAtack()
    {
        targetTransform.GetComponent<ObjectController>().InterruptConstantDamage();                         // interrupt taking damage
        targetTransform = null;                                                                             // set target to null to prevent check this object again
    }


    void StopPlayingEffects()
    {
        audioSource.Stop();                                                                                 // stop playing weapon sound
        audioSource.PlayOneShot(weaponEndSnd);                                                              // play one time damped snd
        lightingBoltScript.enabled = false;                                                                 // disable script
    }

}   // Karol Sobański
