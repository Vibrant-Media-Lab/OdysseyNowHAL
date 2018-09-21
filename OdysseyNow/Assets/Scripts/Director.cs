using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour {
    public static Director instance;
    public bool paused = false;

    private void Awake()
    {
        if(Director.instance != null){
            Destroy(gameObject);
        }
        else{
            instance = this;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.P)){
            paused = !paused;
        }
	}
}
