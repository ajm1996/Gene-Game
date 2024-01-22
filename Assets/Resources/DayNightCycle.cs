/*
    This script will turn on and off the Global Light object with a small transition between the Day cycle and night cycle.
    For this script to work, attach it to the main camera and set the globalLight to the Global Light 2D game object
*/

using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class DayNightCycleManager : MonoBehaviour
{
    public Light2D globalLight; // Global Light 2D object in scene
    public float dayDuration = 30.0f; // Duration of day/night in seconds
    public float transitionDuration = 4.0f; // Duration of the light transition in seconds
    private float timer = 0.0f;
    private bool isDaytime = true;

    void Start()
    {
        if (globalLight == null)
        {
            Debug.LogError("Global light not assigned in DayNightCycleManager.");
            return;
        }
        StartCoroutine(TransitionLight(isDaytime));
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > dayDuration)
        {
            timer = 0.0f;
            isDaytime = !isDaytime;
            StartCoroutine(TransitionLight(isDaytime));
        }
    }

    IEnumerator TransitionLight(bool isDay)
    {
        float targetIntensity = isDay ? .85f : 0.1f;
        float startIntensity = globalLight.intensity;
        float time = 0;

        while (time < transitionDuration)
        {
            globalLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, time / transitionDuration);
            time += Time.deltaTime;
            yield return null; // Wait until next frame
        }

        globalLight.intensity = targetIntensity; // Ensure the final intensity is set
    }
}