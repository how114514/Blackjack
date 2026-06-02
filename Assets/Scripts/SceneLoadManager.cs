using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    [SerializeField] private AssetReference menuScene;
    [SerializeField] private AssetReference gameScene;

    private AssetReference currentScene;

    private async Awaitable LoadSceneTask()
    {
        var handle = currentScene.LoadSceneAsync(LoadSceneMode.Additive);

        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            SceneManager.SetActiveScene(handle.Result.Scene);
        }
    }

    private async Awaitable UnloadSceneTask()
    {
        Scene activeScene = SceneManager.GetActiveScene();

        if (activeScene.isLoaded)
        {
            await Awaitable.FromAsyncOperation(SceneManager.UnloadSceneAsync(activeScene));
        }
    }

    public async void LoadGame()
    {
        await UnloadSceneTask();

        currentScene = gameScene;

        await LoadSceneTask();
    }

    public async void LoadMenu()
    {
        await UnloadSceneTask();

        currentScene = menuScene;

        await LoadSceneTask();
    }
}
