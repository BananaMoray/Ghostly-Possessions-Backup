using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private SceneManager _sceneManager;

    public GameObject[] waves;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {

        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {

        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {

        }
    }

}
