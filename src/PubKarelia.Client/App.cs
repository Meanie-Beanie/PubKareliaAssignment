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
                RunAssignments();

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

    private async void RunAssignments()
    {
        //first move

        await _gameClient.Move(200, 680);
        var joke = await _gameClient.GetJoke();
        Console.WriteLine(joke.Data);

        ResetProgression();
    }

    private async void ResetProgression()
    {
        await _gameClient.Reset();
    }
}