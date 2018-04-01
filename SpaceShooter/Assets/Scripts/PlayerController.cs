using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(SmokeController))]
public class PlayerController : ObjectController
{
    public Transform weaponSpawnMain;                       // reference to main weapon spawn transform
    public Transform weaponSpawnLeft;
    public Transform weaponSpawnRight;

    [Header("Player Statistics")]
    public int maxammunition = 200;                         // how many ammunition player have               
    public float tiltSpeed = 5f;                            // how fast wings'll be rotate
    public float sight = 20f;                               // how far player can display enemy health bar                    
    public float changeColor = 3f;                          // time to change color after take damage
    public bool isStaminaByTime;                            // increase stamina with time
    public bool isStaminaByKill;                            // increase stamina with killing enemies

    [Header("Fuel")]
    public int maxFuel = 100;                               // how many fuel player has
    [Tooltip("when player fuel will over, we change his movement speed by divide a factor")]
    public float factorMoveWithoutFuel = 3;
    [Tooltip("How fast player change his velocity from normal value to value without fuel")]
    public float speedFallDownFuel = 0.01f;

    [Header("Audio")]
    public AudioSource emptyMagazineSnd;                    // if player doesn't have ammo play empty magazine sound;
    public AudioSource wingsTiltSnd;
    public AudioSource engineSnd;

    [Space(10)]
    public int ammunitionLeft;                              // current amount of ammunition that player has 


    private float currentFuel;                              // how many fuel player has in current time
    private bool isFuelOver;                                // flag which inform if there is any fuel
    private bool isAmmoOver;                                // flag which inform if there is any ammo
    private bool isTiltSoundPlaying;                        // check if tilt sound is still playing
    private Ray ray;                                        // ray from player to forward
    private int enemyLayer;                                 // player shoots ray and check if its enemy layer
    private VisualBar enemyHealthBar;                       // it is visual enemy health bar on screen
    private VisualBar fuelBar;                              // it is visual bar script, that control color of fuel bar on screen
    private Text ammunitionText;                            // ammount of ammunition display on screen
    private Image damageImage;                              // damage image will show on screen when player take damage
    private Color damageColor = new Color(255, 0, 0, 0.2f); // what color screen will have when player'll take damage
    private float orginalSpeed;
    private bool isSpeedChanged;                            // flag to know that orginal player speed was change

    // References
    private Animator animator;
    private SmokeController smokeController;
    private MagnetController magnetController;
    private StaminaController staminaController;
    private BombController bombController;

    private const string namePlayerHealth = "PlayerHealth";
    private const string namePlayerFuel = "PlayerFuel";



    void Awake()
    {
        AdjustVisualBars();                                 // this must be in Awake because other script: ShieldController need reference to VisualBar
    }


    protected override void Start()
    {
        base.Start();
        ammunitionLeft = maxammunition;

        // get references
        magnetController = GetComponent<MagnetController>();
        animator = transform.Find("PlayerModel").GetComponent<Animator>();
        damageImage = GameObject.Find("DamageImage").GetComponent<Image>();
        enemyHealthBar = GameObject.Find("EnemyHealth").GetComponent<VisualBar>();
        ammunitionText = GameObject.Find("Ammunition").GetComponent<Text>();
        smokeController = GetComponent<SmokeController>();
        staminaController = GetComponent<StaminaController>();
        bombController = GetComponent<BombController>();

        ammunitionText.text = ammunitionLeft.ToString();
        enemyLayer = LayerMask.GetMask("Enemy");                                        // Get enemy mask
        orginalSpeed = horizontalMove;                                                  // remember orginal speed because some method can change it
        currentFuel = maxFuel;
        healthBar.UpdateBar(currentHealth, maxHealth);                                  // update healthBar
    }


    void Update()
    {
        if (!isAlive) return;                                      // if player is death do nothing

        IsEnemyOnSight();                                          // if player can see enemy, display his health bar
        CheckBoundry();                                            // check if player try leave mthe map

        if (Input.GetKeyDown(KeyCode.Escape))                      // TEST
            Application.Quit();
        if (Input.GetKeyDown(KeyCode.E))                           // TEST
            bombController.UseBomb();
        if (Input.GetKeyDown(KeyCode.R))                           // TEST
            IncreaseHealth(50);
        if (Input.GetKeyDown(KeyCode.F))
            magnetController.UseMagnet();
        if (Input.GetKeyDown(KeyCode.O))                        //TEST
            GameMaster.instance.GetComponent<SilverCoinSpawnController>().SpawnCoins();
        if (Input.GetKeyDown(KeyCode.Q))
            staminaController.UseSkill();
        if (Input.GetKeyDown(KeyCode.C))
            animator.SetTrigger("Rotate");
        if (isShooting)                                            // if player is able to shot
            Shot();                                                // shot

        if (isMoving) Move();                                      // if player is able to move, move his ship
    }


