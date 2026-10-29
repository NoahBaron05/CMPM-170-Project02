using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisasterManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static DisasterManager instance;
    public static DisasterManager Instance { get { return instance; } }

    [SerializeField] private Earthquake earthquake;
    [SerializeField] private HailDisaster hailDisaster;
    [SerializeField] private Button hailButton;
    [SerializeField] private TMP_Text hailButtonLabel;

    [SerializeField] private float duration;
    private bool isDisasterRunning;

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

    public void NaturalDisasterBegin()
    {
        if (isDisasterRunning) return;

        DisasterType disasterType = (DisasterType)Random.Range(0, 3);

        StartCoroutine(StartDisaster(disasterType));
    }

    public void StartHailDisaster()
    {
        if (isDisasterRunning) return;

        StartCoroutine(StartDisaster(DisasterType.Hail));
    }

    public IEnumerator StartDisaster(DisasterType disasterType)
    {
        if (isDisasterRunning) yield break;

        isDisasterRunning = true;
        SetHailButtonState(true, disasterType);

        switch (disasterType)
        {
            case DisasterType.Earthquake:
                Debug.Log("Earthquake disaster started!");
                earthquake.StartEarthquake();
                yield return new WaitForSeconds(duration);
                earthquake.StopEarthquake();

                break;
            case DisasterType.Wind:
                Debug.Log("Wind disaster started!");
                // Implement wind disaster logic here
                break;

            case DisasterType.Hail:
                Debug.Log("Hail disaster started!");
                if (hailDisaster == null)
                {
                    Debug.LogError("Hail disaster could not start because no HailDisaster component is assigned.");
                    isDisasterRunning = false;
                    SetHailButtonState(false, disasterType);
                    yield break;
                }

                hailDisaster.StartHail();
                yield return new WaitForSeconds(duration);
                hailDisaster.StopHail();
                break;
        }

        isDisasterRunning = false;
        SetHailButtonState(false, disasterType);
    }

    private void SetHailButtonState(bool isRunning, DisasterType disasterType)
    {
        if (hailButton != null)
        {
            hailButton.interactable = !isRunning;
        }

        if (hailButtonLabel != null)
        {
            hailButtonLabel.text = isRunning ? $"{disasterType} in progress..." : "Test Hail";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
