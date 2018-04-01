using UnityEngine;
using System.Collections;

public class SmokeController : MonoBehaviour
{
    public ParticleSystem engineSmoke;                          // smoke when object has more than 50% health
    public ParticleSystem whiteSmoke;                           // smoke will generete when object has less than 50% health 
    public ParticleSystem blackSmoke;                           // smoke will generete when object has less than 25% health


    private ParticleSystem currentSmoke;                                // reference to the current active smoke


    void Start()
    {
        currentSmoke = engineSmoke;
        engineSmoke.Play();
        whiteSmoke.Stop();
        blackSmoke.Stop();
    }


    public void UpdateSmoke(float currentValue, int maxValue)
    {
        float percente = currentValue / (float)maxValue;        // calculate percente

        if (percente > 0.5f)
            CheckCurrentSmoke(ref engineSmoke);
        else if (percente < 0.5f && percente > 0.20f)
            CheckCurrentSmoke(ref whiteSmoke);
        else
            CheckCurrentSmoke(ref blackSmoke);
    }


    void CheckCurrentSmoke(ref ParticleSystem compareWith)
    {
        if (currentSmoke.name != compareWith.name)              // if currentSmoke isn't the same that compareWith smoke
        {
            currentSmoke.Stop();                                // stop Playing current smoke
            currentSmoke = compareWith;                         // set currentSmoke to correct smoke that we compare with
            currentSmoke.Play();                                // Play smoke
        }
    }

}   // Karol Sobański


