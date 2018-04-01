using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{
    private Animator animator;
    private CanvasGroup canvasGroup;

    private string isOpen = "isOpen";


    public bool IsOpen
    {
        get { return animator.GetBool(isOpen); }
        set { animator.SetBool(isOpen, value); }
    }


    void Awake()
    {
        animator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();

        RectTransform rect = GetComponent<RectTransform>();
        rect.offsetMax = rect.offsetMin = new Vector2(0, 0);
    }


    void Update()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
            canvasGroup.blocksRaycasts = canvasGroup.interactable = false;
        else
            canvasGroup.blocksRaycasts = canvasGroup.interactable = true;
    } 
}
