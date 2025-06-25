using OpenAI;
using OpenAI.Audio;
using OpenAI.Chat;
using OpenAI.Images;
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
    public AudioSource MyAudioSource;
    public RawImage StoryImage;

    private List<Message> _messages = new List<Message>();

    private OpenAIClient _api;

    private void Start()
    {
        // 1. API Ŭ���̾�Ʈ �ʱ�ȭ -> ChatGPT ����
        _api = new OpenAIClient(APIKeys.OPENAI_API_KEY);

        // CHAT-F
        // C: Context   : ����, ��Ȳ�� ���� �˷����
        // H: Hint      : ���� �亯�� ���� ���
        // A: As A role : ������ �����ض�
        // T: Target    : �亯�� Ÿ���� �˷���� 
        // F: Format    : �亯 ���¸� �����ض�

        string systemMessage = "����: �ʴ� �������� ���� NPC��. �ڽ��� ���� ���Ӽ� ����� �ΰ��̶�� �����Ѵ�.";
        systemMessage += "����: ���� ���ó�� ��ȭ�ϴ� ���� NPC ���";
        systemMessage += "ǥ��: �� ������ '��~'�� ���δ�. �׻� 100���� �̳��� �亯�Ѵ�.";
        systemMessage += "[json ��Ģ]";
        systemMessage += "�亯�� 'ReplyMessage', ";
        systemMessage += "�ܸ�� 'Appearance', ";
        systemMessage += "������ 'Emotion', ";
        systemMessage += "�޸� �̹��� ������ ���� ��ü �̹��� ������ 'StoryImageDescription' ";

        _messages.Add(new Message(Role.System, systemMessage));
    }


    public async void Send()
    {
        // 0. ������Ʈ(=AI���� ���ϴ� ����� ���� �ؽ�Ʈ)�� �о�´�.
        string prompt = PromptField.text;
        if (string.IsNullOrEmpty(prompt))
        {
            return;
        }
        PromptField.text = string.Empty;

        SendButton.interactable = false; // ��ư ��Ȱ��ȭ


        // 2. �޽��� �ۼ� �� �޽���'s ����Ʈ�� �߰�
        Message promptMessage = new Message(Role.User, prompt);
        _messages.Add(promptMessage);

        // 3. �޽��� ������
        //var chatRequest = new ChatRequest(_messages, Model.GPT4oAudioMini, audioConfig:Voice.Alloy);
        var chatRequest = new ChatRequest(_messages, Model.GPT4o);


        // 4. �亯 �ޱ�
        // var response = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
        var (npcResponse, response) = await _api.ChatEndpoint.GetCompletionAsync<NpcResponse>(chatRequest);

        Debug.Log(npcResponse.ReplyMessage);

        // 5. �亯 ����
        var choice = response.FirstChoice;

        // 6. �亯 ���
        Debug.Log($"[{choice.Index}] {choice.Message.Role}: {choice.Message} | Finish Reason: {choice.FinishReason}");
        ResultTextUI.text = npcResponse.ReplyMessage;

        // 7. �亯�� message's �߰�
        Message resultMessage = new Message(Role.Assistant, choice.Message);
        _messages.Add(resultMessage);

        // 8. �亯 ����� ���
        // typecast api�� �̿��� tts
        PlayTTS(npcResponse.ReplyMessage);

        // 9. ���丮 �̹��� ����
        //GenerateImage(npcResponse.StoryImageDescription);
    }


    private async void PlayTTS(string text)
    {
        var request = new SpeechRequest(text);
        var speechClip = await _api.AudioEndpoint.GetSpeechAsync(request);
        MyAudioSource.PlayOneShot(speechClip);
    }

    private async void GenerateImage(string imagePrompt)
    {
        var request = new ImageGenerationRequest(imagePrompt, Model.DallE_3);
        var imageResults = await _api.ImagesEndPoint.GenerateImageAsync(request);

        foreach (var result in imageResults)
        {
            if (result.Texture != null)
            {
                Debug.Log("�ؽ�ó ���� ����");

                StoryImage.texture = result.Texture;

                break; // ù ��° ����� ���
            }
            else
            {
                Debug.LogWarning("�ؽ�ó ���� ����");
            }
        }

        SendButton.interactable = true; // ��ư Ȱ��ȭ
    }
}