using System.Collections;
using UnityEngine;

public class DisasterManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static DisasterManager instance;
    public static DisasterManager Instance { get { return instance; } }

    [SerializeField] private Earthquake earthquake;
    [SerializeField] private WindDisaster windDisaster;
    [SerializeField] private float duration;

    private bool disasterActive = false;

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
    if (disasterActive) yield break;

    disasterActive = true;

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
            break;
    }

    // Reset all blocks
    ResetBlocks();

    disasterActive = false;

    Debug.Log("Disaster ended! Blocks reset.");
}


    private void ResetBlocks()
    {
    BlockMovement[] allBlocks =
        FindObjectsOfType<BlockMovement>();

    foreach (BlockMovement block in allBlocks)
    {
        block.ResetBlock();
    }

    Physics2D.SyncTransforms();

    Debug.Log("All blocks have been reset!");
    }   
    // Update is called once per frame
    void Update()
    {
        
    }
}
