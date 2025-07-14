using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public int ChosenSkinId;

    public static SkinManager instance;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void SetSkinId(int id) => ChosenSkinId = id;
    public int GetSkinId() => ChosenSkinId;
}
