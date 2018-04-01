using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent(typeof(Button), typeof(AudioSource))]
public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    [HideInInspector] public AudioClip pressedSound;
    [HideInInspector] public AudioClip hoveredSound;

    private AudioSource audioSource { get { return GetComponent<AudioSource>(); } }
    private Button button { get { return GetComponent<Button>(); } }


    void Start()
    {
        gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }


    public void OnPointerEnter(PointerEventData ped)
    {
        audioSource.PlayOneShot(hoveredSound);
    }


    public void OnPointerDown(PointerEventData ped)
    {
        audioSource.PlayOneShot(pressedSound);
    }
}   // Karol Sobański
