using UnityEngine;
using System.Collections;

public class GeroBeam : MonoBehaviour
{
    public GameObject HitEffect;
    public float MaxLength = 16.0f;     // maxsymalna dlugosc promienia
    public float AddLength = 0.1f;      //
    public float Width = 10.0f;
    public float NowLengthGlobal;
    public LayerMask shootableMask;


    private int LRSize = 16;             // wielkosc line renderer
    private float NowLength = 0;
    private float FlashSize;
    private float BlockLen;
    private Vector3 forwardVec;
    private Vector3 HitObjSize;
    private Vector3 currentPosition;
    private Renderer flashRenderer;
    private Renderer renderer;
    private GeroBeamHit HitObj;
    private BeamParam BP;
    private GameObject Flash;
    private ScaleWiggle flashScaleWiggle;
    private ShotParticleEmitter SHP_Emitter;
    private LineRenderer LR;


    void Start()
    {
        BP = GetComponent<BeamParam>();
        LR = GetComponent<LineRenderer>();
        HitObj = transform.Find("GeroBeamHit").GetComponent<GeroBeamHit>();
        SHP_Emitter = transform.Find("ShotParticle_Emitter").GetComponent<ShotParticleEmitter>();
        Flash = transform.Find("BeamFlash").gameObject;
        renderer = GetComponent<Renderer>();
        flashScaleWiggle = Flash.GetComponent<ScaleWiggle>();
        flashRenderer = Flash.GetComponent<Renderer>();
        shootableMask = LayerMask.GetMask("Enemy");

        HitObjSize = HitObj.transform.localScale;
        FlashSize = Flash.transform.localScale.x;
        MaxLength = BP.MaxLength;
        BlockLen = MaxLength / LRSize;
        forwardVec = transform.forward;
    }


    void Update()
    {
        if (BP.bEnd)
            StopGenereate();
        else
            SHP_Emitter.ShotPower = 1.0f;


        NowLength = Mathf.Min(1.0f, NowLength + AddLength);           // narastajaca wielko�� od 0 do 1

        LineRendererPosition();                                       // pozycje promienia
        CheckCollision();                                             // czy nie zaszla kolizja 



        LR.SetWidth(Width * BP.Scale, Width * BP.Scale);              // ustaw szerokosc promienia
        LR.SetColors(BP.BeamColor, BP.BeamColor);                     // ustaw kolory promienia
        float ShotFlashScale = FlashSize * Width * 5.0f;
        flashScaleWiggle.DefScale = new Vector3(ShotFlashScale, ShotFlashScale, ShotFlashScale);
        flashRenderer.material.SetColor("_Color", BP.BeamColor * 2);

        renderer.material.SetFloat("_AddTex", Time.frameCount * -0.05f * BP.AnimationSpd * 10);
        renderer.material.SetFloat("_BeamLength", NowLength);

        SHP_Emitter.ChangeColor(BP.BeamColor * 2);
    }


    void StopGenereate()
    {
        BP.Scale *= 0.9f;
        Width *= 0.9f;
        SHP_Emitter.ShotPower = 0.0f;

        if (Width < 0.01f)
            Destroy(gameObject, 2);
    }


    void CheckCollision()
    {
        bool isCollision = false;
        float workNLG;
        NowLengthGlobal = NowLength * LRSize;


        for (int i = 0; i < LRSize; i++)                                                                     // przejdz prz ez wszystkie LR
        {
            currentPosition = transform.position;

            for (int j = 0; j < i; j++)                              
                currentPosition += forwardVec * BlockLen;


            workNLG = Mathf.Min(1.0f, NowLengthGlobal - i);

            if (workNLG <= 0)
                break;

            RaycastHit hit;

            if (Physics.Raycast(currentPosition, forwardVec, out hit, BlockLen * workNLG, shootableMask))
            {
                GameObject hitobj = hit.collider.gameObject;
                NowLength = ((BlockLen * i) + hit.distance) / MaxLength;
                HitObj.transform.position = currentPosition + forwardVec * hit.distance;
                HitObj.transform.rotation = Quaternion.AngleAxis(180.0f, transform.up) * transform.rotation;

                isCollision = true;
                break;
            }
        }
        HitObj.SetViewPat(isCollision && !BP.bEnd);
        HitObj.col = BP.BeamColor * 2;
    }


    void LineRendererPosition()
    {
        for (int i = 0; i < LRSize; i++)
        {
            currentPosition = transform.position;

            for (int j = 0; j < i; j++)
                currentPosition += forwardVec * BlockLen;

            LR.SetPosition(i, currentPosition);
        }
    }
}
