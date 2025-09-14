using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public struct LovePercentageMeaning
{


    public string message;
    public string color;
    public int flower;
    public LovePercentageMeaning(double percentage)
    {
        if (percentage >= 95)
        {
            color = "#f702ef"; message = "Marry Me!"; flower = 1; return;
        }
        else if (percentage >= 90)
        {
            color = "#02f738"; message = "Are you free this friday?"; flower = 1; return;
        }
        else if (percentage >= 80)
        {
            color = "#94e32d"; message = "I think there's something here!"; flower = 1; return;
        }
        else if (percentage >= 70)
        {
            color = "#bae643"; message = "Maybe this could be something"; flower = 2; return;
        }
        else if (percentage >= 60)
        {
            color = "#d2e643"; message = "I think of you as a really good friend"; flower = 2; return;
        }
        else if (percentage >= 50)
        {
            color = "#e6ca43"; message = "You're a pleasant aquaintance"; flower = 3; return;
        }
        else if (percentage >= 30)
        {
            color = "#e6a743"; message = "Why do we even talk?"; flower = 4; return;
        }
        else if (percentage >= 10)
        {
            color = "#e67643"; message = "Who is this?"; flower = 5; return;
        }
        else
        {
            color = "red"; message = "I HATE YOU!"; flower = 5; return;
        }
    }
}

public class WeightedResult<T>(T value, double weight)
{
    public T Value = value;
    public double Weight = weight;
}
public class LoveResults
{
    public double Love_percentage;
    public LovePercentageMeaning Meaning;
    required public WeightedResult<int> ExtraYCount;
    required public WeightedResult<int> HeartCount;
    required public WeightedResult<int> EmojiCount;
    required public int OtherMessageCount;
    required public int UserMessageCount;
    required public WeightedResult<double> AverageResponseTime;
    required public WeightedResult<int> PowerWordCount;
    required public WeightedResult<int> PeriodEndCount;
    required public WeightedResult<double> AverageMessagesPerDay;
    required public WeightedResult<int> PowerPhraseCount;
    required public WeightedResult<int> PowerAbbrevCount;
    required public WeightedResult<int> WinkyCount;
    required public string mostSentEmoji;
    required public double YPerHey;
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

    Dictionary<string, int> emojiCounter = new Dictionary<string, int>();

    private int FindEmojiCount(Message message)
    {

        int emojiCount = 0;

        foreach (var emoji in message.Emojis)
        {
            try
            {
                emojiCounter[emoji]++;
            }
            catch (KeyNotFoundException)
            {
                emojiCounter[emoji] = 1;
            }

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

        TimeSpan days = new TimeSpan(3, 0, 0, 0);
        if (responseTime.CompareTo(days) > 0)
        {
            return days;

        }
        else { return responseTime; }

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
        }
        if (dateTime.CompareTo(lastDay) > 0)
        {
            //Console.WriteLine("old latest date " + lastDay);
            lastDay = dateTime;
        }
    }
    //
    private int FindPowerPhraseCount(string message)
    {
        string lowMessage = message.ToLower();
        int powerPhraseCount = 0;
        List<string> powerPhraseList = new List<String>() { "good morning", "good night", "i love you", "i love u", "goodmorning", "goodnight" };

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
        List<string> powerAbbrevList = new List<String>() { "ily", "gm", "gn" };

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


        double averageResponseTime = results.AverageResponseTime.Value;
        double messagesPerDay = results.AverageMessagesPerDay.Value;
        int powerWordCount = results.PowerWordCount.Value;
        int otherMessageCount = results.OtherMessageCount;
        double messagePowerWordRatio = (double)otherMessageCount / (double)powerWordCount;
        int powerPhraseCount = results.PowerPhraseCount.Value;
        int powerAbbrevCount = results.PowerAbbrevCount.Value;
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
        double heartWinkCount = results.HeartCount.Value + results.WinkyCount.Value;
        int extraYCount = results.ExtraYCount.Value;
        double emojiMessageRatio = (double)results.EmojiCount.Value / (double)otherMessageCount;
        double periodEndMessageRatio = (double)results.PeriodEndCount.Value / (double)otherMessageCount;

        if (averageResponseTime < 900)
        {
            results.AverageResponseTime.Weight = 20;
        }
        else if (averageResponseTime < 1600)
        {
            results.AverageResponseTime.Weight = 15;
        }
        else if (averageResponseTime < 2000)
        {
            results.AverageResponseTime.Weight = 10;
        }
        else
        {
            results.AverageResponseTime.Weight = 20000.0 / averageResponseTime;
        }

        // + 21% if messages per day > 40
        if (messagesPerDay > 40.0)
        {
            results.AverageMessagesPerDay.Weight = 21;
        }
        else
        {
            results.AverageMessagesPerDay.Weight = .525 / (1 / messagesPerDay);
        }

        // + 16% if message/powerWordRatio < 10
        if (messagePowerWordRatio < 10)
        {
            results.PowerWordCount.Weight = 16;
        }
        else
        {
            results.PowerWordCount.Weight = 160.0 / (messagePowerWordRatio);
        }

        // + 5% if phraseAbbrevRatio > 4.25
        if (phraseAbbrevRatio > 4.25)
        {
            results.PowerAbbrevCount.Weight = results.PowerPhraseCount.Weight = 5 / 2;
        }
        else
        {
            results.PowerAbbrevCount.Weight = results.PowerPhraseCount.Weight = (1.17647 / (1 / phraseAbbrevRatio)) / 2;
        }

        // + 17% if messagePhraseRatio < 23
        if (messagePhraseRatio < 23)
        {
            results.PowerPhraseCount.Weight += 17;
        }
        else
        {
            results.PowerPhraseCount.Weight += 391.0 / messagePhraseRatio;
        }

        // + 4% if heart + winky > 15
        if (heartWinkCount > 15)
        {
            results.HeartCount.Weight = results.WinkyCount.Weight = 4 / 2;
        }
        else
        {
            results.HeartCount.Weight = results.WinkyCount.Weight = .26 * heartWinkCount / 2;
        }

        // + 7% if > 7 extra ys
        if (extraYCount > 7)
        {
            results.ExtraYCount.Weight = 7;
        }
        else
        {
            results.ExtraYCount.Weight += 1 * extraYCount;
        }

        // + 10% if emojiMessageRatio > .75
        if (emojiMessageRatio > .75)
        {
            results.EmojiCount.Weight = 10;
        }
        else
        {
            results.EmojiCount.Weight = 13.3333 * emojiMessageRatio;
        }

        // up to -10% for periodEndMessageRatio
        if (periodEndMessageRatio > .15)
        {
            double percentLoss = (periodEndMessageRatio - .15 * 100);
            if (percentLoss > 10)
            {
                percentLoss = 10;
            }
            results.PeriodEndCount.Weight = -percentLoss;
        }

        results.Love_percentage = results.ExtraYCount.Weight + results.HeartCount.Weight + results.EmojiCount.Weight + results.AverageResponseTime.Weight + results.PowerWordCount.Weight + results.PeriodEndCount.Weight + results.AverageMessagesPerDay.Weight + results.PowerPhraseCount.Weight + results.PowerAbbrevCount.Weight + results.WinkyCount.Weight;
        return results.Love_percentage;
    }

