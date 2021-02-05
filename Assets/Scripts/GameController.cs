using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public ScoreUI scoreUI;
    public GameObject gameMain;
    public Animator title;
    public GameObject puck;
    public GameObject player;
    public GameObject playerAI;
    public AudioSource music;
    public AudioSource soundEffects;
    public AudioClip pongStartGame;
    public AudioClip pongStart;
    public AudioClip pongWinRed;
    public AudioClip pongWinBlue;
    public AudioClip pongGoalEffect;
    public AudioClip pongGameOverWin;
    public AudioClip pongGameOverLose;
    public ParticleSystem goalParticle;
    public float goalEmissionTime;
    private Vector3 puckStartPosition;
    private Vector3 playerStartPosition;
    private Vector3 playerAIStartPosition;
    private int gameState;
    private int gameRound;
    private bool playedEndSound;
    public float emissionTimer = 0.0f;

    // Use this for initialization
    void Start()
    {
        // Store initial game object positions and prompt start
        gameState = -1;
        gameRound = 0;
        music.enabled = true;
        playedEndSound = false;
        this.gameMain.SetActive(false);
        this.puckStartPosition = this.puck.transform.position;
        this.playerStartPosition = this.player.transform.position;
        this.playerAIStartPosition = this.playerAI.transform.position;
        scoreUI.showStartPrompt = true;
    }

    // Update is called once per frame
    void Update ()
    {
        if (gameState >= 0)
        {
            var emission = goalParticle.emission;
            emission.enabled = false;
            // Goal particle emission
            if (emissionTimer > 0.0f)
            {
                emission.enabled = true;
                emissionTimer -= Time.deltaTime;
            }

            // Game control
            if (gameState == 0 && !scoreUI.IsGameEnd())
            {
                scoreUI.showStartPrompt = true;
                bool space = Input.GetKey("space");
                if (space)
                {
                    gameState = 1;
                    puck.GetComponent<Puck>().SetStartVelocity();
                    scoreUI.showStartPrompt = false;
                    if (gameRound == 0)
                    {
                        soundEffects.PlayOneShot(pongStart);
                    }
                }
            }
            else if (!playedEndSound && scoreUI.IsGameEnd())
            {
                playedEndSound = true;
                if (scoreUI.IsWin())
                {
                    soundEffects.PlayOneShot(pongGameOverWin);
                }
                else
                {
                    soundEffects.PlayOneShot(pongGameOverLose);
                }
            }
            else if (scoreUI.IsGameEnd())
            {
                bool space = Input.GetKey("space");
                if (space)
                {
                    scoreUI.showStartPrompt = false;
                    gameRound = 0;
                    this.RestartGameState();
                    puck.GetComponent<Puck>().ResetVelocity();
                    puck.GetComponent<Puck>().SetStartVelocity();
                    scoreUI.scorePlayer = 0;
                    scoreUI.scorePlayerAI = 0;
                    if (gameRound == 0)
                    {
                        soundEffects.PlayOneShot(pongStart);
                    }
                }
            }
        }
    }

    public void RestartGameState()
    {
        // Reset game object positions and apply starting velocity to ball
        gameState = 1;
        this.playerAI.GetComponent<AI>().ResetDifficulty();
        this.puck.transform.position = this.puckStartPosition;
        this.player.transform.position = this.playerStartPosition;
        this.playerAI.transform.position = this.playerAIStartPosition;
    }

    public void ResetGameState()
    {
        // Reset game object positions and apply starting velocity to ball
        gameState = 0;
        gameRound += 1;
        this.playerAI.GetComponent<AI>().IncrementDifficulty();
        this.puck.transform.position = this.puckStartPosition;
        this.player.transform.position = this.playerStartPosition;
        this.playerAI.transform.position = this.playerAIStartPosition;
    }

    public void IncrementPlayerScore()
    {
        this.emissionTimer = goalEmissionTime;
        scoreUI.scorePlayer += 1;
        soundEffects.PlayOneShot(pongGoalEffect);
        soundEffects.PlayOneShot(pongWinBlue);
    }

    public void IncrementAIScore()
    {
        this.emissionTimer = goalEmissionTime;
        scoreUI.scorePlayerAI += 1;
        soundEffects.PlayOneShot(pongGoalEffect);
        soundEffects.PlayOneShot(pongWinRed);
    }

    public void StartGame()
    {
        this.gameState = -2;
        soundEffects.PlayOneShot(pongStartGame);
        title.SetBool("Fade", true);
        StartCoroutine(StartTransition());
    }

    IEnumerator StartTransition()
    {
        yield return new WaitForSeconds(2);
        this.gameMain.SetActive(true);
        this.gameState = 0;
    }
}
