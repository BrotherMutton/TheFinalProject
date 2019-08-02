using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject[] hazards;

    public GameObject background;
    public GameObject starfield1;
    public GameObject starfield2;
    private ParticleSystem ps1;
    private ParticleSystem ps2;
    private BGScroller BGScroller;
    
    private AudioSource audioSource;
    public AudioClip winMusic;
    public AudioClip loseMusic;

    public float warpSpeed;
    public float starfieldSpeed;

    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    public float hardModeWait;
    public int winScore;

    public Text scoreText;
    public Text restartText;
    public Text gameOverText;
    public Text devText;
    public Text hardModeText;

    private bool hardMode;
    private bool gameOver;
    private bool restart;
    private bool gameWon;
    private int Score;

    void Start()
    {
        gameOver = false;
        restart = false;
        hardMode = false;
        restartText.text = "";
        gameOverText.text = "";
        devText.text = "";
        hardModeText.text = "H for Hard Mode: DISABLED";
        Score = 0;
        UpdateScore();
        StartCoroutine (SpawnWaves());
        BGScroller = background.GetComponent<BGScroller>();
        ps1 = starfield1.GetComponent<ParticleSystem>();
        ps2 = starfield2.GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            hardMode = !hardMode;
            if (hardMode)
            {
            hardModeText.text = "H for Hard Mode: ENABLED";
            }
            else
            {
            hardModeText.text = "H for Hard Mode: DISABLED";
            }
        }
        if (restart)
        {
            if(Input.GetKeyDown(KeyCode.Backspace))
            {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        if (gameWon)
        {
            float recoveryRate = 0.5f;
            BGScroller.scrollSpeed = Mathf.MoveTowards(BGScroller.scrollSpeed, warpSpeed, recoveryRate * Time.deltaTime);
            float starRate = 1.5f;
            var main1 = ps1.main;
            main1.simulationSpeed = Mathf.MoveTowards(main1.simulationSpeed, starfieldSpeed, starRate * Time.deltaTime);
            var main2 = ps2.main;
            main2.simulationSpeed = Mathf.MoveTowards(main2.simulationSpeed, starfieldSpeed, starRate * Time.deltaTime);
        }
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)         
        {
            for(int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x),spawnValues.y,spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate (hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            if (hardMode)
            {
            yield return new WaitForSeconds(hardModeWait);    
            }
            else
            {
            yield return new WaitForSeconds(waveWait);
            }

            if(gameOver)
            {
                restartText.text = "Press 'Backspace' for Restart";
                restart = true;
                break;
            }
        }
    }

    public void AddScore (int newScoreValue)
    {
        Score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Points: " + Score;
        if(Score >= winScore && gameOver == false)
        {
            gameWon = true;
            GameOver();
        }
    }

    public void GameOver()
    {
        gameOverText.text = "Game over!";
        gameOver = true;
        audioSource.clip = loseMusic;
        audioSource.Play();
        audioSource.loop = false;
        if (gameWon == true)
        {
            gameOverText.text = "You've won!";
            devText.text = "Game created by Matthew Wisner";
            audioSource.clip = winMusic;
            audioSource.Play();
            audioSource.loop = false;
        }
    }
}
