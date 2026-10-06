using System;
using System.Collections.Generic;
using System.Text;

namespace PubKarelia.Client.Dto;

public class GuestBookEntry
{
    public GuestBookEntry(string title, string myMessage, string author)
    {
        Title = title;
        MyMessage = myMessage;
        Author = author;
    }

    public string Title { get; set; } = string.Empty;
    public string MyMessage { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
}
