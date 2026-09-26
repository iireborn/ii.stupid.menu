/*
 * ii's Stupid Menu (Reborn)
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

namespace iiMenu.Managers
{
    public class LogManager
    {
        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="log">The message or object to log.</param>
        public static void Log(object log) =>
            Plugin.PluginLogger.LogInfo(log);

        /// <summary>
        /// Logs a formatted informational message.
        /// </summary>
        /// <param name="log">The message format string.</param>
        /// <param name="args">Arguments to format the message.</param>
        public static void Log(object log, object[] args) =>
            Plugin.PluginLogger.LogInfo(string.Format(log.ToString(), args));

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="log">The error message or object to log.</param>
        public static void LogError(object log) =>
            Plugin.PluginLogger.LogError(log);

        /// <summary>
        /// Logs a formatted error message.
        /// </summary>
        /// <param name="log">The error message format string.</param>
        /// <param name="args">Arguments to format the error message.</param>
        public static void LogError(object log, object[] args) =>
            Plugin.PluginLogger.LogError(string.Format(log.ToString(), args));

        /// <summary>
        /// Logs a warning message (as debug).
        /// </summary>
        /// <param name="log">The warning message or object to log.</param>
        public static void LogWarning(object log) =>
            Plugin.PluginLogger.LogDebug(log);

        /// <summary>
        /// Logs a formatted warning message (as debug).
        /// </summary>
        /// <param name="log">The warning message format string.</param>
        /// <param name="args">Arguments to format the warning message.</param>
        public static void LogWarning(object log, object[] args) =>
            Plugin.PluginLogger.LogDebug(string.Format(log.ToString(), args));
    }
}
