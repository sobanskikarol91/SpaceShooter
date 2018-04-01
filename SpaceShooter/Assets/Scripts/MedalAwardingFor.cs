using UnityEngine;
using System.Collections;

public class MedalAwardingFor : MonoBehaviour
{
    [System.Serializable]
    public class OnePointIncrease
    {
        public MedalController medalController;     // it controlls showing medal on board
        private int max = 1;                        // how many enemies player must to kill to get medal
        private int current;                        // how many enemies player already killed;
        public bool IsMedalAwarded { set; get; }     // flag prevents to get few times this same medal


        public void CheckMedalAwarding()
        {
            if (IsMedalAwarded) return;             // if player already has this medal, do nothing

            current++;                              // add point for invoke this method;
            if (current >= max)                     // if player  exceeded max limit
            {
                medalController.ShowMedalOnBoard(); // give player metal (Show on board)
                IsMedalAwarded = true;              // prevents to having more than one medal
            }
        }


        public void Reset()
        {
            medalController.HideMedal();
            IsMedalAwarded = false;
        }
    };


    public OnePointIncrease[] medals;


    public void Reset()
    {
        foreach (OnePointIncrease mc in medals)
            mc.Reset();

    }


    public void KillingEnemies()
    {
        medals[0].CheckMedalAwarding();
    }


    public void CollectingStars()
    {
        medals[1].CheckMedalAwarding();
    }


    public void SaveMedals()
    {
        string name = "P0_M";                                    // P0 means currentPlayer

        for (int i = 0; i < medals.Length; i++)                        // go through all medals
        {
            if (medals[i].IsMedalAwarded)                      // if some medal is awarned;
                PlayerPrefs.SetString(name + (i + 1), "true");     // save this information
            else
                PlayerPrefs.SetString(name + (i + 1), "false");    // save this information
        }
    }

}   // Karol Sobański
