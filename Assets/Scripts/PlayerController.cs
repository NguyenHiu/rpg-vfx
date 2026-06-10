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
        // moveAction = InputActions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        m_moveVal =  m_moveAction.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        Rb.MovePosition((Vector2)transform.position + WalkSpeed * m_moveVal);
    }
}
