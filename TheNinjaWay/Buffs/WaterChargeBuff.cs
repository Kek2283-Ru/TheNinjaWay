using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TheNinjaWay.Buffs
{
    public class WaterChargeBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
    
    public class WaterChargePlayer : ModPlayer
    {
        private int waterShotCount = 0;
        
        public override void PostUpdate()
        {
            if (Player.HasBuff(ModContent.BuffType<WaterChargeBuff>()))
            {
                Player.DelBuff(Player.FindBuffIndex(ModContent.BuffType<WaterChargeBuff>()));
                
                Vector2 direction = Main.MouseWorld - Player.Center;
                direction.Normalize();
                Vector2 spawnPos = Player.Center + direction * 20f;
                
                waterShotCount++;
                
                Projectile.NewProjectile(Player.GetSource_FromThis(), 
                    spawnPos, 
                    direction * 12f, 
                    ModContent.ProjectileType<Projectiles.WaterProjectile>(), 
                    20, 
                    5f, 
                    Player.whoAmI);
                
                Main.NewText($"💧 Водяная пуля {waterShotCount + 1}!", Color.DodgerBlue);
                
                if (waterShotCount < 2)
                {
                    Player.AddBuff(ModContent.BuffType<WaterChargeBuff>(), 45);
                }
                else
                {
                    waterShotCount = 0;
                }
            }
        }
    }
}