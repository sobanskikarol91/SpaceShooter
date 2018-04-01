using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighScoreController : MonoBehaviour
{
    public Text[] scoreText;
    public MedalLoader[] medals;

    private int medalAmount = 18;
    private int scoreAmount = 10;
    string hightScore = "HighScore";


    void Start()
    {
        //ResetHighScores();
        //ResetMedals();
    }


    void ResetHighScores()
    {
        for (int i = 0; i < scoreText.Length; i++)
            PlayerPrefs.SetInt((hightScore + (i + 1).ToString()), i);
    }


    void ResetMedals()
    {
        for (int i = 0; i <= scoreText.Length; i++)
            for (int j = 1; j <= medalAmount; j++)
                PlayerPrefs.SetString("P" + i + "_M" + j, "false");
    }


    public void SaveScoreInHighScore(int currentScore)
    {
        int i = 1;
        int place = 0;                                                     // place to currentScore

        while (i <= scoreAmount)                                      // go through all scores
        {
            if (currentScore > PlayerPrefs.GetInt(hightScore + i))         // if current score is greatest than this score in high Score
                place = i;

            if (place > 0)
            {
                for (int j = scoreAmount; j > place; j--)                   // start from last score in ranking
                {
                    ChangeMedalsPositios(j - 1, j);                              // first change medal position
                    int previous = PlayerPrefs.GetInt(hightScore + (j - 1));     // save latest score
                    PlayerPrefs.SetInt(hightScore + (j).ToString(), previous);
                }

                ChangeMedalsPositios(0, place);
                PlayerPrefs.SetInt(hightScore + place, currentScore);
                break;                                                     // quit the loop
            }
            i++;                                                           // check another score
        }
    }


    public void UpdateStats()
    {
        UpdateScores();
        UpdateMedals();
    }


    void UpdateScores()                                                                             // update scores on screen in Highscores
    {
        for (int i = 0; i < scoreText.Length; i++)
            scoreText[i].text = PlayerPrefs.GetInt(hightScore + (i + 1).ToString()).ToString();
    }


    void UpdateMedals()                                                                             // update all medals on screen in Highscores
    {
        foreach (MedalLoader ml in medals)
            StartCoroutine(ml.LoadAllMedals());
    }


    void ChangeMedalsPositios(int higherScore, int lowerScore)
    {
        string higherScoreMedal = "P" + higherScore + "_M";      // P1_M                                        
        string lowerScoreMedal = "P" + lowerScore + "_M";        // P2_M

        for (int i = 0; i < medalAmount; i++)                                                       // go through all medals
        {
            string higherScoreCurrentMedal = PlayerPrefs.GetString(higherScoreMedal + (i + 1));       // save current medal from higherScore
            PlayerPrefs.SetString(lowerScoreMedal + (i + 1), higherScoreCurrentMedal);                // and set it to lower position in highscore on current medal position
            //print("Weź: " + higherScoreMedal + (i + 1));
            //print("daj do: " + lowerScoreMedal + (i + 1) + "   wartoosc: " + higherScoreCurrentMedal);
        }
    }
}
