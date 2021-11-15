using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーンを切り替える
/// </summary>
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance = new SceneLoader();

    /// <summary>
    /// 名前を指定したシーンに切り替える
    /// </summary>
    /// <param name="sceneName">切り替え先のシーン名</param>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
