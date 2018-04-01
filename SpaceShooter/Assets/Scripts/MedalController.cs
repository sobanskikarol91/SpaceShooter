using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MedalController : MonoBehaviour
{
    AudioSource audiosource;
    Animator animator;

    public Sprite medalSprite;
    public Sprite medalHole;

    private Image image;
    public bool IsMedalShown { get; set; }



    void Awake()
    {
        animator = GetComponent<Animator>();
        audiosource = GetComponent<AudioSource>();
        image = GetComponent<Image>();
    }


    public void ShowMedalOnBoard()         // MedalAward invoke this
    {
        IsMedalShown = true;
        animator.SetBool("ShowMedal", true);
    }


    public void HideMedal()             
    {
        IsMedalShown = false;
        image.sprite = medalHole;
    }


    void ChangeTexture()                    // Animation invoke this
    {
        audiosource.Play();
        image.sprite = medalSprite;
    }


    void StopAnimation()                    // Animation invoke this
    {
        animator.SetBool("ShowMedal", false);
    }
}   // Karol Sobański
