using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalInputManager : MonoBehaviour
{
    public static LocalInputManager instance;
    public enum ControlScheme { Keyboard, Traditional, OdysseyCon };
    public ControlScheme p1Scheme = ControlScheme.Keyboard;
    public ControlScheme p2Scheme = ControlScheme.Keyboard;

    GameObject p1Option;
    GameObject p2Option;

    private void Awake()
    {
        if (LocalInputManager.instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void Init(){
        StartCoroutine(InitSoon());
    }

    IEnumerator InitSoon(){
        yield return new WaitForSeconds(1f);
        InitialControls();
    }

    void InitialControls(){
        if (p1Option == null || p2Option == null)
        {
            p1Option = GameObject.Find("P1ControllerDropdown");
            p2Option = GameObject.Find("P2ControllerDropdown");

            if (p1Option == null || p2Option == null){
                Debug.Log("Trouble initing!");
                Init();
                return;
            }
        }

        switch (p1Scheme)
        {
            case ControlScheme.Keyboard:
                p1Option.GetComponent<Dropdown>().value = 0;
                break;
            case ControlScheme.Traditional:
                p1Option.GetComponent<Dropdown>().value = 1;
                break;
            case ControlScheme.OdysseyCon:
                p1Option.GetComponent<Dropdown>().value = 2;
                break;
        }

        switch (p2Scheme)
        {
            case ControlScheme.Keyboard:
                p2Option.GetComponent<Dropdown>().value = 0;
                break;
            case ControlScheme.Traditional:
                p2Option.GetComponent<Dropdown>().value = 1;
                break;
            case ControlScheme.OdysseyCon:
                p2Option.GetComponent<Dropdown>().value = 2;
                break;
        }
    }

    private void OnDestroy()
    {
        LocalInputManager.instance.Init();
    }

    public void UpdateControls()
    {
        if(p1Option == null || p2Option == null){
            p1Option = GameObject.Find("P1ControllerDropdown");
            p2Option = GameObject.Find("P2ControllerDropdown");
        }

        switch(p1Option.GetComponent<Dropdown>().value){
            case 0:
                p1Scheme = ControlScheme.Keyboard;
                break;
            case 1:
                p1Scheme = ControlScheme.Traditional;
                break;
            case 2:
                p1Scheme = ControlScheme.Keyboard;
                break;
        }

        switch (p2Option.GetComponent<Dropdown>().value)
        {
            case 0:
                p2Scheme = ControlScheme.Keyboard;
                break;
            case 1:
                p2Scheme = ControlScheme.Traditional;
                break;
            case 2:
                p2Scheme = ControlScheme.Keyboard;
                break;
        }
    }
}
