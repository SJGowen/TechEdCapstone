using UnityEngine;
using Unity.Cinemachine;

public class CameraControl : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private CinemachineCamera virtualCamera;

    void Start()
    {
        virtualCamera = GetComponent<CinemachineCamera>();


        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            virtualCamera.Follow = player.transform;
            virtualCamera.LookAt = player.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
