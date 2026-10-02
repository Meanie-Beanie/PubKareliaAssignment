using PubKarelia.Client.Game;

namespace PubKarelia.Client;

internal class App
{
    readonly HttpClient _httpClient; // Maybe comes in useful, keeping it in for now.
    readonly GameClient _gameClient;

    public App(HttpClient httpClient, GameClient gameClient)
    {
        _httpClient = httpClient;
        _gameClient = gameClient;
    }

    public async Task Start()
    {
        while (true)
        {
            Console.WriteLine("Press \"1\" to run all required assignments in succession. \n Press \"2\" to reset progression. \n Press any other key to exit.");

            if (int.TryParse(Console.ReadLine(), out var input))
                Console.WriteLine("Input has to be number.");

            if (input == 1)
            {
                RunAssignments();
            }

            else if (input == 2)
            {
                ResetProgression();
            }

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

    private void RunAssignments()
    {
        throw new NotImplementedException();
    }

    private void ResetProgression()
    {
            throw new NotImplementedException();
    }
}