    // Count up all the FindStats
    public async Task<LoveResults> FindStats(Func<double, Task> progress_callback)
    {
        await progress_callback(.1);
        int extraYCount = 0;
        int heartCount = 0;
        int winkyCount = 0;
        int emojiCount = 0;
        int otherMessageCount = 0;
        int userMessageCount = 0;
        double responseTimeSum = 0;
        int powerWordCount = 0;
        int periodEndCount = 0;
        int heyCount = 0;
        // number of responses
        int responseSum = 0;
        bool moreMessages = true;
        int messageIndex = 0;
        int userStartIndex = 0;
        //ella code

        int powerAbbrevCount = 0;
        int numDays = 0;
        double msgsPerDay = 0;
        int powerPhraseCount = 0;
        //

        var responseTimeList = new List<double>();

        while (moreMessages && messageIndex < MessageLog.Count)
        {
            await progress_callback((double)messageIndex / MessageLog.Count * 0.9 + .1);
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
                if (currentMessage.Content.ToLower().Contains("hey")) heyCount++;
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
        msgsPerDay = (double)otherMessageCount / numDays;
        string mostSent = emojiCounter.Count() == 0 ? "" : emojiCounter.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;



        WeightedResult<T> wd<T>(T val)
        {
            return new WeightedResult<T>(val, 0);
        }

        LoveResults results = new LoveResults
        {
            Love_percentage = 0,
            ExtraYCount = wd(extraYCount),
            HeartCount = wd(heartCount),
            EmojiCount = wd(emojiCount),
            OtherMessageCount = otherMessageCount,
            UserMessageCount = userMessageCount,
            AverageResponseTime = wd(averageResponseTime),
            PowerWordCount = wd(powerWordCount),
            PeriodEndCount = wd(periodEndCount),
            AverageMessagesPerDay = wd(msgsPerDay),
            PowerPhraseCount = wd(powerPhraseCount),
            PowerAbbrevCount = wd(powerAbbrevCount),
            WinkyCount = wd(winkyCount),
            mostSentEmoji = mostSent,
            YPerHey = extraYCount / heyCount
        };

        results.Meaning = new LovePercentageMeaning(CalculateLovePercent(results));

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
        Console.WriteLine("var most: " + mostSent);
        //
        Console.WriteLine("Total number of power words: " + powerWordCount);
        Console.WriteLine("Number of times other person ended a message with a period: " + periodEndCount);
        Console.WriteLine("Average number of Ys per hey: " + Math.Round((double)extraYCount / heyCount), 2);
        Console.WriteLine("Total love percent: " + results.Love_percentage);
        return results;
    }
}
