using UnityEngine;
using System.Collections;


public class DestroyByCollision : MonoBehaviour
{
    public int damage = 30;
    public float volume = 0.5f;
    public GameObject[] destroyEffects;
    public AudioSource destroySnd;

    [Tooltip("if projectile hit some object, he can disable his ability to move or shot for while")]
    public bool disableControl;
    public float howLong = 2;

    private bool isDestroyed;      // prevents to restroy object few times



    void OnTriggerEnter(Collider other)
    {
        if (isDestroyed) return;                                                                                    // if objest was destroy return
        if (other.tag == "Background" || gameObject.tag == other.tag || other.tag == "PickUp") return;
        if (tag == "PlayerBullet" && other.tag == "Player" || tag == "EnemyBullet" && other.tag == "Enemy")
            return;


        if (destroyEffects.Length > 0)                                                                              // if ther is some destroyeffects attached
        {
            int randIndex = Random.Range(0, destroyEffects.Length);                                                 // choose random index in array
            GameObject randEffect = destroyEffects[randIndex];                                                      // choose GameObject with randIndex from array
            GameObject newEffect = Instantiate(randEffect, transform.position, transform.rotation) as GameObject;
            newEffect.transform.SetParent(GameMaster.instance.hierarchyGuard);
        }


        ObjectController controller = other.GetComponent<ObjectController>();    // get reference to object controller


        if (controller)                          // if there is a script attached  to other
        {
            controller.TakeDamage(damage, transform.position);

            if (disableControl)
                controller.StartCoroutine("DisableShotAndMove", howLong);
        }

        StartCoroutine(Destroy());
    }


    IEnumerator Destroy()
    {
        isDestroyed = true;

        if (destroySnd)
        {
            SoundManager.instance.RandomizeSfx(ref destroySnd);
            while (destroySnd.isPlaying)
                yield return null;
        }

        GameMaster.instance.RemoveObject(transform);
        Destroy(gameObject);
    }

}   // Karol Sobanski