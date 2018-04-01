using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class GatesController : MonoBehaviour
{
    public RectTransform leftGate;
    public RectTransform leftGateEnd;
    public RectTransform rightGate;
    public RectTransform rightGateEnd;
    public bool closed;
    public MenuManager menuManager;
    public GameObject _Menu;
    public Menu menu;

    [SerializeField]
    private float gateOpenSpeed;                // How fast gate will be open 
    [SerializeField]
    private float gateCloseSpeed;               // how fast gate will be close
    [SerializeField]
    private float closedTime = 1;               // how long gates will be closed
    [SerializeField]
    private float openTime = 1;                 // after how many time gate will be open


    private float leftGateBeginX;
    private float rightGateBeginX;
    private AudioSource audioSource;


    void Start()
    {
        leftGateBeginX = leftGate.transform.position.x;
        rightGateBeginX = rightGate.transform.position.x;

        audioSource = gameObject.GetComponent<AudioSource>();
        leftGateEnd.position = new Vector3(leftGateEnd.position.x * 3 / 2, leftGateEnd.position.y, leftGateEnd.position.z);
        rightGateEnd.position = new Vector3(rightGateEnd.position.x * 3 / 2, rightGateEnd.position.y, rightGateEnd.position.z);



        if (closed)
            BeginFromClosedGates();     // set position to closed gates
        else
            StartCoroutine(BeginFromOpenedGates());     // set position to opened gates
    }


    public void OpenGates()
    {
        ShowGates(true);                // show gates
        StartCoroutine(IEOpenGates());
    }


    public void CloseGatesAndChangeSceene()
    {
        ShowGates(true);
        StartCoroutine(IECloseGatesAndChangeSceene());
    }


    IEnumerator IECloseGatesAndChangeSceene()
    {
        yield return null;

        if (_Menu)
            _Menu.SetActive(true);

        yield return StartCoroutine(IECloseGates());
        yield return new WaitForSeconds(closedTime);

        menu.gameObject.GetComponent<GatesController>().UpdateGateState(false);
        menuManager.ShowMenu(menu);
        GameMaster.instance.ClearScenee();
    }


    IEnumerator IEOpenGatesAfterTime()
    {
        yield return new WaitForSeconds(openTime);
        OpenGates();
    }


    IEnumerator IEOpenGates()
    {
        audioSource.Play();

        while (Mathf.Abs((leftGate.position - leftGateEnd.position).magnitude) > 0.01f)
        {
            leftGate.position = Vector3.Lerp(leftGate.position, leftGateEnd.position, gateOpenSpeed * Time.deltaTime);
            rightGate.position = Vector3.Lerp(rightGate.position, rightGateEnd.position, gateOpenSpeed * Time.deltaTime);
            yield return null;
        }
        print("wylacz drzwi!!");
        ShowGates(false); // inactive gates 
    }


    IEnumerator IECloseGates()
    {
        Vector3 leftGateBegin = new Vector3(leftGateBeginX, leftGate.position.y, leftGate.position.z);
        Vector3 rightGateBegin = new Vector3(rightGateBeginX, rightGate.position.y, rightGate.position.z);

        audioSource.Play();

        while ((rightGate.position - rightGateBegin).magnitude > 0.1f)
        {
            leftGate.position = Vector3.Lerp(leftGate.position, leftGateBegin, gateCloseSpeed * Time.deltaTime);
            rightGate.position = Vector3.Lerp(rightGate.position, rightGateBegin, gateCloseSpeed * Time.deltaTime);
            yield return null;
        }

        leftGate.position = leftGateBegin;
        rightGate.position = rightGateBegin;
    }


    public void UpdateGateState(bool open)
    {
        if (open)
            StartCoroutine(BeginFromOpenedGates());
        else
        {
            BeginFromClosedGates();
            StartCoroutine(IEOpenGatesAfterTime());
        }
    }


    public void BeginFromClosedGates() // button will invoke this in another canvas
    {
        ShowGates(true);                 // gates will be active

        leftGate.position = new Vector3(leftGateBeginX, leftGate.position.y, leftGate.position.z);
        rightGate.position = new Vector3(rightGateBeginX, rightGate.position.y, rightGate.position.z);
    }


    IEnumerator BeginFromOpenedGates()
    {
        yield return null;              // it neccessary because awak must invoke first

        ShowGates(false);               // gates will be inactive, it prevents to show gates when we change sceenes

        leftGate.position = new Vector3(leftGateEnd.position.x, leftGate.position.y, leftGate.position.z);
        rightGate.position = new Vector3(rightGateEnd.position.x, rightGate.position.y, rightGate.position.z);
    }


    void ShowGates(bool state)
    {
        leftGate.gameObject.SetActive(state);
        rightGate.gameObject.SetActive(state);
    }
}   // Karol Sobański
