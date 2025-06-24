using TMPro;
using UnityEngine;

public class ChatBubbleUI : MonoBehaviour
{
    public TMP_Text speakerNameText;
    public TMP_Text dialogueText;

    public void SetData(string speaker, string dialogue)
    {
        speakerNameText.text = speaker;
        dialogueText.text = $"¡°{dialogue}¡±";
    }
}
