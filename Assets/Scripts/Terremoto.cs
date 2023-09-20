using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terremoto : MonoBehaviour
{
    public float earthquakeDuration = 10f; // Duracion
    public float earthquakeInterval = 30f; // Intervalo
    public float earthquakeForce = 10f; // Daño a los muros

    private bool isEarthquakeActive = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator TriggerEarthquake()
    {
        while (true)
        {
            yield return new WaitForSeconds(earthquakeInterval);

            StartEarthquake();
            yield return new WaitForSeconds(earthquakeDuration);
            EndEarthquake();
        }
    }

    private void StartEarthquake()
    {
        isEarthquakeActive = true;
        // Implement earthquake visual effects and audio here

        // You can also shake the camera or add screen effects to simulate the earthquake
        // CameraShake.Shake(duration, magnitude);

        // Apply earthquake force to walls (You might need a separate wall manager script)
        WallManager.Instance.ApplyEarthquakeForce(earthquakeForce);
    }

    private void EndEarthquake()
    {
        isEarthquakeActive = false;
        // Implement earthquake ending effects and audio here

        // Stop camera shaking and screen effects
        // CameraShake.StopShake();

        // Reset or stop the earthquake force on walls
        WallManager.Instance.ResetEarthquakeForce();
    }
}
