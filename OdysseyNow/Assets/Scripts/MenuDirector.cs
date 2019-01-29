using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDirector : MonoBehaviour
{
    int i;
    // Start is called before the first frame update
    void Start()
    {
        i = 0;
    }

    public void UpdateVariables(){
        i++;
        print("Updated variables " + i);
    }

    // toggle the active state of gameObject
    public void ToggleGameObject(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    // need to have method per GameObject because buttons can only call functions with one or zero parameters -_-
    // maybe you can create a utility class with multiple parameters and pass that in as the only argument?
    public void MoveWallHorizontally(Slider slider)
    {
        GameObject wall = GameObject.Find("Wall");
        wall.transform.position = new Vector3(slider.value, wall.transform.position.y, wall.transform.position.z);
    }

    public void MoveWallVertically(Slider slider)
    {
        GameObject wall = GameObject.Find("Wall");
        wall.transform.position = new Vector3(wall.transform.position.x, slider.value, wall.transform.position.z);
    }
}
