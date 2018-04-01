using UnityEngine;
using System.Collections;


public abstract class StaminaController : MonoBehaviour
{
    [Header("Stamina settings")]
    public AudioClip readyToUseClip;                        // when stamina is full make noise.
    public AudioClip failClip;                              // when player try to use skill but he hasn't enough stamina, play fail sound
    public AudioClip usedClip;                              // when player use skill play  suitable for skill sound
    public AudioClip endClip;                               // when skill will end

    protected AudioSource audioSource;                      // reference to audioSource


    [SerializeField]
    private int maxStamina = 100;                           // max amount of stamina that player has
    [SerializeField]
    private int skillStamina = 80;                          // how many stamina skill will use
    [SerializeField]
    private float staminaIncreaseSpeed = 1;                 // how fast stamina will increase with time
    [SerializeField]
    private bool isIncreaseWithTime;                        // player can increase his stamina with time
    [SerializeField]
    private bool isIncreaseWithKill;                        // player can increase his stamina with killing enemy

    private bool isStaminaFull = true;                      // flag to know that stamina is increase by time in this moment
    private float currentStamina;                           // how many stamina player has in this moment
    private VisualBar staminaBar;                           // reference to staminaBar that controll bar on screen


    void OnEnable()
    {
        currentStamina = maxStamina;                                                    // set current stamina value to max

        GameMaster.instance.staminaController = this;                                   // set in GameMaster reference to stamina Controller

        staminaBar = GameObject.FindWithTag("StaminaBar").GetComponent<VisualBar>();    // get reference to stamina bar that controlls bar on screen
        staminaBar.UpdateBar(currentStamina, maxStamina);                               // update this value on bar

        audioSource = staminaBar.gameObject.GetComponent<AudioSource>();                // get reference to audioSource in staminaBar object                                                      

        if (isIncreaseWithTime)                                                         // if IncreaseWithTime is On
            StartCoroutine(IncreaseWithTime());                                         // Run coroutine that will increase stamina with time

        if (isIncreaseWithKill)                                                         // if - because can be both option active Time and kill
            GameMaster.instance.StaminaByKill = true;                                   // set in GameMaster bollen to true, because GameMaster will invoke method IncreaseWithKilling when player kill the enemy

        ReadyToUse();                                                                   // invoke animation, sounds that shows that stamina is full
    }


    IEnumerator IncreaseWithTime()
    {
        isStaminaFull = false;

        while (!isStaminaFull)                                          // go to while when current will be full
        {
            currentStamina += Time.deltaTime * staminaIncreaseSpeed;    // increase stamina with time

            if (currentStamina > maxStamina)                            // if current stamina value is greatest than max
            {
                ReadyToUse();                                           // invoke animation, sounds that shows that stamina is full
                isStaminaFull = true;                                   // stop increase stamina, stop while loop
            }

            staminaBar.UpdateBar(currentStamina, maxStamina);           // update stamina bar on screen

            yield return null;                                          // refresh screen
        }
    }


    public void IncreaseStamina(int points)                             // GameMaster and pick up will invoke this when player killed the enemy
    {
        currentStamina += points;                                       // to current stamina add points that enemy gives player for killed him

        if (currentStamina >= maxStamina)                               // if current stamina value is greatest than max
            ReadyToUse();                                               // invoke animation, sounds that shows that stamina is full

        staminaBar.UpdateBar(currentStamina, maxStamina);               // update stamina bar on screen
    }


    public void UseSkill()
    {

        if (currentStamina < skillStamina)
        {
            PlaySound(ref failClip);                                    // play fail sound if player hasn't enough stamina
            return;
        }


        if (isStaminaFull)
        {
            isStaminaFull = false;
            staminaBar.BlueIcone(false);
        }

        Skill();                                                        // call function responsible for skill action

        currentStamina -= skillStamina;
        staminaBar.UpdateBar(currentStamina, maxStamina);

        if (isIncreaseWithTime && !isStaminaFull)
            StartCoroutine(IncreaseWithTime());
    }


    void ReadyToUse()
    {
        if (audioSource.clip != readyToUseClip)
            audioSource.clip = readyToUseClip;

        if (!isStaminaFull)
            audioSource.Play();

        currentStamina = maxStamina;                            // to make sure that is not greatest that max
        staminaBar.BlueIcone(true);                             // active blue icone on screen
        isStaminaFull = true;
    }


    protected void PlaySound(ref AudioClip clip)                // derived class will use it
    {
        if (clip)                                               // if there is no clip, do nothing
            audioSource.PlayOneShot(clip);
    }


    public abstract void Skill();                              // derived class will fill this method, every skill is different
}   // Karol Sobański
