using UnityEngine;

public class AnimatedBacground : MonoBehaviour
{

    [SerializeField] private Vector2 movementDirection;

    private MeshRenderer mesh;


    [Header("Background Type")]
    [SerializeField] private BackgroundType backgroundType; // This is used to set the type of background we want to use, it is stored in BackgroundType.cs

    [SerializeField] private Texture2D[] textures; 

    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
        UpdateBackgroundTexture();
    }


    private void Update()
    {
        mesh.material.mainTextureOffset += movementDirection * Time.deltaTime; // by changing the offset of the image material we create frame reate animated background
    }


    [ContextMenu ("Update Background")]
    private void UpdateBackgroundTexture()
    {
        if (mesh == null)
        {
            mesh = GetComponent<MeshRenderer>();
        }

        // Switched to sharedMaterial.mainTexture to prevent unnecessary material instantiation per frame/update,
        // which is more performant for a constantly animating background.
        mesh.sharedMaterial.mainTexture = textures[(int)backgroundType];
    }
}
