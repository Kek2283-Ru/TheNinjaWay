using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TheNinjaWay.Buffs
{
    public class FireChargeBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }
    }
    
    public class FireChargePlayer : ModPlayer
    {
        public override void PostUpdate()
        {
            if (Player.HasBuff(ModContent.BuffType<FireChargeBuff>()))
            {
                Player.DelBuff(Player.FindBuffIndex(ModContent.BuffType<FireChargeBuff>()));
                
                Vector2 direction = Main.MouseWorld - Player.Center;
                direction.Normalize();
                Vector2 gatherPoint = Player.Center + direction * 80f;
                
                Projectile.NewProjectile(Player.GetSource_FromThis(), 
                    gatherPoint, 
                    direction * 12f, 
                    ModContent.ProjectileType<Projectiles.FireBallProjectile>(), 
                    45, 
                    8f, 
                    Player.whoAmI);
                    
                Main.NewText("💥 Огненный шар сформирован!", Color.OrangeRed);
            }
        }
    }
}