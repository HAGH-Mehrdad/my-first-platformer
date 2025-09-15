using System.Collections;
using TMPro;
using UnityEngine;

public class PopUpSystem : MonoBehaviour
{
    [SerializeField] private GameObject popUpBox;
    [SerializeField] private TextMeshProUGUI popUpText;

    private Animator anim;


    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void PopUp()
    {
        popUpBox.SetActive(true);
        //popUpText.text = text;
        anim.SetTrigger("PopUp");
    }

    public void ClosePopUp()
    {
        StartCoroutine(CloseCoRoutine());
    }

    private IEnumerator CloseCoRoutine()
    {
        anim.SetTrigger("Close");
        yield return new WaitForSeconds(1f);
        popUpBox.SetActive(false);
    }
}
