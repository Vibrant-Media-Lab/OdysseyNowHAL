using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardDirection
{
    public class SoundFXManager : MonoBehaviour
    {
        public static SoundFXManager instance;
        GameObject[] children;
        // Start is called before the first frame update
        void Awake()
        {
            if(instance != null){
                Destroy(gameObject);
            }else{
                instance = this;
            }
        }

        private void Start()
        {
            children = new GameObject[transform.childCount];
            for (int i = 0; i < transform.childCount; i++){
                children[i] = transform.GetChild(i).gameObject;
            }
        }

        public void playSound(string soundName){
            for (int i = 0; i < children.Length; i++){
                if(children[i].name == soundName){
                    children[i].GetComponent<AudioSource>().Play();
                }
            }
        }
    }
}
