using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisasterManager : MonoBehaviour
{
    public static DisasterManager instance;
    public static DisasterManager Instance { get { return instance; } }

    [SerializeField] private Earthquake earthquake;
    [SerializeField] private WindDisaster windDisaster;
    [SerializeField] private HailDisaster hailDisaster;
    [SerializeField] private Button disasterButton;

    [SerializeField] private float duration;
    private bool isDisasterRunning;
    private Button[] disasterButtons;
    private TMP_Text[] disasterLabels;
    private readonly string[] disasterNames = { "Earthquake", "Wind", "Hail" };

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        if (disasterButton == null)
        {
            Debug.LogError("Disaster UI could not start because no button is assigned.");
            return;
        }

        disasterButtons = new Button[3];
        disasterLabels = new TMP_Text[3];
        disasterButtons[0] = disasterButton;

        for (int i = 1; i < disasterButtons.Length; i++)
        {
            disasterButtons[i] = Instantiate(disasterButton, disasterButton.transform.parent, false);
        }

        for (int i = 0; i < disasterButtons.Length; i++)
        {
            Button button = disasterButtons[i];
            button.name = $"{disasterNames[i]} Button";
            RectTransform rect = button.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 1f);
            rect.anchoredPosition = new Vector2(20f, -20f - i * 45f);
            rect.sizeDelta = new Vector2(160f, 38f);

            TMP_Text label = button.GetComponentInChildren<TMP_Text>();
            if (label == null)
            {
                Debug.LogError("Disaster UI button is missing its TextMeshPro label.");
                return;
            }

            disasterLabels[i] = label;
            label.text = disasterNames[i];
            int buttonIndex = i;
            button.onClick.AddListener(() => StartSelectedDisaster((DisasterType)buttonIndex));
        }
    }

    private void StartSelectedDisaster(DisasterType disasterType)
    {
        if (!isDisasterRunning)
        {
            StartCoroutine(StartDisaster(disasterType));
        }
    }

    private IEnumerator StartDisaster(DisasterType disasterType)
    {
        if (!HasDisaster(disasterType)) yield break;

        isDisasterRunning = true;
        SetDisasterButtons(false, disasterType);

        switch (disasterType)
        {
            case DisasterType.Earthquake:
                Debug.Log("Earthquake disaster started!");
                earthquake.StartEarthquake();
                break;
            case DisasterType.Wind:
                Debug.Log("Wind disaster started!");
                windDisaster.StartWind();
                break;
            case DisasterType.Hail:
                Debug.Log("Hail disaster started!");
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
        SetDisasterButtons(true, disasterType);
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

    private void SetDisasterButtons(bool interactable, DisasterType disasterType)
    {
        for (int i = 0; i < disasterButtons.Length; i++)
        {
            disasterButtons[i].interactable = interactable;
            disasterLabels[i].text = interactable
                ? disasterNames[i]
                : i == (int)disasterType ? $"{disasterNames[i]}..." : disasterNames[i];
        }
    }
}
