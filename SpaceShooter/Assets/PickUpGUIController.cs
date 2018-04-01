using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PickUpGUIController : MonoBehaviour
{
    public static PickUpGUIController instance;

    [Header("GUI Icones")]
    public Animator shield;
    public Animator bulletTime;
    public Animator speed;
    public Animator fireRate;
    public Animator bombs;
    public Animator magnet;

    public Text bombAmount;

    // animator parametrs
    private const string active = "active";


    void Start()
    {
        instance = this;
    }


    public void Active(Animator animator)
    {
        animator.SetBool(active, true);
    }


    public void Inactive(Animator animator)
    {
        animator.SetBool(active, false);
    }


    public void ActiveForWhile(Animator animator, float activeTime)
    {
        StartCoroutine(IEActiveForWhile(animator, activeTime));
    }


    IEnumerator IEActiveForWhile(Animator animator, float activeTime)
    {
        Active(animator);
        yield return new WaitForSeconds(activeTime);
        Inactive(animator);
    }

    public void UpdateBombsText(int amount)
    {
        if (amount == 0)
            bombAmount.text = string.Empty;                                                             // update amount of bombs on HUD
        else
            bombAmount.text = amount.ToString();                                                        // update amount of bombs on HUD
    }

}   // Karol Sobański