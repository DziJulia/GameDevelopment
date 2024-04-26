using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIScriptFinal : MonoBehaviour
{
    public GameManagerFinalProject gameManagerFinal;
    public GameObject instructionsPanel;
    public GameObject newGamePanel;
    public FinalPlayerControllerScript player;
    public GameObject gameOver;
    public GameObject panelPause;
    public TMP_Text scoreText;
    public TMP_Text healthText;
    public Button closeButton;
    public Button instructionsButton;
    private bool gameOverStatus;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<FinalPlayerControllerScript>();
        instructionsPanel.SetActive(false);
        newGamePanel.SetActive(false);
        panelPause.SetActive(false);
        gameOver.SetActive(false);
        gameManagerFinal = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerFinalProject>();
        gameOverStatus = true;
        // Get references to buttons and set up click listeners
        closeButton.onClick.AddListener(HideInstructions);
        instructionsButton.onClick.AddListener(ToggleInstructions);

        UpdateUI(); // Update UI text on start
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + gameManagerFinal.playScore;
        healthText.text = "Health: " + gameManagerFinal.playerHealth;
        
        if (gameManagerFinal.playerHealth == 0)
        {
            gameOverStatus = false;
            ToggleGameOver();
            player.canMove = false;
        }
        // Pause the game when "P" key is pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (gameOverStatus)
            {
                Debug.Log("P was Pressed ");
                TogglePause();
            }
        }
            
        // Restart the game when "R" key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (gameOverStatus)
            {
                if (panelPause.activeSelf)
                {
                    panelPause.SetActive(false);
                }

                Debug.Log("R was Pressed ");
                ToggleRestartGame();
            }
        }
    }
    
    // Method to update UI text
    void UpdateUI()
    {
        scoreText.text = "Score: " + gameManagerFinal.playScore;
        healthText.text = "Health: " + gameManagerFinal.playerHealth;
    }
    
    public void ShowInstructions()
    {
        instructionsPanel.SetActive(true);
        gameManagerFinal.isPaused = true;
    }

    public void HideInstructions()
    {
        instructionsPanel.SetActive(false);
        gameManagerFinal.isPaused = false;
    }

    // Method to toggle the instructions
    public void ToggleInstructions()
    {
        Debug.Log("ToggleInstructions() called");
        if (instructionsPanel.activeSelf)
        {
            HideInstructions();
        }
        else
        {
            ShowInstructions();
        }
    }
    
    // Method to toggle the pause
    public void TogglePause()
    {
        if (panelPause.activeSelf)
        {
            panelPause.SetActive(false);
        }
        else
        {
            panelPause.SetActive(true);
        }
    }
    
    // Method to toggle the restart
    public void ToggleRestartGame()
    {
        newGamePanel.SetActive(true);
        // This will call the DeactivateNewGamePanel() method after 3 seconds
        Invoke("DeactivateNewGamePanel", 2);
    }

    void DeactivateNewGamePanel()
    {
        newGamePanel.SetActive(false);
        gameManagerFinal.isPaused = false;
        player.canMove = true;
        gameManagerFinal.hasBeenPickedUp = false;
        gameManagerFinal.playScore = 0;
        gameManagerFinal.playerHealth = 5;
    }
    
    public void ToggleGameOver()
    {
        gameOver.SetActive(true);
        Invoke("RestartGame", 2);
        gameManagerFinal.isPaused = false;
        player.canMove = true;
        gameManagerFinal.hasBeenPickedUp = false;
        gameManagerFinal.playScore = 0;
        gameManagerFinal.difPower = false;
        gameManagerFinal.playerHealth = 3;
    }
    
    // Method to handle quitting the game
    public void QuitGame()
    {
        Application.Quit();
    }
    
    // Function to restart the game
    void RestartGame()
    {
        // Reload the currently active scene
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        gameOver.SetActive(false);
        gameOverStatus = true;
        gameManagerFinal.difPower = false;
        Time.timeScale = 1;
    }
}