using UnityEngine;

public class AnimatedBacground : MonoBehaviour
{

    [SerializeField] private Vector2 movementDirection;

    private MeshRenderer mesh;


    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
    }


    private void Update()
    {
        mesh.material.mainTextureOffset += movementDirection * Time.deltaTime; // by changing the offset of the image material we create frame reate animated background
    }
}
