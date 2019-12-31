using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic_CatAndMouse : MonoBehaviour
{
    public GameObject player_mouse;
    public GameObject player_cat;
    public Collider2D wall;

    public float p_distance;

    Collider2D cod_mouse;
    Collider2D cod_cat;

    // Start is called before the first frame update
    void Start()
    {
        // Player collider assignments
        cod_cat = player_cat.GetComponentInChildren<Collider2D>();
        cod_mouse = player_mouse.GetComponentInChildren<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        this.p_distance = cod_cat.Distance(cod_mouse).distance;
        if (this.p_distance < 0)
        {
            Debug.Log("Cat have the Mouse");
        }

        if (wall.Distance(cod_cat).distance < 0)
        {
            Debug.Log("Cat collide with the stone");
        }

        if (wall.Distance(cod_mouse).distance < 0)
        {
            Debug.Log("Mouse collide with the stone");
        }

    }
}
