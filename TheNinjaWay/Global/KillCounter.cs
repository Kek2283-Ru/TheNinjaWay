using Terraria;
using Terraria.ModLoader;

namespace TheNinjaWay.Global
{
    public class KillCounter : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            Player player = Main.LocalPlayer;
            if (player != null)
            {
                TheNinjaWayPlayer modPlayer = player.GetModPlayer<TheNinjaWayPlayer>();
                modPlayer.OnKillNPC(npc);
            }
        }
    }
}