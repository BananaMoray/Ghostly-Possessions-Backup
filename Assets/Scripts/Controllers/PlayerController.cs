using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    public static event EventHandler<PossessEventArgs> OnPossessObject;
    //public static event EventHandler<PossessEventArgs> OnInteract;
    public static event EventHandler<PossessEventArgs> OnDetectClostestPossessObject;

    [Header("Misc")]
    [SerializeField] 
    private PlayerInput _input;
    [SerializeField] 
    private Camera _mainCamera;

    [Header("Possession Data")]
    public bool IsPossessing = false;
    public bool IsPossessionInProgress = false;
    public GameObject PossessionObject;
    private IPossessable _currentPossession;

    private MeshRenderer _renderer;
    private PlayerMovement _movement;

    public Material PossessMat;
    private Material _playerMat;

    private bool _previousInteract;
    private bool _previousAttack;

    private bool _attack = false;
    private bool _interact = false;
    private bool _boost = false;

    public static float PlayerStrength = 1f;

    [SerializeField]
    private float _holdDuration = 0.5f;
    [SerializeField]
    private float _possessionCooldown = 0.5f;
    private float _holdTimer = 0f;
    private bool _canPossess = true;
    [SerializeField]
    private float _maxTargetDistance = 5f;

    public static GameObject ClosestTarget = null;

    private PlayerFade _fade;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _renderer = GetComponent<MeshRenderer>();
        _movement = GetComponent<PlayerMovement>();
        _fade = GetComponent<PlayerFade>();

        if (PossessMat == null)
        {
            throw new NotImplementedException();
        }
    }

    private void FixedUpdate()
    {

        if (_currentPossession != null && !IsPossessionInProgress)
        {
            _currentPossession.HandlePossessedInput(_movement.MovementInput, _movement.LookInput);
            //_currentPossession.HandlePossessedRotation(_movement.LookInput);
        }

        if (_currentPossession != null)
        {
            HandleAttackInput();
            HandleBoostInput();
        }

        HandleInteractInput();

        _movement.HandleMovement(IsPossessing, IsPossessionInProgress, PossessionObject);

        _previousInteract = _interact;
        _previousAttack = _attack;
    }

    private void HandleInteractInput()
    {

        if (IsPossessing)
        {

            //cancel possession
            if (!_interact && IsPossessionInProgress)
            {
                //_fade.FadeIn(0);
                //Debug.Log("Possession cancelled");
                SetPossessObject(null, false);
            }

            if (_interact && _currentPossession != null)
            {
                
                _fade.FadeIn(_holdDuration);

                _holdTimer += Time.deltaTime; if (_holdTimer >= _holdDuration)
                {
                    UnpossessObject();
                    //_holdTimer = 0f;
                }
            }
            else if (_attack)
            {
                //_fade.FadeOut(0);
            }
            else
                _holdTimer = 0f;

            return;
        }

        FindClosestPossessable();

        if (ClosestTarget == null)
        {
            _holdTimer = 0f;
            return;
        }

        if (_interact && _canPossess)
        {

            if (!IsPossessionInProgress)
            {
                //_fade.FadeOut(1);
                SetPossessObject(ClosestTarget, true);
                IsPossessionInProgress = true;
            }
        }

        _holdTimer = 0f;
    }

    private void HandleAttackInput()
    {
        _currentPossession.HandlePossessedAttack(_attack);
    }
    private void HandleBoostInput()
    {
        _currentPossession.HandlePossessedBoost(_boost);
    }

    private void FindClosestPossessable()
    {
        GameObject[] possessables = GameObject.FindGameObjectsWithTag("Possession");
        float closestDistance = float.MaxValue;
        GameObject newClosest = null;

        foreach (GameObject obj in possessables)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance <= _maxTargetDistance && distance < closestDistance)
            {
                closestDistance = distance;
                newClosest = obj;
                
            }
        }

        if (newClosest != ClosestTarget)
        {
            HighlightTarget(ClosestTarget, false); 
            HighlightTarget(newClosest, true);      
            ClosestTarget = newClosest;
            OnDetectClostestPossessObject?.Invoke(this, new PossessEventArgs(newClosest));
        }
    }

    private void HighlightTarget(GameObject obj, bool isTrue)
    {
        if (obj == null) return;

        Outline outline = obj.GetComponent<Outline>();
        if (outline == null && isTrue)
            outline = obj.AddComponent<Outline>();

        if (outline != null)
            outline.OutlineWidth = 7.0f;
            outline.enabled = isTrue;
    }

    private bool IsInRange(Vector3 position)
    {
        return Vector3.Distance(transform.position, position) < _maxTargetDistance;
    }

    public void PossessObject(GameObject target)
    {

        //Debug.Log($"Player possessed {target.name}");
        _renderer.enabled = false;


        IsPossessionInProgress = false;
        transform.position = target.transform.position;
        _currentPossession = PossessionObject.GetComponent<IPossessable>();

        //sends message to IPossessable
        if (_currentPossession != null)
        {
            _currentPossession.OnStartPossess(this);
        }

        OnPossessObject?.Invoke(this, new PossessEventArgs(target));

        HighlightTarget(ClosestTarget, false);
        ClosestTarget = null;

        StartCoroutine(PossessionCooldown(_possessionCooldown));
    }

    public void UnpossessObject()
    {
        _renderer.enabled = true;

        //sends message to IPossessable if it exists
        if (_currentPossession != null)
        {
            _currentPossession.OnStopPossess();
            _currentPossession = null;
        }
        SetPossessObject(null, false);

        StartCoroutine(PossessionCooldown(_possessionCooldown));
    }

    public static Vector3 StartPossessionPosition;

    public void SetPossessObject(GameObject possessableObject, bool isTrue)
    {
        IsPossessionInProgress = isTrue;
        IsPossessing = isTrue;

        PossessionObject = possessableObject;

        if (isTrue)
        {
            StartPossessionPosition = transform.position;
            //Debug.Log(StartPossessionPosition);
        }
        else
            IsPossessionInProgress = false;

        _movement.ResetProgress();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _movement.MovementInput = context.ReadValue<Vector2>();


    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _movement.LookInput = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            _attack = true;
        else if (context.canceled)
            _attack = false;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
            _interact = true;
        else if (context.canceled)
            _interact = false;
    }
    public void OnBoost(InputAction.CallbackContext context)
    {
        if (context.performed)
            _boost = true;
        else if (context.canceled)
            _boost = false;
    }


    private IEnumerator PossessionCooldown(float seconds)
    {
        _canPossess = false;
        yield return new WaitForSeconds(seconds);
        _canPossess = true;
    }






}

public class PossessEventArgs : EventArgs
{
    public GameObject PossessableObject { get; }

    public PossessEventArgs(GameObject possessableObject)
    {
        PossessableObject = possessableObject;
    }
}
