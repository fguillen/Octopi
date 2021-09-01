
using UnityEngine;

namespace UnityCore
{

    namespace Audio
    {

        public class TestAudio : MonoBehaviour
        {

            public AudioController audioController;

            #region Unity Functions
#if UNITY_EDITOR
            private void Update()
            {

                //Grab
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    audioController.PlayAudio(AudioType.SFX_grabObject, false); //bool is for fades (yes/no)
                }

                //window pull with randomization of clip
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    int windowPullClip = Random.Range(1, 9);
                    switch (windowPullClip)
                    {
                        case 1:
                            audioController.PlayAudio(AudioType.SFX_windowPull_01, false);
                            break;
                        case 2:
                            audioController.PlayAudio(AudioType.SFX_windowPull_02, false);
                            break;
                        case 3:
                            audioController.PlayAudio(AudioType.SFX_windowPull_03, false);
                            break;
                        case 4:
                            audioController.PlayAudio(AudioType.SFX_windowPull_04, false);
                            break;
                        case 5:
                            audioController.PlayAudio(AudioType.SFX_windowPull_05, false);
                            break;
                        case 6:
                            audioController.PlayAudio(AudioType.SFX_windowPull_06, false);
                            break;
                        case 7:
                            audioController.PlayAudio(AudioType.SFX_windowPull_07, false);
                            break;
                        case 8:
                            audioController.PlayAudio(AudioType.SFX_windowPull_08, false);
                            break;
                    }
                }

                //window crashing in floor, with randomization of clip
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    int windowCrashClip = Random.Range(1, 4);
                    switch (windowCrashClip)
                    {
                        case 1:
                            audioController.PlayAudio(AudioType.SFX_windowCrash_01, false);
                            break;
                        case 2:
                            audioController.PlayAudio(AudioType.SFX_windowCrash_02, false);
                            break;
                        case 3:
                            audioController.PlayAudio(AudioType.SFX_windowCrash_03, false);
                            break;
                    }
                }

                //fire on/off - for testing key down (keep pressed) for playback, key up (release) for stoping (it has stop fade active)
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    audioController.PlayAudio(AudioType.SFX_fire, false);
                }
                if (Input.GetKeyUp(KeyCode.Alpha4))
                {
                    audioController.StopAudio(AudioType.SFX_fire, true);
                }

                //bullet shot
                if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    audioController.PlayAudio(AudioType.SFX_bulletShoot, false);
                }

                //bullet impact, with clip randomization
                if (Input.GetKeyDown(KeyCode.Alpha6))
                {
                    int bulletImpactClip = Random.Range(1, 6);
                    switch (bulletImpactClip)
                    {
                        case 1:
                            audioController.PlayAudio(AudioType.SFX_bulletImpact_01, false);
                            break;
                        case 2:
                            audioController.PlayAudio(AudioType.SFX_bulletImpact_02, false);
                            break;
                        case 3:
                            audioController.PlayAudio(AudioType.SFX_bulletImpact_03, false);
                            break;
                        case 4:
                            audioController.PlayAudio(AudioType.SFX_bulletImpact_04, false);
                            break;
                        case 5:
                            audioController.PlayAudio(AudioType.SFX_bulletImpact_05, false);
                            break;
                    }
                }

                //missile shot
                if (Input.GetKeyDown(KeyCode.Alpha7))
                {
                    audioController.PlayAudio(AudioType.SFX_missileShoot, false);
                }

                //missile impact into player
                if (Input.GetKeyDown(KeyCode.Alpha8))
                {
                    audioController.PlayAudio(AudioType.SFX_missileImpact, false);
                }

                //missile explosion (not on player)
                if (Input.GetKeyDown(KeyCode.Alpha9))
                {
                    audioController.PlayAudio(AudioType.SFX_missileExplosion, false);
                }

                //tank turret rotating click (should be called many times while the animation occurs to have effect
                if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                    audioController.PlayAudio(AudioType.SFX_tankTurret, false);
                }

                //Bird grab and bird crash. For testing, keydown for grabbing, key up for crashing
                if (Input.GetKeyDown(KeyCode.T)) 
                {
                    audioController.PlayAudio(AudioType.SFX_birdGrab, false);
                }

                if (Input.GetKeyUp(KeyCode.T))
                {
                    audioController.PlayAudio(AudioType.SFX_birdCrash, false);
                }

                //Vehicle crashes with clip randomization
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    int carCrashClipClip = Random.Range(1, 4);
                    switch (carCrashClipClip)
                    {
                        case 1:
                            audioController.PlayAudio(AudioType.SFX_carCrash_01, false);
                            break;
                        case 2:
                            audioController.PlayAudio(AudioType.SFX_carCrash_02, false);
                            break;
                        case 3:
                            audioController.PlayAudio(AudioType.SFX_carCrash_03, false);
                            break;
                    }
                }

                //airplane crash
                if (Input.GetKeyDown(KeyCode.U))
                {
                    audioController.PlayAudio(AudioType.SFX_airplaneCrash, false);
                }

                //UI button
                if (Input.GetKeyDown(KeyCode.I))
                {
                    audioController.PlayAudio(AudioType.SFX_ui, false);
                }

                //city background loop
                if (Input.GetKeyDown(KeyCode.G)) //for starting
                {
                    audioController.PlayAudio(AudioType.BCKGR_city, true);
                }
                if (Input.GetKeyDown(KeyCode.H)) //for stoping (with fade set to TRUE)
                {
                    audioController.StopAudio(AudioType.BCKGR_city, true);
                }

                //beach background loop
                if (Input.GetKeyDown(KeyCode.J)) //for starting
                {
                    audioController.PlayAudio(AudioType.BCKGR_beach, true);
                }
                if (Input.GetKeyDown(KeyCode.K)) //for stoping (with fade set to TRUE)
                {
                    audioController.StopAudio(AudioType.BCKGR_beach, true);
                }

                //militar march. should be called when the army appears
                if (Input.GetKeyDown(KeyCode.L)) //for starting
                {
                    audioController.PlayAudio(AudioType.MUS_militaryMarch, false);
                }
                if (Input.GetKeyDown(KeyCode.V)) //for stopping (with fade set to TRUE)
                {
                    audioController.StopAudio(AudioType.MUS_militaryMarch, true);
                }

                //mus win. one shot to be called when Octopi finds his friend again
                if (Input.GetKeyDown(KeyCode.B))
                {
                    audioController.PlayAudio(AudioType.MUS_win, false);
                }
            }
#endif
                #endregion
            }
        }
    }

