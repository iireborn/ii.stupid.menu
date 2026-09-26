/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace iiMenu.Utilities
{
    public class FileUtilities
    {
        public static string GetFileExtension(string fileName)
        {
            var cleanName = fileName.Split('?')[0];
            return Path.GetExtension(cleanName).TrimStart('.').ToLower();
        }

        public static string RemoveLastDirectory(string directory) =>
            directory == "" || directory.LastIndexOf('/') <= 0 ? "" : directory[..directory.LastIndexOf('/')];

        public static string RemoveFileExtension(string file)
        {
            int index = 0;
            string output = "";
            string[] split = file.Split(".");
            foreach (string data in split)
            {
                index++;
                if (index == split.Length) continue;
                if (index > 1)
                    output += ".";

                output += data;
            }
            return output;
        }

        public static AudioType GetAudioType(string extension)
        {
            return extension.ToLower() switch
            {
                "mp3" => AudioType.MPEG,
                "wav" => AudioType.WAV,
                "ogg" => AudioType.OGGVORBIS,
                "aiff" => AudioType.AIFF,
                _ => AudioType.WAV,
            };
        }

        public static string GetFullPath(Transform transform)
        {
            string path = "";
            while (transform.parent != null)
            {
                transform = transform.parent;
                path = path == "" ? transform.name : transform.name + "/" + path;
            }
            return path;
        }

        public static string GetGamePath() =>
            Assembly.GetExecutingAssembly().Location.Replace("\\", "/").Split("/BepInEx")[0];

        public static string SanitizeFileName(string input)
        {
            input = input.Trim();
            char[] illegalChars = Path.GetInvalidFileNameChars();
            input = illegalChars.Aggregate(input, (current, c) => current.Replace(c, '_'));

            input = input.Replace("../", "")
                         .Replace("..\\", "")
                         .Replace("./", "")
                         .Replace(".\\", "");

            input = input.Replace(":", "")
                         .Replace("\\", "")
                         .Replace("/", "");

            if (input.Length > 64)
                input = input[..64];

            if (string.IsNullOrWhiteSpace(input))
                input = "file"; // fallback

            return input;
        }
    }
}