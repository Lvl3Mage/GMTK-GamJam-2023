using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManagerShortcut : MonoBehaviour
{
    LevelManager LevelManager;
    // Start is called before the first frame update
    void Start()
    {
        LevelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
    }

    public void LoadLevel(int levelIndex){
        LevelManager.FadeToScene(levelIndex);
    }
    public void ReloadLevel(){
        LevelManager.FadeToScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadNextLevel(){
        LevelManager.FadeToScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
