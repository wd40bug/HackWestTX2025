using System.Text.Json;
using System.IO;
using System;
using System.Security.Cryptography.X509Certificates;
//using Message;

class FileHandle()
{
    static void Main(string[] args)
    {


    public int userId;
    string fileName;
    string jsonString = File.ReadAllText(fileName);
    DMData myData = JsonSerializer.Deserialize<DMData>(jsonString);

    //List<Message> messages = myData.select;}






    }
}

public class DMData {
    public RawMessage message { get; set; }
}
public class RawMessage
{
    public Author author { get; set; }
    public string content { get; set; }
    public DateTime timestamp{ get; set; }
}

public class Author {
    public long id { get; set; }
}