using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameInput;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace TheNinjaWay
{
    public class TheNinjaWayPlayer : ModPlayer
    {
        public float currentChakra = 100f;
        public float maxChakra = 100f;
        public string playerClan = "None";
        private bool hasGivenClan = false;
        public string playerElement = "None";
        private bool hasGivenElement = false;
        public List<string> unlockedElements = new List<string>();
        public const int MAX_ELEMENTS = 3;
        public bool sharinganActive = false;
        private int sharinganDrainTimer = 0;
        public bool byakuganActive = false;
        private int byakuganDrainTimer = 0;
        public bool mangekyoUnlocked = false;
        public bool mangekyoActive = false;
        private int mangekyoDrainTimer = 0;
        public int sharinganKills = 0;
        public int susanooDefenseBoost = 0;
        private int susanooTimer = 0;
        private int susanooCooldown = 0;
        public int leeGatesLevel = 0;
        private int leeGatesTimer = 0;
        public bool gatesActive = false;
        private int genjutsuCooldown = 0;
        public int playerLevel = 0;
        public int killCount = 0;
        public int killsNeeded = 1;
        private int chakraDisplayTimer = 0;
        private Dictionary<NPC, int> kamuiTimers = new Dictionary<NPC, int>();
        private int fireChargeTimer = 0;
        private Vector2 fireDirection = Vector2.Zero;
        private int waterShotTimer = 0;
        private int waterShotCount = 0;
        private int shinraCooldown = 0;
        private int chibakuTimer = 0;
        private int chibakuCooldown = 0;
        private Vector2 chibakuCenter = Vector2.Zero;
        
        public bool rinneganUnlocked = false;
        private int lastMilestone = 0;
        
        public bool isKage = false;
        public string villageName = "";

        public override void SaveData(TagCompound tag)
        {
            tag["playerClan"] = playerClan;
            tag["hasGivenClan"] = hasGivenClan;
            tag["playerElement"] = playerElement;
            tag["hasGivenElement"] = hasGivenElement;
            tag["unlockedElements"] = unlockedElements;
            tag["playerLevel"] = playerLevel;
            tag["killCount"] = killCount;
            tag["killsNeeded"] = killsNeeded;
            tag["mangekyoUnlocked"] = mangekyoUnlocked;
            tag["sharinganKills"] = sharinganKills;
            tag["rinneganUnlocked"] = rinneganUnlocked;
            tag["lastMilestone"] = lastMilestone;
            tag["isKage"] = isKage;
            tag["villageName"] = villageName;
        }

        public override void LoadData(TagCompound tag)
        {
            playerClan = tag.GetString("playerClan");
            hasGivenClan = tag.GetBool("hasGivenClan");
            playerElement = tag.GetString("playerElement");
            hasGivenElement = tag.GetBool("hasGivenElement");
            unlockedElements = new List<string>(tag.GetList<string>("unlockedElements"));
            playerLevel = tag.GetInt("playerLevel");
            killCount = tag.GetInt("killCount");
            killsNeeded = tag.GetInt("killsNeeded");
            mangekyoUnlocked = tag.GetBool("mangekyoUnlocked");
            sharinganKills = tag.GetInt("sharinganKills");
            rinneganUnlocked = tag.GetBool("rinneganUnlocked");
            lastMilestone = tag.GetInt("lastMilestone");
            isKage = tag.GetBool("isKage");
            villageName = tag.GetString("villageName");
            if (hasGivenClan && playerClan != "None") ApplyClanBonus();
            if (hasGivenElement && playerElement != "None") ApplyElementBonus();
            if (playerLevel > 0)
            {
                maxChakra = 100f + (playerLevel * 10);
                currentChakra = maxChakra;
            }
        }

        public override void OnEnterWorld()
        {
            if (!hasGivenClan)
            {
                string[] clans = { "Учиха", "Хьюга", "Узумаки", "Сенджу", "Нака", "Ли" };
                int randomIndex = Main.rand.Next(clans.Length);
                playerClan = clans[randomIndex];
                hasGivenClan = true;
                ApplyClanBonus();
                Main.NewText("══════════════════════════════════════", Color.Gold);
                Main.NewText($"[The Ninja Way] Твой клан: {playerClan}!", Color.Orange);
                Main.NewText($"[Бонус клана] {GetClanBonusDescription()}", Color.LightBlue);
                GiveRandomElement();
                Main.NewText($"🎯 Требуется убить {killsNeeded} врага для 1 уровня", Color.Cyan);
                Main.NewText($"✨ Способность клана: нажми L", Color.Magenta);
                Main.NewText($"🪨 Техника Земли: нажми Z", Color.SaddleBrown);
                Main.NewText($"🔥 Техника Огня: нажми X", Color.OrangeRed);
                Main.NewText($"🍃 Техника Ветра: нажми V", Color.LightGreen);
                Main.NewText($"💧 Техника Воды: нажми C", Color.DodgerBlue);
                Main.NewText($"⚡ Техника Молнии: нажми F", Color.Yellow);
                Main.NewText($"🌀 Показать чакру: нажми H", Color.Cyan);
                if (rinneganUnlocked)
                    Main.NewText($"🌑 Риннеган разблокирован! (с Мангекё): Y-отталк, U-притян, I-чёрная дыра", Color.Purple);
                else if (mangekyoUnlocked)
                    Main.NewText($"🌑 Чтобы разблокировать Риннеган, убей Плантеру!", Color.Purple);
                if (playerClan == "Ли")
                {
                    Main.NewText($"🔥 Пассивные бонусы Ли: +50% урон, +40% скорость", Color.Orange);
                    Main.NewText($"⚡ Врата Ли: нажми L (1→8)", Color.Orange);
                }
                if (isKage)
                {
                    Main.NewText($"👑 ТЫ КАГЕ ДЕРЕВНИ {villageName}!", Color.Gold);
                    Main.NewText($"🌀 Телепортация: T | Призыв игроков: G", Color.Cyan);
                }
                Main.NewText("══════════════════════════════════════", Color.Gold);
            }
        }

        private void CheckKageAchievement()
        {
            if (isKage) return;
            
            if (playerLevel >= 30)
            {
                isKage = true;
                Main.NewText("══════════════════════════════════════", Color.Gold);
                Main.NewText("👑 ТЫ ДОСТИГ 30 УРОВНЯ! ТВОЯ СИЛА СТАЛА УРОВНЯ КАГЕ!", Color.Orange);
                Main.NewText("✨ Ты получил способности: телепортация (клавиша T) и призыв игроков (клавиша G)", Color.Magenta);
                Main.NewText($"🏘️ Твоя скрытая деревня будет называться: {Player.name}гакуре", Color.LightBlue);
                Main.NewText("══════════════════════════════════════", Color.Gold);
                villageName = $"{Player.name}гакуре";
            }
        }

        private void CheckKillMilestones()
        {
            if (lastMilestone >= 10000) return;
            
            if (killCount >= 10000 && lastMilestone < 10000)
            {
                lastMilestone = 10000;
                Main.NewText("══════════════════════════════════════", Color.Gold);
                Main.NewText("👑 ТЫ УЖЕ ЛЕГЕНДА! 10000 поверженных врагов!", Color.Orange);
                Main.NewText("🔥 Твоё имя будут помнить вечно!", Color.Orange);
                Main.NewText("══════════════════════════════════════", Color.Gold);
            }
            else if (killCount >= 4000 && lastMilestone < 4000)
            {
                lastMilestone = 4000;
                Main.NewText("══════════════════════════════════════", Color.Gold);
                Main.NewText("⚡ ТВОЯ СИЛА РАСТЁТ! 4000 поверженных врагов!", Color.LightBlue);
                Main.NewText("💪 Ты становишься легендой...", Color.LightBlue);
                Main.NewText("══════════════════════════════════════", Color.Gold);
            }
            else if (killCount >= 1000 && lastMilestone < 1000)
            {
                lastMilestone = 1000;
                Main.NewText("══════════════════════════════════════", Color.Gold);
                Main.NewText("🎉 МОЛОДЕЦ! 1000 поверженных врагов!", Color.Green);
                Main.NewText("🍥 Твой путь ниндзя только начинается!", Color.Green);
                Main.NewText("══════════════════════════════════════", Color.Gold);
            }
        }

        public void OnKillNPC(NPC npc)
        {
            if (npc.friendly || npc.lifeMax <= 5) return;
            killCount++;
            Main.NewText($"⚡ Убийств: {killCount}/{killsNeeded}", Color.LightGreen);
            if (killCount >= killsNeeded) LevelUp();
            if (sharinganActive && playerClan == "Учиха" && !mangekyoUnlocked)
            {
                sharinganKills++;
                Main.NewText($"👁️ Прогресс Мангекё: {sharinganKills}/50", Color.Orange);
            }
            if (!mangekyoUnlocked && playerClan == "Учиха" && sharinganKills >= 50)
            {
                bool isBoss = npc.type == NPCID.EyeofCthulhu || npc.type == NPCID.SkeletronHead ||
                              npc.type == NPCID.KingSlime || npc.type == NPCID.EaterofWorldsHead ||
                              npc.type == NPCID.BrainofCthulhu || npc.type == NPCID.QueenBee;
                if (isBoss) UnlockMangekyo();
            }
            if (!rinneganUnlocked && mangekyoUnlocked && npc.type == NPCID.Plantera)
            {
                rinneganUnlocked = true;
                Main.NewText("══════════════════════════════════════", Color.Purple);
                Main.NewText("🌑 ТЫ ПОЗНАЛ СИЛУ БОГА! РИННЕГАН ПРОБУДИЛСЯ!", Color.Purple);
                Main.NewText("🔥 Способности Риннегана: Y-отталк, U-притян, I-чёрная дыра", Color.Purple);
                Main.NewText("══════════════════════════════════════", Color.Purple);
            }
            CheckKillMilestones();
        }

        private void LevelUp()
        {
            playerLevel++;
            killCount = 0;
            killsNeeded = (int)Math.Pow(2, playerLevel);
            maxChakra += 10f;
            currentChakra = maxChakra;
            Main.NewText($"🎉 Уровень {playerLevel}! +10 макс. чакры", Color.Orange);
            CheckKageAchievement();
        }

        private void GiveRandomElement()
        {
            string[] elements = { "Огонь", "Вода", "Земля", "Молния", "Ветер" };
            playerElement = elements[Main.rand.Next(elements.Length)];
            hasGivenElement = true;
            ApplyElementBonus();
            Main.NewText($"[Стихия] Твоя стихия: {playerElement}!", GetElementColor(playerElement));
            Main.NewText($"[Бонус] {GetElementBonusDescription()}", GetElementColor(playerElement));
        }

        public bool HasElement(string element) => playerElement == element || unlockedElements.Contains(element);

        public bool AddElement(string element)
        {
            if (HasElement(element)) { Main.NewText($"❌ {element} уже открыта", Color.Red); return false; }
            int count = unlockedElements.Count + (playerElement != "None" ? 1 : 0);
            if (count >= MAX_ELEMENTS) { Main.NewText($"❌ Максимум {MAX_ELEMENTS} стихий", Color.Red); return false; }
            unlockedElements.Add(element);
            Main.NewText($"✨ Ты открыл {element}!", GetElementColor(element));
            return true;
        }

        private Color GetElementColor(string elem)
        {
            switch (elem)
            {
                case "Огонь": return Color.OrangeRed;
                case "Вода": return Color.DodgerBlue;
                case "Земля": return Color.SaddleBrown;
                case "Молния": return Color.Yellow;
                case "Ветер": return Color.LightGreen;
                default: return Color.White;
            }
        }

        private void ApplyClanBonus()
        {
            maxChakra = 100f;
            switch (playerClan)
            {
                case "Учиха": maxChakra = 120f; break;
                case "Хьюга": maxChakra = 110f; break;
                case "Узумаки": maxChakra = 150f; break;
                case "Сенджу": maxChakra = 130f; break;
                default: maxChakra = 100f; break;
            }
            currentChakra = maxChakra;
            if (playerClan == "Ли") { Player.GetDamage(DamageClass.Melee) += 0.50f; Player.moveSpeed += 0.40f; }
        }

        private void ApplyElementBonus()
        {
            switch (playerElement)
            {
                case "Огонь": Player.GetDamage(DamageClass.Generic) += 0.10f; break;
                case "Вода": Player.moveSpeed += 0.10f; break;
                case "Земля": Player.statDefense += 5; break;
                case "Молния": Player.GetAttackSpeed(DamageClass.Generic) += 0.10f; break;
                case "Ветер": Player.maxRunSpeed += 1f; Player.jumpSpeedBoost += 2f; break;
            }
        }

        private string GetClanBonusDescription()
        {
            switch (playerClan)
            {
                case "Учиха": return "+20 макс. чакры";
                case "Хьюга": return "+10 макс. чакры";
                case "Узумаки": return "+50 макс. чакры";
                case "Сенджу": return "+30 макс. чакры";
                case "Ли": return "+50% урон, +40% скорость";
                default: return "стандарт";
            }
        }

        private string GetElementBonusDescription()
        {
            switch (playerElement)
            {
                case "Огонь": return "+10% урона 🔥";
                case "Вода": return "+10% скорости 💧";
                case "Земля": return "+5 защиты 🪨";
                case "Молния": return "+10% скорости атаки ⚡";
                case "Ветер": return "+20% скорости 🍃";
                default: return "";
            }
        }

        public override void UpdateBadLifeRegen()
        {
            if (currentChakra < maxChakra && Main.GameUpdateCount % 60 == 0)
            {
                currentChakra += 2f;
                if (currentChakra > maxChakra) currentChakra = maxChakra;
            }
        }

        public void ShowChakra() => Main.NewText($"🌀 Чакра: {(int)currentChakra} / {(int)maxChakra}", Color.Cyan);

        public void ToggleSharingan()
        {
            if (playerClan != "Учиха") { Main.NewText("❌ Не Учиха", Color.Red); return; }
            if (!sharinganActive)
            {
                if (currentChakra >= 30f) { sharinganActive = true; currentChakra -= 30f; Main.NewText("👁️ Шаринган!", Color.Red); }
                else Main.NewText($"❌ Нужно 30 чакры", Color.Red);
            }
            else { sharinganActive = false; Main.NewText("Шаринган деактивирован", Color.Gray); }
        }

        public void ToggleByakugan()
        {
            if (playerClan != "Хьюга") { Main.NewText("❌ Не Хьюга", Color.Red); return; }
            if (!byakuganActive)
            {
                if (currentChakra >= 20f) { byakuganActive = true; currentChakra -= 20f; Main.NewText("👁️ Бьёкуган!", Color.LightBlue); }
                else Main.NewText($"❌ Нужно 20 чакры", Color.Red);
            }
            else { byakuganActive = false; Main.NewText("Бьёкуган деактивирован", Color.Gray); }
        }

        public void UnlockMangekyo()
        {
            if (mangekyoUnlocked) return;
            mangekyoUnlocked = true;
            Main.NewText("👁️ МАНГЕКЁ ПРОБУДИЛСЯ! Нажми M", Color.DarkRed);
        }

        public void ToggleMangekyo()
        {
            if (playerClan != "Учиха" || !mangekyoUnlocked) return;
            if (!mangekyoActive)
            {
                if (currentChakra >= 50f) { mangekyoActive = true; currentChakra -= 50f; Main.NewText("🌀 МАНГЕКЁ! +30% урона, +20% скорости атаки", Color.DarkRed); }
                else Main.NewText($"❌ Нужно 50 чакры", Color.Red);
            }
            else { mangekyoActive = false; Main.NewText("Мангекё деактивирован", Color.Gray); }
        }

        public void UseAmaterasu(NPC target)
        {
            if (currentChakra >= 30f) { currentChakra -= 30f; target.AddBuff(BuffID.OnFire, 300); target.AddBuff(BuffID.CursedInferno, 300); Main.NewText($"🔥 АМАТЕРАСУ! {target.TypeName} горит!", Color.DarkRed); }
            else Main.NewText($"❌ Нужно 30 чакры", Color.Red);
        }

        public void UseSusanoo()
        {
            if (susanooCooldown > 0 || !mangekyoActive) return;
            if (currentChakra >= 40f)
            {
                currentChakra -= 40f;
                susanooDefenseBoost = 40;
                susanooTimer = 600;
                susanooCooldown = 1800;
                Player.AddBuff(BuffID.Ironskin, 600);
                Player.AddBuff(BuffID.Endurance, 600);
                Main.NewText("🛡️ СУСАНОО! +40 защиты на 10 сек", Color.Purple);
            }
            else Main.NewText($"❌ Нужно 40 чакры", Color.Red);
        }

        public void UseKamui(NPC target)
        {
            if (currentChakra >= 35f) { currentChakra -= 35f; target.position = new Vector2(-10000, -10000); kamuiTimers[target] = 300; Main.NewText($"🌀 КАМУИ! {target.TypeName} исчез на 5 сек", Color.DarkViolet); }
            else Main.NewText($"❌ Нужно 35 чакры", Color.Red);
        }

        public void UseEarthTech()
        {
            if (!HasElement("Земля") || currentChakra < 30f) { Main.NewText("❌ Нет Земли или мало чакры", Color.Red); return; }
            currentChakra -= 30f;
            float sx = Player.Center.X + (70f * Player.direction);
            int tx = (int)(sx / 16), ty = (int)(Player.Bottom.Y / 16), gy = ty;
            for (int i = 0; i < 30; i++)
            {
                Tile t = Main.tile[tx, ty + i];
                if (t != null && t.HasTile && Main.tileSolid[t.TileType]) { gy = ty + i; break; }
            }
            for (int i = 0; i < 5; i++)
            {
                float sy = (gy - i) * 16f - 32f;
                Projectile.NewProjectile(Player.GetSource_FromThis(), new Vector2(sx, sy), Vector2.Zero, ModContent.ProjectileType<Projectiles.EarthWallProjectile>(), 15, 10f, Player.whoAmI);
            }
            Main.NewText("🪨 Каменная стена!", Color.SaddleBrown);
        }

        public void UseFireTech()
        {
            if (!HasElement("Огонь") || currentChakra < 25f) { Main.NewText("❌ Нет Огня или мало чакры", Color.Red); return; }
            currentChakra -= 25f;
            fireDirection = Main.MouseWorld - Player.Center;
            fireDirection.Normalize();
            Vector2 mp = Player.Center + new Vector2(0, -8f) + fireDirection * 10f;
            for (int i = 0; i < 20; i++)
            {
                Vector2 tp = Player.Center + fireDirection * (60 + Main.rand.Next(-10, 10));
                Vector2 tt = tp - mp;
                tt.Normalize();
                Projectile.NewProjectile(Player.GetSource_FromThis(), mp, tt * 10f, ModContent.ProjectileType<Projectiles.FireCircleProjectile>(), 10, 2f, Player.whoAmI);
            }
            Main.NewText("🔥 20 огненных кругов!", Color.OrangeRed);
            fireChargeTimer = 40;
        }

        public void UseWindTech()
        {
            if (!HasElement("Ветер") || currentChakra < 20f) { Main.NewText("❌ Нет Ветра или мало чакры", Color.Red); return; }
            currentChakra -= 20f;
            Vector2 d = Main.MouseWorld - Player.Center;
            d.Normalize();
            Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + d * 20f, d * 14f, ModContent.ProjectileType<Projectiles.WindProjectile>(), 25, 8f, Player.whoAmI);
            Main.NewText("🍃 Ветряная пуля!", Color.LightGreen);
        }

        public void UseWaterTech()
        {
            if (!HasElement("Вода") || currentChakra < 15f) { Main.NewText("❌ Нет Воды или мало чакры", Color.Red); return; }
            currentChakra -= 15f;
            waterShotCount = 1;
            waterShotTimer = 60;
            Vector2 d = Main.MouseWorld - Player.Center;
            d.Normalize();
            Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + d * 20f, d * 12f, ModContent.ProjectileType<Projectiles.WaterProjectile>(), 20, 5f, Player.whoAmI);
            Main.NewText("💧 Водяная пуля!", Color.DodgerBlue);
        }

        public void UseLightningTech()
        {
            if (!HasElement("Молния") || currentChakra < 20f) { Main.NewText("❌ Нет Молнии или мало чакры", Color.Red); return; }
            currentChakra -= 20f;
            Vector2 d = Main.MouseWorld - Player.Center;
            d.Normalize();
            Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + d * 20f, d * 16f, ModContent.ProjectileType<Projectiles.LightningProjectile>(), 30, 8f, Player.whoAmI);
            Main.NewText("⚡ Молния!", Color.Yellow);
        }

        public void TeleportToCursor()
        {
            if (!isKage)
            {
                Main.NewText("❌ Ты ещё не достиг уровня Каге!", Color.Red);
                return;
            }
            
            Vector2 cursorPos = Main.MouseWorld;
            Player.Teleport(cursorPos, 1);
            Main.NewText($"🌀 Каге телепортировался!", Color.Purple);
            
            for (int i = 0; i < 20; i++)
            {
                Dust.NewDust(Player.position, Player.width, Player.height, DustID.PurificationPowder, 0f, 0f, 100, Color.Purple, 2f);
            }
        }
        
        public void SummonPlayers()
        {
            if (!isKage)
            {
                Main.NewText("❌ Ты ещё не достиг уровня Каге!", Color.Red);
                return;
            }
            
            Main.NewText($"👑 Каге {Player.name} призывает всех ниндзя в {villageName}!", Color.Gold);
            
            for (int i = 0; i < 255; i++)
            {
                Player other = Main.player[i];
                if (other != null && other.active && other != Player)
                {
                    other.Teleport(Player.position, 1);
                    Main.NewText($"🌀 {other.name} телепортирован в {villageName}!", Color.LightBlue);
                }
            }
            
            for (int i = 0; i < 30; i++)
            {
                Dust.NewDust(Player.position, Player.width, Player.height, DustID.PurificationPowder, 0f, 0f, 100, Color.Gold, 3f);
            }
        }

        public void UseShinraTensei()
        {
            if (!rinneganUnlocked)
            {
                Main.NewText("❌ Риннеган ещё не пробуждён! Убей Плантеру после получения Мангекё", Color.Red);
                return;
            }
            if (!mangekyoActive)
            {
                Main.NewText("❌ Сначала активируй Мангекё Шаринган (клавиша M)!", Color.Red);
                return;
            }
            if (shinraCooldown > 0)
            {
                Main.NewText($"❌ Перезарядка: {shinraCooldown / 60} сек", Color.Red);
                return;
            }
            if (currentChakra < 40f) { Main.NewText($"❌ Нужно 40 чакры", Color.Red); return; }
            currentChakra -= 40f;
            shinraCooldown = 600;
            Main.NewText("🌑 ШИНРА ТЕНСЕЙ!", Color.Purple);
            foreach (NPC n in Main.npc)
                if (n.active && !n.friendly && n.Distance(Player.Center) < 400f)
                { Vector2 dir = n.Center - Player.Center; dir.Normalize(); n.velocity = dir * 15f; n.velocity.Y -= 5f; }
        }

        public void UseBanshoTenin()
        {
            if (!rinneganUnlocked)
            {
                Main.NewText("❌ Риннеган ещё не пробуждён! Убей Плантеру после получения Мангекё", Color.Red);
                return;
            }
            if (!mangekyoActive)
            {
                Main.NewText("❌ Сначала активируй Мангекё Шаринган (клавиша M)!", Color.Red);
                return;
            }
            if (currentChakra < 30f) { Main.NewText($"❌ Нужно 30 чакры", Color.Red); return; }
            currentChakra -= 30f;
            NPC closest = null;
            float cd = 500f;
            foreach (NPC n in Main.npc)
                if (n.active && !n.friendly && n.Distance(Player.Center) < cd) { cd = n.Distance(Player.Center); closest = n; }
            if (closest != null)
            {
                Vector2 pull = Player.Center - closest.Center;
                pull.Normalize();
                closest.velocity = pull * 12f;
                Main.NewText($"🌑 БАНШО ТЕННИН! {closest.TypeName} притянут!", Color.Purple);
            }
            else Main.NewText("❌ Нет врагов", Color.Red);
        }

        public void UseChibakuTensei()
        {
            if (!rinneganUnlocked)
            {
                Main.NewText("❌ Риннеган ещё не пробуждён! Убей Плантеру после получения Мангекё", Color.Red);
                return;
            }
            if (!mangekyoActive)
            {
                Main.NewText("❌ Сначала активируй Мангекё Шаринган (клавиша M)!", Color.Red);
                return;
            }
            if (chibakuCooldown > 0)
            {
                Main.NewText($"❌ Перезарядка: {chibakuCooldown / 60} сек", Color.Red);
                return;
            }
            if (currentChakra < 60f) { Main.NewText($"❌ Нужно 60 чакры", Color.Red); return; }
            
            currentChakra -= 60f;
            chibakuCooldown = 1800;
            chibakuTimer = 600;
            chibakuCenter = Player.Center + new Vector2(15f * Player.direction, 0f);
            Main.NewText("🌑 ЧИБАКУ ТЕНСЕЙ! Чёрная сфера поднимается из рук!", Color.Purple);
            
            for (int i = 0; i < 30; i++)
            {
                Dust.NewDust(chibakuCenter - new Vector2(30, 30), 60, 60, DustID.PurificationPowder, 0f, -3f, 100, Color.Black, 3f);
            }
        }

        public void UseLeeGates()
        {
            if (playerClan != "Ли" || gatesActive) return;
            int next = leeGatesLevel + 1;
            if (next > 8) { leeGatesLevel = 0; next = 1; }
            int cost = next * 10;
            if (currentChakra >= cost)
            {
                currentChakra -= cost;
                leeGatesLevel = next;
                leeGatesTimer = 600;
                gatesActive = true;
                Main.NewText($"🔥 {leeGatesLevel} ВРАТА! x{1f + leeGatesLevel * 0.5f} урона!", Color.Orange);
            }
            else Main.NewText($"❌ Нужно {cost} чакры", Color.Red);
        }

        public void UseClanAbility()
        {
            if (playerClan == "Учиха" && currentChakra >= 20f)
            {
                currentChakra -= 20f;
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center, new Vector2(8f * Player.direction, 0f), ProjectileID.Fireball, 35, 3f, Player.whoAmI);
                Main.NewText("🔥 Огненный шар!", Color.OrangeRed);
            }
            else if (playerClan == "Хьюга" && currentChakra >= 15f)
            {
                currentChakra -= 15f;
                Player.AddBuff(BuffID.Wrath, 300);
                Main.NewText("💥 64 удара! +урон на 5 сек", Color.DodgerBlue);
            }
            else if (playerClan == "Узумаки" && currentChakra >= 20f)
            {
                currentChakra -= 20f;
                Player.statLife = Math.Min(Player.statLife + 50, Player.statLifeMax2);
                Player.HealEffect(50);
                Main.NewText("💚 +50 здоровья!", Color.LightGreen);
            }
            else if (playerClan == "Сенджу" && currentChakra >= 20f)
            {
                currentChakra -= 20f;
                Player.AddBuff(BuffID.Ironskin, 600);
                Player.AddBuff(BuffID.Regeneration, 600);
                Main.NewText("🌳 Защита и реген на 10 сек", Color.ForestGreen);
            }
            else if (playerClan == "Ли") UseLeeGates();
            else if (playerClan != "Ли") Main.NewText($"❌ Нет чакры", Color.Red);
        }

        public override void PostUpdate()
        {
            if (genjutsuCooldown > 0) genjutsuCooldown--;
            if (susanooCooldown > 0) susanooCooldown--;
            if (shinraCooldown > 0) shinraCooldown--;
            if (chibakuCooldown > 0) chibakuCooldown--;

            if (fireChargeTimer > 0 && --fireChargeTimer == 0 && fireDirection != Vector2.Zero)
            {
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + fireDirection * 80f, fireDirection * 12f, ModContent.ProjectileType<Projectiles.FireBallProjectile>(), 45, 8f, Player.whoAmI);
                Main.NewText("💥 Огненный шар!", Color.OrangeRed);
                fireDirection = Vector2.Zero;
            }

            if (waterShotTimer > 0 && --waterShotTimer == 0 && ++waterShotCount < 4)
            {
                Vector2 d = Main.MouseWorld - Player.Center;
                d.Normalize();
                Projectile.NewProjectile(Player.GetSource_FromThis(), Player.Center + d * 20f, d * 12f, ModContent.ProjectileType<Projectiles.WaterProjectile>(), 20, 5f, Player.whoAmI);
                Main.NewText("💧 Водяная пуля!", Color.DodgerBlue);
                waterShotTimer = waterShotCount < 3 ? 60 : 0;
                if (waterShotCount >= 3) waterShotCount = 0;
            }

            if (chibakuTimer > 0)
            {
                chibakuTimer--;
                
                if (chibakuTimer > 580)
                {
                    chibakuCenter.Y -= 1.5f;
                }
                
                for (int i = 0; i < 8; i++)
                {
                    Dust dust = Dust.NewDustDirect(chibakuCenter - new Vector2(20, 20), 40, 40, DustID.PurificationPowder, 0f, 0f, 100, Color.Black, 2.5f);
                    dust.noGravity = true;
                }
                Lighting.AddLight(chibakuCenter, 0.2f, 0f, 0.2f);
                
                foreach (NPC n in Main.npc)
                {
                    if (n.active && !n.friendly && n != null && n.Distance(chibakuCenter) < 500f)
                    {
                        Vector2 pull = chibakuCenter - n.Center;
                        float dist = pull.Length();
                        float strength = 12f * (1f - Math.Min(1f, dist / 500f));
                        pull.Normalize();
                        n.velocity += pull * strength;
                        
                        if (dist < 40f)
                        {
                            n.SimpleStrikeNPC(25, 0, false, 0, DamageClass.Generic);
                            n.velocity = Vector2.Zero;
                        }
                    }
                }
                
                foreach (Item item in Main.item)
                {
                    if (item.active && item.Distance(chibakuCenter) < 400f && !item.vanity)
                    {
                        Vector2 pull = chibakuCenter - item.Center;
                        if (pull.Length() > 5f)
                        {
                            pull.Normalize();
                            item.velocity += pull * 8f;
                        }
                        else
                        {
                            item.velocity = Vector2.Zero;
                        }
                    }
                }
                
                if (chibakuTimer == 0)
                {
                    Main.NewText("Чибаку Тенсей рассеялся...", Color.Purple);
                    chibakuCenter = Vector2.Zero;
                }
            }

            if (sharinganActive && ++sharinganDrainTimer >= 20)
            {
                sharinganDrainTimer = 0;
                if ((currentChakra -= 5f) <= 0) { currentChakra = 0; sharinganActive = false; Main.NewText("Чакра кончилась", Color.Red); }
                Lighting.AddLight(Player.Center, 1f, 0.2f, 0.2f);
            }

            if (byakuganActive && ++byakuganDrainTimer >= 20)
            {
                byakuganDrainTimer = 0;
                if ((currentChakra -= 5f) <= 0) { currentChakra = 0; byakuganActive = false; Main.NewText("Чакра кончилась", Color.Red); }
                Lighting.AddLight(Player.Center, 1f, 1f, 1f);
                foreach (NPC n in Main.npc) if (n.active && !n.friendly) n.AddBuff(BuffID.Ichor, 2);
                foreach (Item i in Main.item) i.color = (i.active && i.value > 0) ? Color.Yellow : Color.White;
            }

            if (mangekyoActive && ++mangekyoDrainTimer >= 20)
            {
                mangekyoDrainTimer = 0;
                if ((currentChakra -= 10f) <= 0) { currentChakra = 0; mangekyoActive = false; Main.NewText("Чакра кончилась", Color.Red); if (susanooTimer > 0) { susanooTimer = 0; susanooDefenseBoost = 0; } }
                Lighting.AddLight(Player.Center, 1.5f, 0.1f, 0.1f);
                Player.GetDamage(DamageClass.Generic) += 0.3f;
                Player.GetAttackSpeed(DamageClass.Generic) += 0.2f;
            }

            if (susanooTimer > 0 && --susanooTimer == 0) { susanooDefenseBoost = 0; Main.NewText("Сусаноо рассеялся", Color.Purple); }
            else if (susanooTimer > 0) Player.statDefense += susanooDefenseBoost;

            if (gatesActive && --leeGatesTimer <= 0) { gatesActive = false; Main.NewText("Врата закрылись...", Color.Gray); }
            else if (gatesActive)
            {
                Player.GetDamage(DamageClass.Melee) *= 1f + leeGatesLevel * 0.5f;
                Player.moveSpeed += 0.2f + (leeGatesLevel - 1) * 0.1f;
                if (Main.rand.NextBool(5)) Dust.NewDust(Player.position, Player.width, Player.height, DustID.SolarFlare, 0f, 0f, 100, default, 2f);
            }

            List<NPC> toRemove = new List<NPC>();
            foreach (var kv in kamuiTimers)
            {
                int t = kv.Value - 1;
                if (kv.Key == null || !kv.Key.active) toRemove.Add(kv.Key);
                else if (t <= 0) { kv.Key.position = Player.Center + new Vector2(200, 0); Main.NewText($"{kv.Key.TypeName} вернулся", Color.Gray); toRemove.Add(kv.Key); }
                else kamuiTimers[kv.Key] = t;
            }
            foreach (NPC n in toRemove) kamuiTimers.Remove(n);
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (byakuganActive) { hit.Crit = true; hit.Damage = (int)(hit.Damage * 1.25f); Main.NewText($"👁️ +25% урона!", Color.LightBlue); }
            if (playerElement == "Огонь") { target.AddBuff(BuffID.OnFire, 180); Main.NewText($"🔥 {target.TypeName} загорелся!", Color.OrangeRed); }
            else if (playerElement == "Вода") { target.AddBuff(BuffID.Slow, 180); Main.NewText($"💧 {target.TypeName} замедлен!", Color.DodgerBlue); }
            else if (playerElement == "Земля" && Main.rand.NextFloat() < 0.25f) Main.NewText($"🪨 Доп. урон земли!", Color.SaddleBrown);
            else if (playerElement == "Молния" && Main.rand.NextFloat() < 0.2f) Main.NewText($"⚡ Молния!", Color.Yellow);
            else if (playerElement == "Ветер" && Main.rand.NextFloat() < 0.25f) Main.NewText($"🍃 Усиление ветра!", Color.LightGreen);
        }

        public override void ProcessTriggers(TriggersSet ts)
        {
            if (TheNinjaWay.SharinganHotKey.JustPressed) ToggleSharingan();
            if (TheNinjaWay.ByakuganHotKey.JustPressed) ToggleByakugan();
            if (TheNinjaWay.ClanAbilityHotKey.JustPressed) UseClanAbility();
            if (TheNinjaWay.MangekyoHotKey.JustPressed) ToggleMangekyo();
            if (TheNinjaWay.SusanooHotKey.JustPressed) UseSusanoo();
            if (TheNinjaWay.KamuiHotKey.JustPressed && mangekyoActive)
            {
                NPC t = GetTargetedNPC();
                if (t != null) UseKamui(t);
                else Main.NewText("Наведись на врага", Color.Red);
            }
            if (TheNinjaWay.EarthTechHotKey.JustPressed) UseEarthTech();
            if (TheNinjaWay.FireTechHotKey.JustPressed) UseFireTech();
            if (TheNinjaWay.WindTechHotKey.JustPressed) UseWindTech();
            if (TheNinjaWay.WaterTechHotKey.JustPressed) UseWaterTech();
            if (TheNinjaWay.LightningTechHotKey.JustPressed) UseLightningTech();
            if (TheNinjaWay.ShowChakraHotKey.JustPressed) ShowChakra();
            if (TheNinjaWay.ShinraTenseiHotKey.JustPressed) UseShinraTensei();
            if (TheNinjaWay.BanshoTeninHotKey.JustPressed) UseBanshoTenin();
            if (TheNinjaWay.ChibakuTenseiHotKey.JustPressed) UseChibakuTensei();
            if (TheNinjaWay.TeleportHotKey.JustPressed) TeleportToCursor();
            if (TheNinjaWay.SummonHotKey.JustPressed) SummonPlayers();

            if (ts.MouseRight && mangekyoActive && genjutsuCooldown <= 0)
            {
                NPC t = GetTargetedNPC();
                if (t != null) { UseAmaterasu(t); genjutsuCooldown = 60; }
            }
            else if (ts.MouseRight && sharinganActive && !mangekyoActive && genjutsuCooldown <= 0)
            {
                NPC t = GetTargetedNPC();
                if (t != null && currentChakra >= 10f)
                {
                    currentChakra -= 10f;
                    t.AddBuff(ModContent.BuffType<Buffs.Genjutsu>(), 180);
                    Main.NewText($"Гендзюцу! {t.TypeName} обездвижен", Color.Purple);
                    genjutsuCooldown = 240;
                }
                else if (t != null) Main.NewText($"❌ Нужно 10 чакры", Color.Red);
            }
        }

        private NPC GetTargetedNPC()
        {
            foreach (NPC n in Main.npc)
                if (n.active && !n.friendly && n.Hitbox.Contains(Main.MouseWorld.ToPoint()))
                    return n;
            return null;
        }
    }
}