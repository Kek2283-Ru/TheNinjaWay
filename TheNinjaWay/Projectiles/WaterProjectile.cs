using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace TheNinjaWay.Projectiles
{
    public class WaterProjectile : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.WaterBolt;

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = false;
        }

        public override void AI()
        {
            Projectile.rotation += 0.2f;
            
            for (int i = 0; i < 2; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Water, 0f, 0f, 100, Color.LightBlue, 1f);
            }
            
            Lighting.AddLight(Projectile.Center, 0.2f, 0.5f, 0.8f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Slow, 300);
            Main.NewText($"💧 {target.TypeName} поражён водяной пулей и замедлен!", Color.DodgerBlue);
        }
    }
}