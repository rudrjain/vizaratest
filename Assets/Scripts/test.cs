using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public GameObject cube;
    public AudioSource sound;
    public bool flag = false;
    public bool bounce = true;
    private Vector3 position, position1;


    public void OnMouseDown()
    {
        //cube.SetActive(false);
        sound.Play();
        flag = true;
        position = cube.transform.position;
        position1.y = position.y + 1f;
    }

    private void Update()
    {
        if(flag)
        {
            if(bounce)
            {
                cube.transform.position = Vector3.Lerp(cube.transform.position, position1, 0.5f);
                if (cube.transform.position.y + 0.05f >= position1.y)
                {
                    bounce = false;
                }
            }
            else
            {
                cube.transform.position = Vector3.Lerp(cube.transform.position, position, 0.5f);
                if (cube.transform.position.y - 0.05f <= position1.y)
                {
                    bounce = true;
                }
            }
        }
    }
}