    public void Refuel(float newFuel)                              // pick up fuel tank has access 
    {
        currentFuel += newFuel;                                    // add new fuel

        if (currentFuel > maxFuel)                                 // if current fuel is greatest than max
            currentFuel = maxFuel;                                 // set current value to max

        if (isFuelOver)
        {
            isFuelOver = false;                                    // if fuel over was true, now ship can fly

            float newHorizontal = horizontalMove * factorMoveWithoutFuel;                // change his horizontal movement speed to normal after refuel
            float newVertical = verticalMove * factorMoveWithoutFuel;                    // change his vertical movement speed to normal after refuel

            StartCoroutine(LerpMovementChange(newHorizontal, newVertical));              // use Lerp to change velocity       
        }
    }


    public void IncreaseHealth(int health)                                               // pick Ups increase Health has access
    {
        currentHealth += health;

        if (currentHealth > maxHealth)                                                   // if current  health is greatest that max
            currentHealth = maxHealth;                                                   // set current helath to max    value

        smokeController.UpdateSmoke(currentHealth, maxHealth);                           // Update smoke, allows to change type of smoke
        healthBar.UpdateBar(currentHealth, maxHealth);                                   // update health bar on screen
    }


    public override void TakeDamage(float damage, Vector3 damagePosition)
    {
        if (isShieldActive) return;                                                     // if shield is active don't take damage to player

        ExplosionController.instance.RandomExplosionEffect(damagePosition);
        currentHealth -= damage;
        StartCoroutine(DamageScreen());                                                 // show red screen
        smokeController.UpdateSmoke(currentHealth, maxHealth);                          // if damage is too high create damage smoke
        CameraShake.instance.DamageShake();                                             // shake camera effect on screen
        healthBar.UpdateBar(currentHealth, maxHealth);                                  // update healthBar 

        if (currentHealth <= 0 && isAlive)                                              // if player haven't health but he isn't dead
            StartCoroutine(Death());                                                    // kill the player
    }


    public void AddAmmo(int ammo)
    {
        ammunitionLeft += ammo;                                 // add amunition
        ammunitionText.text = ammunitionLeft.ToString();       // update text on GUI

        if (ammunitionText.color != Color.white)                // if ammo color is different than white
            ammunitionText.color = Color.white;                 // set Color to white

        isAmmoOver = false;                                     // change flag
    }


    public void ActiveShield(bool active)                       // ShieldController has access to this method
    {
        isShieldActive = active;
        healthBar.BlueIcone(active);                            // if shiled is active, heart icon will  change his color to blue
    }


    public void ChangeWeapons(ref GameObject newWeapon)
    {
        if (visualWeapons.Count == 1 && visualWeapons[0].weapon != newWeapon)                        // change one weapon to another if player have different that he found 
        {
            Destroy(weaponSpawnMain.GetChild(0).gameObject);                                         // destroy current weapon on main spawner
            visualWeapons.Clear();                                                                   // cleaer list 
            visualWeapons.Add(new VisualWeapon(newWeapon, weaponSpawnMain));                         // use constructor  to create new visual Weapon
            visualWeapons[0].weaponScripts = WeaponCreator(ref newWeapon, ref weaponSpawnMain);      // get reference to new  created weapon  script
        }
        else if (visualWeapons.Count == 1 && visualWeapons[0].weapon == newWeapon)                   // add second weapon if player have the same that he found
        {
            Destroy(weaponSpawnMain.GetChild(0).gameObject);                                         // destroy current weapon on main spawner
            visualWeapons.Clear();                                                                   // cleaer list 
            visualWeapons.Add(new VisualWeapon(newWeapon, weaponSpawnRight));
            visualWeapons[0].weaponScripts = WeaponCreator(ref newWeapon, ref weaponSpawnLeft);      // get reference to new  created weapon  script

            visualWeapons.Add(new VisualWeapon(newWeapon, weaponSpawnLeft));
            visualWeapons[1].weaponScripts = WeaponCreator(ref newWeapon, ref weaponSpawnRight);     // get reference to new  created weapon  script
        }
        else if (visualWeapons.Count == 2 && visualWeapons[0].weapon != newWeapon)
        {
            Destroy(weaponSpawnLeft.GetChild(0).gameObject);                                         // destroy current weapon on LeftSpawner
            Destroy(weaponSpawnRight.GetChild(0).gameObject);                                        // destroy current weapon on RightSpawner
            visualWeapons.Clear();                                                                   // cleaer list 
            visualWeapons.Add(new VisualWeapon(newWeapon, weaponSpawnRight));
            visualWeapons[0].weaponScripts = WeaponCreator(ref newWeapon, ref weaponSpawnLeft);      // get reference to new  created weapon  script

            visualWeapons.Add(new VisualWeapon(newWeapon, weaponSpawnLeft));
            visualWeapons[1].weaponScripts = WeaponCreator(ref newWeapon, ref weaponSpawnRight);     // get reference to new  created weapon  script
        }
    }


