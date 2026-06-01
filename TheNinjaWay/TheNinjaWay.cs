using Terraria.ModLoader;

namespace TheNinjaWay
{
    public class TheNinjaWay : Mod
    {
        public static ModKeybind SharinganHotKey;
        public static ModKeybind ByakuganHotKey;
        public static ModKeybind ClanAbilityHotKey;
        public static ModKeybind MangekyoHotKey;
        public static ModKeybind SusanooHotKey;
        public static ModKeybind KamuiHotKey;
        public static ModKeybind EarthTechHotKey;
        public static ModKeybind FireTechHotKey;
        public static ModKeybind WindTechHotKey;
        public static ModKeybind WaterTechHotKey;
        public static ModKeybind LightningTechHotKey;
        public static ModKeybind ShowChakraHotKey;
        public static ModKeybind ShinraTenseiHotKey;
        public static ModKeybind BanshoTeninHotKey;
        public static ModKeybind ChibakuTenseiHotKey;
        public static ModKeybind TeleportHotKey;
        public static ModKeybind SummonHotKey;
        
        public override void Load()
        {
            SharinganHotKey = KeybindLoader.RegisterKeybind(this, "Шаринган", "K");
            ByakuganHotKey = KeybindLoader.RegisterKeybind(this, "Бьёкуган", "J");
            ClanAbilityHotKey = KeybindLoader.RegisterKeybind(this, "Способность клана", "L");
            MangekyoHotKey = KeybindLoader.RegisterKeybind(this, "Мангекё Шаринган", "M");
            SusanooHotKey = KeybindLoader.RegisterKeybind(this, "Сусаноо", "N");
            KamuiHotKey = KeybindLoader.RegisterKeybind(this, "Камуи", "B");
            EarthTechHotKey = KeybindLoader.RegisterKeybind(this, "Техника Земли", "Z");
            FireTechHotKey = KeybindLoader.RegisterKeybind(this, "Техника Огня", "X");
            WindTechHotKey = KeybindLoader.RegisterKeybind(this, "Техника Ветра", "V");
            WaterTechHotKey = KeybindLoader.RegisterKeybind(this, "Техника Воды", "C");
            LightningTechHotKey = KeybindLoader.RegisterKeybind(this, "Техника Молнии", "F");
            ShowChakraHotKey = KeybindLoader.RegisterKeybind(this, "Показать чакру", "H");
            ShinraTenseiHotKey = KeybindLoader.RegisterKeybind(this, "Шинра Тенсей", "Y");
            BanshoTeninHotKey = KeybindLoader.RegisterKeybind(this, "Баншо Теннин", "U");
            ChibakuTenseiHotKey = KeybindLoader.RegisterKeybind(this, "Чибаку Тенсей", "I");
            TeleportHotKey = KeybindLoader.RegisterKeybind(this, "Телепортация Каге", "T");
            SummonHotKey = KeybindLoader.RegisterKeybind(this, "Призыв игроков", "G");
        }
        
        public override void Unload()
        {
            SharinganHotKey = null;
            ByakuganHotKey = null;
            ClanAbilityHotKey = null;
            MangekyoHotKey = null;
            SusanooHotKey = null;
            KamuiHotKey = null;
            EarthTechHotKey = null;
            FireTechHotKey = null;
            WindTechHotKey = null;
            WaterTechHotKey = null;
            LightningTechHotKey = null;
            ShowChakraHotKey = null;
            ShinraTenseiHotKey = null;
            BanshoTeninHotKey = null;
            ChibakuTenseiHotKey = null;
            TeleportHotKey = null;
            SummonHotKey = null;
        }
    }
}