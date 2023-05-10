using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("---Player---")]
    [SerializeField] AudioSource player;

    [Header("------ Audio ------")]
    [SerializeField] AudioClip[] audSteps;
    [SerializeField] AudioClip[] audJump;
    [SerializeField] AudioClip[] auddamage;

    private bool isPlayingSteps;

    public IEnumerator MoveSound(bool running, float audStepsVol)
    {
        isPlayingSteps = true;

        player.PlayOneShot(audSteps[UnityEngine.Random.Range(0, audSteps.Length)], audStepsVol);

        if (running)
        {
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }

        isPlayingSteps = false;
    }

    public void JumpSound(float audJumpVol)
    {
        player.PlayOneShot(audJump[UnityEngine.Random.Range(0, audJump.Length)], audJumpVol);
        //yield return new Null();
    }

    public IEnumerator DamageSound(float auddamageVol)
    {
        player.PlayOneShot(auddamage[UnityEngine.Random.Range(0, auddamage.Length)], auddamageVol);
        yield return new Null();
    }
}
