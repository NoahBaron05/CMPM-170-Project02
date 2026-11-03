using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DisasterManager : MonoBehaviour
{
    public static DisasterManager instance;
    public static DisasterManager Instance { get { return instance; } }

    [SerializeField] private Earthquake earthquake;
    [SerializeField] private WindDisaster windDisaster;
    [SerializeField] private HailDisaster hailDisaster;

    [Header("Gameplay UI")]
    [SerializeField] private Button earthquakeButton;
    [SerializeField] private Button windButton;
    [SerializeField] private Button hailButton;
    [SerializeField] private GameObject gameplayPanel;

    [Header("Main Menu UI")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private Button playButton;
    [SerializeField] private Button rulesButton;

    [Header("Rules UI")]
    [SerializeField] private GameObject rulesPanel;
    [SerializeField] private Button backButton;

    [Header("Game End UI")]
    [SerializeField] private GameObject gameEndPanel;
    [SerializeField] private Button replayButton;

    [SerializeField] private float duration = 30f;

    private bool isDisasterRunning;
    private bool hasStarted;
    private Button[] disasterButtons;

    private void Awake()
    {
        Time.timeScale = 1f;
        instance = this;
    }

    private void Start()
    {
        if (!HasAllUIReferences())
        {
            Debug.LogError("Game UI references are missing. Assign the panels and buttons in the Inspector.");
            return;
        }

        disasterButtons = new[] { earthquakeButton, windButton, hailButton };
        earthquakeButton.onClick.AddListener(() => StartSelectedDisaster(DisasterType.Earthquake));
        windButton.onClick.AddListener(() => StartSelectedDisaster(DisasterType.Wind));
        hailButton.onClick.AddListener(() => StartSelectedDisaster(DisasterType.Hail));
        playButton.onClick.AddListener(StartGame);
        rulesButton.onClick.AddListener(ShowRules);
        backButton.onClick.AddListener(ShowMainMenu);
        replayButton.onClick.AddListener(RestartScene);

        SetDisasterButtons(false);
        ShowMainMenu();
    }

    private bool HasAllUIReferences()
    {
        return earthquakeButton != null && windButton != null && hailButton != null &&
               gameplayPanel != null && mainMenuPanel != null && playButton != null &&
               rulesButton != null && rulesPanel != null && backButton != null &&
               gameEndPanel != null && replayButton != null;
    }

    private void StartSelectedDisaster(DisasterType disasterType)
    {
        if (hasStarted && !isDisasterRunning)
        {
            StartCoroutine(StartDisaster(disasterType));
        }
    }

    private IEnumerator StartDisaster(DisasterType disasterType)
    {
        if (!HasDisaster(disasterType)) yield break;

        isDisasterRunning = true;
        SetDisasterButtons(false);

        switch (disasterType)
        {
            case DisasterType.Earthquake:
                earthquake.StartEarthquake();
                break;
            case DisasterType.Wind:
                windDisaster.StartWind();
                break;
            case DisasterType.Hail:
                hailDisaster.StartHail();
                break;
        }

        yield return new WaitForSeconds(duration);

        switch (disasterType)
        {
            case DisasterType.Earthquake:
                earthquake.StopEarthquake();
                break;
            case DisasterType.Wind:
                windDisaster.StopWind();
                break;
            case DisasterType.Hail:
                hailDisaster.StopHail();
                break;
        }

        isDisasterRunning = false;
        gameplayPanel.SetActive(false);
        SetBlockMovement(false);
        Time.timeScale = 0f;
        gameEndPanel.SetActive(true);
    }

    private bool HasDisaster(DisasterType disasterType)
    {
        bool isAvailable = disasterType switch
        {
            DisasterType.Earthquake => earthquake != null,
            DisasterType.Wind => windDisaster != null,
            DisasterType.Hail => hailDisaster != null,
            _ => false
        };

        if (!isAvailable)
        {
            Debug.LogError($"{disasterType} disaster is not assigned in the Inspector.");
        }

        return isAvailable;
    }

    private void SetDisasterButtons(bool interactable)
    {
        if (disasterButtons == null) return;

        foreach (Button button in disasterButtons)
        {
            button.interactable = interactable;
        }
    }

    private void ShowMainMenu()
    {
        hasStarted = false;
        Time.timeScale = 0f;
        SetBlockMovement(false);
        mainMenuPanel.SetActive(true);
        rulesPanel.SetActive(false);
        gameEndPanel.SetActive(false);
        gameplayPanel.SetActive(false);
    }

    private void ShowRules()
    {
        mainMenuPanel.SetActive(false);
        rulesPanel.SetActive(true);
    }

    private void StartGame()
    {
        hasStarted = true;
        Time.timeScale = 1f;
        SetBlockMovement(true);
        mainMenuPanel.SetActive(false);
        rulesPanel.SetActive(false);
        gameEndPanel.SetActive(false);
        gameplayPanel.SetActive(true);
        SetDisasterButtons(true);
    }

    private void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SetBlockMovement(bool enabled)
    {
        foreach (BlockMovement blockMovement in FindObjectsByType<BlockMovement>())
        {
            blockMovement.enabled = enabled;
        }
    }
}
