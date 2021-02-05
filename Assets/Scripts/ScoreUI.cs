using UnityEngine;
using System.Collections;

public class ScoreUI : MonoBehaviour {

	public int scorePlayer;
	public int scorePlayerAI;
    public GUIStyle style;
    public bool showStartPrompt;
    private bool gameEnd = false;
    private bool gameWin = false;
    private int defaultFontSize;
    public Color textColor;
    public Color blueColor;
    public Color vsColor;
    public Color redColor;
    public Texture2D textBackground;
    private Texture2D emptyBackground = null;

    public int winningScore = 5;

    private void Start()
    {
        defaultFontSize = style.fontSize;
    }

    void OnGUI()
	{
        // calculate the top screen center of the screen
        int scale = Screen.height;
		float x = Screen.width/2f;
		float y = 30f;
		float width = 300f;
		float height = 20f;
        string textPlayer = scorePlayer.ToString();
        string textVS = "vs";
        string textPlayerAI = scorePlayerAI.ToString();
        // draw the label at the top center of the screen
        y = Screen.height / 15.0f;
        style.normal.background = textBackground;
        style.normal.textColor = vsColor;
        style.fontSize = defaultFontSize * scale / 400;
        GUI.Label(new Rect(x - (width / 2f), y, width, height), textVS, style);
        style.normal.background = emptyBackground;
        style.normal.textColor = blueColor;
        style.fontSize = defaultFontSize * scale / 300;
        GUI.Label(new Rect(x - (width / 2f) - (width * scale / 4000f), y, width, height), textPlayer, style);
        style.normal.textColor = redColor;
        GUI.Label(new Rect(x - (width / 2f) + (width * scale / 4000f), y, width, height), textPlayerAI, style);
        style.normal.background = textBackground;
        style.normal.textColor = textColor;

        gameEnd = false;
        if (scorePlayer >= winningScore || scorePlayerAI >= winningScore) //someone wins 
		{
            gameEnd = true;
			// disable ball
			GameObject ball = GameObject.Find("Ball");
			if (ball != null)
			{
				ball.SetActive(false);
			}

			// create winning message
			string winMessage = "You Win";
            gameWin = true;
			if (scorePlayerAI >= winningScore)
			{
                gameWin = false;
				winMessage = "Game Over";
            }
            string replayMessage = "Press [SPACE] to play again";

            // show winning message at the center of the screen
            y = Screen.height / 3.5f;
            GUI.Label(new Rect(x - (width / 2f), y + (height / 2f), width, height), winMessage, style);
            y = Screen.height / 1.75f;
            style.fontSize = defaultFontSize * scale / 350;
            GUI.Label(new Rect(x - (width / 2f), y + 1.8f * height, width, height), replayMessage, style);
            //Application.Quit();
		}
        else
        {
            // Show starting prompt
            if (this.showStartPrompt)
            {
                string startPrompt = "Press [SPACE] to start";
                y = Screen.height / 3.5f;
                GUI.Label(new Rect(x - (width / 2f), y + (height / 2f), width, height), startPrompt, style);
            }
        }
    }

    public bool IsGameEnd()
    {
        return gameEnd;
    }

    public bool IsWin()
    {
        return gameWin;
    }
}
