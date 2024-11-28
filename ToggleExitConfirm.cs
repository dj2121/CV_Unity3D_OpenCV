using UnityEngine;
using System.Collections;

public class ToggleExitConfirm : MonoBehaviour {

	public GameObject Exitalert;

	public void ToggleExit()

	{
		Exitalert.SetActive (true);
	}
}
