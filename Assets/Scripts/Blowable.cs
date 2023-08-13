using System;
using UnityEngine;

public class Blowable : MonoBehaviour
{

    //Settings

    [Header("Settings")]

    [SerializeField]
    private float m_WindStrengthMultiplier = 1.0f;

    //References

    [Header("References")]

    protected Rigidbody m_BlowableRigidbody;

    //Events

    //Public Fields

    //Private Fields

    protected Vector3 m_CurWindForce;

    //Initialization Methods

    protected virtual void GetReferences ()
    {
        m_BlowableRigidbody = GetComponent<Rigidbody>();
    }

    private void SubscribeToEvents ()
	{
		
	}

    //Play Methods

    protected void Awake ()
	{
		GetReferences();
		SubscribeToEvents();
	}

	private void Start()
    {
        m_CurWindForce = Vector3.zero;
    }

	private void Update()
	{
		
	}

    private void FixedUpdate()
    {
        UpdateWind();
    }

    //Public Methods

    public bool IsInWind()
    {
        return m_CurWindForce != Vector3.zero;
    }

    //Private Methods

    protected virtual void UpdateWind()
    {
        if (IsInWind())
        {
            m_BlowableRigidbody.AddForce(m_CurWindForce);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Wind"))
        {
            Wind wind = other.GetComponent<Wind>();
            m_CurWindForce = wind.Direction * wind.WindStrength * m_WindStrengthMultiplier;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wind"))
        {
            m_CurWindForce = Vector3.zero;
        }
    }

}