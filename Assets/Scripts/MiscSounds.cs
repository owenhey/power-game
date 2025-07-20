using UnityEngine;

namespace Assets.Scripts.Misc {
    public class MiscSounds : MonoBehaviour {
        public static MiscSounds instance;

        public AudioSource clickSound;
        public AudioSource hoverSound;

        public void Awake(){
            instance = this;
        }

        public void PlayClick(){
            clickSound.Play();
        }
    }
}