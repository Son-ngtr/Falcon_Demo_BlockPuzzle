using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Lớp chứa các hàm xử lý màn hình menu
public class MenuButton : MonoBehaviour
{
    private void Awake()
    {
        if (Application.isEditor == false)
        {
            Debug.unityLogger.logEnabled = false;
        }
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
