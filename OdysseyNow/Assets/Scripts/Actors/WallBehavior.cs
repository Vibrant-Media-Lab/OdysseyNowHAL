using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actors
{
    public class WallBehavior : MonoBehaviour
    {
        float yTall = 0f;
        float yShort = -5f;

        float xMid = 0f;
        float xLeft = -5f;
        float xRight = 5f;

        float x;
        float y;
        float z;

        public float xOffset;

        private void Start()
        {
            x = xMid;
            y = yTall;
            z = transform.position.z;
            xOffset = 0;
        }

        private void Update()
        {
            updatePosition();
        }

        void updatePosition()
        {
            transform.position = new Vector3(x + xOffset, y, z);
        }

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