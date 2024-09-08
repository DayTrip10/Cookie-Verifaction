using LabFusion.Player;
using System.Linq;

namespace cookieverifier
{
    public static class WhitelistManager
    {
        private static readonly string[] whitelistedIds =
        {
            "76561198889496180", // Add more Steam IDs to this array if necessary
        };

        // Returns true if the player's Steam ID is in the whitelist
        public static bool IsPlayerVerified()
        {
            if (PlayerIdManager.LocalLongId == 0)
            {
                return false;
            }

            string localIdString = PlayerIdManager.LocalLongId.ToString();
            return whitelistedIds.Contains(localIdString);
        }
    }
}
