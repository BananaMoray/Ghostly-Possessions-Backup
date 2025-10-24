using System;
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
    public static event EventHandler<PossessEventArgs> OnDetectClostestPossessObject;

    [Header("Misc")]
    [SerializeField] 
    private PlayerInput _input;
    [SerializeField] 
    private Camera _mainCamera;

    [Header("Possession Data")]
    public static bool IsPossessing = false;
    public bool IsPossessionInProgress = false;
    public GameObject PossessionObject;
    private IPossessable _currentPossession;

    private MeshRenderer _renderer;
    private PlayerMovement _movement;

    public Material PossessMat;
    private Material _playerMat;

    private bool _previousSelect;
    private bool _previousDeselect;

    private bool _deselect = false;
    private bool _select = false;

    public static float PlayerStrength = 1f;

    [SerializeField]
    private float _holdDuration = 0.5f;
    private float _holdTimer = 0f;
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

    private void Update()
    {
        if (_currentPossession != null && !IsPossessionInProgress)
            _currentPossession.HandlePossessedMovement(_movement.MovementInput);

        HandleUnpossessInput();
        HandlePossessInput();
        _movement.HandleMovement(IsPossessing, IsPossessionInProgress, PossessionObject);

        _previousSelect = _select;
        _previousDeselect = _deselect;
    }

    private void HandlePossessInput()
    {

        if (IsPossessing)
        {
            //cancel possession
            if (!_select && IsPossessionInProgress)
            {
                //_fade.FadeIn(0);
                //Debug.Log("Possession cancelled");
                SetPossessObject(null, false);
            }

            return;
        }

        FindClosestPossessable();

        if (ClosestTarget == null)
        {
            _holdTimer = 0f;
            return;
        }

        if (_select)
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

    private void HandleUnpossessInput()
    {

        if (_deselect)
        {
            if (IsPossessing && !IsPossessionInProgress)
            {
                _fade.FadeIn(_holdDuration);

                _holdTimer += Time.deltaTime; if (_holdTimer >= _holdDuration)
                {
                    UnpossessObject();
                    //_holdTimer = 0f;
                }
            }
            else if (_deselect)
            {
                //_fade.FadeOut(0);
            }
            else
                _holdTimer = 0f;
        }
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
            _currentPossession.OnPossess(this);

        OnPossessObject?.Invoke(this, new PossessEventArgs(target));
    }

    public void UnpossessObject()
    {
        //if (!IsPossessing || IsPossessionInProgress)
        //{
        //    SetPossessObject(null, false);
        //}
        _renderer.enabled = true;

        //sends message to IPossessable if it exists
        if (_currentPossession != null)
        {
            _currentPossession.OnDepossess();
            _currentPossession = null;
        }

        SetPossessObject(null, false);
    }

    public static Vector2 StartPossessionPosition;

    public void SetPossessObject(GameObject possessableObject, bool isTrue)
    {
        IsPossessionInProgress = isTrue;
        IsPossessing = isTrue;

        PossessionObject = possessableObject;

        if (isTrue)
            StartPossessionPosition = transform.position;
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

    public void OnDeselect(InputAction.CallbackContext context)
    {
        if (context.performed)
            _deselect = true;
        else if (context.canceled)
            _deselect = false;
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (context.performed)
            _select = true;
        else if (context.canceled)
            _select = false;
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
