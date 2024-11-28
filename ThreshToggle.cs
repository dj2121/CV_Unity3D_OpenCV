using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ThreshToggle : MonoBehaviour {

    public GameObject ThreshPanel;
    public Texture2D EdgeImage;
    public GameObject ImageMain;

    public void ThreshPanelToggle()
    {
        Rect rec = new Rect(0, 0, EdgeImage.width, EdgeImage.height);
        ImageMain.GetComponent<Image>().sprite = Sprite.Create(EdgeImage, rec, new Vector2(0.5f, 0.5f));
        ThreshPanel.SetActive(true);
    }
}
