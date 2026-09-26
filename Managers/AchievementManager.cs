/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using iiMenu.Classes.Menu;
using iiMenu.Extensions;
using iiMenu.Menu;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Valve.Newtonsoft.Json.Linq;
using static iiMenu.Menu.Main;
using static iiMenu.Utilities.AssetUtilities;

namespace iiMenu.Managers
{
    public static class AchievementManager
    {
        private static List<Achievement> _achievements;
        public static List<Achievement> Achievements
        {
            get
            {
                if (_achievements != null) return _achievements;
                _achievements = new List<Achievement>();

                string[] files = Directory.GetFiles($"{PluginInfo.BaseDirectory}/Achievements");
                foreach (string file in files)
                {
                    if (file.EndsWith(".json"))
                        _achievements.Add(Achievement.FromJObject(JObject.Parse(File.ReadAllText(file))));
                }

                return _achievements;
            }
            set => _achievements = value;
        }

        public static void EnterAchievementTab()
        {
            int achievementCount = Achievements.Count;

            List<ButtonInfo> achievementButtons = new List<ButtonInfo> { new ButtonInfo { buttonText = "Exit Achievements", method = () => Buttons.CurrentCategoryName = "Main", isTogglable = false, toolTip = "Returns you back to the main page." } };
            
            if (achievementCount <= 0)
                achievementButtons.Add(
                    new ButtonInfo
                    {
                        buttonText = "You have no achievements.",
                        label = true
                    });
            else 
                for (int i = 0; i < achievementCount; i++)
                {
                    Achievement achievement = Achievements[i];
                    achievementButtons.Add(
                        new ButtonInfo
                        {
                            buttonText = $"Achievement{i}",
                            overlapText = achievement.name,
                            method = () => PromptSingle($"{achievement.description}<{PluginInfo.ServerResourcePath}/{achievement.icon}>", null, "Done"),
                            isTogglable = false,
                            toolTip = achievement.description
                        });
                }

            Buttons.buttons[Buttons.GetCategory("Achievements")] = achievementButtons.ToArray();
            Buttons.CurrentCategoryName = "Achievements";
        }

        public static bool HasAchievement(string name) =>
            Achievements.Any(a => a.name == name);

        public static void UnlockAchievement(Achievement achievement)
        {
            if (HasAchievement(achievement.name))
                return;

            Play2DAudio(LoadSoundFromURL($"{PluginInfo.ServerResourcePath}/Audio/Menu/achievement.ogg", "Audio/Menu/achievement.ogg"), buttonClickVolume / 10f);
            NotificationManager.SendNotification($"<color=grey>[</color><color=purple>ACHIEVEMENT</color><color=grey>]</color> Achievement unlocked! \"{achievement.name}\"");

            Achievements.Add(achievement);
            File.WriteAllText($"{PluginInfo.BaseDirectory}/Achievements/{achievement.name.Hash()}.json", achievement.ToJObject().ToString());
        }

        public struct Achievement
        {
            public string name;

            public string description;
            public string icon;

            public readonly JObject ToJObject() => new JObject
            {
                ["name"] = name,

                ["description"] = description,
                ["icon"] = icon
            };

            public static Achievement FromJObject(JObject obj) => new Achievement
            {
                name = (string)obj["name"],
                description = (string)obj["description"],
                icon = (string)obj["icon"]
            };
        }
    }
}
