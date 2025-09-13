using System.Text.Json;
using System.IO;
using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.ComponentModel;
using Microsoft.AspNetCore.Components.Forms;
//using Message;

namespace BlazorTest.Components.Pages
{
    public class FileHandle
    {


        public static async Task<List<Message>> ParseJson(IBrowserFile file, string token)
        {
            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);

            var jsonString = await reader.ReadToEndAsync();
            DMData? myData = JsonSerializer.Deserialize<DMData>(jsonString);
            if (myData is null)
            {
                return [];
            }

            var msgs = new List<Message>(myData.messageCount);


            foreach (var message in myData.messages)
            {
                var msg = new Message
                {
                    Time = message.timestamp,
                    Self = message.author.id == token,
                    Content = message.content,
                };
                msgs.Add(msg);
            }
            return msgs;
        }
    }

    public class DMData
    {
        required public List<RawMessage> messages { get; set; }
        required public int messageCount { get; set; }
    }
    public class RawMessage
    {
        public string id { get; set; }
        public Author author { get; set; }
        public string content { get; set; }
        public DateTime timestamp { get; set; }
    }

    public class Author
    {
        public string id { get; set; }
    }
}
