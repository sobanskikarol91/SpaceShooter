using UnityEngine;
using System.Collections;


[RequireComponent(typeof(AudioSource))]
public abstract class Weapon : MonoBehaviour
{
    public float timeBetweenBullets = 0.15f;                        // time between each shot
    public Transform bulletSpawn;                                   // place where bullet will spawn in weapon
    public AudioClip gunAudio;

    protected float timeToShot;                                     // time to next shot
    protected ParticleSystem gunParticle;
    protected Light gunLight;
    protected AudioSource audioSource;


    private bool isFireRateIncreased;                               // prevents to increase many firerate many time in while
    private float orginalFireRate;

    
    protected virtual void Update()
    {
        timeToShot += Time.deltaTime;                               // update time to next shot
    }


    void Awake()
    {
        audioSource = GetComponent<AudioSource>();                                   // get reference to AudioSource
    }


    protected virtual void Start()
    {
        gunParticle = bulletSpawn.GetComponent<ParticleSystem>();                    // get reference to ParticleSystem
        gunLight = bulletSpawn.GetComponent<Light>();                                // get reference to Light
        orginalFireRate = timeBetweenBullets;                                        
    }


    protected void ShotEffects()
    {
        if (gunAudio)                                                               // if there is audio clip attached to object
            SoundManager.instance.RandomizeSfx(ref gunAudio, ref audioSource);

        if (gunLight)                                                               // if there is gunLight
            gunLight.enabled = true;

        gunParticle.Stop();
        gunParticle.Play();
    }


    public void ChangeFireRate(float IncrasePercentFireRate, float howLong)
    {
        if (isFireRateIncreased)                                                      // if corutine is already play
        {
            StopCoroutine("IEChangeFireRate");                                        // stop it 
            timeBetweenBullets = orginalFireRate;                                     // and set orginal value
        }

        StartCoroutine(IEChangeFireRate(IncrasePercentFireRate, howLong));
    }


    IEnumerator IEChangeFireRate(float IncrasePercentFireRate, float howLong)
    {
        isFireRateIncreased = true;

        timeBetweenBullets /= IncrasePercentFireRate / 100;    // increase by percent amount

        yield return new WaitForSeconds(howLong);

        timeBetweenBullets = orginalFireRate;
        isFireRateIncreased = false;
    }


    public abstract bool Shot(bool controllShot);                                   // controll shot allows parent bullet to gameobject, when ship is rotate bullet rotate with him

}   // Karol Sobanski
