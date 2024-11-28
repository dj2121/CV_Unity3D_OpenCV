using UnityEngine;
using System.Collections;

public class PPToggle : MonoBehaviour {

    public GameObject PPTogg;

    public void doPPToggle()
    {
        PPTogg.SetActive(true);
    }
}
