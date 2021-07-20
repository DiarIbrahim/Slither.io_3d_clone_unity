using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SceneManager : MonoBehaviour
{
    CinemachineFreeLook cinemachinecam;

    [HideInInspector] public bool is_cameraMode_classic = false;


    public static SceneManager main;
    private void Awake()
    {
        main = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        cinemachinecam = FindObjectOfType<CinemachineFreeLook>();
        changeCameraMode();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            changeCameraMode();
        }
        
    }


    void changeCameraMode()
    {

        is_cameraMode_classic = !is_cameraMode_classic;


        if (is_cameraMode_classic)
        {
            cinemachinecam.gameObject.SetActive(false);
            Camera.main.transform.GetComponent<CameraContoller>().enabled = true;
        }
        else
        {
            cinemachinecam.gameObject.SetActive(true);
            Camera.main.transform.GetComponent<CameraContoller>().enabled = false;


        }

    }

}


