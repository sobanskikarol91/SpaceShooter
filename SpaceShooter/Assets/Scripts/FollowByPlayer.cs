using UnityEngine;
using System.Collections;

public class FollowByPlayer : MonoBehaviour
{
    public float rotateSpeed = 500;


    private Transform player;
    private bool isPlayerAlive;


    void Start()
    {
        isPlayerAlive = GameMaster.instance.IsPlayerAlive;  // reference to property isPlayerAlive

        if (isPlayerAlive)
            player = GameMaster.instance.PlayerShip;        // set reference to player ship transform
    }


    void Update()
    {
        if (isPlayerAlive)                                                                                                                                               // if player is still alive
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.position - transform.position), rotateSpeed * Time.deltaTime);      // follow player
    }
}   // Karol Sobanski
