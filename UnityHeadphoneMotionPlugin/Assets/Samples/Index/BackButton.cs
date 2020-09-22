using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Button))]
public class SceneBackButton : MonoBehaviour
{
    [SerializeField] GameObject hideOnLoad = null;

    private Scene activeScene;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

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
        SceneManager.UnloadSceneAsync(activeScene);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"OnSceneLoaded: {scene.name} index: {scene.buildIndex} mode: {mode}");
        if (scene.buildIndex == 0) return;

        activeScene = scene;
        if (hideOnLoad != null)
        {
            hideOnLoad.SetActive(false);
        }
        gameObject.SetActive(true);
    }

    private void OnSceneUnloaded(Scene scene)
    {
        if (hideOnLoad != null)
        {
            hideOnLoad.SetActive(true);
        }
        gameObject.SetActive(false);
    }

}
