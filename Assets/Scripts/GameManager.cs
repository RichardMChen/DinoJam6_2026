using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CharacterBase player1;
    public CharacterBase player2;
    public bool canFight = false;

    [Header("Pause")]
    public bool isGamePaused { get; set; } = false;
    public PauseMenu pauseMenu;

    [Header("Fight Intro")]
    public float introCountDown = 2.0f;
    [SerializeField] private float introTimer = 0f;
    [SerializeField] private float introTextWaitInterval = 1.0f;
    [SerializeField] private string[] introTextGroup;
    public bool introFinished { get; set; }
    public TMP_Text introText;

    [Header("Round End")]
    public TMP_Text roundEndText;
    public GameObject roundEndButtonGroup;
    //public Button rematchButton;
    //public Button mainMenuButton;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        roundEndText.gameObject.SetActive(false);
        roundEndButtonGroup.gameObject.SetActive(false);
        introText.gameObject.SetActive(true);
        introTimer = introCountDown;
        StartCoroutine(IntroTimerCo());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) //Pause game key
        {
            PauseGame();
        }

        if (player1.healthScript.currentHealth <= 0 || player2.healthScript.currentHealth <= 0)
        {
            if (player1.healthScript.currentHealth <= 0)
            {
                roundEndText.text = "You Lose";
            }
            else if (player2.healthScript.currentHealth <= 0)
            {
                roundEndText.text = "You Win!!!";
            }
            StartCoroutine(RoundEndCo());
        }
    }

    IEnumerator IntroTimerCo()
    {
        int introTextCounter = 0;
        while (introTextCounter < introTextGroup.Length)
        {
            if (introTextCounter <= introTextGroup.Length)
            {
                introText.text = introTextGroup[introTextCounter]; //Change the text displayed on the intro text
            }
            //introTimer -= 1.0f;
            introTextCounter++;
            yield return new WaitForSeconds(introTextWaitInterval);
        }

        introText.gameObject.SetActive(false);
        canFight = true;
        //yield return null;
    }

    public void PauseGame()
    {
        if (roundEndInProgress)
        {
            return;
        }
        if (!isGamePaused)
        {
            Time.timeScale = 0f;
            isGamePaused = true;
            pauseMenu.TogglePauseMenu();
            //Debug.Log("Game Paused");
        }
        else
        {
            Time.timeScale = 1f;
            isGamePaused = false;
            pauseMenu.TogglePauseMenu();
            //Debug.Log("Game Resume");
        }
    }

    private bool roundEndInProgress = false;
    IEnumerator RoundEndCo()
    {
        if (roundEndInProgress)
        {
            yield return null;
        }
        canFight = false;
        roundEndText.gameObject.SetActive(true);
        roundEndInProgress = true;

        yield return new WaitForSeconds(1.0f);
        roundEndButtonGroup.SetActive(true);
    }

    public void Rematch()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
