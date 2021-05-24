using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARCore;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Interaction : MonoBehaviour
{
    public GameObject head;

    private int flag = 0;
    private bool interact = false;
    public Material robotMat;
    
    public AudioSource sound;

    public void OnMouseDown()
    {
        if (Input.touchCount > 0 && !isPointerOverUIObject())
        {
            flag++;
            interact = true;
            //head.SetActive(false);
            if (flag == 2)
            {
                sound.Play();
            }
        }
    }

    private void Update()
    {
        if(interact)
        {
            if (flag == 1)
            {
                head.transform.Rotate(5f, 0f, 0f);
            }
            else if (flag == 2)
            {
                //head.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                interact = false;
            }
            else if(flag==3)
            {
                robotMat.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                flag = 0;
                interact = false;
                sound.Pause();
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
}
