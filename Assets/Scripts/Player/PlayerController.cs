using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    public InputActionAsset InputActions;
    public Rigidbody2D Rb;

    [Header("Stats")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float speedBuff;
    public float SpeedBuff => speedBuff;
    public Action<float> OnSpeedBuffChange;

    [Header("Hand")]
    public WeaponController Weapon;

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
        m_moveVal = m_moveAction.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        Rb.linearVelocity = m_moveVal.normalized * GetSpeed();
    }

    public float GetSpeed()
    {
        return walkSpeed * SpeedBuff;
    }

    public void SetSpeedBuff(float newVal)
    {
        speedBuff = newVal;
        OnSpeedBuffChange?.Invoke(SpeedBuff);
    }
}
