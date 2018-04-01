using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class VisualBar : MonoBehaviour
{
    public Color maxColor = new Color(0 / 255f, 255 / 255f, 0 / 255f);
    public Color minColor = new Color(255 / 255f, 0 / 255f, 0 / 255f);
    public Vector3 offset = new Vector3(0, 0, 1);       // offset adjusts  bar position

    [Tooltip("allows play icone animation attached to bar and sound if bar's value'll be low")]
    public bool isEffects;

    public Transform barIcone;                          // icone bar represents visual bar, it could be  icon heart or fuel


    [Range(0, 1)]
    [Tooltip("how many percent of total amount bar value is necesarry to play warning animation")]
    public float warningPercent;

    private AudioSource audioSource;                    // will play warning sound if bar value is low
    private Animator iconeAnimation;
    private Image barImage;
    private bool isEffectPlaying;

    private const string nameWarning = "warning";


    void Awake()
    {
        if (isEffects)
            audioSource = GetComponent<AudioSource>();

        barImage = gameObject.GetComponent<Image>();                                          // get reference to Image 
        HideBar();                                                                            // bar will not display on screen before game start
    }


    public void UpdateBar(float currentValue, float maxValue)
    {
        StartCoroutine(IEUpdateBar(currentValue, maxValue));
    }


    public IEnumerator IEUpdateBar(float currentValue, float maxValue)
    {
        yield return null;
        float percent = currentValue / (float)maxValue;
        if (percent < 0)                                                                  // it's prevent scale bar in left side
            percent = 0;

        if (isEffects)
            EffectHandle(percent);

        transform.localScale = new Vector3(percent, 1, 1);
        barImage.color = Color.Lerp(minColor, maxColor, percent);
    }


    public void DisplayBar(Transform newParent)                                          // EnemyController use this method
    {
        transform.parent.transform.position = newParent.transform.position + offset;     //  adjust  bar position by offset and parrent position
    }


    public void ParentandDisplayBar(Transform newParent)                                 // ShieldController use this method
    {
        transform.parent.transform.SetParent(newParent.transform);                       // parent health bar to gameobject, that they can move together,
        transform.parent.transform.position = newParent.transform.position + offset;     // and adjust  bar position by offset
    }


    public void HideBar()
    {
        barImage.color = new Color(0, 0, 0, 0);
    }


    void EffectHandle(float percent)
    {
        iconeAnimation = barIcone.gameObject.GetComponent<Animator>();                 // Get reference to icone animation that allows to control when play animation

        if (percent < warningPercent && !isEffectPlaying && percent != 0)              // if  bar's  fall to warningPercent value and effects isn't playing, and player is still alive 
        {
            if (audioSource) audioSource.Play();
            iconeAnimation.SetBool(nameWarning, true);
            isEffectPlaying = true;
        }
        else if (percent > warningPercent && isEffectPlaying || percent == 0)          // stop play effect if currentValue of bar is greatest than warningPercent and effects is Playing, or valie is zero
        {
            if (audioSource) audioSource.Stop();
            iconeAnimation.SetBool(nameWarning, false);
            isEffectPlaying = false;
        }
    }


    public void BlueIcone(bool enable)                                                 // this method allows turn off/on blue heart on screen playerController has access
    {
        if (!iconeAnimation)                                                           // if there is no component attached
            iconeAnimation = barIcone.gameObject.GetComponent<Animator>();             // Get reference to icone animation that allows to control when play animation
        iconeAnimation.SetBool("blueIcone", enable);
    }
}   // Karol Sobanski





