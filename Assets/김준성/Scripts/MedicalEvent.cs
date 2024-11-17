using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MedicalEvent : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform
    public GameObject dialogueObject; // Dialogue ������Ʈ (SC)
    public GameObject guardObject; // ���� ������Ʈ
    public GameObject annaObject; // �ȳ� ������Ʈ
    public Camera mainCamera; // ���� ī�޶�
    public SmoothCameraFollow smoothFollower; // SmoothFollower ��ũ��Ʈ
    public float playerTargetX = 2f; // �÷��̾��� ��ǥ X ��ġ
    public float moveSpeed = 2f; // �÷��̾� �̵� �ӵ�
    private PlayerMove playerMove; // PlayerMove ��ũ��Ʈ ����
    private Animator playerAnimator; // �÷��̾��� �ִϸ����� ����

    void Start()
    {
        // PlayerMove ��ũ��Ʈ�� �ִϸ����� ��������
        playerMove = player.GetComponent<PlayerMove>();
        playerAnimator = player.GetComponent<Animator>();

        // PlayerMove ��ũ��Ʈ�� ��Ȱ��ȭ
        playerMove.enabled = false;


        // �̺�Ʈ ������ ����
        StartCoroutine(EventSequence());

    }

    private IEnumerator EventSequence()
    {
        // 1. �÷��̾��� isWalk �ִϸ��̼� Ȱ��ȭ
        playerAnimator.SetBool("isWalk", true);

        // �÷��̾ X=2 ��ġ�� �̵�
        while (player.position.x < playerTargetX)
        {
            player.position = Vector2.MoveTowards(player.position, new Vector2(playerTargetX, player.position.y), moveSpeed * Time.deltaTime);
            yield return null;
        }

        // 2. �÷��̾ ���߸� isWalk�� ��Ȱ��ȭ�ϰ� Idle ���·� ��ȯ
        playerAnimator.SetBool("isWalk", false);
        playerAnimator.SetBool("isIdle", true);

        // 3. ��ǥ ��ġ�� ���� �� Dialogue ������Ʈ Ȱ��ȭ
        dialogueObject.SetActive(true);
        // ������ �ȳ� ������Ʈ�� Ȱ��ȭ
        guardObject.SetActive(true);
        annaObject.SetActive(true);
        // ��ȭ�� ���� ������ ���
        yield return new WaitUntil(() => dialogueObject.activeSelf == false);

        // 4. ī�޶� ������ ������� �ǵ�����
        mainCamera.orthographicSize = 7;

        // 5. SmoothFollower ��ũ��Ʈ ����
        smoothFollower.enabled = true;

        // 6. PlayerMove ��ũ��Ʈ�� �ٽ� Ȱ��ȭ
        playerMove.enabled = true;

        // 7. QustID �� 3���� ����
        PlayerPrefs.SetInt("QustID", 3);
        SceneManager.LoadScene("MedicHall"); // �̵��� �� ������ ��ȯ

    }
}
