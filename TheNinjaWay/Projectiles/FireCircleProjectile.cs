using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TheNinjaWay.Projectiles
{
    public class FireCircleProjectile : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Fireball;

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 60;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.rotation += 0.3f;
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 1f);
            Lighting.AddLight(Projectile.Center, 1f, 0.5f, 0.2f);
        }
    }
}