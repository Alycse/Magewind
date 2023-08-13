using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class DebugDisplay : MonoBehaviour
{

	//Settings

	//References

	//Events

	//Public Fields

	//Private Fields

	private MeshRenderer m_MeshRenderer;

    //Initialization Methods

    private void GetReferences ()
	{
		m_MeshRenderer = GetComponent<MeshRenderer> ();
	}

	private void SubscribeToEvents ()
	{
		
	}

	//Play Methods

	private void Awake ()
	{
		GetReferences();
		SubscribeToEvents();
	}

	private void Start()
	{
		m_MeshRenderer.enabled = false;
    }

	private void Update()
	{
		
	}

	//Public Methods

	//Private Methods

}