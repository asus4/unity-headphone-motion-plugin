using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Button))]
public class SceneLoadButton : MonoBehaviour
{
    public string sceneName = "";
    public LoadSceneMode mode = LoadSceneMode.Single;

    private void OnEnable()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        var button = GetComponent<Button>();
        button.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        SceneManager.LoadScene(sceneName, mode);
    }
}
