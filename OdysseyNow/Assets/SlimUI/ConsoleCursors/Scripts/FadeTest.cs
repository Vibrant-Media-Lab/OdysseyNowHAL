using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTest : MonoBehaviour {
	bool canFade;

	void Start(){
		canFade = true;
	}
	void Update () {
		if(Input.GetKeyDown("r") && canFade){
			StartCoroutine(FadeQuick());
		}
	}
	
	IEnumerator FadeQuick(){
		canFade = false;
		GetComponent<Animator>().SetBool("Fade",true);
		yield return new WaitForSeconds(1.5f);
		GetComponent<Animator>().SetBool("Fade",false);
		yield return new WaitForSeconds(0.15f);
		canFade = true;
	}
}
