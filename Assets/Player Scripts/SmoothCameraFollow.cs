using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    public Transform target; // ���� ��� (���ΰ�)
    public float smoothSpeed = 0.125f; // ī�޶� �̵� �ӵ�
    public Vector3 offset; // ������ (ī�޶�� ���ΰ� ������ �Ÿ�)

    void LateUpdate()
    {
        if (target == null) return; // ����� ������ ����

        // ���ϴ� ��ġ ��� (���ΰ� ��ġ + ������)
        Vector3 desiredPosition = target.position + offset;
        // Z ���� -10���� ����
        desiredPosition.z = -10f;

        desiredPosition.y = Mathf.Max(desiredPosition.y, 0.37f);


        // �ε巯�� �̵� ó��
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        // ī�޶� ��ġ ����
        transform.position = smoothedPosition;
    }
}
