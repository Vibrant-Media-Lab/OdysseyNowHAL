using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
