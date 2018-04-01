using UnityEngine;
using System.Collections;

public class ShurikenBallController : MonoBehaviour {

    private float lastingTime =0.25f;
   

    void Start () {
        StartCoroutine(DecreaseTime());       
    }


    IEnumerator DecreaseTime()
    {
        while (lastingTime > 0)
        {

            lastingTime -= Time.deltaTime;
            yield return null;
        }

        transform.GetChild(0).gameObject.SetActive(true);
    }


}

