using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleLight : MonoBehaviour
{
    private readonly float intensityModifier = 2.0f;
    private float intensityRangeMinimum;
    private float intensityRangeMaximum;
    private float lightIntensity;
    private float seed;
    private Vector3 baseXYZ;
    

    private Light firePointLight;
    // Start is called before the first frame update

    private void Awake()
    {
        // find a point light
        firePointLight = gameObject.GetComponentInChildren<Light>();
        if (firePointLight != null)
        {
            // we have a point light, set the intensity to 0 so it can fade in nicely
            lightIntensity = firePointLight.intensity;
            intensityRangeMinimum = firePointLight.intensity;
            intensityRangeMaximum = intensityRangeMinimum + 0.5f;
            firePointLight.intensity = 0.0f;
            baseXYZ = firePointLight.gameObject.transform.position;
        }
        seed = Random.Range(1.0f, 3.0f);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (firePointLight == null)
        {
            return;
        }
        if (seed != 0)
        {
            bool setIntensity = true;
            float intensityModifier2 = 1.0f;

            if (setIntensity)
            {
                float intensity = Mathf.Clamp(intensityModifier * intensityModifier2 * Mathf.PerlinNoise(seed + Time.time, seed + 1 + Time.time),
                    intensityRangeMinimum, intensityRangeMinimum);
                firePointLight.intensity = intensity;
            }

            float x = baseXYZ.x + Mathf.PerlinNoise(seed + Time.time, seed + 0.5f + Time.time);
            float y = baseXYZ.y + Mathf.PerlinNoise(seed + Time.time, seed + 0.5f + Time.time);
            float z = baseXYZ.z + Mathf.PerlinNoise(seed + Time.time, seed + 0.5f + Time.time);
            firePointLight.gameObject.transform.position = new Vector3(x, y, z);
        }
    }
}
