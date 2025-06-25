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

        // 사용자 채팅 출력
        AddChat("나", userText);
        inputField.text = "";

        // GPT 처리 호출
        FindObjectOfType<ChatGPTTest>().Send();
    }


    public void AddChat(string speaker, string text)
    {
        GameObject chatGO = Instantiate(chatBubblePrefab, contentParent);
        ChatBubbleUI bubble = chatGO.GetComponent<ChatBubbleUI>();

        bool isUser = speaker == "나";
        bubble.SetData(speaker, text, isUser);

        // 스크롤 아래로 자동 이동
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
