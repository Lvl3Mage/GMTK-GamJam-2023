using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    int LoadingLevelIndex;
    public Animator Animator;
    public void FadeToScene(int levelindex){
        Animator.SetTrigger("Change_Scene");
        LoadingLevelIndex = levelindex;
    }
    void OnFadeComplete(){
        SlowMotion.ClearSubscribers();
        SceneManager.LoadScene(LoadingLevelIndex);
    }
}
