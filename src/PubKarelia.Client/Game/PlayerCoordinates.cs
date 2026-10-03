using System;
using System.Collections.Generic;
using System.Text;

namespace PubKarelia.Client.Game;

// I have never done this before, even using regular old records is new to me, so it will be interesting to see if this even works.
// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record
public record struct PlayerCoordinates(int x, int y);