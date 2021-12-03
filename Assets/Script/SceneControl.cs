using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{


    public void loadAnimationScene()
    {
        SceneManager.LoadScene(0);
    }

    public void loadCollisionScene()
    {
        SceneManager.LoadScene(1);
    }

    public void loadTileMapScene()
    {
        SceneManager.LoadScene(2);
    }


    public void ExitApplication()
    {
        Application.Quit();
    }
}
