using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Actors;

namespace CardDirection
{
    /// <summary>
    /// Handles the playground menu options.
    /// </summary>
    public class MenuDirector : MonoBehaviour
    {
        //Whether the second player is included or not
        bool secondPlayer = true;
        //true if there is a ball
        bool includeBall = true;
        //true if there is a wall
        bool includeWall = true;
        //current position of the wall
        int wallPosition = 1;
        //current height of the wall (see wall's script)
        int wallHeight = 1;
        //What happens when a ball hits a wall
        string wallBallCollision = "Pass";
        //what happens when two players collide
        string p1P2Collision = "None";
        //what happens when p2 hits the ball
        string p2BallCollision = "Bounce";
        //true if p2 moves slower than normal
        bool p2Inertia = false;
        //handles what extinguishes if the reset button is clicked
        string onResetExtinguish = "None";

        //references to all of the actors
        public GameObject p1;
        public GameObject p2;
        public PlayerTargetController p1Target;
        public PlayerTargetController p2Target;
        public GameObject ball;
        public GameObject wall;

        /// <summary>
        /// Update all of the actors based on settings.
        /// </summary>
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

        /// <summary>
        /// Update all of the configuration parameters.
        /// </summary>
        /// <param name="_secondPlayer"></param>
        /// <param name="_includeBall"></param>
        /// <param name="_includeWall"></param>
        /// <param name="_wallPosition"></param>
        /// <param name="_wallHeight"></param>
        /// <param name="_wallBallCollision"></param>
        /// <param name="_p1P2Collision"></param>
        /// <param name="_p2BallCollision"></param>
        /// <param name="_p2Inertia"></param>
        /// <param name="_onResetExtinguish"></param>
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
        

        /* The following methods update card variables.
         * These methods are called by the UI elements in the menu.
        */

        /// <summary>
        /// Updates the 'secondPlayer' variable based on menu selection.
        /// </summary>
        /// <param name="tg">The toggle object that is true or false.</param>
        public void UpdateSecondPlayer(Toggle tg)
        {
            secondPlayer = tg.isOn;
            p1.SetActive(secondPlayer);
        }

        /// <summary>
        /// Updates the 'includeBall' variable based on menu selection.
        /// </summary>
        /// <param name="tg">The toggle object that is true or false.</param>
        public void UpdateIncludeBall(Toggle tg)
        {
            includeBall = tg.isOn;
            ball.SetActive(includeBall);
        }

        /// <summary>
        /// Updates the 'includeWall' variable based on menu selection.
        /// </summary>
        /// <param name="tg">The toggle object that is true or false.</param>
        public void UpdateIncludeWall(Toggle tg)
        {
            includeWall = tg.isOn;
            wall.SetActive(includeWall);
        }

        /// <summary>
        /// Updates the wall position variable based on the setting on the wall position slider.
        /// </summary>
        /// <param name="sl">The wall position slider object</param>
        public void UpdateWallPosition(Slider sl)
        {
            wallPosition = (int)sl.value;
            wall.GetComponent<WallBehavior>().setHorizontal(wallPosition);
        }

        /// <summary>
        /// Updates the wall height variable based on the setting on the wall height slider.
        /// </summary>
        /// <param name="sl">The wall height slider object</param>
        public void UpdateWallHeight(Slider sl)
        {
            wallHeight = (int)sl.value;
            wall.GetComponent<WallBehavior>().setHeight(wallHeight);
        }

        /// <summary>
        /// Update the wallBallCollision options based on the menu's dropdown
        /// </summary>
        /// <param name="dr">The dropdown for wallBallCollision</param>
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

        /// <summary>
        /// Update the collision option between the two players.
        /// </summary>
        /// <param name="dr">Dropdown with the p1p2 collision options</param>
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

        /// <summary>
        /// Update the p2-ball collision option based on menu dropdown.
        /// </summary>
        /// <param name="dr">Dropdown containing the p2-ball collision option</param>
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

        /// <summary>
        /// update the p2 inertia variable based on option selected in menu.
        /// </summary>
        /// <param name="tg">The toggle holding the p2 inertia option</param>
        public void UpdateP2Inertia(Toggle tg)
        {
            p2Inertia = tg.isOn;
            p2.GetComponent<PlayerCubeController>().inertia = p2Inertia;
        }

        /// <summary>
        /// Update the on-reset-extinguish variable based on the menu dropdown.
        /// </summary>
        /// <param name="dr">The dropdown containing the menu option for on-reset-extinguish</param>
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