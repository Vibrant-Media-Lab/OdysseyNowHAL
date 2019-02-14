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

    /* The following methods update card variables.
     * These methods are called by the UI elements in the menu.
    */

    public void UpdateSecondPlayer(Toggle tg){
        secondPlayer = tg.isOn;
        p2.SetActive(secondPlayer);
    }

    public void UpdateIncludeBall(Toggle tg){
        includeBall = tg.isOn;
        ball.SetActive(includeBall);
    }

    public void UpdateIncludeWall(Toggle tg){
        includeWall = tg.isOn;
        wall.SetActive(includeWall);
    }

    public void UpdateWallPosition(Slider sl){
        wallPosition = (int) sl.value;
        wall.GetComponent<WallBehavior>().setHorizontal(wallPosition);
    }

    public void UpdateWallHeight(Slider sl){
        wallHeight = (int) sl.value;
        wall.GetComponent<WallBehavior>().setHeight(wallHeight);
    }

    public void UpdateWallBallCollision(Dropdown dr){
        wallBallCollision = dr.options[dr.value].text;
        BallController b = ball.GetComponent<BallController>();
        if (wallBallCollision.Equals("Pass")){
            b.wallBounce = false;
            b.wallExtinguish = false;
        } else if (wallBallCollision.Equals("Extinguish")) {
            b.wallBounce = false;
            b.wallExtinguish = true;
        } else if (wallBallCollision.Equals("Bounce")){
            b.wallBounce = true;
            b.wallExtinguish = false;
        }
    }

    public void UpdateP1P2Collision(Dropdown dr){
        p1P2Collision = dr.options[dr.value].text;
        PlayerCubeController p = p2.GetComponentInChildren<PlayerCubeController>();
        if (p1P2Collision.Equals("None")){
            p.playerExtinguish = false;
        } else if (p1P2Collision.Equals("Extinguish")){
            p.playerExtinguish = true;
        }
    }

    public void UpdateP2BallCollision(Dropdown dr){
        p2BallCollision = dr.options[dr.value].text;
        BallController b = ball.GetComponent<BallController>();
        if (p2BallCollision.Equals("Bounce")){
            b.playerExtinguish = false;
        } else if (p2BallCollision.Equals("Extinguish")){
            b.playerExtinguish = true;
        }
    }

    public void UpdateP2Inertia(Toggle tg){
        p2Inertia = tg.isOn;
        //TODO: Set P2 inertia behavior based on variable
        if(p2Inertia){

        } else {

        }
    }

    public void UpdateOnResetExtinguish(Dropdown dr){
        onResetExtinguish = dr.options[dr.value].text;
        //TODO: Set reset behavior based on variable
        //DEPENDENCY: Make reset clicks work.
        if (onResetExtinguish.Equals("None")) {

        } else if (onResetExtinguish.Equals("P1 & P2")) {

        } else if (onResetExtinguish.Equals("P2")) {

        } else if (onResetExtinguish.Equals("Ball")) {

        }
    }
}
