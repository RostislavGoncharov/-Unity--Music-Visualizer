using Crosstales.FB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    [SerializeField] AudioBase audioBase;
    [SerializeField] Canvas menu;

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

    public void HideMenu()
    {
        menu.gameObject.SetActive(false);
    }
}
