using Unity.Cinemachine;
using UnityEngine;

public class LevelCamera : MonoBehaviour
{
    private CinemachineCamera CinemachineCamera;

    private void Awake()
    {
        CinemachineCamera = GetComponentInChildren<CinemachineCamera>(true); // true to include inactive children
        EnableCamera(false); // in order to not have the camera enabled if the designer forgot to disable additional level cameras
    }

    public void SetNewTarget(Transform newTarget)
    {
        CinemachineCamera.Follow = newTarget;
    }

    public void EnableCamera(bool enable)
    {
        CinemachineCamera.gameObject.SetActive(enable);
    }
}
