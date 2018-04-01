using UnityEngine;
using System.Collections;

public class BombController : MonoBehaviour
{
    [Tooltip("How many bombs player has when he starts game")]
    public int startAmount = 1;
    public GameObject bomb;


    private int currentBombs;


    void Start()
    {
        currentBombs = startAmount;                                                                       // asign start bombs value to current value

        if (currentBombs > 0)                                                                             // if player  has any bomb
            PickUpGUIController.instance.Active(PickUpGUIController.instance.bombs);                      // light bomb on HUD

        PickUpGUIController.instance.UpdateBombsText(currentBombs);                                       // update amount of bombs on HUD
    }


    public void AddBomb(int newBomb)
    {
        if (currentBombs == 0)                                                                            // if player already hasn't any bomb
            PickUpGUIController.instance.Active(PickUpGUIController.instance.bombs);                      // light bomb on HUD

        currentBombs += newBomb;                                                                          // add new bomb
        PickUpGUIController.instance.UpdateBombsText(currentBombs);                                       // update amount of bombs on HUD
    }


    public void UseBomb()
    {
        if (currentBombs == 0) return;                                                                    // if player hasn't any bomb - do nothing  

        if (currentBombs == 1)                                                                            // if it was last bomb
            PickUpGUIController.instance.Inactive(PickUpGUIController.instance.bombs);                    // light off bomb icone

        GameObject newBomb = Instantiate(bomb, transform.position, Quaternion.identity) as GameObject;    // Create bomb on map
        newBomb.transform.SetParent(GameMaster.instance.hierarchyGuard);                                  // parent do guardhierarchy

        currentBombs--;                                                                                   // subtract bomb
        PickUpGUIController.instance.UpdateBombsText(currentBombs);                                       // update amount of bombs on HUD
    }
}   // Karol Sobański
