using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class BootstrapLoader : MonoBehaviour
{
    [Header("Addressable Scenes")]
    [SerializeField] private AssetReference bootstrapScene;
    [SerializeField] private AssetReference mainScene;

    private async void Start()
    {
        await LoadGame();
    }

    private async Task LoadGame()
    {
        // 1️⃣ 加载持久化场景
        if (bootstrapScene != null)
        {
            var bootstrapHandle =
                bootstrapScene.LoadSceneAsync(LoadSceneMode.Additive);

            await bootstrapHandle.Task;
        }

        // 2️⃣ 加载主场景
        if (mainScene != null)
        {
            var mainHandle =
                mainScene.LoadSceneAsync(LoadSceneMode.Additive);

            await mainHandle.Task;

            if (mainHandle.Status == AsyncOperationStatus.Succeeded)
            {
                // 设置主场景为 Active
                SceneManager.SetActiveScene(mainHandle.Result.Scene);
            }
        }

        // 3️⃣ 卸载自己
        Scene loaderScene = gameObject.scene;

        await Awaitable.FromAsyncOperation(
            SceneManager.UnloadSceneAsync(loaderScene)
        );
    }
}