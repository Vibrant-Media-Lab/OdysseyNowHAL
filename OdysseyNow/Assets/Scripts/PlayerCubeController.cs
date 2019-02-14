using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using d = Director;

public class PlayerCubeController : MonoBehaviour {
    private Vector3 newPos;
    public GameObject tgt;
    public int lagfactor = 10;

    public bool playerExtinguish = false;

    //TODO: Handle reset button clicks

	void FixedUpdate () {
        if (!d.instance.paused){
            float tgtx = tgt.transform.position.x;
            float tgty = tgt.transform.position.y;

            // Goes 1/8 of the distance to the target (hopefully will have some lag)
            // Also if the target distance is pretty close, stop moving. Close enough counts in horseshoes, hand grenades, and the Magnavox Odyssey
            float oldx = gameObject.transform.position.x;
            float destx = (tgtx - oldx) / lagfactor + gameObject.transform.position.x;
            if (Mathf.Abs(oldx - destx) < 0.05) destx = oldx;

            float oldy = gameObject.transform.position.y;
            float desty = (tgty - oldy) / lagfactor + gameObject.transform.position.y;
            if (Mathf.Abs(oldy - desty) < 0.05) desty = oldy;
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
                gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}
