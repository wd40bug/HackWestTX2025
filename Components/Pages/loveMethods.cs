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
    public TimeSpan TimeBetweenResponse(int messageIndex)
    {
        int responseIndex = messageIndex;
        TimeSpan responseTime;
        // loop until there is a response or end of log is reached
        while (MessageLog[responseIndex].Self)
        {
            responseIndex++;
            if (responseIndex > MessageLog.Count)
            {
                // End iteration and return impossible value if last message is from user (no response from other person)
                responseTime = new TimeSpan(-1, -1, -1);
                return responseTime;
            }
        }
        responseTime = MessageLog[responseIndex].Time.Subtract(MessageLog[messageIndex].Time);
        return responseTime;
    }

    public List<TimeSpan> MakeResponseTimeList()
    {
        List<TimeSpan> responseTimeList = new List<TimeSpan>();
        bool moreMessages = true;
        int messageIndex = 0;
        while (moreMessages && messageIndex < MessageLog.Count)
        {
            Message currentMessage = MessageLog[messageIndex];
            //Only count when user initiates convo
            if (!currentMessage.Self)
            {
                messageIndex++;
                continue;
            }
            // check if last message in log
            if (TimeBetweenResponse(messageIndex).Ticks < 0)
            {
                moreMessages = false;
                break;
            }
            responseTimeList.Add(TimeBetweenResponse(messageIndex));
            //Wait for next instance of conversation initiation
            while (MessageLog[messageIndex].Self && messageIndex < MessageLog.Count)
            {
                messageIndex++;
            }
        }
        return responseTimeList;
    }

    public double FindAverageResponseTime()
    {
        double sum = 0;
        List<TimeSpan> responseTimeList = MakeResponseTimeList();
        for (int i = 0; i < responseTimeList.Count; i++)
        {
            sum += responseTimeList[i].TotalSeconds;
        }
        double averageResponseTime = sum / responseTimeList.Count;
        return averageResponseTime;
    }
}
