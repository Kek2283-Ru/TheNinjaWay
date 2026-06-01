using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TheNinjaWay.Buffs
{
    public class WaterChargeBuff3 : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
    
    public class WaterChargePlayer3 : ModPlayer
    {
        public override void PostUpdate()
        {
            if (Player.HasBuff(ModContent.BuffType<WaterChargeBuff3>()))
            {
                Player.DelBuff(Player.FindBuffIndex(ModContent.BuffType<WaterChargeBuff3>()));
                
                Vector2 direction = Main.MouseWorld - Player.Center;
                direction.Normalize();
                Vector2 spawnPos = Player.Center + direction * 20f;
                
                Projectile.NewProjectile(Player.GetSource_FromThis(), 
                    spawnPos, 
                    direction * 12f, 
                    ModContent.ProjectileType<Projectiles.WaterProjectile>(), 
                    20, 
                    5f, 
                    Player.whoAmI);
                
                Main.NewText("💧 Водяная пуля 3!", Color.DodgerBlue);
            }
        }
    }
}