using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOTTargetControl : MonoBehaviour {
    public float startx, starty;
    public GameObject ball;
    // Use this for initialization
    void Start () {
        gameObject.transform.position = new Vector3(startx, starty,0);
    }
	
	// Update is called once per frame
	void Update () {
        float y = ball.transform.position.y;
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, y, gameObject.transform.position.z);
	}
}
