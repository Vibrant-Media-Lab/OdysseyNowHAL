using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoOpening : MonoBehaviour {

	public GameObject popUpWelcome;
	public Animator inputArrow;
	public float waitTime;

	// JUST FOR DEMO
	void Start () {
		popUpWelcome.SetActive(false);
		StartCoroutine(LoadPopUpWelcome());
	}
	
	IEnumerator LoadPopUpWelcome(){
		yield return new WaitForSeconds(waitTime);
		popUpWelcome.SetActive(true);
	}

	// Changes direction of Input Arrow
	public void ChangeArrowDirection(int x){
		inputArrow.SetInteger("direction", x);
	}
}
