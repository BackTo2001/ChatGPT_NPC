using OpenAI;
using OpenAI.Chat;
using OpenAI.Models;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatGPTTest : MonoBehaviour
{
    public TextMeshProUGUI ResultTextUI;   // 결과 텍스트
    public TMP_InputField PromptField;     // 입력 필드
    public Button SendButton;              // 보내기 버튼

    private const string OPEANAI_API_KEY =
        "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";

    public async void Send()
    {
        // 0. 프롬프트(=AI에게 원하는 명령을 적은 텍스트)를 읽어온다.
        string prompt = PromptField.text;
        if (string.IsNullOrEmpty(prompt))
        {
            return;
        }
        PromptField.text = string.Empty;


        // 1. API 클라이언트 초기화 -> ChatGPT 접속
        var api = new OpenAIClient(OPEANAI_API_KEY);

        // 2. 메시지 작성
        var messages = new List<Message>
        {
            new Message(Role.User, prompt),
        };

        // 3. 메시지 보내기
        var chatRequest = new ChatRequest(messages, Model.GPT4o);

        // 4. 답변 받기
        var response = await api.ChatEndpoint.GetCompletionAsync(chatRequest);

        // 5. 답변 선택
        var choice = response.FirstChoice;

        // 6. 답변 출력
        Debug.Log($"[{choice.Index}] {choice.Message.Role}: {choice.Message} | Finish Reason: {choice.FinishReason}");
        ResultTextUI.text = choice.Message;
    }

}