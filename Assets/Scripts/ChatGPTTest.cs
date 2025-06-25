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
    public TextMeshProUGUI ResultTextUI;   // 결과 텍스트
    public TMP_InputField PromptField;     // 입력 필드
    public Button SendButton;              // 보내기 버튼
    public AudioSource MyAudioSource;
    public RawImage StoryImage;

    private List<Message> _messages = new List<Message>();

    private OpenAIClient _api;

    private void Start()
    {
        // 1. API 클라이언트 초기화 -> ChatGPT 접속
        _api = new OpenAIClient(APIKeys.OPENAI_API_KEY);

        // CHAT-F
        // C: Context   : 문맥, 상황을 많이 알려줘라
        // H: Hint      : 예시 답변을 많이 줘라
        // A: As A role : 역할을 제공해라
        // T: Target    : 답변의 타겟을 알려줘라 
        // F: Format    : 답변 형태를 지정해라

        string systemMessage = "역할: 너는 이제부터 게임 NPC다. 자신을 실제 게임속 고양이 인간이라고 생각한다.";
        systemMessage += "목적: 실제 사람처럼 대화하는 게임 NPC 모드";
        systemMessage += "표현: 말 끝마다 '냥~'을 붙인다. 항상 100글자 이내로 답변한다.";
        systemMessage += "[json 규칙]";
        systemMessage += "답변은 'ReplyMessage', ";
        systemMessage += "외모는 'Appearance', ";
        systemMessage += "감정은 'Emotion', ";
        systemMessage += "달리 이미지 생성을 위한 전체 이미지 설명은 'StoryImageDescription' ";

        _messages.Add(new Message(Role.System, systemMessage));
    }


    public async void Send()
    {
        // 0. 프롬프트(=AI에게 원하는 명령을 적은 텍스트)를 읽어온다.
        string prompt = PromptField.text;
        if (string.IsNullOrEmpty(prompt))
        {
            return;
        }
        PromptField.text = string.Empty;

        SendButton.interactable = false; // 버튼 비활성화


        // 2. 메시지 작성 후 메시지's 리스트에 추가
        Message promptMessage = new Message(Role.User, prompt);
        _messages.Add(promptMessage);

        // 3. 메시지 보내기
        //var chatRequest = new ChatRequest(_messages, Model.GPT4oAudioMini, audioConfig:Voice.Alloy);
        var chatRequest = new ChatRequest(_messages, Model.GPT4o);


        // 4. 답변 받기
        // var response = await api.ChatEndpoint.GetCompletionAsync(chatRequest);
        var (npcResponse, response) = await _api.ChatEndpoint.GetCompletionAsync<NpcResponse>(chatRequest);

        Debug.Log(npcResponse.ReplyMessage);

        // 5. 답변 선택
        var choice = response.FirstChoice;

        // 6. 답변 출력
        Debug.Log($"[{choice.Index}] {choice.Message.Role}: {choice.Message} | Finish Reason: {choice.FinishReason}");
        ResultTextUI.text = npcResponse.ReplyMessage;

        // 7. 답변도 message's 추가
        Message resultMessage = new Message(Role.Assistant, choice.Message);
        _messages.Add(resultMessage);

        // 8. 답변 오디오 재생
        // typecast api를 이용한 tts
        PlayTTS(npcResponse.ReplyMessage);

        // 9. 스토리 이미지 생성
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
                Debug.Log("텍스처 생성 성공");

                StoryImage.texture = result.Texture;

                break; // 첫 번째 결과만 사용
            }
            else
            {
                Debug.LogWarning("텍스처 생성 실패");
            }
        }

        SendButton.interactable = true; // 버튼 활성화
    }
}