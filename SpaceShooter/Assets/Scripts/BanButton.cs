using UnityEngine;
using System.Collections;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class BanButton : MonoBehaviour
{
    public Color inactive;
    public Color active;
    public Text text;


    private Button button;


    void Awake()
    {
        button = GetComponent<Button>();
    }


    void OnEnable()
    {
        button.interactable = false;
        setInactiveColor();
    }


    public void setActiveColor()
    {
        text.color = active;
    }


    public void setInactiveColor()
    {
        text.color = inactive;
    }
}
