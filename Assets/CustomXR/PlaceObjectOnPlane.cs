using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

public class PlaceObjectOnPlane : MonoBehaviour
{
    public GameObject instructionPanel,instructionIcon;
    private bool objecttoPlace = true;
    public GameObject weatherPanel,occlusionButton;

    private ARPlaneManager planeManager;
    private ARRaycastManager raycastManager;
    public PlaneSetupManager _PlaneSetupManager;

    private List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    public GameObject placedPrefab,reflectionProbe;
    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
    }

    private void Update()
    {
        if(objecttoPlace)
        {
            if (Input.touchCount > 0)
            {
                PlaceObject();
            }
        }
        
    }

    private void PlaceObject()
    {
        Touch touch = Input.GetTouch(0);

        if(touch.phase == TouchPhase.Began &&!isPointerOverUIObject())
        {
            if (raycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = s_Hits[0].pose;
                placedPrefab.SetActive(true);
                placedPrefab.transform.position = hitPose.position;
                //placedPrefab.transform.rotation = hitPose.rotation;
                reflectionProbe.SetActive(true);
                reflectionProbe.transform.position = hitPose.position;
                //reflectionProbe.transform.rotation = hitPose.rotation;
               // _PlaneSetupManager.SetOcclusionMaterial();
                //Instantiate(placedPrefab, hitPose.position, hitPose.rotation);
                objecttoPlace = false;
                weatherPanel.SetActive(true);
                occlusionButton.SetActive(true);
                OpenInstruction();
                instructionIcon.SetActive(true);
            }
        }
    }

    private bool isPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public void OpenInstruction()
    {
        instructionPanel.SetActive(true);
    }
    public void CloseInstruction()
    {
        instructionPanel.SetActive(false);
    }
}
