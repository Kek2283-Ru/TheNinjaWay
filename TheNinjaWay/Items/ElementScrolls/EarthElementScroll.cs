using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace TheNinjaWay.Items.ElementScrolls
{
    public class EarthElementScroll : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 30;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.value = 5000;
            Item.rare = ItemRarityID.Green;
            Item.consumable = true;
            Item.maxStack = 1;
        }

        public override bool CanUseItem(Player player)
        {
            TheNinjaWayPlayer modPlayer = player.GetModPlayer<TheNinjaWayPlayer>();
            return !modPlayer.HasElement("Земля");
        }

        public override bool? UseItem(Player player)
        {
            TheNinjaWayPlayer modPlayer = player.GetModPlayer<TheNinjaWayPlayer>();
            
            if (modPlayer.AddElement("Земля"))
            {
                Main.NewText("🪨 Ты открыл стихию Земли! Теперь ты можешь использовать технику Каменная стена", Color.SaddleBrown);
                return true;
            }
            
            Main.NewText("❌ Ты уже достиг лимита стихий (максимум 3)", Color.Red);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Moonglow, 5)
                .AddIngredient(ItemID.StoneBlock, 20)
                .AddTile(TileID.Bookcases)
                .Register();
        }
    }
}