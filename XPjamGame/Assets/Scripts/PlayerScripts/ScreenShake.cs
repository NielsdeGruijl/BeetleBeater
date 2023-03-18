using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [SerializeField] float shakeMagnitude;
    [SerializeField] float shakeDuration;

    [SerializeField] private AnimationCurve curve;

    private Vector3 startPos;
    private Vector2 offset;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(Shake(shakeDuration));
        }

        if (Input.GetKey(KeyCode.Space))
        {
            ProlongedShake();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            transform.position = startPos;
        }
    }

    private void ProlongedShake()
    {
        float x = Random.Range(0.0f, 1.0f) * 0.05f;
        float y = Random.Range(0.0f, 1.0f) * 0.05f;

        transform.position = new Vector3(x, y, startPos.z);
    }

    IEnumerator Shake(float duration)
    {
        float elapsedTime = 0;

        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);

            float x = Random.Range(0.0f, 1.0f) * strength * shakeMagnitude;
            float y = Random.Range(0.0f, 1.0f) * strength * shakeMagnitude;

            transform.position = new Vector3(x, y, startPos.z);

            yield return null;
        }

        transform.position = startPos;
    }
}
