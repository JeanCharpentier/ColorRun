using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float pDuration, float pMagnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float _elapsed = 0.0f;

        while(_elapsed < pDuration)
        {
            float x = Random.Range(-1.0f, 1.0f) * pMagnitude;
            float y = Random.Range(-1.0f, 1.0f) * pMagnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            _elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
