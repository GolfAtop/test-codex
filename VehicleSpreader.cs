using System;
using System.Windows.Forms;
using Rage;
using Rage.Attributes;
using Rage.Native;

[assembly: Plugin("VehicleSpreader", Author = "Codex", Description = "Moves nearby vehicles when F10 is pressed")]

namespace VehicleSpreader
{
    public static class EntryPoint
    {
        private static bool _terminate;

        public static void Main()
        {
            Game.LogTrivial("VehicleSpreader plugin initialized");
            GameFiber.StartNew(GameLoop);
        }

        private static void GameLoop()
        {
            while (!_terminate)
            {
                GameFiber.Yield();
                if (Game.IsKeyDown(Keys.F10))
                {
                    MoveNearbyVehicles();
                }
            }
        }

        private static void MoveNearbyVehicles()
        {
            try
            {
                Ped player = Game.LocalPlayer.Character;
                var vehicles = World.GetEntities<Vehicle>(player.Position, 20f, GetEntitiesFlags.ConsiderGroundVehicles);

                foreach (var veh in vehicles)
                {
                    if (!veh || !veh.Exists() || veh == player.CurrentVehicle)
                        continue;

                    Vector3 forward = veh.ForwardVector;
                    Vector3 side = veh.RightVector * 0.5f;
                    veh.Velocity = forward * 5f + side;
                }
            }
            catch (Exception e)
            {
                Game.LogTrivial($"VehicleSpreader error: {e.Message}");
            }
        }

        public static void Finally()
        {
            _terminate = true;
            Game.LogTrivial("VehicleSpreader plugin terminated");
        }
    }
}
