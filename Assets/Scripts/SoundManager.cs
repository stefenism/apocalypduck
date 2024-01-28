using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    [SerializeField] private AudioSource audioSourcePrefab = default;

    private static SoundManager instance = default;
    private static float initialPitch = default;

    private void Start(){
        instance = this;
        initialPitch = audioSourcePrefab.pitch;
    }

    public static void PlaySound(AudioClip clip, Transform soundTransform, float pitchVariation = 0f){
        AudioSource audioSource = Instantiate(instance.audioSourcePrefab, soundTransform.position, soundTransform.rotation);
        audioSource.pitch = initialPitch + Random.RandomRange(-pitchVariation, pitchVariation);
        audioSource.PlayOneShot(clip);
        Destroy(audioSource.gameObject, clip.length);
    }
}
