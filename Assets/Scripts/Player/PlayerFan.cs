using System;
using UnityEngine;

public class PlayerFan : MonoBehaviour
{

    //Settings

    [Header("Settings")]

    [SerializeField]
    private float m_WindDistanceToPlayer;

    //References

    [Header("References")]

    [SerializeField]
    private PlayerLook m_PlayerLook;

    [SerializeField]
    private MeshRenderer m_FanMeshRenderer;

    [SerializeField]
    private UnityEngine.Object m_WindObject;

    [SerializeField]
    private Animation m_TempFanBlowAnimation;

    //Events

    //Public Fields

    //Private Fields

    //Initialization Methods

    private void GetReferences()
    {

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
        HideFan();
    }

    private void Update()
    {
        UpdateFan();
    }

    //Public Methods

    //Private Methods

    private void UpdateFan()
    {
        if (Input.GetButtonDown("ProduceWind") && m_PlayerLook.IsAiming)
        {
            ProduceWind();
        }
    }

    private void ProduceWind()
    {
        m_PlayerLook.RotateBodyToForward();

        Quaternion windRotation = m_PlayerLook.transform.rotation;
        Vector3 offsetFromPlayer = m_PlayerLook.transform.forward * m_WindDistanceToPlayer;
        Vector3 windPosition = transform.position + offsetFromPlayer;

        Instantiate(m_WindObject, windPosition, windRotation);

        m_TempFanBlowAnimation.Play();
    }

    private void HideFan()
    {
        m_FanMeshRenderer.enabled = false;
    }

}