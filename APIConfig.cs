using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class APIConfig : MonoBehaviour {

    public InputField APIField;
    public string API;

	void Start () {

        API = PlayerPrefs.GetString("APIKey", "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
        APIField.text = API;

	}
	
	public void setAPI()
    {
        API = APIField.text;
        PlayerPrefs.SetString("APIKey", API);
    }
}
