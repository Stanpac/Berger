using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class GCamCheat : MonoBehaviour
{
    public CinemachineCamera StartCam;
    public CinemachineBrain cinemachineBrain;
    
    private List<CinemachineCamera> Cams = new List<CinemachineCamera>();
    private GameObject CurrentCamera;

    public void SwitchCamera(CinemachineCamera cam)
    {
        if (CurrentCamera != null)
        {
            CurrentCamera.SetActive(false);
        }
        
        cam.gameObject.SetActive(true);
        CurrentCamera = cam.gameObject;
    }

    private void Awake()
    {
        foreach (var cam in FindObjectsOfType<CinemachineCamera>()) {
            Cams.Add(cam);
            cam.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        StartCam.gameObject.SetActive(true);
    }
}   
