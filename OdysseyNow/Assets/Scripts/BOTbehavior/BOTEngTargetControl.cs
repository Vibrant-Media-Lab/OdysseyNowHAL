using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOTEngTargetControl : MonoBehaviour {

    public float startx, starty;
    static float targetY;
    public float speed;
    public GameObject opponent;
    public GameObject ball;

    void Start () {
        gameObject.transform.position = new Vector3(startx, starty,0);
        targetY = 0;
        StartCoroutine(RandomEng());
    }

	void Update ()
    {
        //smoothly move the target
        if (targetY - gameObject.transform.position.y > 0.1)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + speed * Time.deltaTime, 0);
        }
        else if (targetY - gameObject.transform.position.y < -0.1)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - speed * Time.deltaTime, 0);
        }
    }

    //Randomly select a spot every 0.2 sec
    IEnumerator RandomEng()
    {
        while (true)
        {
            //no ridiculous movement
            targetY = Random.Range(-8, 8);
            yield return new WaitForSeconds(0.2f);
        }
    }
    
}
