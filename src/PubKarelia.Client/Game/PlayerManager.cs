using System;
using System.Collections.Generic;
using System.Text;

namespace PubKarelia.Client.Game;

internal class PlayerManager
{
    public bool HasCompletedFirstMove { get; private set; } = false;

    // How many pixels can a player move in a turn OUTSIDE first move.
    public int maxMovement { get; } = 50;

    public Coordinates PlayerCoordinates { get; private set; } = new(X: 500, Y: 900);
    public Coordinates MaximumMapSize { get; init; } = new Coordinates(X: 1000, Y: 1000);


    public PlayerManager(Coordinates playerCoordinates)
    {
        PlayerCoordinates = playerCoordinates;
    }

    public void UpdatePlayerCoordinates(Coordinates playerCoordinates)
        => PlayerCoordinates = playerCoordinates;

    public bool CanMove(int distanceX, int distanceY)
    {
        // Movement only allowed within 0 to 1000 range.
        if (distanceX < 0 || distanceY < 0)
            return false;

        // only First move can go past Max Movement
        if (HasCompletedFirstMove && distanceX > maxMovement || distanceY > maxMovement)
            return false;

        var desiredLocation = new Coordinates(distanceX + PlayerCoordinates.X, distanceY + PlayerCoordinates.Y);

        // If first move has not been completed, it has special rules and we'll check them here.
        if (!HasCompletedFirstMove)
        {
            // Since first move can be as long as they want, we allow them to move anywhere they want within borders.
            if (CheckIfWithinMapBounds(desiredLocation))
                return true;

            else
                return false;
        }

        // We'll check if desired location of the player is within the playlimits first.
        if (!CheckIfWithinMapBounds(desiredLocation))
            return false;

        else
        {
            return true;
        }
    }

    private bool CheckIfWithinMapBounds(Coordinates desiredLocation)
    {
        if (desiredLocation.X > MaximumMapSize.X || desiredLocation.X < 0)
            return false;

        else if (desiredLocation.Y > MaximumMapSize.Y || desiredLocation.Y < 0)
            return false;

        else
            return true;
    }
}
