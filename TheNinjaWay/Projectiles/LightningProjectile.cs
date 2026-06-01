using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace TheNinjaWay.Projectiles
{
    public class LightningProjectile : ModProjectile
    {
        private int hitCount = 0;

        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.ThunderStaffShot;

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
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Electric, 0f, 0f, 100, Color.Yellow, 1.2f);
            }
            
            Lighting.AddLight(Projectile.Center, 1f, 0.9f, 0.2f);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            hitCount++;
            
            target.velocity.X += Projectile.velocity.X * 1.5f;
            target.velocity.Y -= 2f;
            
            Main.NewText($"⚡ {target.TypeName} поражён молнией!", Color.Yellow);
            
            if (hitCount < 3)
            {
                NPC closest = null;
                float closestDist = 200f;
                
                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && !npc.friendly && npc != target && npc.Distance(target.Center) < closestDist)
                    {
                        closestDist = npc.Distance(target.Center);
                        closest = npc;
                    }
                }
                
                if (closest != null)
                {
                    Vector2 direction = closest.Center - target.Center;
                    direction.Normalize();
                    
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), 
                        target.Center, 
                        direction * 12f, 
                        ModContent.ProjectileType<LightningProjectile>(), 
                        hit.Damage / 2, 
                        3f, 
                        Projectile.owner);
                }
            }
        }
    }
}