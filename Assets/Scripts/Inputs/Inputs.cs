using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inputs : MonoBehaviour, InputActions.IInGameActions
{
    public static Inputs Instance;
    private InputActions playerInputActions;

    [HideInInspector] public Vector2 mousePosition;
    [HideInInspector] public float scrollDelta;
    [HideInInspector] public bool isLeftClicking = false;
    [HideInInspector] public bool isWheelClicking = false;
    [HideInInspector] public bool isSpacePress = false;

    //EVENTOS
    public static event Action OnLeftClickPerformed;
    public static event Action OnRightClickPerformed;
    public static event Action OnESCPerformed;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); return; }

        playerInputActions = new InputActions();
        playerInputActions.InGame.SetCallbacks(this);
    }

    private void OnEnable()
    {
        playerInputActions.InGame.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.InGame.Disable();
    }

    public void OnScroll(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.GetCurrentGameState() == GameManager.GameState.Playing) scrollDelta = context.ReadValue<Vector2>().y;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }


    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isLeftClicking = true;
            OnLeftClickPerformed?.Invoke(); //notificar click izq
        }
        else
        {
            isLeftClicking = false;
        }
    }

    public void OnESC(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnESCPerformed?.Invoke();
        }
    }

    public void OnWheelClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isWheelClicking = true;
        }
        else
        {
            isWheelClicking = false;
        }
    }

    public void OnSpace(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isSpacePress = true;
        }
        else
        {
            isSpacePress = false;
        }
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnRightClickPerformed?.Invoke(); //notificar click drch
        }
    }

    public void OnSelectNumbers(InputAction.CallbackContext context)
    {
        
    }

    private void Update()
    {
        //Debug.Log("Raton: " + mousePosition);
    }
}
