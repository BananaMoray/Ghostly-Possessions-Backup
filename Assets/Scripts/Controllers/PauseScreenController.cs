using UnityEngine;
using UnityEngine.InputSystem;

public class PauseScreenController : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseCanvas;

    private bool _pauseInput;
    private bool _previousPauseInput;

    public bool IsPaused;


    private void Awake()
    {
        _pauseCanvas.SetActive(false);
        
    }

    private void Update()
    {
        if (_pauseInput && _pauseInput != _previousPauseInput)
        {
            if (IsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        _previousPauseInput = _pauseInput;
    }

    public void PauseGame()
    {
        _pauseCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        _pauseCanvas.SetActive(false);
        Time.timeScale = 1;
    }

    public void TogglePause()
    {
        if (IsPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
            _pauseInput = true;
        else if (context.canceled)
            _pauseInput = false;
    }

}
