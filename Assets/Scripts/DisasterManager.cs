using System.Collections;
using UnityEngine;

public class DisasterManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static DisasterManager instance;
    public static DisasterManager Instance { get { return instance; } }

    [SerializeField] private Earthquake earthquake;

    [SerializeField] private float duration;
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
        DisasterType disasterType = (DisasterType)Random.Range(0, 3);

        StartCoroutine(StartDisaster(disasterType));
    }

    public IEnumerator StartDisaster(DisasterType disasterType)
    {
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
                // Implement hail disaster logic here
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
