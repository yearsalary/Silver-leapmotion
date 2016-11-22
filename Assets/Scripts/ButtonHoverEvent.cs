using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonHoverEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public bool isOver = false;
    public Vector3 startScale;

    void Start()
    {
        startScale = this.transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.transform.localScale = new Vector3(this.transform.localScale.x + 0.3f, this.transform.localScale.y + 0.3f, 0);
        //Debug.Log("Mouse enter");
        isOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.transform.localScale = new Vector3(this.transform.localScale.x - 0.3f, this.transform.localScale.y - 0.3f, 0);
        isOver = false;
    }

    public void onBackButton()
    {
        SceneManager.LoadScene("TraineeSelectScene");
    }
}
