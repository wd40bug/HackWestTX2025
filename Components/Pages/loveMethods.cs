public struct Message
{
    public DateTime Time;
    public String Content;
    public bool Self;

    public Message(DateTime time, String content, bool self)
    {
        Time = time;
        Content = content;
        Self = self;
    }
}

public class ChatLog(List<Message> messageLog)
{
    private List<Message> MessageLog { get; set; } = messageLog;

    public DateTime GetDateTime(int messageIndex)
    {
        return MessageLog[messageIndex].Time;
    }

    public bool GetSelf(int messageIndex)
    {
        return MessageLog[messageIndex].Self;
    }

    public String GetContent(int messageIndex)
    {
        return MessageLog[messageIndex].Content;
    }

    // Returns the amount of time it took for the other person to respond. Takes index of initial message as parameter
    // public TimeSpan TimeBetweenResponse(int messageIndex)
    // {
    //     int responseIndex = messageIndex;
    //     TimeSpan responseTime;
    //     // loop until there is a response or end of log is reached
    //     while (MessageLog[responseIndex].Self)
    //     {
    //         responseIndex++;
    //         if (responseIndex > MessageLog.Count)
    //         {
    //             responseTime = new TimeSpan(-1, -1, -1);
    //             return responseTime;
    //         }
    //     }
    //     responseTime = MessageLog(messageIndex).Subtra

    // }
}
