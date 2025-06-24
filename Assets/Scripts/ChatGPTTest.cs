using OpenAI;
using OpenAI.Chat;
using OpenAI.Models;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatGPTTest : MonoBehaviour
{
    public TextMeshProUGUI ResultTextUI;   // ��� �ؽ�Ʈ
    public TMP_InputField PromptField;     // �Է� �ʵ�
    public Button SendButton;              // ������ ��ư

    private const string OPEANAI_API_KEY =
        "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";

    public async void Send()
    {
        // 0. ������Ʈ(=AI���� ���ϴ� ����� ���� �ؽ�Ʈ)�� �о�´�.
        string prompt = PromptField.text;
        if (string.IsNullOrEmpty(prompt))
        {
            return;
        }
        PromptField.text = string.Empty;


        // 1. API Ŭ���̾�Ʈ �ʱ�ȭ -> ChatGPT ����
        var api = new OpenAIClient(OPEANAI_API_KEY);

        // 2. �޽��� �ۼ�
        var messages = new List<Message>
        {
            new Message(Role.User, prompt),
        };

        // 3. �޽��� ������
        var chatRequest = new ChatRequest(messages, Model.GPT4o);

        // 4. �亯 �ޱ�
        var response = await api.ChatEndpoint.GetCompletionAsync(chatRequest);

        // 5. �亯 ����
        var choice = response.FirstChoice;

        // 6. �亯 ���
        Debug.Log($"[{choice.Index}] {choice.Message.Role}: {choice.Message} | Finish Reason: {choice.FinishReason}");
        ResultTextUI.text = choice.Message;
    }

}