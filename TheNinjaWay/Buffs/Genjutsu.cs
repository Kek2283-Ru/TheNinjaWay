using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TheNinjaWay.Buffs
{
    public class Genjutsu : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }
        
        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.velocity = Vector2.Zero;
            npc.damage = 0;
            Lighting.AddLight(npc.Center, 0.8f, 0.2f, 0.8f);
        }
    }
}