/* WARNING: WELCOME TO CONVERSION-RATE HELL
 * --- IT IS A MIRACLE THIS WORKS ---
 * 
 * 
 * ... jk, it's not that weird, it's just very excessive in the pursuit of accuracy.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using d = CardDirection.Director;

namespace Actors {
    public class PlayerCubeController : MonoBehaviour {
        private Vector3 newPos;
        public GameObject tgt;
        public float lagfactor;
        float normalLag = 10;
        float slowLag = 50;
        public bool inertia = false;
        public bool playerExtinguish = false;

        float TV_PIXEL_CONSTANT = 1024.0f / 5.0f;

        // X TV Pixel --> Unity X
        float ConvertHorizontalPixelToUnity(float x) {
            float percent = x / 1023.0f;
            return -((percent * 16f) - 8f);
        }

        // Y TV Pixel --> Unity Y
        float ConvertVerticalPixelToUnity(float y) {
            float percent = y / 1023.0f;
            return (percent * 12f) - 6f;
        }

        // Unity X --> X TV Pixel
        float ConvertHorizontalUnityToPixel(float x) {
            x = (x / -6.75f) * 47.0f;
            x = x + 115.0f;
            return x;
        }

        // Unity Y --> Y TV Pixel
        float ConvertVerticalUnityToPixel(float y) {
            y = (y / 5.0f) * 52.0f;
            y = y + 126.0f;
            return y;
        }

        float ConvertPixelToVoltage(float p) {
            return p / TV_PIXEL_CONSTANT;
        }

        float ConvertVoltageToPixel(float v) {
            return v * TV_PIXEL_CONSTANT;
        }

        /// <summary>
        /// Takes a horizontal unity unit and returns a voltage
        /// </summary>
        /// <returns></returns>
        float convertHorizontalToVoltage(float x) {
            return ConvertPixelToVoltage(ConvertHorizontalUnityToPixel(x));
        }

        /// <summary>
        /// Takes a vertical unity unit and returns a voltage
        /// </summary>
        /// <returns></returns>
        float convertVerticalToVoltage(float y) {
            return ConvertPixelToVoltage(ConvertVerticalUnityToPixel(y));
        }

        float convertVoltageToHorizontal(float x) {
            return ConvertHorizontalPixelToUnity(ConvertVoltageToPixel(x));
        }

        float convertVoltageToVertical(float y) {
            return ConvertVerticalPixelToUnity(ConvertVoltageToPixel(y));
        }

        float horizontalVelocity(float x) {
            float alpha = (x + 8) / 16.0f;
            x = convertHorizontalToVoltage(x);

            float v = (x / (0.068f + 0.25f * alpha)) + ((5.6f - x) / (0.00015f + 0.25f * (1 - alpha)));

            if (inertia) {
                v = v * (10f / 110f);
            }

            return Mathf.Abs(convertVoltageToHorizontal(v));
        }

        float verticalVelocity(float y) {
            float alpha = (y + 6) / 12.0f;
            y = convertVerticalToVoltage(y);

            float v = (y / (0.705f + 0.235f * alpha)) + ((5.6f - y) / (0.0846f + 0.235f * (1 - alpha)));

            if (inertia) {
                v = v * (4.7f / 51.7f);
            }

            return Mathf.Abs(convertVoltageToVertical(v));
        }

        void FixedUpdate() {
            if (!d.instance.paused) {
                float targetX = tgt.transform.position.x;
                float targetY = tgt.transform.position.y;

                float oldX = gameObject.transform.position.x;
                float hV = horizontalVelocity(oldX) * Time.deltaTime;
                float xDiff = targetX - oldX;
                float destinationX = targetX;
                if (Mathf.Abs(xDiff) < hV) {
                    destinationX = targetX;
                } else if (xDiff > 0f) {
                    destinationX = oldX + hV;
                } else if (xDiff < 0f) {
                    destinationX = oldX - hV;
                }

                float oldY = gameObject.transform.position.y;
                float vV = verticalVelocity(oldY) * Time.deltaTime;
                float yDiff = targetY - oldY;
                float destinationY = targetY;
                if (Mathf.Abs(yDiff) < vV) {
                    destinationY = targetY;
                } else if (yDiff > 0f) {
                    destinationY = oldY + vV;
                } else if (yDiff < 0f) {
                    destinationY = oldY - vV;
                }

                newPos = new Vector3(destinationX, destinationY, gameObject.transform.position.z);
                gameObject.transform.SetPositionAndRotation(newPos, gameObject.transform.rotation);
            }
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.tag == "Player1") {
                if (playerExtinguish) {
                    extinguish();
                }
            }
        }

        void unExtinguish() {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            gameObject.GetComponent<BoxCollider>().enabled = true;
        }

        void extinguish() {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}