using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardDirection
{
    /// <summary>
    /// Manages the size settings of actors.
    /// </summary>
    public class ElementSettings : MonoBehaviour
    {
        //Singleton instance
        public static ElementSettings instance;

        //size settings for each actor
        public float p1Size;
        public float p2Size;
        public float wallSize;
        public float ballSize;

        /// <summary>
        /// On awake, set initial sizes, grab previous sizes if this isn't the first ElementSettings object, and make this a singleton
        /// </summary>
        private void Awake()
        {
            p1Size = 1f;
            p2Size = 1f;
            wallSize = 0.5f;
            ballSize = 0.5f;

            if (instance != null)
            {
                p1Size = instance.p1Size;
                p2Size = instance.p2Size;
                wallSize = instance.wallSize;
                ballSize = instance.ballSize;
                Destroy(instance.gameObject);
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Finds an object with a given name and tag. This is a helper method.
        /// </summary>
        /// <param name="name">Name of the object we're searching for.</param>
        /// <param name="tag">The tag on the object we're searching for.</param>
        /// <returns>The object we're searching for, or null.</returns>
        public static GameObject FindFromNameAndTag(string name, string tag)
        {
            GameObject[] withTag = GameObject.FindGameObjectsWithTag(tag);
            for (int i = 0; i < withTag.Length; i++)
            {
                if (withTag[i].name.Equals(name))
                {
                    return withTag[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Updates all of the sizes of visible elements
        /// </summary>
        public void updateAllSizes()
        {
            GameObject p1 = FindFromNameAndTag("PlayerBody", "Player1");
            p1.transform.localScale = new Vector3(p1Size, p1Size, p1Size);

            GameObject p2 = FindFromNameAndTag("PlayerBody", "Player2");
            p2.transform.localScale = new Vector3(p2Size, p2Size, p2Size);

            GameObject wall = FindFromNameAndTag("Wall", "Wall");
            if (wall != null)
            {
                wall.transform.localScale = new Vector3(wallSize, wall.transform.localScale.y, wallSize);
            }

            GameObject ball = FindFromNameAndTag("Ball", "Ball");
            if (ball != null)
            {
                ball.transform.localScale = new Vector3(ballSize, ballSize, ballSize);
            }
        }

        /// <summary>
        /// Gets values and updates the size variables with those values.
        /// </summary>
        public void setAllSizes()
        {
            float masterSize = GameObject.Find("OverallSize").GetComponent<Slider>().value;
            GameObject.Find("P1Size").GetComponent<Slider>().value = masterSize;
            GameObject.Find("P2Size").GetComponent<Slider>().value = masterSize;
            GameObject.Find("WallSize").GetComponent<Slider>().value = masterSize;
            GameObject.Find("BallSize").GetComponent<Slider>().value = masterSize;

            p1Size = GameObject.Find("P1Size").GetComponent<Slider>().value / 15.0f;
            p2Size = GameObject.Find("P2Size").GetComponent<Slider>().value / 15.0f;
            wallSize = GameObject.Find("WallSize").GetComponent<Slider>().value / 30.0f;
            ballSize = GameObject.Find("BallSize").GetComponent<Slider>().value / 30.0f;
            updateAllSizes();
        }

        /// <summary>
        /// Set the p1 size (called when p1 slider is adjusted)
        /// </summary>
        public void setP1Size()
        {
            p1Size = GameObject.Find("P1Size").GetComponent<Slider>().value / 15.0f;
            updateAllSizes();
        }

        /// <summary>
        /// Set the p2 size (called when p2 slider is adjusted)
        /// </summary>
        public void setP2Size()
        {
            p2Size = GameObject.Find("P2Size").GetComponent<Slider>().value / 15.0f;
            updateAllSizes();
        }

        /// <summary>
        /// Set the wall size (called when wall slider is adjusted)
        /// </summary>
        public void setWallSize()
        {
            wallSize = GameObject.Find("WallSize").GetComponent<Slider>().value / 30.0f;
            updateAllSizes();
        }

        /// <summary>
        /// Set the ball size (called when ball slider is adjusted)
        /// </summary>
        public void setBallSize()
        {
            ballSize = GameObject.Find("BallSize").GetComponent<Slider>().value / 30.0f;
            updateAllSizes();
        }
    }
}
