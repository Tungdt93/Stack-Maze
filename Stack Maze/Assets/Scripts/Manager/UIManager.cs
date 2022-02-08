using UnityEngine;
using TMPro;

public class UIManager : Manager<UIManager>
{
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private TextMeshProUGUI currentLevelText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private TextMeshProUGUI finishedLevelText;
    [SerializeField] private TextMeshProUGUI finishedScoreText;

    public TextMeshProUGUI CurrentLevelText { get => currentLevelText; set => currentLevelText = value; }
    public TextMeshProUGUI ScoreText { get => scoreText; set => scoreText = value; }
    public GameObject GameplayPanel { get => gameplayPanel; set => gameplayPanel = value; }
    public GameObject LevelCompletePanel { get => levelCompletePanel; set => levelCompletePanel = value; }
    public TextMeshProUGUI FinishedLevelText { get => finishedLevelText; set => finishedLevelText = value; }
    public TextMeshProUGUI FinishedScoreText { get => finishedScoreText; set => finishedScoreText = value; }

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    public override void ManagerTask()
    {

    }

    public void ShowGameplayPanel(bool isShow)
    {
        gameplayPanel.SetActive(isShow);
    }

    public void ShowLevelCompletePanel(bool isShow)
    {
        levelCompletePanel.SetActive(isShow);
    }

    public void NextLevel(bool loadNextLevel)
    {
        if (loadNextLevel)
        {
            ShowGameplayPanel(true);
            ShowLevelCompletePanel(false);
        }
    }
}
