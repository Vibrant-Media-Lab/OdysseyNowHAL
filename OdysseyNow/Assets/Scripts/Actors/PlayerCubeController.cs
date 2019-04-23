using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using d = CardDirection.Director;

namespace Actors
{
    public class PlayerCubeController : MonoBehaviour
    {
        private Vector3 newPos;
        public GameObject tgt;
        public float lagfactor;
        float normalLag = 10;
        float slowLag = 50;
        public bool inertia = false;
        public bool playerExtinguish = false;

        Rigidbody rb;


        private void Start()
        {
            rb = gameObject.GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            if (!d.instance.paused)
            {
                /*
                if (inertia)
                {
                    return;
                }

                float tgtX = tgt.transform.position.x;
                float tgtY = tgt.transform.position.y;

                //x-domain: -8 to 8
                //y-range: -6 to 6

                float xAlpha = (tgtX + 8) / 16f;
                float yAlpha = (tgtY + 6) / 12f;

                //float dVx =
                */

                if (inertia)
                {
                    lagfactor = slowLag;
                }
                else
                {
                    lagfactor = normalLag;
                }

                float tgtx = tgt.transform.position.x;
                float tgty = tgt.transform.position.y;

                // Goes 1/8 of the distance to the target (hopefully will have some lag)
                // Also if the target distance is pretty close, stop moving. Close enough counts in horseshoes, hand grenades, and the Magnavox Odyssey
                float oldx = gameObject.transform.position.x;
                float destx = (tgtx - oldx) / lagfactor + gameObject.transform.position.x;
                //if (Mathf.Abs(oldx - destx) < 0.05) destx = oldx;

                float oldy = gameObject.transform.position.y;
                float desty = (tgty - oldy) / lagfactor + gameObject.transform.position.y;
                //if (Mathf.Abs(oldy - desty) < 0.05) desty = oldy;
                newPos = new Vector3(destx, desty, gameObject.transform.position.z);
                gameObject.transform.SetPositionAndRotation(newPos, gameObject.transform.rotation);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player1")
            {
                if (playerExtinguish)
                {
                    extinguish();
                }
            }
        }

        void unExtinguish()
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            gameObject.GetComponent<BoxCollider>().enabled = true;
        }

        void extinguish()
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            CardDirection.SoundFXManager.instance.playSound("Crowbar");
        }
    }
}