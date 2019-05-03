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
    /// <summary>
    /// Controls the visible player cube. Since this does not actually depend at all on player input, this should probably be called 'PlayerCubeBehavior'.
    /// </summary>
    public class PlayerCubeController : MonoBehaviour {
        private Vector3 newPos;

        //The player cube target corresponding to this block
        public GameObject tgt;

        //True if the player cube moves slower, with inertia
        public bool inertia = false;

        //True if the player is supposed to extinguish on collision with other object
        public bool playerExtinguish = false;

        //The conversion rate between TV pixel and voltage
        float TV_PIXEL_CONSTANT = 1024.0f / 5.0f;

        /// <summary>
        /// X TV Pixel --> Unity X
        /// </summary>
        /// <param name="x">X TV pixel</param>
        /// <returns>Unity x value</returns>
        float ConvertHorizontalPixelToUnity(float x) {
            float percent = x / 1023.0f;
            return -((percent * 16f) - 8f);
        }

        /// <summary>
        /// Y TV Pixel --> Unity Y
        /// </summary>
        /// <param name="y">TV pixel y</param>
        /// <returns>Unity y value</returns>
        float ConvertVerticalPixelToUnity(float y) {
            float percent = y / 1023.0f;
            return (percent * 12f) - 6f;
        }

        /// <summary>
        /// Unity X --> X TV Pixel
        /// </summary>
        /// <param name="x">Unity x value</param>
        /// <returns>TV pixel x</returns>
        float ConvertHorizontalUnityToPixel(float x) {
            x = (x / -6.75f) * 47.0f;
            x = x + 115.0f;
            return x;
        }

        /// <summary>
        /// Unity Y --> Y TV Pixel
        /// </summary>
        /// <param name="y">Y value in unity units</param>
        /// <returns>TV pixel y</returns>
        float ConvertVerticalUnityToPixel(float y) {
            y = (y / 5.0f) * 52.0f;
            y = y + 126.0f;
            return y;
        }

        /// <summary>
        /// Converts a TV-pixel unit to voltage
        /// </summary>
        /// <param name="p">TV-pixel value</param>
        /// <returns>Voltage.</returns>
        float ConvertPixelToVoltage(float p) {
            return p / TV_PIXEL_CONSTANT;
        }

        /// <summary>
        /// Converts voltage to TV-pixel unit
        /// </summary>
        /// <param name="v">Voltage.</param>
        /// <returns>TV pixel value</returns>
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

        /// <summary>
        /// Convert a horizontal voltage to an x value in Unity units.
        /// </summary>
        /// <param name="x">Voltage in x direction.</param>
        /// <returns>X value in unity units.</returns>
        float convertVoltageToHorizontal(float x) {
            return ConvertHorizontalPixelToUnity(ConvertVoltageToPixel(x));
        }

        /// <summary>
        /// Converts a voltage value to a y value in unity.
        /// </summary>
        /// <param name="y">Voltage in the y direction.</param>
        /// <returns>Unity unit y coordinate</returns>
        float convertVoltageToVertical(float y) {
            return ConvertVerticalPixelToUnity(ConvertVoltageToPixel(y));
        }

        /// <summary>
        /// Calculates the current horizontal velocity. 
        /// </summary>
        /// <param name="x">The current x location of the box.</param>
        /// <returns></returns>
        float horizontalVelocity(float x) {
            float alpha = (x + 8) / 16.0f;
            x = convertHorizontalToVoltage(x);

            float v = (x / (0.068f + 0.25f * alpha)) + ((5.6f - x) / (0.00015f + 0.25f * (1 - alpha)));

            if (inertia) {
                v = v * (10f / 110f);
            }

            return Mathf.Abs(convertVoltageToHorizontal(v));
        }

        /// <summary>
        /// Calculates the vertical velocity
        /// </summary>
        /// <param name="y">The current y location of the box.</param>
        /// <returns></returns>
        float verticalVelocity(float y) {
            float alpha = (y + 6) / 12.0f;
            y = convertVerticalToVoltage(y);

            float v = (y / (0.705f + 0.235f * alpha)) + ((5.6f - y) / (0.0846f + 0.235f * (1 - alpha)));

            if (inertia) {
                v = v * (4.7f / 51.7f);
            }

            return Mathf.Abs(convertVoltageToVertical(v));
        }

        /// <summary>
        /// On fixed update, move the player box towards its target, following the most complicated mechanism possible...
        /// </summary>
        void FixedUpdate() {
            if (!d.instance.paused) {
                float targetX = tgt.transform.position.x;
                float targetY = tgt.transform.position.y;

                //Handles new x coordinate
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

                //handles the new y coordinate
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

                //simply sets new location
                newPos = new Vector3(destinationX, destinationY, gameObject.transform.position.z);
                gameObject.transform.SetPositionAndRotation(newPos, gameObject.transform.rotation);
            }
        }

        /// <summary>
        /// On trigger enter, if p2 hit p1 and p2 is supposed to extinguish when that happens, extinguish.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.tag == "Player1") {
                if (playerExtinguish) {
                    extinguish();
                }
            }
        }

        /// <summary>
        /// Make the player visible and interactable.
        /// </summary>
        void unExtinguish() {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            gameObject.GetComponent<BoxCollider>().enabled = true;
        }

        /// <summary>
        /// Make the player invisible and uninteractable
        /// </summary>
        void extinguish() {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}