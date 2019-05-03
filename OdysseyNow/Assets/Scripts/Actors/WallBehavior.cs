using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actors
{
    /// <summary>
    /// This is the behavior of the wall game object. It manages its x position and whether it is short or tall.
    /// </summary>
    public class WallBehavior : MonoBehaviour
    {
        //These are the two configs for wall height
        float yTall = 0f;
        float yShort = -5f;

        //These are the three configs for wall position (the wall can start on the left, right, or center, based on card)
        float xMid = 0f;
        float xLeft = -5f;
        float xRight = 5f;

        //These are the chosen configurations. The location of the wall is based on these values.
        float x;
        float y;
        float z;

        //This is the offset from its central position. This is public so it can altered from another script, controlling user input, since wall offset was on a knob on the original odyssey.
        //TODO: make some user input change this.
        public float xOffset;

        /// <summary>
        /// On start, set initial configuration to default: middle, tall.
        /// </summary>
        private void Start()
        {
            x = xMid;
            y = yTall;
            z = transform.position.z;
            xOffset = 0;
        }

        /// <summary>
        /// Every frame, update the position to reflect current config. This is mostly redundant.
        /// </summary>
        private void Update()
        {
            updatePosition();
        }

        /// <summary>
        /// Sets the transform's position based on current config of x, y, z.
        /// </summary>
        void updatePosition()
        {
            transform.position = new Vector3(x + xOffset, y, z);
        }

        /// <summary>
        /// Set the height of the wall to tall or short. Accessible by other classes.
        /// </summary>
        /// <param name="height">Wall height of tall (1) or short (0). Other values are ignored.</param>
        public void setHeight(float height)
        {
            if (height == 1)
            {
                y = yTall;
            }
            else if (height == 0)
            {
                y = yShort;
            }
        }

        /// <summary>
        /// Set the horizontal location of the wall to left, right, or center. Accessible by other classes.
        /// </summary>
        /// <param name="horizontal">Wall position of left (0), middle (1), or right (2). All other values are ignored.</param>
        public void setHorizontal(float horizontal)
        {
            if (horizontal == 0)
            {
                x = xLeft;
            }
            else if (horizontal == 1)
            {
                x = xMid;
            }
            else if (horizontal == 2)
            {
                x = xRight;
            }
        }
    }
}