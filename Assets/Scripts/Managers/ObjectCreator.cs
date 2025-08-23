using UnityEngine;
using System.Collections;

public class ObjectCreator : MonoBehaviour
{
    public static ObjectCreator instance;

    [Header("Arrow")]
    public GameObject arrowPrefab;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    //This method is versitile and if I want to pull another object from the game manager, I can
    public void CreateGameObject(GameObject prefab, Transform target, float delay = 0)
    {
        StartCoroutine(CreateGameObjectCoroutine(prefab, target, delay));
    }

    private IEnumerator CreateGameObjectCoroutine(GameObject prefab, Transform target, float delay)
    {
        Vector3 position = target.position;//We store the target trasform first becuase later we might loose the trasform of the game object we want to instantiate.

        yield return new WaitForSeconds(delay);


        GameObject newGameObject = Instantiate(prefab, position, Quaternion.identity);
    }
}
