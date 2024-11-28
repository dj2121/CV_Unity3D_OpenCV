using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SFB;

public class LoadImage : MonoBehaviour {


	public Texture2D ImageTexture1;
    public Texture2D ImageTexture2;
    public Texture2D ImageTexture3;

	public GameObject ImageMain;
    public GameObject PostProcessPanel;
	public GameObject WelcomeMessage;
    public GameObject ProcessButton;
    public GameObject PreProcessTogg;
    public GameObject DownloadImageTogg;
    public string[] paths;


	public void doLoadImage()
	{
        StartCoroutine(FileSelection());
	}

	IEnumerator FileSelection(){

        paths = null;
        paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", "", false);

		if(paths[0] != null) 
		{
			WWW www = new WWW ("file:///" + paths[0]);
			www.LoadImageIntoTexture (ImageTexture1);
            www.LoadImageIntoTexture(ImageTexture2);
            www.LoadImageIntoTexture(ImageTexture3);
			Rect rec = new Rect(0, 0, ImageTexture3.width, ImageTexture3.height);
			ImageMain.GetComponent<Image> ().sprite = Sprite.Create(ImageTexture3, rec, new Vector2(0.5f, 0.5f));
			WelcomeMessage.SetActive (false);
            ProcessButton.SetActive(true);
            PreProcessTogg.SetActive(true);
            DownloadImageTogg.SetActive(true);
            PostProcessPanel.SetActive(false);
		}
		yield return null;
	}

}
