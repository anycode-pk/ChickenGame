using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class StartLevel : MonoBehaviour
{
    public string levelName;
    public VideoPlayer videoPlayer;
    public void LoadLevel()
    {
        SceneManager.LoadScene(levelName);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("QUIT");
    }
    
    public void StartGame()
    {
        videoPlayer.Play();
        videoPlayer.loopPointReached += VideoFinished;
    }
    
    public void VideoFinished(VideoPlayer vp)
    {
        LoadLevel();
    }
}
