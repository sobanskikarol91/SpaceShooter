using UnityEngine;
using System.Collections;


public class EnemyController : ObjectController
{
    public int pointForKill;                          // points for player
    public bool isTakingCollisionDamage;              // take damage with colider with player
    public int DamageByCollision;                     // how many damage player will take with collision


    protected override void Start()
    {
        base.Start();

        SetMaterialArrays();
        healthBar = GameObject.Find("EnemyHealth").GetComponent<VisualBar>();
    }


    protected virtual void Update()
    {
        if (isShooting)                                                         // check if enemy can shoot
            if (GameMaster.instance.IsPlayerAlive)                              // check if player is still alive 
                Shot();
            else
                isShooting = false;                                             // if player is death stop shooting

        if (isMoving)
            Move();
    }


    public void DisplayHealthBar()                                              // Player has access to this method,  if he see anamy he will invoke method
    {
        healthBar.DisplayBar(transform);                                        // Display visual health bar on screen attatch to enemy
        healthBar.UpdateBar(currentHealth, maxHealth);                          // Update health bar values
    }


    public override void TakeDamage(float damage, Vector3 damagePosition)         // Player has access to this method, 
    {
        if (!isAlive) return;                                                   // if enemy is killed  do nothing


        StartCoroutine(SwitchMaterial());                                       // switch material for while to white
        ExplosionController.instance.RandomExplosionEffect(damagePosition);

        currentHealth -= damage;

        healthBar.UpdateBar(currentHealth, maxHealth);

        if (currentHealth <= 0)
            DestroyEffect();
    }


    public void SetShooting(bool state)                                         // GameMaster will invoke this
    {
        if (isAlive)                                                             // if enemy is still alive
            isShooting = state;
    }


    protected override void DestroyEffect()
    {
        isAlive = false;

        GameMaster.instance.DropRandomItem(transform.position);                 // choose random item to drop
        GameMaster.instance.AddKilledEnemy(pointForKill);                       // Add Killed enemy to stats and send points for him

        base.DestroyEffect();
        Death();
    }


    protected virtual void Death()                                              // virtual because sometimes we will destroy parent (RocketTower)
    {
        GameMaster.instance.RemoveObject(transform);
        Destroy(gameObject);
    }


    protected override void CheckBoundry()                                      // Enemy Boundry
    {
        rigidbody.position = new Vector3(
        Mathf.Clamp(rigidbody.position.x, boundryPosition.left, boundryPosition.right),
        rigidbody.transform.position.y,
        rigidbody.transform.position.z);
    }


    protected virtual void Shot()
    {
        for (int i = 0; i < visualWeapons.Count; i++)                            // Shot from all weapons
            visualWeapons[i].weaponScripts.Shot(false);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player") && isTakingCollisionDamage)
        {
            other.GetComponent<PlayerController>().TakeDamage(DamageByCollision, transform.position);         // take damage to player
            TakeDamage(maxHealth, transform.position);                                                        // destroy object after collision
        }
    }


    public override void ConstantDamageByTime(float damage)                         // ConstantWeapon'll invoke this
    {
        base.ConstantDamageByTime(damage);
    }


    public override void InterruptConstantDamage()                                  // ConstantWeapon'll invoke this
    {
        healthBar.HideBar();                                                        // hide healthbar;
        base.InterruptConstantDamage();
    }


    public override void IEConstandDamageByTimeAdditional()
    {
        DisplayHealthBar();                                                         // display healthbar when  object is taking damage
    }
}   // Karol Sobanski


