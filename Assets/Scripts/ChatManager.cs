using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    [Header("UI References")]
    public Transform contentParent; // ScrollView의 Content
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

        AddChat("나", userText);
        inputField.text = "";

        // 예시로 AI 응답도 추가
        Invoke(nameof(AddAIResponse), 0.5f);
    }

    private void AddAIResponse()
    {
        AddChat("카이엔 아르벨", "그런 말투로 말하면 입맛 떨어진다니까?");
    }

    public void AddChat(string speaker, string text)
    {
        GameObject chatGO = Instantiate(chatBubblePrefab, contentParent);
        ChatBubbleUI bubble = chatGO.GetComponent<ChatBubbleUI>();
        bubble.SetData(speaker, text);

        // 스크롤 아래로 자동 이동
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
