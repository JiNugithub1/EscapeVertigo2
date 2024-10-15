using UnityEngine;
using UnityEngine.UI;

public class BackgroundColorChanger : MonoBehaviour
{
    public Image leftImage; // ���� �̹���
    public Image rightImage; // ������ �̹���
    public Image animationImage; // �ִϸ��̼��� Image ������Ʈ

    void Update()
    {
        // �ִϸ��̼��� �� ���� �� �𼭸� ���� ��������
        Color topLeftColor = GetTopLeftColor(animationImage);

        // ���� �� ������ �̹��� ���� ����
        leftImage.color = Color.Lerp(leftImage.color, topLeftColor, Time.deltaTime);
        rightImage.color = Color.Lerp(rightImage.color, topLeftColor, Time.deltaTime);
    }

    // �ִϸ��̼� �̹����� �� ���� �� �𼭸� ���� �������� �޼���
    private Color GetTopLeftColor(Image img)
    {
        // �ؽ�ó�� ������
        Rect rect = img.rectTransform.rect;
        Texture2D texture = img.sprite.texture;

        // ���� �� �𼭸��� ���� �ȼ��� ������
        int x = Mathf.FloorToInt(rect.x);
        int y = Mathf.FloorToInt(rect.y + rect.height); // ���ʿ��� �Ʒ�������
        Color color = texture.GetPixel(x, y);

        return color;
    }
}
