using UnityEngine;

public class LevelCameraTrigger : MonoBehaviour
{
    private LevelCamera levelCamera;



    private void Awake()
    {
        levelCamera = GetComponentInParent<LevelCamera>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null) // Question: when would it be possible to player trigger the collison but it is null ?
        {
            levelCamera.SetNewTarget(player.transform);
            levelCamera.EnableCamera(true); // when entering new section with another level camera object
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();



        if (player != null) // Question: when would it be possible to player trigger the collison but it is null ?
        {
            levelCamera.EnableCamera(false); // when leaving new section with another level camera object
        }
    }
}
