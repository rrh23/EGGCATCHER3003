using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LogicScript : MonoBehaviour
{
    public int points;
    private int basicEgg, healingEgg, mineEgg;
    
    public Text pointText;
    public TextMeshProUGUI basicPoints, healingPoints, minePoints;
    
    public HealthBar healthBar;

    public GameObject Canvas;
    public GameObject gameOverScreen;
    public GameObject pauseScreen;
    public GameObject winScreen;

    private AudioSource audioSource;
    
    public static LogicScript Instance;
    
    public VolumeMixers volumeMixers;
    
    public int currentHealth;
    public int maxHealth = 100;
    //public GameObject egg;
    //public GameObject catcher;
    public bool isGameOver = false, isBigWinner = false;
    private void Start()
    {
        //reference BGM
        GameObject audioObject = GameObject.Find("BGM");
        if (audioObject != null)
        {
            audioSource = audioObject.GetComponent<AudioSource>();
        }


        //reset points
        basicEgg = healingEgg = mineEgg = points = 0;

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(currentHealth);
        healthBar.SetHealth(currentHealth);

        //trigger every interval
        StartCoroutine(TriggerEveryInterval());
    }

    public bool isPaused = false;

    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause();
        }

        //DEBUGGING ; REMOVE ONCE FINISHED
        //if(Input.GetKeyDown(KeyCode.E))
        //{
        //    gameOver();
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    points += 100;
        //}

        if (isPaused)
        {
            // Pause the game
            Time.timeScale = 0f;
            isPaused = true;
            audioSource.Pause();
            pauseScreen.SetActive(true);
            //Debug.Log("Game Paused");
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                mainMenu();
            }

        }
        else if (!isPaused)
        {
            // Resume the game
            Time.timeScale = 1f;
            audioSource.UnPause();
            isPaused = false;
            pauseScreen.SetActive(false);
            //Debug.Log("Game Resumed");
        }

        if(points >= 300)
        {
            Time.timeScale = 0f;
            //isPaused = true;
            bigWinner();
            points = 0;
        }
    }

    public void CatchedEgg(EggType eggType)
    {
        // Handle logic based on the egg type
        switch (eggType)
        {
            case EggType.Basic:
                addPoints(1);
                heal(2);
                basicEgg += 1;
                volumeMixers.playSound("collect");
                break;

            case EggType.Healing:
                addPoints(1);
                heal(20);
                healingEgg += 1;
                volumeMixers.playSound("heal");
                break;

            case EggType.Mine:
                harm(30);
                subPoints(1);
                mineEgg += 1;
                volumeMixers.playSound("mine");
                break;
            case EggType.Void:
                harm(currentHealth);
                subPoints(points);
                volumeMixers.playSound("void");
                break;
        }
    }
    
    public void MissedEgg(EggType eggType)
    {
        switch (eggType)
        {
            case EggType.Basic:
                subPoints(1);
                harm(10);
                break;

            case EggType.Healing:
                subPoints(1);
                harm(10);
                break;

            case EggType.Mine:
                addPoints(1);
                heal(2);
                break;
            case EggType.Void:
                addPoints(1);
                heal(2);
                break;
        }

        if (currentHealth <= 0)
        {
            gameOver();
        }
    }

    public void addPoints(int point)
    {
        points += point;
        pointText.text = points.ToString();
    }

    public void subPoints(int point)
    {
        points -= point;
        if(points < 0)
        {
            points = 0;
            gameOver();
        }
        pointText.text = points.ToString();
    }

    public void heal(int heal)
    {
        if(currentHealth < 100)
        {
            currentHealth += heal;
            healthBar.SetHealth(currentHealth);
        }
        else currentHealth = 100;
    }

    public void harm(int heal)
    {
        currentHealth -= heal;
        healthBar.SetHealth(currentHealth);
    }

    public void pause()
    {
        isPaused = true;
    }
    public void unPause()
    {
        isPaused = false;
    }
    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void gameOver()
    {
        isGameOver = true;
        gameOverScreen.SetActive(true);
        //GameObject audioObject = GameObject.Find("BGM");

        
        //audioSource.Pause();
        volumeMixers.PlayEND();

    }
    
    public void bigWinner()
    {
        isBigWinner = true;
        volumeMixers.PlayWIN();

        basicPoints.text = basicEgg.ToString();
        healingPoints.text = healingEgg.ToString();
        minePoints.text = mineEgg.ToString();

        Canvas.SetActive(false);
        winScreen.SetActive(true);
    }
    public void mainMenu()
    {
        SceneManager.LoadScene(0);
    }

    //change gravity every [interval] seconds
    public float interval = 10f;
    public float gravityChange = 1f;
    IEnumerator TriggerEveryInterval()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            TriggerAction();
        }
    }

    void TriggerAction()
    {
        Debug.Log("Triggered at: " + Time.time);

        //rb.gravityScale += gravityChange;
        gravityChange += 1;
    }
}


