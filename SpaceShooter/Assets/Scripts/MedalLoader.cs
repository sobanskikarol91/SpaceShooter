using UnityEngine;
using System.Collections;

public class MedalLoader : MonoBehaviour
{
    public string playerMedalName = "P0_M";
    public float timeToShowNextMedal = 0.2f;
    private MedalController[] medals;


    void Awake()
    {
        medals = GetComponentsInChildren<MedalController>();
    }


    public IEnumerator LoadAllMedals()
    {
        yield return null;                                                      // Awake will be first 
        HideAlreadyVisibleMedals();                                             // check if any medal is visible after last battle
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < medals.Length; i++)                                 // go through all medals
        {
            //print("Medal: " + playerMedalName + (i + 1) + " " + PlayerPrefs.GetString(playerMedalName + i + 1));
            if (PlayerPrefs.GetString(playerMedalName + (i + 1)) == "true")       // if medal was awarded
            {
                yield return new WaitForSeconds(timeToShowNextMedal);
                medals[i].ShowMedalOnBoard();                                   // display medal on screen
            }
        }
    }


    void HideAlreadyVisibleMedals()
    {
        foreach (MedalController mc in medals)                                 // check all medals
            if (mc.IsMedalShown)                                               // if medal was shown
                mc.HideMedal();                                                // hide him
    }
}
