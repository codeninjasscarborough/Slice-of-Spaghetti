using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSceneLoader : MonoBehaviour
{
    public string sceneName;

    public void PlayAnimation()
    {
        GetComponent<Animator>().SetTrigger("Click");
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
        
}
