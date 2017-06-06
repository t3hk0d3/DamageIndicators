﻿using UnityEngine;
using Verse;
using System.IO;
using System.Reflection;

namespace DamageMotes
{
    public class DMModSettings : ModSettings
    {
        public bool EnableIndicatorNeutralFaction;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref EnableIndicatorNeutralFaction, "EnableIndicatorNeutralFaction");
        }

        public void DoWindowContents(Rect inRect)
        {
            var list = new Listing_Standard()
            {
                ColumnWidth = inRect.width
            };
            list.Begin(inRect);
            list.CheckboxLabeled("EnableIndicatorNeutralFactions".Translate(), ref EnableIndicatorNeutralFaction, "EnableIndicatorNeutralFactions_Desc".Translate());
            list.End();
        }

        public void WriteSettings(DMMod instance) => LoadedModManager.WriteModSettings(instance.Content.Identifier, instance.GetType().Name, this);
    }
    public class DMMod : Mod
    {
        public DMModSettings settings = new DMModSettings();

        public DMMod(ModContentPack content) : base(content)
        {
            Pack = content;
            string path = (string)typeof(LoadedModManager).GetMethod("GetSettingsFilename", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { Content.Identifier, GetType().Name });
            if (File.Exists(path))
                settings = GetSettings<DMModSettings>();
        }

        public ModContentPack Pack { get; }

        public override void WriteSettings() => settings.WriteSettings(this);

        public override string SettingsCategory() => Pack.Name;

        public override void DoSettingsWindowContents(Rect inRect) => settings.DoWindowContents(inRect);
    }
}