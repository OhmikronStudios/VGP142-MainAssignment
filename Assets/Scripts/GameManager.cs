using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager _instance = null;

    //private GameObject pauseMenu;
    //[SerializeField] int scoreText;
    public static GameManager Instance
    { get { return _instance; } }

    private void Awake()
    {
        if (_instance)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        Debug.Log("quit application");
    #if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
    #else
             Application.Quit();
    #endif
    }
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
        Debug.Log("click");

    }

    public void LoadGameOver()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadWinScreen()
    {
        SceneManager.LoadScene(3);
    }
}



