using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [Header("Camera shake")]
    [SerializeField] private Vector2 shakeForce;

    private CinemachineImpulseSource impulseSource;



    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void ShakeCamera(float shakeDir)// shakeDir is the direction of the shake, 
    {
        impulseSource.DefaultVelocity = new Vector2 (shakeForce.x * shakeDir, shakeForce.y);
        impulseSource.GenerateImpulse();
    }
}
