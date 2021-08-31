
using UnityEngine;

namespace UnityCore {

    namespace Audio {

        public class TestAudio : MonoBehaviour
        {

            public AudioController audioController;

#region Unity Functions
#if UNITY_EDITOR
            private void Update() {

                //Grab and windows
                if (Input.GetKeyDown(KeyCode.Alpha1)) {
                    audioController.PlayAudio(AudioType.SFX_grabObject, false); //bool is for fades (yes/no)
                }
                if (Input.GetKeyDown(KeyCode.Alpha2)) {
                    audioController.PlayAudio(AudioType.SFX_windowPull_01, false);
                }
                if (Input.GetKeyDown(KeyCode.Alpha3)) {
                    audioController.PlayAudio(AudioType.SFX_windowCrash, false);
                }

                //fire on/off
                if (Input.GetKeyDown(KeyCode.Alpha4)) {
                    audioController.PlayAudio(AudioType.SFX_fire, false);
                }
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    audioController.StopAudio(AudioType.SFX_fire, false);
                }

                //guns
                if (Input.GetKeyUp(KeyCode.Alpha5)) {
                    audioController.PlayAudio(AudioType.SFX_bulletShoot, false);
                }
                if (Input.GetKeyDown(KeyCode.Alpha6))
                {
                    audioController.PlayAudio(AudioType.SFX_bulletImpact_01, false);
                }
                if (Input.GetKeyDown(KeyCode.Alpha7)) {
                    audioController.PlayAudio(AudioType.SFX_missileShoot, false);
                }
                if (Input.GetKeyDown(KeyCode.Alpha8))
                {
                    audioController.PlayAudio(AudioType.SFX_missileImpact, false);
                }
                if (Input.GetKeyDown(KeyCode.Alpha9))
                {
                    audioController.PlayAudio(AudioType.SFX_missileExplosion, false);
                }
                if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                    audioController.PlayAudio(AudioType.SFX_tankTurret, false);
                }

                //Bird
                if (Input.GetKeyDown(KeyCode.T))
                {
                    audioController.PlayAudio(AudioType.SFX_birdGrab, false);
                }
                if (Input.GetKeyUp(KeyCode.T))
                {
                    audioController.PlayAudio(AudioType.SFX_birdCrash, false);
                }

                //Vehicle crashes
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    audioController.PlayAudio(AudioType.SFX_carCrash, false);
                }
                if (Input.GetKeyDown(KeyCode.U))
                {
                    audioController.PlayAudio(AudioType.SFX_airplaneCrash, false);
                }

                //UI
                if (Input.GetKeyDown(KeyCode.I))
                {
                    audioController.PlayAudio(AudioType.SFX_ui, false);
                }

                //Background loops
                if (Input.GetKeyDown(KeyCode.O))
                {
                    audioController.PlayAudio(AudioType.BCKGR_city, true);
                }
                if (Input.GetKeyDown(KeyCode.P))
                {
                    audioController.StopAudio(AudioType.BCKGR_city, true);
                }
                if (Input.GetKeyDown(KeyCode.G))
                {
                    audioController.PlayAudio(AudioType.BCKGR_beach, true);
                }
                if (Input.GetKeyDown(KeyCode.H))
                {
                    audioController.StopAudio(AudioType.BCKGR_beach, true);
                }

                //music
                if (Input.GetKeyDown(KeyCode.J))
                {
                    audioController.PlayAudio(AudioType.MUS_level, false);
                }
                if (Input.GetKeyDown(KeyCode.K))
                {
                    audioController.StopAudio(AudioType.MUS_level, true);
                }
                if (Input.GetKeyDown(KeyCode.L))
                {
                    audioController.PlayAudio(AudioType.MUS_stinger, false);
                }
            }
#endif
#endregion
        }
    }
}
