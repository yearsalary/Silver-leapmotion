using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour {
    public SpriteRenderer gameEndImage;
    public Sprite[] sprites;
    //public bool isSucced;

    public void changeImage(bool isSucced)
    {
        if (isSucced)
        {
            gameObject.GetComponent<Image>().sprite = sprites[0];
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = sprites[1] ;
        }
    }
}
