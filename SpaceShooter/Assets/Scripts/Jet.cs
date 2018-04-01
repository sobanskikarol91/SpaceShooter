using UnityEngine;
using System.Collections;

public class Jet : EnemyController
{
    protected override void Update()
    {
        if (isShooting)
            if (GameMaster.instance.IsPlayerAlive)
                for (int i = 0; i < visualWeapons.Count; i++)             // Shot from all weapons
                    visualWeapons[i].weaponScripts.Shot(true);            // parented bullets
            else
                isShooting = false;

        if (isMoving)
            Move();
    }
}
