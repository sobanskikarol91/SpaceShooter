using UnityEngine;
using System.Collections;

public class BeamParam : MonoBehaviour
{

    public Color BeamColor = Color.white;
    public float AnimationSpd = 0.1f;
    public float Scale = 1.0f;
    public float MaxLength = 32.0f;


    public bool bEnd = false;
    public bool bGero = false;

    public void SetBeamParam(BeamParam param)
    {
        BeamColor = param.BeamColor;
        AnimationSpd = param.AnimationSpd;
        Scale = param.Scale;
        MaxLength = param.MaxLength;
    }

    

    void Start()
    {
        BeamColor.r = 1.1f;
        BeamColor.g = 0.1f;
        BeamColor.b = 0.1f;

        BeamParam param = GetComponent<BeamParam>();

        if (!param) return;

        BeamColor = param.BeamColor;
        AnimationSpd = param.AnimationSpd;
        Scale = param.Scale;
        MaxLength = param.MaxLength;
    }
}
