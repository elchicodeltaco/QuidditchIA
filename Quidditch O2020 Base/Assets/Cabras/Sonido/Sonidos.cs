using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonidos : MonoBehaviour
{
    public CabrasTeam MiTeam;


    void Start()
    {
        InvokeRepeating("PlayCabra", 3.0f, 17.0f);
    }

    void Update()
    {
        if(!GameManager.instancia.isGameStarted() == true)
        {
            AudioCabras.instancia.PlaySFX(0);
        }

        if (GameManager.instancia.isGameOver() == true)
        {
            AudioCabras.instancia.PlaySFX(1);
        }

        if (MiTeam.cabrasTeamNumber == GameManager.instancia.IsRecovering())
        {
            AudioCabras.instancia.PlaySFX(2);
        }

        if (MiTeam.cabrasTeamNumber != GameManager.instancia.IsRecovering() && GameManager.instancia.IsRecovering() != 0)
        {
            AudioCabras.instancia.PlaySFX(4);
        }
    }   
    
    private void PlayCabra()
    {
        AudioCabras.instancia.PlaySFX(3);
    }

}
