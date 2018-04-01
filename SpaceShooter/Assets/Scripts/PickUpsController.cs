using UnityEngine;
using System.Collections;


[RequireComponent(typeof(AudioSource), typeof(Animator))]
public class PickUpsController : MonoBehaviour
{
    public int value;
    public float activeTime;

    public bool recoveryPackage;
    public bool fuel;
    public bool ammo;
    public bool bomb;
    public bool shield;
    public bool star;
    public bool magnet;
    public bool silverCoin;
    public bool bulletTime;
    public bool stamina;
    public bool speed;
    public bool fireRate;
    public GameObject gun;


    private AudioSource audioSource;
    private Animator animator;
    private const string collected = "collected";
    private bool isCollected;                               // its prevent to collect item more times


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }


    void OnTriggerStay(Collider other)
    {
        if (other.name.Equals("MagnetingSphere") && silverCoin)
        {
            Vector3 distanceVector = other.GetComponentInParent<Transform>().position - transform.position; // Player.transform
            float startingDistance = other.GetComponentInParent<MagnetController>().getStartingdDistance();
            GetComponent<Rigidbody>().velocity = distanceVector.normalized * startingDistance * startingDistance / distanceVector.magnitude * 40 * Time.deltaTime;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("MagnetingSphere") && silverCoin)
        {
            this.GetComponent<Rigidbody>().velocity = (other.GetComponentInParent<Transform>().position - transform.position) * 40 * Time.deltaTime;

            float distance = (other.GetComponentInParent<Transform>().position - transform.position).magnitude;
            other.GetComponentInParent<MagnetController>().setStartingDistance(distance);
        }

        if (other.tag.Equals("Player") && !isCollected)
        {
            isCollected = true;

            if (bomb)
                other.GetComponent<BombController>().AddBomb(value);
            else if (shield)
                other.GetComponent<ShieldController>().ActiveShield(value);                      // get refenence to shield Controller and active player shield
            else if (star)
                GameMaster.instance.AddStars(value);                                             // increase player score and add star
            else if (bulletTime)
                GameMaster.instance.BulletTimeOn();                                              // change timeScale to achive bullettime effect
            else if (silverCoin)
                GameMaster.instance.IncreaseScore(1);
            else if (stamina)
                other.GetComponent<StaminaController>().IncreaseStamina(value);
            else if (magnet)
                other.GetComponent<MagnetController>().UseMagnet();
            else
            {
                PlayerController playerController = other.GetComponent<PlayerController>();          // Get reference to player controller

                if (recoveryPackage)
                    playerController.IncreaseHealth(value);
                else if (fuel)
                    playerController.Refuel(value);
                else if (ammo)
                    playerController.AddAmmo(value);
                else if (fireRate)
                    playerController.ChangeFireRate(value, activeTime);
                else if (speed)
                    playerController.ChangeSpeed(value, activeTime);
                else if (gun)                                                                        // if there is any gun attached
                {
                    playerController.ChangeWeapons(ref gun);                                         // change player weapon 
                    playerController.AddAmmo(value);                                                 // add aditional ammo
                }
            }


            if (audioSource)                                                                     // if there is sound attacheds
                audioSource.Play();

            animator.SetTrigger(collected);

            StartCoroutine(DestroyAfterFinishAnimation());
        }
    }


    IEnumerator DestroyAfterFinishAnimation()
    {
        yield return new WaitForEndOfFrame();

        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;

        Destroy(gameObject, animationLength);
    }
}   // Karol Sobanski
