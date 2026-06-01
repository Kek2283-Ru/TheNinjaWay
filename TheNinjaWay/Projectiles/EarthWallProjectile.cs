using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace TheNinjaWay.Projectiles
{
    public class EarthWallProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.velocity = Vector2.Zero;
            
            if (Main.rand.NextBool(10))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Stone, 0f, 0f, 100, default, 1f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.velocity.X = (target.Center.X - Projectile.Center.X) * 3f;
            target.velocity.Y = -8f;
        }
    }
}