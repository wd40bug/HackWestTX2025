using System.Text.RegularExpressions;

public struct LoveResults
{
    public double love_percentage;
}
public struct Message
{
    public DateTime Time;
    public string Content;
    public bool Self;

    public Message(DateTime time, string content, bool self)
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

    public string GetContent(int messageIndex)
    {
        return MessageLog[messageIndex].Content;
    }

    // Sees if a message contains "hey" and returns the number of extra ys (not including the initial one)
    public int FindYCount(string message)
    {
        int yCount = 0;
        message = message.ToLower();
        Match match = Regex.Match(message, @"\bheyy+");
        if (match.Success)
        {
            int index = match.Index;
            int i = 3;
            // count number of extra ys
            while (index + i < message.Length && message[index + i] == 'y')
            {
                yCount++;
                i++;
            }
            return yCount;
        }
        else
        {
            return 0;
        }
    }

    // return number of instances of "<3" in a message
    public int FindHeartCount(string message)
    {
        MatchCollection matches = Regex.Matches(message, "<3");
        int heartCount = matches.Count;
        return heartCount;
    }

    public int FindEmojiCount(string message)
    {
        string emojiPattern = "(\u00a9|\u00ae|[\u2000-\u3300]|\ud83c[\ud000-\udfff]|\ud83d[\ud000-\udfff]|\ud83e[\ud000-\udfff])";
        int emojiCount = Regex.Count(message, emojiPattern);
        return emojiCount;
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
            if (responseIndex >= MessageLog.Count)
            {
                // End iteration and return impossible value if last message is from user (no response from other person)
                responseTime = new TimeSpan(-1, -1, -1);
                return responseTime;
            }
        }
        responseTime = MessageLog[responseIndex].Time.Subtract(MessageLog[messageIndex].Time);
        return responseTime;
    }

    // Count up all the statistics
    public double FindAverageResponseTime()
    {
        int extraYCount = 0;
        int heartCount = 0;
        int emojiCount = 0;
        int otherMessageCount = 0;
        int userMessageCount = 0;
        double responseTimeSum = 0;
        // number of responses
        int responseSum = 0;
        bool moreMessages = true;
        int messageIndex = 0;
        int userStartIndex = 0;

        List<double> responseTimeList = new List<double>();

        while (moreMessages && messageIndex < MessageLog.Count)
        {
            Message currentMessage = MessageLog[messageIndex];
            // Test if first message is from user
            if (messageIndex == 0 && currentMessage.Self)
            {
                userStartIndex = 0;
            }
            // Test if user initiated a conversation
            else if (currentMessage.Self && !MessageLog[messageIndex - 1].Self)
            {
                userStartIndex = messageIndex;
            }
            // Test for other person's response
            else if (messageIndex != 0 && !currentMessage.Self && MessageLog[messageIndex - 1].Self)
            {
                // test if message log ends
                TimeSpan responseTime = TimeBetweenResponse(userStartIndex);
                if (responseTime.Ticks <= 0)
                {
                    moreMessages = false;
                }
                else
                {
                    responseTimeList.Add(responseTime.TotalSeconds);
                    responseTimeSum += responseTime.TotalSeconds;
                    responseSum++;
                }
            }

            // For stats about other person
            if (!currentMessage.Self)
            {
                otherMessageCount++;
                extraYCount += FindYCount(currentMessage.Content);
                heartCount += FindHeartCount(currentMessage.Content);
                emojiCount += FindEmojiCount(currentMessage.Content);
            }
            if (currentMessage.Self)
            {
                userMessageCount++;
            }



            messageIndex++;
        }
        double averageResponseTime = responseTimeSum / responseSum;
        Console.WriteLine("Number of messages from other person: " + otherMessageCount + "\nNumber of messages from user: " + userMessageCount);
        Console.WriteLine("Average response time: " + averageResponseTime);
        Console.WriteLine("Total number of extra ys: " + extraYCount);
        Console.WriteLine("Total number of <3s: " + heartCount);
        Console.WriteLine("Total number of emojis: " + emojiCount);
        return averageResponseTime;
    }
}
