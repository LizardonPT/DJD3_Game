using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Class that interact with the buttons pressed by the player
/// in the setting menu.
/// </summary>
public class Settings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown = default;
    private Resolution[] resolutions;
    private int currentresolutionIndex;

    /// <summary>
    /// Private method called before the first frame.
    /// </summary>
    private void Start()
    {
        // Adds all the resolutions of the existing ones to the dropdown.
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        currentresolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height
                + " Hz " + +resolutions[i].refreshRate;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentresolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentresolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    /// <summary>
    /// Private method which sets the resolution chosen by the player.
    /// </summary>
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width,
            resolution.height,
            Screen.fullScreen);
    }

    /// <summary>
    /// Private method which sets the visual quality chosen by the player.
    /// </summary>
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    /// <summary>
    /// Private method which sets the game in fullscreen.
    /// </summary>
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}
