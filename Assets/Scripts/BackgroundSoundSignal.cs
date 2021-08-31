
using UnityEngine;

namespace UnityCore
{

    namespace Audio {

        public class BackgroundSoundSignal : MonoBehaviour
        {
            public AudioController audioController;
            public float FadeTime;
            public float TargetVolume;

            private void Start()
            {
                audioController._duration = FadeTime;
                audioController._target = TargetVolume;
            }

            public void PlayBckgrSound() {
      
                    audioController.PlayAudio(AudioType.BCKGR_city, true, FadeTime, TargetVolume); //bool is for fades (yes/no)
            }
        }
    }
}
