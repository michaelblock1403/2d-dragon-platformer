using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; } //Singleton, sodass dieses Skript einfach für andere Skripts zugänglich ist
    private AudioSource source; //Zugriff auf die AudioSource-Komponente

    // Awake is called when the script instance loads
    void Awake()
    {
        source = GetComponent<AudioSource>(); //Grab reference for AudioSource from object and store it in the variable (cf.: source)

        //Behalte dieses Objekt, auch wenn wir in die nächste Szene geladen werden
        if (instance == null)
        {
            instance = this; //Create the singleton
            DontDestroyOnLoad(gameObject);
        }
        //Destroy duplicate gameObjects
        else if (instance != null && instance != this)
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(AudioClip _sound)
    {
        source.PlayOneShot(_sound); //PlayOneShot erlaubt es uns, einen AudioClip einmal abzuspielen
    }
}
