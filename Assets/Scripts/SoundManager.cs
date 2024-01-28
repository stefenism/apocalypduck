using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    [SerializeField] private AudioSource audioSource = default;

    private static SoundManager instance = default;
    private static float initialPitch = default;

    private void Start(){
        instance = this;
        initialPitch = audioSource.pitch;
    }

    public static void PlaySound(AudioClip clip, float pitchVariation = 0f){
        instance.audioSource.pitch = initialPitch + Random.RandomRange(-pitchVariation, pitchVariation);
        instance.audioSource.PlayOneShot(clip);
    }
}
