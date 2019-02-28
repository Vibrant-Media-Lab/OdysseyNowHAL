using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtonBehavior : MonoBehaviour
{
    public bool load;
    public GameObject otherMenu;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(ButtonClicked);
    }



    void ButtonClicked(){
        if(load){
            SceneManager.LoadScene(gameObject.name.Replace(" ", ""));
        }
        else if(otherMenu != null){
            otherMenu.SetActive(true);
            transform.parent.gameObject.SetActive(false);
        }
    }
}
