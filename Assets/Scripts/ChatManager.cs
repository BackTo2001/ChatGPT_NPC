using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    [Header("UI References")]
    public Transform contentParent; // ScrollView�� Content
    public GameObject chatBubblePrefab;
    public TMP_InputField inputField;
    public Button sendButton;
    public ScrollRect scrollRect;

    private void Start()
    {
        sendButton.onClick.AddListener(OnSendClicked);
    }

    private void OnSendClicked()
    {
        string userText = inputField.text.Trim();
        if (string.IsNullOrEmpty(userText)) return;

        AddChat("��", userText);
        inputField.text = "";

        // ���÷� AI ���䵵 �߰�
        Invoke(nameof(AddAIResponse), 0.5f);
    }

    private void AddAIResponse()
    {
        AddChat("ī�̿� �Ƹ���", "�׷� ������ ���ϸ� �Ը� �������ٴϱ�?");
    }

    public void AddChat(string speaker, string text)
    {
        GameObject chatGO = Instantiate(chatBubblePrefab, contentParent);
        ChatBubbleUI bubble = chatGO.GetComponent<ChatBubbleUI>();
        bubble.SetData(speaker, text);

        // ��ũ�� �Ʒ��� �ڵ� �̵�
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
