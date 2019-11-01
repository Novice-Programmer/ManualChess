using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public ParticleSystem[] addParticle;
    public AudioSource audioSource;
    public float effectTime;
    public float audioTime;

    private void OnEnable()
    {
        if (audioSource != null)
        {
            AlwaysObject.Instance.ETCSoundSet(audioSource);
            StartCoroutine(SoundAdd());
        }
        StartCoroutine(EffectAdd());
    }

    IEnumerator EffectAdd()
    {
        yield return new WaitForSeconds(effectTime);
        for (int i = 0; i < addParticle.Length; i++)
        {
            addParticle[i].Play(true);
        }
    }

    IEnumerator SoundAdd()
    {
        yield return new WaitForSeconds(audioTime);
        audioSource.Play();
    }
}
