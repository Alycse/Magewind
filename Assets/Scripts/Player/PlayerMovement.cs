using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerParasol))]
public class PlayerMovement : MonoBehaviour
{
    //Settings

    [Header("Settings")]

    [SerializeField]
    private float m_MoveSpeed;

    [SerializeField]
    private float m_ParasolMoveSpeed;

    [SerializeField]
    private float m_JumpForce;

    [SerializeField]
    private float m_ParasolGravityScale;

    [SerializeField]
    private float m_GroundCheckLength;

    //References

    [Header("References")]

    [SerializeField]
    private PlayerLook m_PlayerLook;

    private Rigidbody m_PlayerRigidbody;
    private PlayerParasol m_PlayerParasol;
    private PlayerBlowable m_PlayerBlowable;

    //Events

    //Public Fields

    //Private Fields

    private Vector3 m_MovementInput;
    private bool m_IsGrounded;
    private float m_OriginalGravity;

    //Initialization Methods

    private void GetReferences()
    {
        m_PlayerRigidbody = GetComponent<Rigidbody>();
        m_PlayerParasol = GetComponent<PlayerParasol>();
        m_PlayerBlowable = GetComponent<PlayerBlowable>();
    }

    private void SubscribeToEvents()
    {

    }

    //Play Methods

    private void Awake()
    {
        GetReferences();
        SubscribeToEvents();
    }

    private void Start()
    {
        m_OriginalGravity = Physics.gravity.y;
    }

    private void Update()
    {
        UpdateMovement();
        UpdateJump();
    }

    void FixedUpdate()
    {
        UpdateParasolMovement();
        UpdateParasolGravity();
    }

    //Public Methods

    //Private Methods

    private void UpdateMovement()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");

        m_MovementInput = cameraForward * inputVertical + cameraRight * inputHorizontal;

        m_IsGrounded = Physics.Raycast(transform.position, Vector3.down, m_GroundCheckLength);

        if (m_MovementInput != Vector3.zero)
        {
            m_PlayerLook.RotateBodyToForward();

            if (!m_PlayerParasol.IsParasolOpen || m_IsGrounded)
            {
                m_PlayerRigidbody.MovePosition(m_PlayerRigidbody.position + m_MovementInput * m_MoveSpeed * Time.deltaTime);
            }

            if (!m_PlayerParasol.IsParasolOpen && !m_PlayerBlowable.IsInWind())
            {
                m_PlayerRigidbody.velocity = new Vector3(0.0f, m_PlayerRigidbody.velocity.y, 0.0f);
            }
        }
    }

    private void UpdateJump()
    {
        if (Input.GetButtonDown("Jump") && m_IsGrounded)
        {
            Jump();
        }
    }

    private void UpdateParasolMovement()
    {
        if (m_MovementInput != Vector3.zero)
        {
            if (m_PlayerParasol.IsParasolOpen && !m_IsGrounded)
            {
                m_PlayerRigidbody.AddForce(m_MovementInput * m_ParasolMoveSpeed, ForceMode.Acceleration);
            }
        }
    }

    private void UpdateParasolGravity()
    {
        if (!m_PlayerBlowable.IsInWind())
        {
            float gravityForce = m_PlayerParasol.IsParasolOpen ? m_OriginalGravity * m_ParasolGravityScale : m_OriginalGravity;
            m_PlayerRigidbody.AddForce(new Vector3(0, gravityForce, 0), ForceMode.Acceleration);
        }
        else if (!m_PlayerParasol.IsParasolOpen)
        {
            m_PlayerRigidbody.AddForce(new Vector3(0, m_OriginalGravity * m_ParasolGravityScale, 0), ForceMode.Acceleration);
        }
    }

    private void Jump()
    {
        m_PlayerRigidbody.AddForce(Vector3.up * m_JumpForce, ForceMode.Impulse);
    }
}
