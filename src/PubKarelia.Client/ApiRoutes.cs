using System;
using System.Collections.Generic;
using System.Text;

namespace PubKarelia.Client;

internal static class ApiRoutes
{
    public const string Base = "https://pubkareliaapi20250319124213-g2f5c0apewdcb7ae.northeurope-01.azurewebsites.net/PubKarelia";

    public const string GetJoke = Base + "/GetJoke";
}
