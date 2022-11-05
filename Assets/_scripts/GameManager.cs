/*
 * This class handles all user interactions with the scene:
 * startMenus, file loading, etc.
 */

using Crosstales.FB;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public delegate void StartPlaying();
    public static event StartPlaying OnStartPlaying;

    [SerializeField] AudioBase audioBase;
    [SerializeField] Canvas startMenu;
    [SerializeField] Canvas pauseMenu;
    [SerializeField] AudioClip defaultAudioClip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        else
        {
            Instance = this;
        }
    }

    // Open a wav file using File Browser Pro and assign it to the audio clip.
    async public void ChooseAudioClip()
    {
        string path = FileBrowser.Instance.OpenSingleFile("wav");
        Debug.Log(path);

        if (path == "")
        {
            return;
        }

        var newClip = await LoadClip(path);
        audioBase.audioSource.clip = newClip;
    }

    // Get wav file from drive using the path returned by File Browser Pro.
    async Task<AudioClip> LoadClip(string path)
    {
        AudioClip clip = null;
        using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.WAV))
        {
            uwr.SendWebRequest();

            try
            {
                while (!uwr.isDone) await Task.Delay(5);

                if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.Log($"{uwr.error}");
                    // Set audio clip to default if there's been a loading error.
                    clip = defaultAudioClip;
                }

                else
                {
                    clip = DownloadHandlerAudioClip.GetContent(uwr);
                }
            }

            catch (Exception err)
            {
                Debug.Log($"{err.Message}, {err.StackTrace}");
            }
        }

        return clip;
    }

    public void Launch()
    {
        audioBase.audioSource.Play();
        OnStartPlaying?.Invoke();
        AudioBase.isActive = true;

        startMenu.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
    }

    public void TogglePauseMenu()
    {
        if (!AudioBase.isActive)
        {
            return;
        }

        pauseMenu.gameObject.SetActive(!pauseMenu.isActiveAndEnabled);
    }

    // Reset the scene and pull up the main menu
    public void ResetScene()
    {
        audioBase.audioSource.Stop();
        audioBase.audioSource.clip = defaultAudioClip;
        startMenu.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }
}
