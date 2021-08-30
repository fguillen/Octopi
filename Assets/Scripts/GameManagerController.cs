using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerController : MonoBehaviour
{
    [SerializeField] int maxNumPeople = 100;
    [SerializeField] int maxNumCars = 40;
    [SerializeField] int maxNumHelicopters = 5;
    [SerializeField] int maxNumTanks = 10;
    [SerializeField] int maxNumSoldiers = 20;

    int actualNumPeople;
    int actualNumCars;
    int actualNumHelicopters;
    int actualNumTanks;
    int actualNumSoldiers;

    public static GameManagerController instance;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        #if UNITY_EDITOR
            ChangeVelocity();
        #endif
    }

    void ChangeVelocity()
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
    }

    public void IncreasePeople()
    {
        actualNumPeople ++;
    }

    public void DecreasePeople()
    {
        actualNumPeople --;
    }

    public bool CanMorePeople()
    {
        return maxNumPeople > actualNumPeople;
    }


    public void IncreaseCars()
    {
        actualNumCars ++;
    }

    public void DecreaseCars()
    {
        actualNumCars --;
    }

    public bool CanMoreCars()
    {
        return maxNumCars > actualNumCars;
    }


    public void IncreaseHelicopters()
    {
        actualNumHelicopters ++;
    }

    public void DecreaseHelicopters()
    {
        actualNumHelicopters --;
    }

    public bool CanMoreHelicopters()
    {
        return maxNumHelicopters > actualNumHelicopters;
    }


    public void IncreaseTanks()
    {
        actualNumTanks ++;
    }

    public void DecreaseTanks()
    {
        actualNumTanks --;
    }

    public bool CanMoreTanks()
    {
        return maxNumTanks > actualNumTanks;
    }


    public void IncreaseSoldiers()
    {
        actualNumSoldiers ++;
    }

    public void DecreaseSoldiers()
    {
        actualNumSoldiers --;
    }

    public bool CanMoreSoldiers()
    {
        return maxNumSoldiers > actualNumSoldiers;
    }
}
