using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class FlickeringLight : MonoBehaviour
{
    public Light2D light2D; // 2D Light ������Ʈ
    public float minIntensity = 0.1f; // �ּ� ���
    public float maxIntensity = 1.0f; // �ִ� ���
    public float flickerMinDuration = 0.1f; // �ּ� ������ �ð�
    public float flickerMaxDuration = 0.3f; // �ִ� ������ �ð�
    public float waitMinTime = 0.1f; // �ּ� ��� �ð�
    public float waitMaxTime = 1.0f; // �ִ� ��� �ð�

    private void Start()
    {
        if (light2D == null)
        {
            light2D = GetComponent<Light2D>(); // Light2D ������Ʈ�� �ڵ����� ã���ϴ�.
        }
        StartCoroutine(Flicker()); // Flicker �ڷ�ƾ ����
    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            // ���� �ּ� ���� ����
            light2D.intensity = minIntensity;
            float flickerDuration = Random.Range(flickerMinDuration, flickerMaxDuration);
            yield return new WaitForSeconds(flickerDuration);

            // ���� �ִ� ���� ����
            light2D.intensity = maxIntensity;
            flickerDuration = Random.Range(flickerMinDuration, flickerMaxDuration);
            yield return new WaitForSeconds(flickerDuration);

            // ���� ������ �� ���
            float waitTime = Random.Range(waitMinTime, waitMaxTime);
            yield return new WaitForSeconds(waitTime);
        }
    } // Flicker �޼��� ����
} // FlickeringLight Ŭ���� ����
