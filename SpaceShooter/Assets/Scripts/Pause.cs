using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pause : MonoBehaviour
{
    public GameObject menuPanel;
    public bool pauseOn;


    void Start()
    {
        menuPanel.gameObject.SetActive(false);
    }


    public void CallMenu()
    {
        StartCoroutine(IECallMenu());
    }


    public void StopPause()  // After end the game will end, GameMaster will disable possibility to call the pause menu by invoke this
    {
        StopAllCoroutines();
    }


    IEnumerator IECallMenu()      
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))               // if player press escape
                 SetPause();

            yield return null;
        }
    }

    public void SetPause()
    {
        CursorController.instance.SetCursorLock();      // hide the cursor

        pauseOn = !pauseOn;

        menuPanel.gameObject.SetActive(pauseOn);
        Time.timeScale = pauseOn ? 0 : 1;
    }
}   // Karol Sobański
