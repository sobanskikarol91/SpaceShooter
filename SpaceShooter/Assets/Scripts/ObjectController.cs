using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class VisualWeapon                             // class keeps information about what kind of weapon object has and where can be spawn
{
    public GameObject weapon;
    public Transform weaponSpawn;

    [HideInInspector]
    public Weapon weaponScripts;
    public VisualWeapon(GameObject weapon, Transform weaponSpawn)
    {
        this.weapon = weapon;
        this.weaponSpawn = weaponSpawn;
    }
}


[RequireComponent(typeof(Rigidbody))]
public abstract class ObjectController : MonoBehaviour
{
    [Header("Main Statistics")]
    public int maxHealth = 100;
    public float horizontalMove = 0f;
    public float verticalMove = -1f;
    public GameObject destroyExplosion;               // destroy explosion particle effect


    [Header("Weapons")]
    public List<VisualWeapon> visualWeapons = new List<VisualWeapon>();
    [HideInInspector]
    public int weaponLevel = 1;


    [Header("Control")]
    [SerializeField]
    protected bool isShooting = true;                    // some weapons can disable object ability to shot
    [SerializeField]
    protected bool isMoving = true;                      // some weapons can disable object ability to move

    protected Boundry boundryPosition;
    protected Rigidbody rigidbody;
    protected VisualBar healthBar;
    protected float currentHealth;
    protected bool isAlive = true;                                    // flag prevents killing object many times after his dead
    protected bool isShieldActive;                                    // when shield is active object can't die

    [HideInInspector]
    public Material white;
    // object take damage to other when hit
    [Tooltip("How many frames object will change his material to white after taking damage")]
    private float whiteFrames = 2;
    private MeshRenderer[] meshRenderers;             // all renderer in model will be change to white color after taking damage
    private Material[] orginalMaterials;
    private IEnumerator IEInteruptConstantDamageFun;

    protected virtual void Start()
    {
        boundryPosition = GameMaster.instance.boundry;                // set boundry form GameMaster
        rigidbody = GetComponent<Rigidbody>();                        // get reference to health bar
        currentHealth = maxHealth;

        if (visualWeapons.Count > 0)                                  // get Reference to all Weapons
            for (int i = 0; i < visualWeapons.Count; i++)
                visualWeapons[i].weaponScripts = WeaponCreator(ref visualWeapons[i].weapon, ref visualWeapons[i].weaponSpawn);
    }


    public IEnumerator DisableShotAndMove(float howLong)      // some weapons projectiles can disable object ability to move and shoot for while
    {
        if (!isShieldActive)                                  // if shield isn't active
        {
            rigidbody.velocity = Vector3.zero;                // set object velocity to zero

            isShooting = isMoving = false;
            yield return new WaitForSeconds(howLong);
            isShooting = isMoving = true;
        }
        else
            yield return null;
    }


    public virtual void ConstantDamageByTime(float damage)                         // ConstantWeapon'll invoke this
    {
          IEInteruptConstantDamageFun = IEConstantDamageByTime(damage);            // remember with corutine will start
          StartCoroutine(IEInteruptConstantDamageFun);                             // start it
    }


    public virtual void InterruptConstantDamage()                                  // ConstantWeapon'll invoke this
    {
        StopCoroutine(IEInteruptConstantDamageFun);                                // stop this corutine
    }


    protected Weapon WeaponCreator(ref GameObject weapon, ref Transform weaponSpawn)
    {
        GameObject newWeapon = Instantiate(weapon, weaponSpawn.position, weapon.transform.rotation) as GameObject;             // Create Weapon in weapon spown pint
        newWeapon.transform.SetParent(weaponSpawn);                                                                            // parentto spawn weapon point
        return newWeapon.GetComponent<Weapon>();                                                                               // return Weapon script
    }


    protected virtual void DestroyEffect()                                                                                     // this part player and enemy has the same
    {
        if (destroyExplosion != null)
        {
            GameObject newExplosion = Instantiate(destroyExplosion, transform.position, transform.rotation) as GameObject;     // Create explosion
            newExplosion.transform.SetParent(GameMaster.instance.hierarchyGuard);
        }
    }


    protected virtual void Move()
    {
        Vector3 movement = new Vector3(horizontalMove, 0, verticalMove);
        rigidbody.velocity = movement;
    }


    protected IEnumerator SwitchMaterial()
    {
        SetMaterialToWhite();

        for (int i = 0; i < whiteFrames; i++)                             // wait whiteFrames before change material again
            yield return null;

        SetMaterialToOrginal();
    }


    IEnumerator IEConstantDamageByTime(float damage)                      //  weapon like lighting bolt can produce constant damage by time
    {
        int framesToExplosion = 10;
        int currentFrame = 0;

        while (true)                                                      // only Method interuptConstantDamage can  interrupt this, or break
        {
            currentFrame++;

            if (currentFrame == framesToExplosion)                        // it prevents to call random explosion everyframe
            {
                ExplosionController.instance.RandomExplosionEffect(transform.position);
                currentFrame = 0;
            }

            IEConstandDamageByTimeAdditional();                           // method with additional option, can be implement in  inhereted classes
            currentHealth -= damage;
            healthBar.UpdateBar(currentHealth, maxHealth);

            if (currentHealth <= 0)
            {
                DestroyEffect();
                break;                                                    // exit the loop
            }

            yield return null;
        }
    }


    protected void SetMaterialArrays()
    {
        meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();      // get all mesh renderes in objects

        int size = meshRenderers.Length;
        orginalMaterials = new Material[size];                                   // set array size

        for (int i = 0; i < size; i++)
            orginalMaterials[i] = meshRenderers[i].material;
    }


    void SetMaterialToWhite()
    {
        for (int i = 0; i < meshRenderers.Length; i++)
            if (meshRenderers[i])
                meshRenderers[i].material = white;
    }


    void SetMaterialToOrginal()
    {
        for (int i = 0; i < meshRenderers.Length; i++)
            if (meshRenderers[i])
                meshRenderers[i].material = orginalMaterials[i];
    }


    public abstract void IEConstandDamageByTimeAdditional();                   // additional options in IEConstandDamageByTime
    public abstract void TakeDamage(float damage, Vector3 damagePosition);
    protected abstract void CheckBoundry();                                    // enemy and player have your own boundry

}   //Karol Sobanski