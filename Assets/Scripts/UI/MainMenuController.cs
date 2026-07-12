using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private CanvasGroup main;
    [SerializeField] private CanvasGroup settings;
    [SerializeField] private CanvasGroup credits;

    private List<CanvasGroup> screens = new();

    private Coroutine loadingScene = null;

    private void Start()
    {
        screens.Add(main);
        screens.Add(settings);
        screens.Add(credits);
    }

    public void SwitchScreen(int index)
    {
        switch (index)
        {
            case 0: 
                Invoke(nameof(ToggleMain), 1f);
                break;

            case 1:
                Invoke(nameof(ToggleSettings), 1f);
                break;
            case 2:
                Invoke(nameof(ToggleCredits), 1f);
                break;
            default:
                Debug.LogWarning("Out of range.");
                break;
        }
    }

    // ienumerator function load scene
    // load scene async (game scene)
    // wait sophia seconds
    // unload current scene async
    public void LoadGame()
    {
        if (loadingScene == null) 
            loadingScene = StartCoroutine(LoadSceneAfterAnimation());
    }
        

    private IEnumerator LoadSceneAfterAnimation()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadSceneAsync(1);
        yield return new WaitForSeconds(1f);
        SceneManager.UnloadSceneAsync(0);
        loadingScene = null;
    }

    private void ToggleMain()
    {
        ToggleAllOff();

        main.alpha = 1;
        main.interactable = true;
        main.blocksRaycasts = true;
    }

    private void ToggleSettings()
    {
        ToggleAllOff();

        settings.alpha = 1;
        settings.interactable = true;
        settings.blocksRaycasts = true;
    }

    private void ToggleCredits()
    {
        ToggleAllOff();

        credits.alpha = 1;
        credits.interactable = true;
        credits.blocksRaycasts = true;
    }

    private void ToggleAllOff()
    {
        foreach(var group in screens)
        {
            group.alpha = 0;
            group.interactable = false;
            group.blocksRaycasts = false;
        }

        foreach (var anims in main.GetComponentsInChildren<ResetAnimationImages>())
        {
            anims.SwitchBack();
        }
    }
}
