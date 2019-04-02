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
    }

    private void LateUpdate()
    {
        if (pluggedIn)
        {
            if (p1Console)
            {
                p1.position = new Vector2(0f, 0f);
            } else {

            }

            if (p2Console)
            {
                p2.position = new Vector2(0f, 0f);
            } else {

            }

            ball.position = new Vector2(0f, 0f);
            line.position = new Vector2(0f, 0f);
        }
    }
}
