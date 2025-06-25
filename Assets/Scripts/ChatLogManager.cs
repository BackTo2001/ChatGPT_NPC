using System.Collections.Generic;

public static class ChatLogManager
{
    public static List<(string speaker, string message)> ChatLogs { get; private set; } = new();

    public static void AddLog(string speaker, string message)
    {
        ChatLogs.Add((speaker, message));
    }

    public static void Clear()
    {
        ChatLogs.Clear();
    }
}
