using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake instance;

    [SerializeField] float shakeMagnitude;
    [SerializeField] float shakeDuration;

    [SerializeField] private AnimationCurve curve;

    private Vector3 startPos;
    private Vector2 offset;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(Shake(shakeDuration, shakeMagnitude));
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

    public IEnumerator Shake(float duration, float magnitude)
    {
        float elapsedTime = 0;

        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);

            float x = Random.Range(0.0f, 1.0f) * strength * magnitude;
            float y = Random.Range(0.0f, 1.0f) * strength * magnitude;

            transform.position = new Vector3(x, y, startPos.z);

            yield return null;
        }

        transform.position = startPos;
    }
}
