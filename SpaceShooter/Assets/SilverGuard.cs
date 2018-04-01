using UnityEngine;
using System.Collections;


public class SilverGuard : EnemyController
{
    [Header("AI Enemy")]
    [Tooltip("random times to change direction frome one to other")]
    public int minDirectionTime = 1;            // minimum amount of time, after which ship change his fly direction * change side from left to right or contrariwise)
    public int maxDirectionTime = 3;


    private float directionTimeLeft;            // time left to change direction

    
    void ChangeDirectionX()                     // change direction on X axis ( turn to left or right)
    {
        directionTimeLeft -= Time.deltaTime;
        if (directionTimeLeft > 0) return;

        
        float randomTime = Random.Range(minDirectionTime, maxDirectionTime);        // set new Time to change direction
        directionTimeLeft = randomTime;
        horizontalMove = -horizontalMove;                                           // change direction on X axis      
        print(horizontalMove);       
    }


    protected override void Move()
    {
        base.Move();

        ChangeDirectionX();
        CheckBoundry();
    }

}
