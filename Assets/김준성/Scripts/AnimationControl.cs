using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        // �ִϸ��̼��� �� �� ����
        animator.SetTrigger("Play");
    }

    // �ִϸ��̼��� ���ߴ� �޼���
    public void StopAnimation()
    {
        // �ִϸ������� Stop Ʈ���Ÿ� Ȱ��ȭ�Ͽ� �ִϸ��̼��� ����
        animator.SetTrigger("Stop");
    }
}
