using System.Text.Json;
using System.IO;
using System;
using System.Security.Cryptography.X509Certificates;
//using Message;

public class FileHandle()
{

    static void Main(string[] args)
    {
        string fileName = "DuckWithThumbs.json";
        string jsonString = File.ReadAllText(fileName);
        DMData myData = JsonSerializer.Deserialize<DMData>(jsonString);

        //Console.WriteLine("Content: " + myData.message.content);
        //List<Message> messages = myData.selectr}
        
        
    }
    
}

public class DMData {
    public RawMessage message { get; set; }
}
public class RawMessage
{
    public long id{ get; set; }
    public Author author { get; set; }
    public string content { get; set; }
    public DateTime timestamp{ get; set; }
}

public class Author {
    public long id { get; set; }
}