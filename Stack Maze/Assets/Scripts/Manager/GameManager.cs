using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Manager<GameManager>
{
    public static int sceneIndex = 0;

    [SerializeField] private GameObject[] platforms;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private PlatformController[] platformControllers;
    [SerializeField] private PlatformController firstPlatform;

    private GameObject platformHolder;
    private GameObject player;
    private PlayerController playerController;
    private int score;
    private int currentLevel;
    private int numberOfPlatforms;
    private bool levelCompleted = false;

    public GameObject Player { get => player; set => player = value; }
    public GameObject[] Platforms { get => platforms; set => platforms = value; }
    public bool LevelCompleted { get => levelCompleted; set => levelCompleted = value; }

    public override void Init()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    public override void ManagerTask()
    {
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        score = 0;
        levelCompleted = false;
        platformHolder = GameObject.FindGameObjectWithTag("PlatformHolder");
        numberOfPlatforms = platformHolder.transform.childCount;
        platforms = new GameObject[numberOfPlatforms];

        for (int i = 0; i < numberOfPlatforms; i++)
        {
            platforms[i] = platformHolder.transform.GetChild(i).gameObject;
        }

        platformControllers = new PlatformController[numberOfPlatforms];
        for (int i = 0; i < numberOfPlatforms; i++)
        {
            platformControllers[i] = platforms[i].GetComponent<PlatformController>();
        }
        firstPlatform = platformControllers[0];

        //Vector3 firstStackPosition = platformControllers[0].FirstStack.transform.position;
        Vector3 firstStackPosition = firstPlatform.FirstStack.transform.position;
        Vector3 playerPosition = new Vector3(firstStackPosition.x,
            firstStackPosition.y + (platformControllers[0].FirstStack.transform.localScale.y / 2f),
            firstStackPosition.z);

        player = Instantiate(playerPrefab, playerPosition, Quaternion.identity);
        playerController = player.GetComponent<PlayerController>();
    }

    private void Awake()
    {
        Init();
        ManagerTask();
    }

    private void Update()
    {
        Gameplay();
    }

    private void Gameplay()
    {
        score = playerController.Score;
        UIManager.instance.ScoreText.text = score.ToString();
        UIManager.instance.CurrentLevelText.text = "LEVEL " + currentLevel .ToString();

        if (levelCompleted)
        {
            UIManager.instance.FinishedScoreText.text = score.ToString();
            UIManager.instance.FinishedLevelText.text = "LEVEL " + currentLevel.ToString();
            UIManager.instance.ShowGameplayPanel(false);
            UIManager.instance.ShowLevelCompletePanel(true);
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        sceneIndex++;
        if (sceneIndex > SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            PlayerPrefs.SetInt("CurrentLevel", currentLevel + 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        StartCoroutine(WaitToEndLevel());
    }

    IEnumerator WaitToEndLevel()
    {
        yield return new WaitForEndOfFrame();
        ManagerTask();
        UIManager.instance.NextLevel(true);
    }
}
