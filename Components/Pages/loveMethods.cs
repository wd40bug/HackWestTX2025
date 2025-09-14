using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

public struct LovePercentageMeaning
{
    public string message;
    public string color;
    public LovePercentageMeaning(double percentage)
    {
        if (percentage >= 95)
        {
            color = "#f702ef"; message = "Marry Me!"; return;
        }
        else if (percentage >= 90)
        {
            color = "#02f738"; message = "Are you free this friday?"; return;
        }
        else if (percentage >= 80)
        {
            color = "#94e32d"; message = "Maybe this could be something"; return;
        }
        else if (percentage >= 70)
        {
            color = "#bae643"; message = "I think of you as a really good friend"; return;
        }
        else if (percentage >= 60)
        {
            color = "#d2e643"; message = "I don't think romance is in the cards"; return;
        }
        else if (percentage >= 50)
        {
            color = "#e6ca43"; message = "You're a pleasant aquaintance"; return;
        }
        else if (percentage >= 30)
        {
            color = "#e6a743"; message = "Why do we even talk?"; return;
        }
        else if (percentage >= 10)
        {
            color = "#e67643"; message = "Who is this?"; return;
        }
        else
        {
            color = "red"; message = "I HATE YOU!"; return;
        }
    }
}

public class WeightedResult<T>(T value, double weight)
{
    public T Value = value;
    public double Weight = weight;
}
public struct LoveResults
{
    public double Love_percentage;
    public LovePercentageMeaning Meaning;
    public WeightedResult<int> ExtraYCount;
    public WeightedResult<int> HeartCount;
    public WeightedResult<int> EmojiCount;
    public WeightedResult<int> OtherMessageCount;
    public WeightedResult<int> UserMessageCount;
    public WeightedResult<double> AverageResponseTime;
    public WeightedResult<int> PowerWordCount;
    public WeightedResult<int> PeriodEndCount;
    public WeightedResult<double> AverageMessagesPerDay;
    public WeightedResult<int> PowerPhraseCount;
    public WeightedResult<int> PowerAbbrevCount;
    public WeightedResult<int> WinkyCount;
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
        List<string> powerWordList = new List<String>() {"love", "ily", "heart", "dear", "honey", "hun", "hon", "haha",
        "sweet", "cute", "pretty", "handsome", "beautiful", "hot", "sexy"};

