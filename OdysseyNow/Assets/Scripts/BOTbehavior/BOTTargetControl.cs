using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOTTargetControl : MonoBehaviour {
    public float startx, starty;
    public GameObject ball;
    private float randomy,lagf;

    void Start () {
        gameObject.transform.position = new Vector3(startx, starty,0);
        StartCoroutine(RandomPos()); StartCoroutine(RandomSpeed());
    }
	
	void Update () {
        float y;
        if (ball.transform.position.x>0)
        {
            //Always follows the ball(LagFactor reduced to allow the AI catch up to the ball)
            BOTCubeController.lagfactor = (int)lagf;
            y = ball.transform.position.y;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, y, gameObject.transform.position.z);
        } else
        {
            BOTCubeController.lagfactor = 20;
            y = randomy;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, y, gameObject.transform.position.z);
        }
    }

    IEnumerator RandomSpeed()
    {
        while (true)
        {
            lagf = Random.Range(4, 10);
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator RandomPos()
    {
        while (true)
        {
            randomy = Random.Range(-4, 4);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
