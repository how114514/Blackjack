using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    [SerializeField] private AssetReference menuScene;
    [SerializeField] private AssetReference gameScene;

    [SerializeField] private SceneFader sceneFader;

    private async Awaitable LoadSceneTask(AssetReference sceneReference)
    {
        var handle = sceneReference.LoadSceneAsync(LoadSceneMode.Additive);

        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
            SceneManager.SetActiveScene(handle.Result.Scene);
    }

    private async Awaitable UnloadSceneTask()
    {
        Scene activeScene = SceneManager.GetActiveScene();

        if (activeScene.isLoaded)
            await Awaitable.FromAsyncOperation(SceneManager.UnloadSceneAsync(activeScene));
    }

    public async void LoadGame()
    {
        await sceneFader.FadeIn();

        await UnloadSceneTask();

        await LoadSceneTask(gameScene);

        await sceneFader.FadeOut();
    }

    public async void LoadMenu()
    {
        await sceneFader.FadeIn();

        await UnloadSceneTask();

        await LoadSceneTask(menuScene);

        await sceneFader.FadeOut();
    }
}