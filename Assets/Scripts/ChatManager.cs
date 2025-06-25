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

        // ����� ä�� ���
        AddChat("��", userText);
        inputField.text = "";

        // GPT ó�� ȣ��
        FindObjectOfType<ChatGPTTest>().Send();
    }


    public void AddChat(string speaker, string text)
    {
        GameObject chatGO = Instantiate(chatBubblePrefab, contentParent);
        ChatBubbleUI bubble = chatGO.GetComponent<ChatBubbleUI>();

        bool isUser = speaker == "��";
        bubble.SetData(speaker, text, isUser);

        // ��ũ�� �Ʒ��� �ڵ� �̵�
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
