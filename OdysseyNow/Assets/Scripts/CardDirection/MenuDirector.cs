using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Actors;

namespace CardDirection
{
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

        public GameObject p1;
        public GameObject p2;
        public PlayerTargetController p1Target;
        public PlayerTargetController p2Target;
        public GameObject ball;
        public GameObject wall;

        public void UpdateAll()
        {
            //UpdateSecondPlayer
            p1.SetActive(secondPlayer);
            //UpdateIncludeBall
            ball.SetActive(includeBall);
            //UpdateIncludeWall
            wall.SetActive(includeWall);
            //UpdateWallPosition
            wall.GetComponent<WallBehavior>().setHorizontal(wallPosition);
            //UpdateWallHeight
            wall.GetComponent<WallBehavior>().setHeight(wallHeight);
            //UpdateWallBallCollision
            BallController b = ball.GetComponent<BallController>();
            if (wallBallCollision.Equals("Pass"))
            {
                b.wallBounce = false;
                b.wallExtinguish = false;
            }
            else if (wallBallCollision.Equals("Extinguish"))
            {
                b.wallBounce = false;
                b.wallExtinguish = true;
            }
            else if (wallBallCollision.Equals("Bounce"))
            {
                b.wallBounce = true;
                b.wallExtinguish = false;
            }
            //UpdateP1P2Collision
            PlayerCubeController p = p2.GetComponentInChildren<PlayerCubeController>();
            if (p1P2Collision.Equals("None"))
            {
                p.playerExtinguish = false;
            }
            else if (p1P2Collision.Equals("Extinguish"))
            {
                p.playerExtinguish = true;
            }
            //UpdateP2BallCollision
            if (p2BallCollision.Equals("Bounce"))
            {
                b.playerExtinguish = false;
            }
            else if (p2BallCollision.Equals("Extinguish"))
            {
                b.playerExtinguish = true;
            }
            //UpdateP2Inertia
            p2.GetComponent<PlayerCubeController>().inertia = p2Inertia;
            //UpdateOnResetExtinguish
            if (onResetExtinguish.Equals("None"))
            {
                ball.GetComponent<BallController>().resetExtinguishBall = false;
                p1Target.resetExtinguish = false;
                p2Target.resetExtinguish = false;
            }
            else if (onResetExtinguish.Equals("P1 & P2"))
            {
                ball.GetComponent<BallController>().resetExtinguishBall = false;
                p1Target.resetExtinguish = true;
                p2Target.resetExtinguish = true;
            }
            else if (onResetExtinguish.Equals("P2"))
            {
                ball.GetComponent<BallController>().resetExtinguishBall = false;
                p1Target.resetExtinguish = false;
                p2Target.resetExtinguish = true;
            }
            else if (onResetExtinguish.Equals("Ball"))
            {
                ball.GetComponent<BallController>().resetExtinguishBall = true;
                p1Target.resetExtinguish = false;
                p2Target.resetExtinguish = false;
            }
        }

        public void UpdateAll(bool _secondPlayer, bool _includeBall, bool _includeWall, int _wallPosition, int _wallHeight, string _wallBallCollision, string _p1P2Collision, string _p2BallCollision, bool _p2Inertia, string _onResetExtinguish)
        {
            secondPlayer = _secondPlayer;
            includeBall = _includeBall;
            includeWall = _includeBall;
            wallPosition = _wallPosition;
            wallHeight = _wallHeight;
            wallBallCollision = _wallBallCollision;
            p1P2Collision = _p1P2Collision;
            p2BallCollision = _p2BallCollision;
            p2Inertia = _p2Inertia;
            onResetExtinguish = _onResetExtinguish;
            UpdateAll();
        }

        void RemoveOptions()
        {

        }

        /* The following methods update card variables.
         * These methods are called by the UI elements in the menu.
        */

        public void UpdateSecondPlayer(Toggle tg)
        {
            secondPlayer = tg.isOn;
            p1.SetActive(secondPlayer);
        }

        public void UpdateIncludeBall(Toggle tg)
        {
            includeBall = tg.isOn;
            ball.SetActive(includeBall);
        }

        public void UpdateIncludeWall(Toggle tg)
        {
            includeWall = tg.isOn;
            wall.SetActive(includeWall);
        }

        public void UpdateWallPosition(Slider sl)
        {
            wallPosition = (int)sl.value;
            wall.GetComponent<WallBehavior>().setHorizontal(wallPosition);
        }

        public void UpdateWallHeight(Slider sl)
        {
            wallHeight = (int)sl.value;
            wall.GetComponent<WallBehavior>().setHeight(wallHeight);
        }

        public void UpdateWallBallCollision(Dropdown dr)
        {
            wallBallCollision = dr.options[dr.value].text;
            BallController b = ball.GetComponent<BallController>();
            if (wallBallCollision.Equals("Pass"))
            {
                b.wallBounce = false;
                b.wallExtinguish = false;
            }
            else if (wallBallCollision.Equals("Extinguish"))
            {
                b.wallBounce = false;
                b.wallExtinguish = true;
            }
            else if (wallBallCollision.Equals("Bounce"))
            {
                b.wallBounce = true;
                b.wallExtinguish = false;
            }
        }

        public void UpdateP1P2Collision(Dropdown dr)
        {
            p1P2Collision = dr.options[dr.value].text;
            PlayerCubeController p = p2.GetComponentInChildren<PlayerCubeController>();
            if (p1P2Collision.Equals("None"))
            {
                p.playerExtinguish = false;
            }
            else if (p1P2Collision.Equals("Extinguish"))
            {
                p.playerExtinguish = true;
            }
        }

        public void UpdateP2BallCollision(Dropdown dr)
        {
            p2BallCollision = dr.options[dr.value].text;
            BallController b = ball.GetComponent<BallController>();
            if (p2BallCollision.Equals("Bounce"))
            {
                b.playerExtinguish = false;
            }
            else if (p2BallCollision.Equals("Extinguish"))
            {
                b.playerExtinguish = true;
            }
        }

        public void UpdateP2Inertia(Toggle tg)
        {
            p2Inertia = tg.isOn;
            p2.GetComponent<PlayerCubeController>().inertia = p2Inertia;
        }

        public void UpdateOnResetExtinguish(Dropdown dr)
        {
            onResetExtinguish = dr.options[dr.value].text;
            if (onResetExtinguish.Equals("None"))
            {
                ball.GetComponent<BallController>().resetExtinguishBall = false;
                p1Target.resetExtinguish = false;
                p2Target.resetExtinguish = false;
            }
            else if (onResetExtinguish.Equals("P1 & P2"))
            {
                ball.GetComponent<BallController>().resetExtinguishBall = false;
                p1Target.resetExtinguish = true;
                p2Target.resetExtinguish = true;
            }
            else if (onResetExtinguish.Equals("P2"))
            {
                ball.GetComponent<BallController>().resetExtinguishBall = false;
                p1Target.resetExtinguish = false;
                p2Target.resetExtinguish = true;
            }
            else if (onResetExtinguish.Equals("Ball"))
            {
                ball.GetComponent<BallController>().resetExtinguishBall = true;
                p1Target.resetExtinguish = false;
                p2Target.resetExtinguish = false;
            }
        }
    }
}