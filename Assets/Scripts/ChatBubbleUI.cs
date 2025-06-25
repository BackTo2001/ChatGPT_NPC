using TMPro;
using UnityEngine;

public class ChatBubbleUI : MonoBehaviour
{
    [Header("Containers")]
    public GameObject leftContainer;
    public GameObject rightContainer;

    [Header("Left Side")]
    public TextMeshProUGUI leftSpeaker;
    public TextMeshProUGUI leftText;

    [Header("Right Side")]
    public TextMeshProUGUI rightSpeaker;
    public TextMeshProUGUI rightText;

    public void SetData(string speaker, string message, bool isUser)
    {
        leftContainer.SetActive(!isUser);
        rightContainer.SetActive(isUser);

        if (isUser)
        {
            rightText.text = message;
            rightSpeaker.text = speaker;
        }
        else
        {
            leftText.text = message;
            leftSpeaker.text = speaker;
        }
        Debug.Log($"[SetData] speaker: {speaker}, isUser: {isUser}, message: {message}");
    }
}
