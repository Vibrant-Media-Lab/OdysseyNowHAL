using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDirector : MonoBehaviour
{
    bool secondPlayer = true;
    bool includeBall = true;
    bool includeWall = true;
    int wallPosition = 1;
    int wallHeight = 1;
    string wallBallCollision = "Pass";
    string p1P2Collision = "None";
    string p2BallCollision = "Bounce";
    bool p2Inertia = false;
    string onResetExtinguish = "None";

    public GameObject p2;
    public GameObject ball;
    public GameObject wall;

    // Start is called before the first frame update
    void Start()
    {

    }

    /* The following methods update card variables.
     * These methods are called by the UI elements in the menu.
    */

    public void UpdateSecondPlayer(Toggle tg){
        secondPlayer = tg.isOn;
        p2.SetActive(secondPlayer);
    }

    public void UpdateIncludeBall(Toggle tg){
        includeBall = tg.isOn;
        ball.SetActive(secondPlayer);
    }

    public void UpdateIncludeWall(Toggle tg){
        includeWall = tg.isOn;
        wall.SetActive(secondPlayer);
    }

    public void UpdateWallPosition(Slider sl){
        wallPosition = (int) sl.value;
        //TODO: Set wall position based on variable
    }

    public void UpdateWallHeight(Slider sl){
        wallHeight = (int) sl.value;
        //TODO: Set wall height based on variable
    }

    public void UpdateWallBallCollision(Dropdown dr){
        wallBallCollision = dr.options[dr.value].text;
        //TODO: Set wall-ball collision behavior based on variable
        if(wallBallCollision.Equals("Pass")){

        } else if (wallBallCollision.Equals("Extinguish")) {

        } else if (wallBallCollision.Equals("Bounce")){

        }
    }

    public void UpdateP1P2Collision(Dropdown dr){
        p1P2Collision = dr.options[dr.value].text;
        //TODO: Set player-player collision behavior based on variable
        if (p1P2Collision.Equals("None")){

        } else if (p1P2Collision.Equals("Extinguish")){

        }
    }

    public void UpdateP2BallCollision(Dropdown dr){
        p2BallCollision = dr.options[dr.value].text;
        //TODO: Set player-ball collision behavior based on variable
        if (p2BallCollision.Equals("Bounce")){

        } else if (p2BallCollision.Equals("Extinguish")){

        }
    }

    public void UpdateP2Inertia(Toggle tg){
        p2Inertia = tg.isOn;
        //TODO: Set P2 inertia behavior based on variable
        if(p2Inertia){

        } else {

        }
    }

<<<<<<< HEAD
    public void UpdateOnResetExtinguish(Dropdown dr){
        onResetExtinguish = dr.options[dr.value].text;
        //TODO: Set reset behavior based on variable
        if (onResetExtinguish.Equals("None")) {

        } else if (onResetExtinguish.Equals("P1 & P2")) {

        } else if (onResetExtinguish.Equals("P2")) {

        } else if (onResetExtinguish.Equals("Ball")) {

        }
    }

=======
    // toggle the active state of gameObject
    public void ToggleGameObject(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    // need to have method per GameObject because buttons can only call functions with one or zero parameters -_-
    // maybe you can create a utility class with multiple parameters and pass that in as the only argument?
    public void MoveWallHorizontally(Slider slider)
    {
        GameObject wall = GameObject.Find("Wall");
        wall.transform.position = new Vector3(slider.value, wall.transform.position.y, wall.transform.position.z);
    }

    public void MoveWallVertically(Slider slider)
    {
        GameObject wall = GameObject.Find("Wall");
        wall.transform.position = new Vector3(wall.transform.position.x, slider.value, wall.transform.position.z);
    }
>>>>>>> 27a2cb96c8cf6c2a619abdbc0712381f3e3aca95
}
