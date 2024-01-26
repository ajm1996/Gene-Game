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
    public float transitionDuration = 4.0f; // Duration of the light transition in seconds
    public float dayFinalIntensity = 0.85f; // Final intenisty value for daytime for the global light
    public float nightFinalIntensity = 0.1f; // Final intenisty value for nighttime for the global light
    public bool isDaytime = true;

    private AudioManager audioManager;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (globalLight == null)
        {
            Debug.LogError("Global light not assigned in DayNightCycleManager.");
            return;
        }
        // Initialize global light to day 
        globalLight.intensity = dayFinalIntensity; 
    }

    public void SetDay() {
        isDaytime = true;
        StartCoroutine(TransitionLight(isDaytime));
    }

    public void SetNight() {
        isDaytime = false;
        StartCoroutine(TransitionLight(isDaytime));
    }

    IEnumerator TransitionLight(bool isDay)
    {
        float targetIntensity = isDay ? dayFinalIntensity : nightFinalIntensity;
        float startIntensity = globalLight.intensity;
        float time = 0;

        bool soundPlayed = false;

        while (time < transitionDuration)
        {
            globalLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, time / transitionDuration);
            time += Time.deltaTime;
            if(!soundPlayed) {
                if (isDay) {
                    if (globalLight.intensity > startIntensity + 0.3f * (targetIntensity - startIntensity)) {
                        audioManager.Play("Morning");
                        soundPlayed = true;
                    }
                } else {
                    if (globalLight.intensity < startIntensity - 0.4f * (startIntensity - targetIntensity)) { 
                        audioManager.Play("Night");
                        soundPlayed = true;
                    }
                }
            }
            
            yield return null; // Wait until next frame
        }

        globalLight.intensity = targetIntensity; // Ensure the final intensity is set
    }
}