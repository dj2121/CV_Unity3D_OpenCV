using UnityEngine;
using System.Collections;

public class ToggleExitCancel : MonoBehaviour {

	public GameObject Exitalert;

	public void doExitCancel()

	{
		Exitalert.SetActive (false);
	}
}
