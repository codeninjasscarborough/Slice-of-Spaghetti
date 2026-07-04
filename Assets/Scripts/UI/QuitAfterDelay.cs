using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitAfterDelay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(QuitGame), 2.5f);
    }

    // This will quit the game and close the tab
    void QuitGame()
    {
#if UNITY_EDITOR
       UnityEditor.EditorApplication.isPlaying = false;
#else 
        Application.Quit();
#endif
    }
}
