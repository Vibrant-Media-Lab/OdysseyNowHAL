using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardDirection
{
    /// <summary>
    /// Manages all sound FX. Publicly accessible as a singleton so that all other scripts can call playSound to play a sound effect.
    /// </summary>
    public class SoundFXManager : MonoBehaviour
    {
        //Singleton instance
        public static SoundFXManager instance;
        //Array of children, which contain all audio sources.
        GameObject[] children;
        
        /// <summary>
        /// Make a singleton instance
        /// </summary>
        void Awake()
        {
            if(instance != null){
                Destroy(gameObject);
            }else{
                instance = this;
            }
        }

        /// <summary>
        /// On start, gather children objects. These children contain audio sources.
        /// </summary>
        private void Start()
        {
            children = new GameObject[transform.childCount];
            for (int i = 0; i < transform.childCount; i++){
                children[i] = transform.GetChild(i).gameObject;
            }
        }

        /// <summary>
        /// Search for child game object with name given and play the audio source on that object.
        /// </summary>
        /// <param name="soundName">Name of the audio.</param>
        public void playSound(string soundName){
            for (int i = 0; i < children.Length; i++){
                if(children[i].name == soundName){
                    children[i].GetComponent<AudioSource>().Play();
                }
            }
        }
    }
}
