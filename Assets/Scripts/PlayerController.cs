using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    public InputActionAsset InputActions;
    public Rigidbody2D Rb;

    [Header("Stats")]
    public float WalkSpeed;

    private InputAction m_moveAction;
    private Vector2 m_moveVal;

    void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }

    void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }

    void Start()
    {
        m_moveAction = InputSystem.actions.FindAction("Move");
     }

    void Update()
    {
        m_moveVal =  m_moveAction.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        Rb.linearVelocity = m_moveVal.normalized * WalkSpeed;
    }
}
