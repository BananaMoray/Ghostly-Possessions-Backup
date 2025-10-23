using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CameraController : MonoBehaviour
{
    private Camera _mainCamera;
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private Vector3 _offset = new Vector3(0,1,-10);
    [SerializeField]
    private float _cameraSpeed = 10f;
    [SerializeField]
    //private float _cameraPossessionSpeed = 10f;
    private GameObject _cameraTarget = null;
    private Vector3 _cameraTargetPos = Vector3.zero;

    void Awake()
    {
        _mainCamera = Camera.main;
        _mainCamera.transform.position = _player.transform.position + _offset;

    }

    private void Update()
    {
        UpdateCameraTarget();
        _mainCamera.transform.position = Vector3.Lerp(_mainCamera.transform.position, _cameraTargetPos + _offset, _cameraSpeed * Time.deltaTime);
    }

    private void UpdateCameraTarget()
    {

        if (_cameraTarget == null) _cameraTarget = _player;

        //REMEMBER DO NOT USE LERP FOR EVERYTHING START THINKING
        _cameraTargetPos = (_player.transform.position + _cameraTarget.transform.position) / 2f;

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
        Debug.Log("Target changed");
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
