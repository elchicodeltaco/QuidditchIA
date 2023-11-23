using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCabras : MonoBehaviour
{
    //singleton
    public static AudioCabras instancia;

    private void Awake()
    {
        if (instancia == null) //la variable no esta asignada
            instancia = this;
        else if (instancia != this) //hay otro asignado
            Destroy(gameObject);
    }

    public List<AudioSource> SFXAudioSource;
    public AudioSource MusicSource;

    //lista de audio
    public List<AudioClip> SFX_List;

    public void PlaySFX(int clipNumber)
    {
        //buscamos un audio source que no este reproduciendo un sfx
        foreach(AudioSource source in SFXAudioSource)
        {
            if(!source.isPlaying)
            {
                source.clip = SFX_List[clipNumber];
                source.Play();
                return;
            }
        }
    }
}
