using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    public static CursorController instance;
    public Texture2D cursorTex;

    
    void Awake()
    {
        if (instance == null)
            instance = this;

        Cursor.SetCursor(cursorTex, Vector2.zero, CursorMode.Auto);
    }


    public void SetCursorLock()
    {
        if (Cursor.visible)               // if cursor is still visible
            HideCursor();                 // hide him
        else
            ShowCursor();                 // in another case show
    }


    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}