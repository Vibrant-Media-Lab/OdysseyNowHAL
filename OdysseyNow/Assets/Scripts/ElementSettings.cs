using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementSettings : MonoBehaviour
{
    public static ElementSettings instance;
    public float p1Size;
    public float p2Size;
    public float wallSize;
    public float ballSize;

    private void Awake()
    {
        p1Size = 1f;
        p2Size = 1f;
        wallSize = 0.5f;
        ballSize = 0.5f;

        if (instance != null){
            p1Size = instance.p1Size;
            p2Size = instance.p2Size;
            wallSize = instance.wallSize;
            ballSize = instance.ballSize;
            Destroy(instance.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    GameObject FindFromNameAndTag(string name, string tag){
        GameObject[] withTag = GameObject.FindGameObjectsWithTag(tag);
        for (int i = 0; i < withTag.Length; i++){
            if(withTag[i].name.Equals(name)){
                return withTag[i];
            }
        }
        return null;
    }

    public void updateAllSizes(){
        GameObject p1 = FindFromNameAndTag("PlayerBody", "Player1");
        p1.transform.localScale = new Vector3(p1Size, p1Size, p1Size);

        GameObject p2 = FindFromNameAndTag("PlayerBody", "Player2");
        p2.transform.localScale = new Vector3(p2Size, p2Size, p2Size);

        GameObject wall = FindFromNameAndTag("Wall", "Wall");
        wall.transform.localScale = new Vector3(wallSize, wall.transform.localScale.y, wallSize);

        GameObject ball = FindFromNameAndTag("Ball", "Ball");
        ball.transform.localScale = new Vector3(ballSize, ballSize, ballSize);
    }

    public void setP1Size(){
        p1Size = GameObject.Find("P1Size").GetComponent<Slider>().value/15.0f;
        updateAllSizes();
    }

    public void setP2Size(){
        p2Size = GameObject.Find("P2Size").GetComponent<Slider>().value / 15.0f;
        updateAllSizes();
    }

    public void setWallSize(){
        wallSize = GameObject.Find("WallSize").GetComponent<Slider>().value / 30.0f;
        updateAllSizes();
    }

    public void setBallSize(){
        ballSize = GameObject.Find("BallSize").GetComponent<Slider>().value / 30.0f;
        updateAllSizes();
    }
}
