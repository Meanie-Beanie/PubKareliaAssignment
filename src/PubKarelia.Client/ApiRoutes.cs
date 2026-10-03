using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PubKarelia.Client;

internal static class ApiRoutes
{
    public const string Base = "https://pubkareliaapi20250319124213-g2f5c0apewdcb7ae.northeurope-01.azurewebsites.net/PubKarelia";

    // GET

    public const string CurrentPosition = Base + "/CurrentPosition";
    public const string GetJoke = Base + "/GetJoke";
    public const string ThrowDart = Base + "/ThrowDart";
    public const string GetMoominLemonade = Base + "/GetMoominLemonade";
    //public const string OrderPizza = Base + "/OrderPizza";

    public static string OrderPizza(int toppingsCount)
        => $"{Base + "/OrderPizza"}?ToppingsCount={toppingsCount}";


    public const string ReadMessages = Base + "/ReadMessages";

    // PUT
    public static string Move(int x, int y)
        => $"{Base + "/Move"}?X={x}&Y={y}";

    public const string WC = Base + "/WC";

    // POST
    public const string WriteMessage = Base + "/WriteMessage";

    // DELETE
    public const string WCFlush = Base + "/WCFlush";
    public const string Reset = Base + "/Reset";

}
