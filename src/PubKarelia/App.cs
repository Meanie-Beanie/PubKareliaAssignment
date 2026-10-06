using PubKarelia.Client.Game;

namespace PubKarelia.Client;

internal class App
{
    readonly GameClient _gameClient;

    public App(HttpClient httpClient, GameClient gameClient)
    {
        _gameClient = gameClient;
    }

    public async Task Start()
    {
        while (true)
        {
            Console.WriteLine("Press \"1\" to run all required assignments in succession. \n Press \"2\" to reset progression. \n Press any other key to exit.");

            if (!int.TryParse(Console.ReadLine(), out var input))
                Console.WriteLine("Input has to be number.");

            if (input == 1)
                await RunAssignments();

            else if (input == 2)
                ResetProgression();

            else
                break;
        }

        //while (true)
        //{
        //    // not really asked by the assignment
        //    //Console.WriteLine("Press \"1\" to run all required assignments in succession. \n Press \"2\" to see and run each command.");

        //    //if (int.TryParse(Console.ReadLine(), out var input))
        //    //    Console.WriteLine("Input has to be number.");

        //    //if (input == 1)
        //    //{
        //    //    RunAssignments();
        //    //}
        //}
    }

    private async Task RunAssignments()
    {
        // Going to the joke area and getting a joke
        await _gameClient.Move(225, 650);
        var joke = await _gameClient.GetJoke();
        Console.WriteLine("Joke: " + joke.Data);

        // Pizza order
        await _gameClient.Move(225, 500);
        await _gameClient.Move(225, 350);
        var pizza = await _gameClient.OrderPizza(1);
        Console.WriteLine("Pizza: " + pizza.Data);

        // Drink
        await _gameClient.Move(375, 350);
        await _gameClient.Move(525, 350);
        await _gameClient.Move(650, 350);
        var drink = await _gameClient.GetMoominLemonade();
        Console.WriteLine("Drink:" + drink.Data);

        // WC
        await _gameClient.Move(800, 500);
        await _gameClient.Move(900, 500);

        var wc = await _gameClient.WC();
        Console.WriteLine("WC: " + wc.Data);
        var wcFlush = await _gameClient.WCFlush();
        Console.WriteLine("WC flush: " + wcFlush.Data);

        // Guest book
        await _gameClient.Move(800, 500);
        var writtenEntry = await _gameClient.WriteMessage("Hei!", "Toivottavasti kaikilla menee hyvin!", "Olli");
        Console.WriteLine("Writing entry: " + writtenEntry.Data);

        var getEntries = await _gameClient.ReadMessages();
        var entries = (List<string>)getEntries.Data;

        foreach (var guestEntry in entries)
        {
            Console.WriteLine($"Guest book entry: {guestEntry}");
        }


        // Darts
        await _gameClient.Move(800, 640);
        var dartResult = await _gameClient.ThrowDart();
        Console.WriteLine("Dart: " + dartResult.Data);
    }

    private async void ResetProgression()
    {
        await _gameClient.Reset();
    }
}