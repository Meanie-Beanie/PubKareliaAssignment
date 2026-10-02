using System;
using System.Collections.Generic;
using System.Text;

namespace PubKarelia.Client.Dto;

public class CurrentPositionDto
{
    public bool IsEmpty { get; set; }
    public int X { get; set; }
    public int Y { get; set; }

}
