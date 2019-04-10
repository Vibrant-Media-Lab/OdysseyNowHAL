using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleMirror : MonoBehaviour
{
    public static ConsoleMirror instance;
    
    public Transform p1;
    public Transform p2;
    public Transform ball;
    public Transform line;
    public bool p1Console;
    public bool p2Console;

    public bool pluggedIn;

    float p1X = 0f;
    float p1Y = 0f;

    float p2X = 0f;
    float p2Y = 0f;

    float ballX = 0f;
    float ballY = 0f;

    float wallX = 0f;

    SerialController sc;

    public void p1Reset(){
        //if(pluggedIn)
            //sc.SendSerialMessage("p1Reset");
    }

    private void Awake()
    {
        if (ConsoleMirror.instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        pluggedIn = false;
        sc = gameObject.GetComponent<SerialController>();
    }

    private void LateUpdate()
    {
        if (pluggedIn)
        {
            if (!p1Console)
            {
                //sc.SendSerialMessage("setP1 " + xConvertToConsole(p1.position.x) + " " + yConvertToConsole(p1.position.y) + " ");
            }

            if (!p2Console)
            {

            } 

            p1.position = new Vector2(p1X, p1Y);
            p2.position = new Vector2(p2X, p2Y);
            ball.position = new Vector2(ballX, ballY);
            line.position = new Vector2(wallX, line.position.y);
        }
    }

    float xConvertToUnity(float x){
        x = x - 115.0f;
        x = (x / 47.0f) * -6.75f;
        return x;
    }

    float xConvertToConsole(float x){
        x = (x / -6.75f) * 47.0f;
        x = x + 115.0f;
        return x;
    }

    float yConvertToUnity(float y){
        y = y - 126.0f;
        y = (y / 52.0f) * 5.0f;
        return y;
    }

    float yConvertToConsole(float y){
        y = (y / 5.0f) * 52.0f;
        y = y + 126.0f;
        return y;
    }

    void OnMessageArrived(string msg){
        msg = msg.Substring(0, msg.Length - 3) + "}";
        ConsoleData data = JsonUtility.FromJson<ConsoleData>(msg); 
        p1X = xConvertToUnity(data.P1_X_READ);
        p1Y = yConvertToUnity(data.P1_Y_READ);
        p2X = xConvertToUnity(data.P2_X_READ);
        p2Y = yConvertToUnity(data.P2_Y_READ);
        ballX = xConvertToUnity(data.BALL_X_READ);
        ballY = yConvertToUnity(data.BALL_Y_READ);
        wallX = xConvertToUnity(data.WALL_X_READ);
    } 

    void OnConnectionEvent(bool success){
        if(!success){
            pluggedIn = false;
        }
    }
}
