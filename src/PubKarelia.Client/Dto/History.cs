using System;
using System.Collections.Generic;
using System.Text;

namespace PubKarelia.Client.Dto;

public class History
{
    public string Result { get; set; } = "";
    public string Id { get; set; } = "";
    public string StudentId { get; set; } = "";
    public int Action { get; set; } = 0;
    public int _ts { get; set; } = 0; // ? what is this?
}
