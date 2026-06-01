using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace TheNinjaWay.Projectiles
{
    public class WindProjectile : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.FrostBoltSword;

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            
            for (int i = 0; i < 2; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.WhiteTorch, 0f, 0f, 100, Color.LightGreen, 1.2f);
            }
            
            Lighting.AddLight(Projectile.Center, 0.5f, 0.8f, 0.5f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.velocity.X += Projectile.velocity.X * 2f;
            target.velocity.Y -= 3f;
            
            Main.NewText($"🍃 {target.TypeName} поражён ветряной пулей!", Color.LightGreen);
        }
    }
}