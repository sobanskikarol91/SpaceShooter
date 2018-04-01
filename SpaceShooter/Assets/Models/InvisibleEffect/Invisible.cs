using UnityEngine;
using System.Collections;


public class Invisible : StaminaController
{
    [Header("Skill settings")]
    public float activeTime;                                                                    // how long skill will be active;
    public Renderer rendererShipModel;                                                          // reference to render in ship model
    public Texture[] dissolveMaps;                                                              // will me neccessary to show different changes betwen states
    public float smooth = 1;                                                                    // how fast ship will be change
    public float refractionMaterialMax = 70;                                                    // how strong reflection will be

    private bool isSkillActived;                                                                // flag to set ships visibility
    private Material refractionMaterial;                                                        // material that will be change when ship will be invisible
    private Material dissolveMaterial;                                                          // material that will be change to when ship will be visible


    void Start()
    {
        dissolveMaterial = rendererShipModel.materials[0];
        refractionMaterial = rendererShipModel.materials[1];
        StartCoroutine(turnInvisible());
    }


    public override void Skill()
    {
        if (isSkillActived) return;                                                             // if skill is already actived do nothing
        StartCoroutine(IESkill());
    }


    IEnumerator IESkill()
    {
        int randomIndex = Random.Range(0, dissolveMaps.Length);                                
        rendererShipModel.material.SetTexture("_SliceGuide", dissolveMaps[randomIndex]);        // choose random texture

        yield return StartCoroutine(turnVisible());   
        yield return new WaitForSeconds(activeTime);                                            // wait all the time when skill is active

        StartCoroutine(turnInvisible());                                                        // show player ship
    }


    IEnumerator turnVisible()
    {
        float changeTime = 0;
        PlaySound(ref usedClip);

        GameMaster.instance.EnemiesShoot(false);                                                // disable enemy ability to shoot

        while (changeTime < 1)
        {
            changeTime += Time.deltaTime * smooth;

            refractionMaterial.SetFloat("_BumpAmt", Mathf.Lerp(0, refractionMaterialMax, changeTime)); //
            dissolveMaterial.SetFloat("_SliceAmount", Mathf.Lerp(0.0f, 1.0f, changeTime));
            yield return null;
        }
        
        isSkillActived = true;
    }


    IEnumerator turnInvisible()
    {
        isSkillActived = false;
        float changeTime = 0;
        PlaySound(ref endClip);
        
        while (changeTime < 1)
        {
            changeTime += Time.deltaTime * smooth;

            refractionMaterial.SetFloat("_BumpAmt", Mathf.Lerp(refractionMaterialMax, 0, changeTime));
            dissolveMaterial.SetFloat("_SliceAmount", Mathf.Lerp(1.0f, 0.0f, changeTime));
            yield return null;
        }

        GameMaster.instance.EnemiesShoot(true);                                                             // enemy can see player so he can shoot
    }

}   // Karol Sobański