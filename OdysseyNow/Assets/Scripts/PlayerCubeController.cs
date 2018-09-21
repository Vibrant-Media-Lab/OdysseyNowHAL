using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using d = Director;

public class PlayerCubeController : MonoBehaviour {
    private Vector3 mousePosition, newPos;
    private float x, y, z;
    public float moveSpeed = 3;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!d.instance.paused){
            /*x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");
            z = gameObject.transform.position.z;
            newPos = new Vector3(x, y, z);
            gameObject.transform.SetPositionAndRotation(newPos, gameObject.transform.rotation);*/


            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);
        }
    }
}
