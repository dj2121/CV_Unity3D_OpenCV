using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

public class SetCannyThreshold : MonoBehaviour {

    public GameObject PreProcessObj;
    public GameObject ThreshPanel;
    public GameObject ImageMain;
    public Texture2D MainTexture;
    public Slider ThreshSlider;

    public void doSetThreshold()
    {
        CvProcess ScrObj = PreProcessObj.GetComponent<CvProcess>();
        ScrObj.threshhold = (int)ThreshSlider.value;
        Rect rec = new Rect(0, 0, MainTexture.width, MainTexture.height);
        ImageMain.GetComponent<Image>().sprite = Sprite.Create(MainTexture, rec, new Vector2(0.5f, 0.5f));
        ThreshPanel.SetActive(false);
    }
}
