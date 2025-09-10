using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    public LogicScript logic;
    public GameObject Egg, HealingEgg, MineEgg, VoidEgg;

    public float spawnRate;

    public float widthOffset = 30f;

    public float basicEggSpawnRate, healingEggSpawnRate, mineEggSpawnRate, voidEggSpawnRate;

    private float basicEggTimer, healingEggTimer, mineEggTimer, voidEggTimer;


    void Start()
    {
        //gets logic
        logic = GameObject.FindWithTag("Logic").GetComponent<LogicScript>();

        //spawns egg immediately
        spawnEgg(Egg);

        //initializes spawn rate
        basicEggSpawnRate = 1f;
        healingEggSpawnRate= 3f;
        mineEggSpawnRate = 2f;
        voidEggSpawnRate = 10f;


        //basicEggSpawnRate = spawnRate;
        //healingEggSpawnRate = spawnRate * 0.7f;
    }

    float SetSpawnRate(float baseRate, float timeFactor, float pointFactor, float minRate, float maxRate)
    {
        //SPAWNRATEFORMULA
        return Mathf.Clamp(baseRate - (timeFactor * Time.time) - (pointFactor * logic.points), minRate, maxRate);
    }

    void Update()
    {


        //SPAWN RATES (Change the numbers a bit for difference)
        //baseRate = Initial spawn rate
        //timeFactor = Spawn speed increase over time
        //pointFactor = Spawn speed increase with points
        //minRate = Minimum allowed spawn rate
        //maxRate = take a guess

        basicEggSpawnRate = SetSpawnRate(1f, 0.05f, 0.001f, 0.5f, 3f);

        if (!logic.isGameOver && !logic.isBigWinner)
        {
            basicEggTimer += Time.deltaTime;
            if (basicEggTimer >= basicEggSpawnRate)
            {
                spawnEgg(Egg);
                //randomizes shit idk
                //basicEggSpawnRate *= Random.Range(0.5f, 5f);
                basicEggTimer = 0f;
            }

            //spawns mine eggs at 100 points
            if (logic.points >= 100)
            {

                mineEggSpawnRate = SetSpawnRate(2f, 0.01f, 0.001f, 1f, 3f);

                mineEggTimer += Time.deltaTime;
                if(mineEggTimer >= mineEggSpawnRate)
                {
                    spawnEgg(MineEgg);
                    mineEggTimer = 0f;
                }
            }

            //spawns healing egg if >200 points
            if (logic.points >= 200)
            {
                //reset spawn rate
                basicEggSpawnRate = SetSpawnRate(1f, 0.05f, 0.001f, 0.5f, 3f);
                mineEggSpawnRate = SetSpawnRate(5f, 0.01f, 0.001f, 1f, 3f);

                healingEggSpawnRate = SetSpawnRate(3f, 0.01f, 0.001f, 0.5f, 3f);
                voidEggSpawnRate = SetSpawnRate(10f, 0.001f, 0.0001f, 0.1f, 10f);

                healingEggTimer += Time.deltaTime;
                voidEggTimer += Time.deltaTime;
                

                if (healingEggTimer >= healingEggSpawnRate)
                {
                    spawnEgg(HealingEgg);
                    healingEggTimer = 0f;
                }
                if (voidEggTimer >= voidEggSpawnRate)
                {
                    spawnEgg(VoidEgg);
                    voidEggTimer = 0f;
                }
            }
        }

        

    }
    void spawnEgg(GameObject EggType)
    {
        float lowestpos = transform.position.x - widthOffset;
        float highestpos = transform.position.x + widthOffset;
        Instantiate(EggType, new Vector3(Random.Range(lowestpos, highestpos), transform.position.y, 0), transform.rotation);
        //Debug.Log("EGG.");
    }

}

