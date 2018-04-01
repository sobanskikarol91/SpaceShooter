using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public Menu currentMenu;

    public float timeToDeactivateMenu = 0.5f;


    public void Start()
    {
        ShowFirstMenu(currentMenu);
    }


    public void ShowFirstMenu(Menu menu)
    {
        if (currentMenu != null)
            currentMenu.IsOpen = false;

        currentMenu = menu;
        StartCoroutine(ActiveCurrentMenu());
        currentMenu.IsOpen = true;
    }


    public void ShowMenu(Menu menu)
    {
        if (currentMenu != null)
            currentMenu.IsOpen = false;


        StartCoroutine(DeactivateMenu(currentMenu));

        currentMenu = menu;
        StartCoroutine(ActiveCurrentMenu());
    }


    public IEnumerator ActiveCurrentMenu()
    {
        currentMenu.gameObject.SetActive(true);
        currentMenu.IsOpen = true;
        yield return new WaitForEndOfFrame();
    }


    public IEnumerator DeactivateMenu(Menu previousMenu)
    {
        yield return new WaitForSeconds(timeToDeactivateMenu);
        previousMenu.gameObject.SetActive(false);
        yield return new WaitForSeconds(timeToDeactivateMenu);
    }


    public void Exit()
    {
        Application.Quit();
    }
}
