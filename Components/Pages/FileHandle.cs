using System.Text.Json;
using System.IO;
using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Razor.TagHelpers;
//using Message;

namespace BlazorTest.Components.Pages {
    public class FileHandle
    {


        void ParseJson()
        {
            int authorId = 10;
            DMData myData;
            string fileName = "DuckWithThumbs.json";

            using StreamReader reader = new StreamReader(fileName);
            
            string jsonString = reader.ReadToEnd();
            myData = JsonSerializer.Deserialize<DMData>(jsonString);

            List<Message> msgs = new List<Message>(new Message[myData.messageCount]);

            for (int i = 0; i < myData.messageCount; i++)
            {
                Message temp = new Message();

                temp.Content = myData.message.content;
                temp.Time = myData.message.timestamp;

                if (myData.message.author.id == authorId)
                {
                    temp.Self = true;
                }
                else
                {
                    temp.Self = false;
                }

                msgs[i] = temp;
            }
            
        }
        
         
        
    
}

public class DMData
{
        public RawMessage message { get; set; }
        public int messageCount{ get; set; }
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
}
