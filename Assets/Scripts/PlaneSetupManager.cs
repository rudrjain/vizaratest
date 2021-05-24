using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class PlaneSetupManager : MonoBehaviour
{
    public ARPlaneManager planeManager;
    public Material occlusionMat, PlaneMat,m1,m2,m3;
    public GameObject planePrefab,MarblePanel;
    public Text txt;
    private bool occlusion = false;

    public void SetOcclusionMaterial()
    {
        planePrefab.GetComponent<Renderer>().material = occlusionMat;
        foreach(var plane in planeManager.trackables)
        {
            plane.GetComponent<Renderer>().material = occlusionMat;
        }
        txt.text = "Turn Off Occlusion";
        MarblePanel.SetActive(false);
    }
    public void SetPlaneMaterial()
    {
        planePrefab.GetComponent<Renderer>().material = PlaneMat;
        foreach (var plane in planeManager.trackables)
        {
            plane.GetComponent<Renderer>().material = PlaneMat;
        }
        txt.text = "Turn On Occlusion";
        MarblePanel.SetActive(true);
    }
    public void mat1()
    {
        PlaneMat.CopyPropertiesFromMaterial(m1);
        SetPlaneMaterial();
    }
    public void mat2()
    {
        PlaneMat.CopyPropertiesFromMaterial(m2);
        SetPlaneMaterial();
    }
    public void mat3()
    {
        PlaneMat.CopyPropertiesFromMaterial(m3);
        SetPlaneMaterial();
    }
    public void toggleOcclusion()
    {
        if(!occlusion)
        {
            SetOcclusionMaterial();
            occlusion = !occlusion;
        }
        else
        {
            SetPlaneMaterial();
            occlusion = !occlusion;
        }
    }

}