    protected override void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (Mathf.Abs(x) == 1 && !isTiltSoundPlaying)                               // if player move to horizontal
        {
            isTiltSoundPlaying = true;
            SoundManager.instance.RandomizeSfx(ref wingsTiltSnd);                   // play ship sound
        }
        else if (Mathf.Abs(x) < 1 && isTiltSoundPlaying)
            isTiltSoundPlaying = false;

        Vector3 movement = new Vector3(x * horizontalMove, 0, y * verticalMove);

        if (!isFuelOver)
            CheckFuel();                                                              // check if there is any fuel in  fuel tank

        if (Time.timeScale != 0)
            rigidbody.velocity = movement / Time.timeScale;                           // it's ignore bulelt Time effect

        ChangeShipRotation();                                                         // Tilt wings
    }


    public void ChangeSpeed(int increasePercent, float activeTime)
    {
        if(isSpeedChanged)
        {
            StopCoroutine("IEChangeSpeed");
               horizontalMove = verticalMove = orginalSpeed;  // set orginal speed
        }
        PickUpGUIController.instance.ActiveForWhile(PickUpGUIController.instance.speed, activeTime);
        StartCoroutine(IEChangeSpeed(increasePercent, activeTime));
    }


    public void ChangeFireRate(float IncrasePercentFireRate, float activeTime)
    {
        PickUpGUIController.instance.ActiveForWhile(PickUpGUIController.instance.fireRate, activeTime);    // active HUD Icone

        foreach (VisualWeapon w in visualWeapons)                                                          // go through all visualWeapon class
            w.weaponScripts.ChangeFireRate(IncrasePercentFireRate, activeTime);                            // set weapon script and call set new fire rate for established time
    }


    protected override void CheckBoundry()                                            // Check if player try leave the boundy
    {
        rigidbody.position = new Vector3(                                             // set his position in boundry
        Mathf.Clamp(rigidbody.position.x, boundryPosition.left, boundryPosition.right),
        rigidbody.transform.position.y,
        Mathf.Clamp(rigidbody.position.z, boundryPosition.bottom, boundryPosition.up));
    }


    void ChangeShipRotation()
    {
        float TiltDeegres = tiltSpeed * -rigidbody.velocity.x;                                        // how far ship's wings can tilt

        if (Time.timeScale == 1)                                                                      // if there is no bullet time
            rigidbody.rotation = Quaternion.Euler(new Vector3(0, 0, TiltDeegres));
        else
            rigidbody.rotation = Quaternion.Euler(new Vector3(0, 0, TiltDeegres * Time.timeScale));   // if there is bullet time, we change player velocity so we need to reduce his rotation by timeScale
    }


    void AdjustVisualBars()                                                                          // Get references to GUI  bars after start game
    {
        healthBar = GameObject.Find(namePlayerHealth).GetComponent<VisualBar>();
        fuelBar = GameObject.Find(namePlayerFuel).GetComponent<VisualBar>();
    }


    void IsEnemyOnSight()                                                                            // function display health bar enemy, if player can see him on forward
    {
        ray.origin = transform.position;                                                             // set ray from player position
        ray.direction = transform.forward;                                                           // to forward direction
        RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit, sight, enemyLayer))                                 // if ray hit enemy
            raycastHit.transform.GetComponent<EnemyController>().DisplayHealthBar();
        else
            enemyHealthBar.HideBar();                                                                // bar will be unvisible
    }


    void CheckFuel()
    {
        currentFuel -= Time.deltaTime;                                              // decrease fuel
        fuelBar.UpdateBar(currentFuel, maxFuel);                                    // update visual fuel bar on screen

        if (currentFuel < 0)                                                        // if fuel is over 
        {
            isFuelOver = true;                                                      // set flag fuel is over

            float noFuelHorizontal = horizontalMove / factorMoveWithoutFuel;        // set noFuel horizontal speed
            float noFuelVertical = verticalMove / factorMoveWithoutFuel;            // set noFuel vertical speed

            StartCoroutine(LerpMovementChange(noFuelHorizontal, noFuelVertical));   // decrease player speed to speed without fuel
        }
    }


    IEnumerator IEChangeSpeed(int percentSpeed, float howLong)
    {
        isSpeedChanged = true;
        float newSpeed = orginalSpeed + orginalSpeed * percentSpeed / 100;            // new speed increased by percent value

        yield return StartCoroutine(LerpMovementChange(newSpeed, newSpeed));          // set new speed for both values horizontal and vertical

        yield return new WaitForSeconds(howLong);                                     // wait some time

        yield return StartCoroutine(LerpMovementChange(orginalSpeed, orginalSpeed));  // set orginal speed back  
        isSpeedChanged = false;
    }


    IEnumerator LerpMovementChange(float newHorizontal, float newVertical)                  // change player ship movement  from one value to another by using lerp
    {
        while (Mathf.Abs(horizontalMove - newHorizontal) > 0.1f)                            // if  velocities aren't similar
        {
            horizontalMove = Mathf.Lerp(horizontalMove, newHorizontal, speedFallDownFuel * Time.deltaTime);
            verticalMove = Mathf.Lerp(verticalMove, newVertical, speedFallDownFuel * Time.deltaTime);
            yield return null;
        }
        horizontalMove = newHorizontal;                                                     // to make sure that values are the same
    }


    IEnumerator DamageScreen()
    {
        damageImage.color = damageColor;                                                                        // change screen color to red
        yield return null;

        while (damageImage.color != Color.clear)
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, changeColor * Time.deltaTime);       // change color from red to clear
            yield return null;
        }
    }


    IEnumerator Death()
    {
        isAlive = false;
        yield return null;                                                                   // wait because Update must finish perform

        gameObject.GetComponent<SphereCollider>().enabled = false;                           // disenabled collider
        transform.Find("PlayerModel").transform.gameObject.SetActive(false);            // hide player model;
        if (transform.Find("MagnetingSphere"))
        {
            transform.Find("MagnetingSphere").transform.gameObject.SetActive(false);
        }
        if (transform.Find("MagnetingSphereClone"))
        {
            transform.Find("MagnetingSphereClone").transform.gameObject.SetActive(false);
        }
        gameObject.GetComponent<CirculatingDroidController>().DestroyAllDroids();

        Instantiate(destroyExplosion, transform.position, Quaternion.identity);

        GameMaster.instance.PlayerDie();
        enemyHealthBar.HideBar();
        Destroy(gameObject, 1);
    }


    void Shot()
    {
        if (Input.GetKey(KeyCode.Space) && !isAmmoOver)                                             // if player press fire button and he have ammo
        {
            CameraShake.instance.ShotShake();                                                       // shake camera when player shot

            for (int i = 0; i < visualWeapons.Count; i++)                                           // Shot from all weapons
                if (visualWeapons[i].weaponScripts.Shot(false))                                     // if gun can fire because time to shot next bullet is left
                    DecreaseAmmunition();                                                           // decrease ammunition

        }
        else if (Input.GetKeyDown(KeyCode.Space))                                                   // else if  player press fire button and he haven't ammo
            SoundManager.instance.RandomizeSfx(ref emptyMagazineSnd);                               // play sound empty magazine
    }


    void DecreaseAmmunition()
    {
        if (ammunitionLeft > 0)
        {
            ammunitionLeft--;                                                                  // decrease ammunition amount
            ammunitionText.text = ammunitionLeft.ToString();                                   // Update GUI text

            if (ammunitionLeft < 10)                                                           // if ammunition is less than 10 bullets
                ammunitionText.color = Color.red;                                              // change ammo label to red Color
        }
        else
            isAmmoOver = true;                                                                 // there is no ammo in magazine so player can't shot
    }


    public override void IEConstandDamageByTimeAdditional()
    {
        throw new System.NotImplementedException();
    }
}   // Karol Sobański