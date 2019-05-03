using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CardDirection
{
    /// <summary>
    /// Manages the configuration of input methods.
    /// </summary>
    public class LocalInputManager : MonoBehaviour
    {
        //Singleton instance.
        public static LocalInputManager instance;
        //The possible control schemes
        public enum ControlScheme { Keyboard, Traditional, OdysseyCon, OriginalConsole, AI, OdysseyConLegacy };
        //Control schemes for both players
        public ControlScheme p1Scheme = ControlScheme.Keyboard;
        public ControlScheme p2Scheme = ControlScheme.Keyboard;

        //Dropdown's for the two options
        GameObject p1Option;
        GameObject p2Option;

        /// <summary>
        /// On awake, make a singleton
        /// </summary>
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

        /// <summary>
        /// Public method that initializes the input manager, starting a coroutine. This had to be separate from the coroutine itself so that the coroutine would belong to this object.
        /// </summary>
        public void Init()
        {
            StartCoroutine(InitSoon());
        }

        /// <summary>
        /// Wait one second and then init the menu controls.
        /// </summary>
        /// <returns></returns>
        IEnumerator InitSoon()
        {
            yield return new WaitForSeconds(1f);
            InitialControls();
        }

        /// <summary>
        /// Sets up the previously selected options for control schemes. This is basically just necessary for when one chooses a control scheme, plays a card, and then returns to the main menu. Updates the dropdowns.
        /// </summary>
        void InitialControls()
        {
            if (p1Option == null || p2Option == null)
            {
                p1Option = GameObject.Find("P1ControllerDropdown");
                p2Option = GameObject.Find("P2ControllerDropdown");

                if (p1Option == null || p2Option == null)
                {
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
                case ControlScheme.OriginalConsole:
                    p1Option.GetComponent<Dropdown>().value = 3;
                    break;
                case ControlScheme.AI:
                    p1Option.GetComponent<Dropdown>().value = 4;
                    break;
                case ControlScheme.OdysseyConLegacy:
                    p1Option.GetComponent<Dropdown>().value = 5;
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
                case ControlScheme.OriginalConsole:
                    p2Option.GetComponent<Dropdown>().value = 3;
                    break;
                case ControlScheme.AI:
                    p2Option.GetComponent<Dropdown>().value = 4;
                    break;
                case ControlScheme.OdysseyConLegacy:
                    p2Option.GetComponent<Dropdown>().value = 5;
                    break;
            }
        }

        /// <summary>
        /// When the input manager is destroyed, be sure to initialize a new one.
        /// </summary>
        private void OnDestroy()
        {
            LocalInputManager.instance.Init();
        }

        /// <summary>
        /// Update the control scheme based on options selected on the dropdowns.
        /// </summary>
        public void UpdateControls()
        {
            if (p1Option == null || p2Option == null)
            {
                p1Option = GameObject.Find("P1ControllerDropdown");
                p2Option = GameObject.Find("P2ControllerDropdown");
            }

            switch (p1Option.GetComponent<Dropdown>().value)
            {
                case 0:
                    p1Scheme = ControlScheme.Keyboard;
                    break;
                case 1:
                    p1Scheme = ControlScheme.Traditional;
                    break;
                case 2:
                    p1Scheme = ControlScheme.OdysseyCon;
                    break;
                case 3:
                    p1Scheme = ControlScheme.OriginalConsole;
                    break;
                case 4:
                    p1Scheme = ControlScheme.AI;
                    break;
                case 5:
                    p1Scheme = ControlScheme.OdysseyConLegacy;
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
                    p2Scheme = ControlScheme.OdysseyCon;
                    break;
                case 3:
                    p2Scheme = ControlScheme.OriginalConsole;
                    break;
                case 4:
                    p2Scheme = ControlScheme.AI;
                    break;
                case 5:
                    p2Scheme = ControlScheme.OdysseyConLegacy;
                    break;
            }
        }
    }
}
