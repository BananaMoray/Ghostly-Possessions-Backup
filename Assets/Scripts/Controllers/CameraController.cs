using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    private Camera _mainCamera;
    [SerializeField]
    private GameObject _player;
    private PlayerMovement _playerMovement;

    [SerializeField]
    private Vector3 _offset = new Vector3(0,1,-10);
    [SerializeField]
    private float _cameraSpeed = 10f;
    private Vector3 _camVelocity;
    [SerializeField]
    //private float _cameraPossessionSpeed = 10f;
    private GameObject _cameraTarget = null;
    private Vector3 _cameraTargetPos = Vector3.zero;

    void Awake()
    {
        _mainCamera = Camera.main;
        _mainCamera.transform.position = _player.transform.position + _offset;
        _player = GameObject.FindWithTag("Player");
        if( _player != null ) 
            _playerMovement = _player.GetComponent<PlayerMovement>();
    }

    private void LateUpdate()
    {
        UpdateCameraTarget();

        //_mainCamera.transform.position = Vector3.MoveTowards(_mainCamera.transform.position, _cameraTargetPos + _offset, _cameraSpeed * Time.deltaTime);
        _mainCamera.transform.position = Vector3.Lerp(_mainCamera.transform.position, _cameraTargetPos + _offset, _cameraSpeed * Time.deltaTime);
    }

    private void UpdateCameraTarget()
    {

        if (_cameraTarget == null) _cameraTarget = _player;

        //REMEMBER DO NOT USE LERP FOR EVERYTHING START THINKING
        Vector2 look = _playerMovement.LookInput;

        if (look.sqrMagnitude < 0.05f) 
            look = Vector2.zero;

        if (look.sqrMagnitude > 0.1f)
        {
            //works only with absolute look input
            _cameraTargetPos = _player.transform.position + new Vector3(_playerMovement.LookInput.x, 0, _playerMovement.LookInput.y) * 5;

            //_cameraTargetPos = _player.transform.position + _player.transform.eulerAngles * 5;
        }
        else
        {
            if (_cameraTarget != null && _cameraTarget != _player)
                _cameraTargetPos = Vector3.Lerp(_player.transform.position, _cameraTarget.transform.position, 0.5f);
            else
                _cameraTargetPos = _player.transform.position;
        }

    }

    private void OnEnable()
    {
        //hashtag subscribing to the player event
        PlayerController.OnPossessObject += PlayerController_OnPossessObject;
        PlayerController.OnDetectClostestPossessObject += PlayerController_OnDetectClostestPossessObject;
    }

    private void OnDisable()
    {
        //hashtag subscribing to the player event
        PlayerController.OnPossessObject -= PlayerController_OnPossessObject;
        PlayerController.OnDetectClostestPossessObject -= PlayerController_OnDetectClostestPossessObject;
    }
    private void PlayerController_OnDetectClostestPossessObject(object sender, PossessEventArgs e)
    {
        _cameraTarget = e.PossessableObject;
    }

    private void PlayerController_OnPossessObject(object sender, PossessEventArgs e)
    {
        //Debug.Log($"possess thing: {e.PossessedObject.name}");
        //_mainCamera.transform.position = _player.transform.position + _offset;
        StartCoroutine(ZoomEffect());
    }

    [Header("Camera Zoom Variables")]
    [SerializeField]
    private float _defaultFOV = 60f;
    [SerializeField]
    private float _zoomFOV = 40f;
    [SerializeField]
    public float _zoomInDuration = 0.2f;
    [SerializeField]
    public float _zoomOutDuration = 1f;
    [SerializeField]
    public float _waitDuration = 0.5f;

    [Header("Zoom Curves")]
    [SerializeField]
    private AnimationCurve _zoomInCurve;
    [SerializeField]
    private AnimationCurve _zoomOutCurve;

    private IEnumerator ZoomEffect()
    {
        float time = 0f;
        while (time < _zoomInDuration)
        {
            time += Time.deltaTime;
            float current = Mathf.Clamp01(time / _zoomInDuration);
            float curveValue = _zoomInCurve.Evaluate(current);
            _mainCamera.fieldOfView = Mathf.Lerp(_defaultFOV, _zoomFOV, curveValue);
            yield return null;
        }

        yield return new WaitForSeconds(_waitDuration);

        time = 0f;
        while (time < _zoomOutDuration)
        {
            time += Time.deltaTime;
            float current = Mathf.Clamp01(time / _zoomOutDuration);
            float curveValue = _zoomOutCurve.Evaluate(current);
            _mainCamera.fieldOfView = Mathf.Lerp(_zoomFOV, _defaultFOV, curveValue);
            yield return null;
        }

        // --- Ensure final FOV is exact ---
        _mainCamera.fieldOfView = _defaultFOV;
    }
}
