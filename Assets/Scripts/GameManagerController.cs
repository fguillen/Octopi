using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityCore.Audio;

public class GameManagerController : MonoBehaviour
{
    [SerializeField] int maxNumPeople = 100;
    [SerializeField] int maxNumCars = 40;
    [SerializeField] int maxNumHelicopters = 5;
    [SerializeField] int maxNumTanks = 10;
    [SerializeField] int maxNumSoldiers = 20;

    [SerializeField] PlayableDirector endScene;

    List<PersonController> activePeople = new List<PersonController>();
    List<CarController> activeCars = new List<CarController>();
    List<HelicopterController> activeHelicopters = new List<HelicopterController>();
    List<TankController> activeTanks = new List<TankController>();
    List<SoldierController> activeSoldiers = new List<SoldierController>();

    bool armyActive = false;
    bool endGame = false;
    bool soundArmyPlaying = false;

    public bool cityActive = false;

    public static GameManagerController Instance;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        #if UNITY_EDITOR
            DebugKeys();
        #endif
    }

    void DebugKeys()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale += 0.5f;
            Debug.Log($"GameManager.timeScale: {Time.timeScale}");
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            float timeScale = Time.timeScale;
            timeScale -= 0.5f;
            if(timeScale <= 0)
                timeScale = 0.5f;

            Time.timeScale = timeScale;

            Debug.Log($"GameManager.timeScale: {Time.timeScale}");
        }

        if(Input.GetKeyDown(KeyCode.Z))
            ActivateArmy();

        if(Input.GetKeyDown(KeyCode.E))
            PlayEndScene();
    }

    public void ActivateArmy()
    {
        if(!endGame && !armyActive)
        {
            Debug.Log("ActiveArmy()");
            armyActive = true;
            Invoke("SoundArmyPlay", 20f);
        }
    }

    public void IncreasePeople(PersonController element)
    {

        activePeople.Add(element);
    }

    public void DecreasePeople(PersonController element)
    {
        activePeople.Remove(element);
    }

    public bool CanMorePeople()
    {
        return maxNumPeople > activePeople.Count();
    }


    public void IncreaseCars(CarController element)
    {
        activeCars.Add(element);
    }

    public void DecreaseCars(CarController element)
    {
        activeCars.Remove(element);
    }

    public bool CanMoreCars()
    {
        return maxNumCars > activeCars.Count();
    }


    public void IncreaseHelicopters(HelicopterController element)
    {
        activeHelicopters.Add(element);
    }

    public void DecreaseHelicopters(HelicopterController element)
    {
        activeHelicopters.Remove(element);
    }

    public bool CanMoreHelicopters()
    {
        return armyActive && maxNumHelicopters > activeHelicopters.Count();
    }


    public void IncreaseTanks(TankController element)
    {
        activeTanks.Add(element);
    }

    public void DecreaseTanks(TankController element)
    {
        activeTanks.Remove(element);
    }

    public bool CanMoreTanks()
    {
        return armyActive && maxNumTanks > activeTanks.Count();
    }


    public void IncreaseSoldiers(SoldierController element)
    {
        activeSoldiers.Add(element);
    }

    public void DecreaseSoldiers(SoldierController element)
    {
        activeSoldiers.Remove(element);
    }

    public bool CanMoreSoldiers()
    {
        return armyActive && maxNumSoldiers > activeSoldiers.Count();
    }

    public bool CanShoot()
    {
        return !endGame;
    }

    public bool EndGame()
    {
        return endGame;
    }

    [ContextMenu("PlayEndScene")]
    public void PlayEndScene()
    {
        Debug.Log("PlayEndScen()");

        StopCity();
        AudioController.instance.StopAudio(UnityCore.Audio.AudioType.MUS_militaryMarch, true);
        //AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.MUS_win, false);

        if(!endGame)
            endScene.Play();

        endGame = true;
        armyActive = false;

        SendArmyAway();
    }

    void SendArmyAway()
    {
        foreach (var element in activeSoldiers)
        {
            element.GoAwayFromPlayer();
        }

        foreach (var element in activeTanks)
        {
            element.GoAwayFromPlayer();
        }

        foreach (var element in activeHelicopters)
        {
            element.GoAwayFromPlayer();
        }
    }

    public void StartCity()
    {
        cityActive = true;
        PlayCityBackground();
    }

    public void StopCity()
    {
        cityActive = false;
        StopCityBackground();
    }

    void PlayCityBackground()
    {
        AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.BCKGR_city, true);
    }

    public void StopCityBackground()
    {
        AudioController.instance.StopAudio(UnityCore.Audio.AudioType.BCKGR_city, true);
    }

    public void SoundArmyPlay()
    {
        if(!soundArmyPlaying)
        {
            AudioController.instance.PlayAudio(UnityCore.Audio.AudioType.MUS_militaryMarch, false);
            soundArmyPlaying = true;
        }
    }
}
