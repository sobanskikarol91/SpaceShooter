using UnityEngine;
using System.Collections;

public class SilverCoinSpawnController : MonoBehaviour
{
    public GameObject SilverCoinGuard;
    public GameObject silverCoin;


    private Vector3 spawnPosistionVector;
    private bool areCoinsOnTheField;
    private GameObject coinTmp;
    private float randomX;
    private float randomZ;


    public void SpawnCoins()
    {
        randomX = Random.Range(-17, 5);
        randomZ = Random.Range(1, 15);
        spawnPosistionVector = new Vector3(randomX, 0, randomZ);

        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
            {
                coinTmp = Instantiate(silverCoin);
                coinTmp.gameObject.SetActive(true);
                coinTmp.transform.position = spawnPosistionVector + new Vector3(i, 0, j);
                coinTmp.transform.SetParent(GameMaster.instance.hierarchyGuard);
            }
    }

}