using UnityEngine;
using System.Collections;

public class DestroyByBoundry : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        GameMaster.instance.RemoveObject(other.transform);
        Destroy(other.gameObject); 
    }
}   // Karol Sobanski
