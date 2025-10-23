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

    [Header("Input Types")]
    [Tooltip("Input Type A, click objects to possess them, E to unpossess")]
    [SerializeField] 
    private bool _isInputTypeA;
    [Tooltip("Input Type B, requires user to be near objects")]
    [SerializeField] 
    private bool _isInputTypeB;

    [Header("Misc")]
    [SerializeField] 
    private PlayerInput _input;
    [SerializeField] 
    private Camera _mainCamera;

    [Header("Possession Data")]
    public static bool IsPossessing = false;
    public bool IsPossessionInProgress = false;
    public GameObject PossessionObject;

    private MeshRenderer _renderer;
    private PlayerMovement _movement;
    [SerializeField]
    private Material _possessionMat, _normalMat;

    private bool _previousSelect;
    private bool _previousDeselect;

    private bool _deselect = false;
    private bool _select = false;

    [Header("Only applicable if InputTypeB")]
    [SerializeField]
    private float _holdDuration = 0.5f;
    private float _holdTimer = 0f;
    [SerializeField]
    private float _maxTargetDistance = 5f;

    public static GameObject ClosestTarget = null;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _renderer = GetComponent<MeshRenderer>();
        _movement = GetComponent<PlayerMovement>();
        if (_possessionMat == null)
        {
            throw new NotImplementedException();
        }
    }

    private void Update()
    {
        HandleUnpossessInput();
        HandlePossessInput();
        _movement.HandleMovement(IsPossessing, IsPossessionInProgress, PossessionObject);

        _previousSelect = _select;
        _previousDeselect = _deselect;
    }

    private void HandlePossessInput()
    {
        if (_isInputTypeA && _select && !_previousSelect)
        {
            if (!IsPossessing && !IsPossessionInProgress)
                TryStartPossession();
        }

        if (_isInputTypeB)
        {
            if (!IsPossessing && !IsPossessionInProgress)
            {
                FindClosestPossessable();

                if (ClosestTarget != null)
                {
                    if (_select)
                    {
                        _holdTimer += Time.deltaTime;
                        if (_holdTimer >= _holdDuration)
                        {
                            SetPossessObject(ClosestTarget, true);
                            IsPossessionInProgress = true;
                            _holdTimer = 0f;
                        }
                    }
                    else
                    {
                        _holdTimer = 0f; // reset if button released
                    }
                }
                else
                {
                    _holdTimer = 0f; // no target in range
                }
            }
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

    private void HandleUnpossessInput()
    {
        if (_isInputTypeA && _deselect && !_previousDeselect)
        {
            if (IsPossessing && !IsPossessionInProgress)
                UnpossessObject();
        }

        if (_isInputTypeB && _deselect)
        {
            if (IsPossessing && !IsPossessionInProgress)
            {
                _holdTimer += Time.deltaTime;

                if (_holdTimer >= _holdDuration)
                {
                    UnpossessObject();
                    _holdTimer = 0f;
                }
            }
            else
            {
                _holdTimer = 0f; // reset if button released
            }
        }
    }

    private void TryStartPossession()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform.CompareTag("Possession"))
            {
                if (IsInRange(hit.transform.position))
                {
                    SetPossessObject(hit.transform.gameObject, true);
                    IsPossessionInProgress = true;
                }
                else Debug.Log("Too far away");
            }
        }
    }

    private bool IsInRange(Vector3 position)
    {
        return Vector3.Distance(transform.position, position) < _maxTargetDistance;
    }

    public void PossessObject(GameObject target)
    {
        //Debug.Log($"Player possessed {target.name}");
        _renderer.enabled = false;
        transform.position = target.transform.position;
        _normalMat = target.GetComponent<Renderer>().material;
        target.GetComponent<Renderer>().material = _possessionMat;
        OnPossessObject?.Invoke(this, new PossessEventArgs(target));
    }

    public void UnpossessObject()
    {
        if (!IsPossessing || IsPossessionInProgress)
        {
            //Debug.Log("Cannot unpossess right now.");
            return;
        }

        //Debug.Log("Player unpossessed the object.");
        _renderer.enabled = true;
        PossessionObject.GetComponent<Renderer>().material = _normalMat;
        SetPossessObject(null, false);
    }

    public void SetPossessObject(GameObject possessableObject, bool isTrue)
    {
        IsPossessing = isTrue;
        PossessionObject = possessableObject;
        
        if (!isTrue)
        {
            _movement.ResetProgress();
            IsPossessionInProgress = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _movement._movementInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _movement._lookInput = context.ReadValue<Vector2>();
    }

    public void OnDeselect(InputAction.CallbackContext context)
    {
        _deselect = context.action.triggered;
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        _select = context.action.triggered;
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
