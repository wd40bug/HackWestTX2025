// i love git :)
using System.Text.RegularExpressions;

public struct LoveResults
{
    public double love_percentage;
    public int extraYCount;
    public int heartCount;
    public int emojiCount;
    public int otherMessageCount;
    public int userMessageCount;
    public double averageResponseTime;
    public int powerWordCount;
    public int powerPhraseCount;
    public int powerAbbrevCount;
}

public struct Message
{
    public DateTime Time;
    public string Content;
    public bool Self;
    public List<string> Emojis;

    public Message(DateTime time, string content, bool self, List<string> emojis)
    {
        Time = time;
        Content = content;
        Self = self;
        Emojis = emojis;
    }
}

public class ChatLog(List<Message> messageLog)
{
    private List<Message> MessageLog { get; set; } = messageLog;
    DateTime firstDay = DateTime.MaxValue;
    DateTime lastDay = DateTime.MinValue;

    private DateTime GetDateTime(int messageIndex)
    {
        return MessageLog[messageIndex].Time;
    }

    private bool GetSelf(int messageIndex)
    {
        return MessageLog[messageIndex].Self;
    }

    private string GetContent(int messageIndex)
    {
        return MessageLog[messageIndex].Content;
    }

    // Sees if a message contains "hey" and returns the number of extra ys (not including the initial one)
    private int FindYCount(string message)
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
    private int FindHeartCount(string message)
    {
        MatchCollection matches = Regex.Matches(message, "<3");
        int heartCount = matches.Count;
        return heartCount;
    }

    // return number of instances of ";)" in a message
    //ella code

    public int FindWinkyCount(string message)
    {
        string escapedMsg = Regex.Escape(message);
        string escapedSmiley = Regex.Escape(";)");
        MatchCollection matches = Regex.Matches(escapedMsg, escapedSmiley);
        int winkyCount = matches.Count;
        return winkyCount;
    }
    //

   
    private int FindEmojiCount(Message message)
    {
        int emojiCount = 0;
        foreach (var emoji in message.Emojis)
        {
            emojiCount++;
        }
        return emojiCount;
    }

    

    // Returns the amount of time it took for the other person to respond. Takes index of initial message as parameter
    private TimeSpan TimeBetweenResponse(int messageIndex)
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

    //find first and last day of messages by checking if the message is earlier/later than earliest/latest date
    //ella code
    public void CountDays(int messageIndex)
    {
        DateTime dateTime = MessageLog[messageIndex].Time;
        if (dateTime.CompareTo(firstDay) < 0)
        {
            //Console.WriteLine("old earliest date " + firstDay);
            firstDay = dateTime;
            Console.WriteLine("new earliest date " + firstDay);
        }
        if (dateTime.CompareTo(lastDay) > 0)
        {
            //Console.WriteLine("old latest date " + lastDay);
            lastDay = dateTime;
            Console.WriteLine("new latest date " + lastDay);
        }
    }
    //
    private int FindPowerPhraseCount(string message)
    {
        string lowMessage = message.ToLower();
        int powerPhraseCount = 0;
        List<string> powerPhraseList = new List<String>() {"good morning", "good night", "i love you", "i love u", "goodmorning", "goodnight"};

        foreach (string powerWord in powerPhraseList)
        {
            powerPhraseCount += Regex.Matches(lowMessage, powerWord).Count;
        }
        return powerPhraseCount;
    }
    private int FindPowerAbbrevCount(string message)
    {
        string lowMessage = message.ToLower();
        int powerAbbrevCount = 0;
        List<string> powerAbbrevList = new List<String>() {"ily", "gm", "gn"};

        foreach (string powerWord in powerAbbrevList)
        {
            powerAbbrevCount += Regex.Matches(lowMessage, powerWord).Count;
        }
        return powerAbbrevCount;
    }
    private int FindPowerWordCount(string message)
    {
        string lowMessage = message.ToLower();
        int powerWordCount = 0;
        List<string> powerWordList = new List<String>() {"love", "hot", "sexy", "lol", "beautiful", "sugar", "heart", "dear", "honey", "hun", "hon", "haha",
        "sweet", "cute", "pretty", "handsome"};

        foreach (string powerWord in powerWordList)
        {
            powerWordCount += Regex.Matches(lowMessage, powerWord).Count;
        }
        return powerWordCount;
    }

    // Count up all the FindStats
    public double FindStats()
    {
        int extraYCount = 0;
        int heartCount = 0;
        int winkyCount = 0;
        int emojiCount = 0;
        int otherMessageCount = 0;
        int userMessageCount = 0;
        double responseTimeSum = 0;
        int powerWordCount = 0;
        // number of responses
        int responseSum = 0;
        bool moreMessages = true;
        int messageIndex = 0;
        int userStartIndex = 0;
        //ella code
        int powerAbbrevCount = 0;
        int numDays = 0;
        float msgsPerDay = 0;
        int powerPhraseCount = 0;
        //

        List<double> responseTimeList = new List<double>();

        while (moreMessages && messageIndex < MessageLog.Count)
        {
            Message currentMessage = MessageLog[messageIndex];
            //ella code
            
            //check date and update
            CountDays(messageIndex);
            //
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
                winkyCount += FindWinkyCount(currentMessage.Content);
                emojiCount += FindEmojiCount(currentMessage);
                powerWordCount += FindPowerWordCount(currentMessage.Content);
                powerAbbrevCount += FindPowerAbbrevCount(currentMessage.Content);
                powerPhraseCount += FindPowerPhraseCount(currentMessage.Content);
            }
            if (currentMessage.Self)
            {
                userMessageCount++;
            }

            messageIndex++;
        }

        double averageResponseTime = responseTimeSum / responseSum;
        //ella code
        numDays = (lastDay.Date - firstDay.Date).Days;
        msgsPerDay = (float)otherMessageCount / numDays;
        //
        Console.WriteLine("Number of messages from other person: " + otherMessageCount + "\nNumber of messages from user: " + userMessageCount);
        Console.WriteLine("Average response time: " + averageResponseTime);
        Console.WriteLine("Total number of extra ys: " + extraYCount);
        Console.WriteLine("Total number of <3s: " + heartCount);
        Console.WriteLine("Total number of emojis: " + emojiCount);
        //ella code
        Console.WriteLine("numDays: " + numDays);
        Console.WriteLine("Messages per day from other person: " + msgsPerDay);
        Console.WriteLine("Total number of ;)s: " + winkyCount);
        Console.WriteLine("Total number of spelled out phrases: " + powerPhraseCount + " vs abbreviated: " + powerAbbrevCount);
        //
        Console.WriteLine("Total number of power words: " + powerWordCount);
        return averageResponseTime;
    }
}