        foreach (string powerWord in powerWordList)
        {
            powerWordCount += Regex.Matches(lowMessage, powerWord).Count;
        }
        return powerWordCount;
    }

    // Count up the number of times the other user ended a message with a period
    private int MessageEndsInPeriodTest(string message)
    {
        if (message.EndsWith("."))
        {
            return 1;
        }
        return 0;
    }

    private double CalculateLovePercent(LoveResults results)
    {
        double lovePercent = 0;


        // Weights
        // Average Response time       20       done
        // Messages per day            21       done
        // powerWordRatio              16       done
        // powerPhraseRatio            17       done
        // heartCount + winkyCount     4        done
        // extraYCount                 7       done
        // emojiCount                  10        done
        // periodEndCount              -10      done
        // powerPhrase/PowerAbbreviation 5      done


        double averageResponseTime = results.AverageResponseTime;
        double messagesPerDay = results.AverageMessagesPerDay;
        int powerWordCount = results.PowerWordCount;
        int otherMessageCount = results.OtherMessageCount;
        double messagePowerWordRatio = (double)otherMessageCount / (double)powerWordCount;
        int powerPhraseCount = results.PowerPhraseCount;
        int powerAbbrevCount = results.PowerAbbrevCount;
        double phraseAbbrevRatio;
        if (powerAbbrevCount != 0)
        {
            phraseAbbrevRatio = (double)powerPhraseCount / (double)powerAbbrevCount;
        }
        else if (powerPhraseCount != 0)
        {
            phraseAbbrevRatio = 1;
        }
        else
        {
            phraseAbbrevRatio = 0;
        }
        double messagePhraseRatio = (double)otherMessageCount / (double)powerPhraseCount;
        double heartWinkCount = results.HeartCount + results.WinkyCount;
        int extraYCount = results.ExtraYCount;
        double emojiMessageRatio = (double)results.EmojiCount / (double)otherMessageCount;
        double periodEndMessageRatio = (double)results.PeriodEndCount / (double)otherMessageCount;

        if (averageResponseTime < 900)
        {
            lovePercent += 20;
        }
        else if (averageResponseTime < 1600)
        {
            lovePercent += 15;
        }
        else if (averageResponseTime < 2000)
        {
            lovePercent += 10;
        }
        else
        {
            lovePercent += 20000.0 / averageResponseTime;
        }

        // + 21% if messages per day > 40
        if (messagesPerDay > 40.0)
        {
            lovePercent += 21;
        }
        else
        {
            lovePercent += .525 / (1 / messagesPerDay);
        }

        // + 16% if message/powerWordRatio < 10
        if (messagePowerWordRatio < 10)
        {
            lovePercent += 16;
        }
        else
        {
            lovePercent += 160.0 / (messagePowerWordRatio);
        }

        // + 5% if phraseAbbrevRatio > 4.25
        if (phraseAbbrevRatio > 4.25)
        {
            lovePercent += 5;
        }
        else
        {
            lovePercent += 1.17647 / (1 / phraseAbbrevRatio);
        }

        // + 17% if messagePhraseRatio < 23
        if (messagePhraseRatio < 23)
        {
            lovePercent += 17;
        }
        else
        {
            lovePercent += 391.0 / messagePhraseRatio;
        }

        // + 4% if heart + winky > 15
        if (heartWinkCount > 15)
        {
            lovePercent += 4;
        }
        else
        {
            lovePercent += .26 * heartWinkCount;
        }

        // + 7% if > 7 extra ys
        if (extraYCount > 7)
        {
            lovePercent += 7;
        }
        else
        {
            lovePercent += 1 * extraYCount;
        }

        // + 10% if emojiMessageRatio > .75
        if (emojiMessageRatio > .75)
        {
            lovePercent += 10;
        }
        else
        {
            lovePercent += 13.3333 * emojiMessageRatio;
        }

        // up to -10% for periodEndMessageRatio
        if (periodEndMessageRatio > .15)
        {
            double percentLoss = (periodEndMessageRatio - .15 * 100);
            if (percentLoss > 10)
            {
                percentLoss = 10;
            }
            lovePercent -= percentLoss;
        }

        results.Love_percentage = lovePercent;
        return lovePercent;
    }

    // Count up all the FindStats
    public LoveResults FindStats()
    {
        int extraYCount = 0;
        int heartCount = 0;
        int winkyCount = 0;
        int emojiCount = 0;
        int otherMessageCount = 0;
        int userMessageCount = 0;
        double responseTimeSum = 0;
        int powerWordCount = 0;
        int periodEndCount = 0;
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

        var responseTimeList = new List<double>();

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
                periodEndCount += MessageEndsInPeriodTest(currentMessage.Content);
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

        LoveResults results = new LoveResults
        {
            Love_percentage = 0,
            ExtraYCount = extraYCount,
            HeartCount = heartCount,
            EmojiCount = emojiCount,
            OtherMessageCount = otherMessageCount,
            UserMessageCount = userMessageCount,
            AverageResponseTime = averageResponseTime,
            PowerWordCount = powerWordCount,
            PeriodEndCount = periodEndCount,
            AverageMessagesPerDay = msgsPerDay,
            PowerPhraseCount = powerPhraseCount,
            PowerAbbrevCount = powerAbbrevCount,
            WinkyCount = winkyCount
        };

        results.Love_percentage = CalculateLovePercent(results);

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
        Console.WriteLine("Number of times other person ended a message with a period: " + periodEndCount);
        Console.WriteLine("Total love percent: " + results.Love_percentage);
        return results;
    }
}
