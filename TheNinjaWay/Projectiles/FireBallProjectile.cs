using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TheNinjaWay.Projectiles
{
    public class FireBallProjectile : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Fireball;

        public override void SetDefaults()
        {
            Projectile.width = 96;
            Projectile.height = 96;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            Projectile.rotation += 0.1f;
            for (int i = 0; i < 3; i++)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 1.5f);
            }
            Lighting.AddLight(Projectile.Center, 1.5f, 0.7f, 0.3f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 300);
            for (int i = 0; i < 15; i++)
            {
                Dust.NewDust(target.position, target.width, target.height, DustID.Torch, 0f, 0f, 100, default, 2f);
            }
        }
    }
}