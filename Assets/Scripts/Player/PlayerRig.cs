using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRig : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_OpenParasol, m_CloseParasol, m_Jump1, m_Jump2, m_Jump3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayOpenParasolSound()
    {
        m_OpenParasol.Play();
    }

    public void PlayClosedParasolSound()
    {
        m_CloseParasol.Play();
    }

    private void JumpSound()
    {
        int rInt = Random.Range(1, 4);

        if (rInt == 1)
        {
            m_Jump1.Play();
        }
        else if (rInt == 2)
        {
            m_Jump2.Play();
        }
        else if (rInt == 3)
        {
            m_Jump3.Play();
        }
    }
}
