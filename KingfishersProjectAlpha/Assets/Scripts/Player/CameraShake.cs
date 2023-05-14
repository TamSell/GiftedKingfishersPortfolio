using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public bool startShake = false;
    public AnimationCurve shakeCurve;
    public float duration = 1;
    void Update()
    {
        if (startShake)
        {
            startShake= false;
            StartCoroutine(Shaking());
        }
    }

    IEnumerator Shaking()
    {
        Vector3 startPosition = transform.localPosition;
        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            elapsedTime+= Time.deltaTime;
            float shakeStrength = shakeCurve.Evaluate(elapsedTime / duration);
            transform.localPosition = startPosition + Random.insideUnitSphere * shakeStrength;
            yield return 0.6f;
        }
       // transform.position = startPosition;
    }
}
