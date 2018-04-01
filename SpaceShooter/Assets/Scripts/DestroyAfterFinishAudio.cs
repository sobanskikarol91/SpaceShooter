using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class DestroyAfterFinishAudio : MonoBehaviour
{
    void Start()
    {
        float destroyTime = GetComponent<AudioSource>().clip.length;     // get information about current audio clip length
        Destroy(gameObject, destroyTime);                                // destroy after audio will finish
    }
}
