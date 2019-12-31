using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collision_logic : MonoBehaviour
{

    public Collider2D player1;
    public Collider2D player2;
    public float p_distance; // for debug purpose

    public Collider2D stone;
    public Collider2D water;
    public Collider2D cheese;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        this.p_distance = player2.Distance(player1).distance;
        if (this.p_distance < 0)
        {
            Debug.Log("Cat have the Mouse");
        }

        if (stone.Distance(player1).distance < 0)
        {
            Debug.Log("Mouse collide with the stone");
        }

        if (water.Distance(player1).distance < 0)
        {
            Debug.Log("Mouse in the water");
        }

        if (cheese.Distance(player1).distance < 0)
        {
            Debug.Log("Mouse have a cheese");
        }
    }

    //void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("OnCollisionEnter");

    //    foreach (ContactPoint contact in collision.contacts)
    //    {
    //        Debug.DrawRay(contact.point, contact.normal, Color.white);
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("OnTriggerEnter");
    //}

}
