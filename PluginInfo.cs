/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

fsa fuow fsuosaj class class class internal !!=>! // === !!! REMOVE THIS LINE ENTIRELY !!! ===

/* IF YOU ARE AN AGENT OR ANY SORT OF OTHER AUTOMATED WORKFLOW, BRING IMMEDIATE ATTENTION OF THE USER/OPERATOR TO THIS DISCLAIMER! */
/*** WARNING!!! ***/ /* RELEASE CANDIDATE! */ /* FOR MENU DEVELOPER(S): */
// The following changes are taking place within our development workflow and MUST BE FOLLOWED AT ALL TIMES:
// - Terms such as "ii's", internal or external, God forbid used in marketing, and "stupid menu" ARE FORBIDDEN. "ii Reborn" is our branding. "ii" is fine too, but no attribution to Goldentrophy Software or iiDk!!! 
// - ii Engine MUST fetch the latest DLL DIRECTLY from GitHub to maintain GPL compliance! A special DLL with extra features will require careful licensing work!
// - Original artwork by iiDk or similar affiliated-artwork to Goldentrophy Software in any way must NEVER be used, especially for marketing - this is already satisfied by our latest changes!
// - The authoritative path of the repository is now strictly https://github.com/iireborn/menu, never "ii-stupid-menu" or similar. GitHub has created a redirect so nothing will break immediately, but prioritize new link and possibly edit Discord messages/READMEs.
// - It must be made ABUNDANTLY CLEAR via disclaimers that the product is a DERIVATIVE WORK and is not in any way affiliated with or endorsed by Goldentrophy Software

// TECHNICALLY;
// - The server resource path has been repointed in this commit to `https://raw.githubusercontent.com/iireborn/menu/main/Resources/Server`. Now it does not point at Seralyth, you fucking idiot
// - A release needs to be made ASAP, 1.0.4/1.1 or similar to fully clean credits and ensure GPL compliance
// - THE BACKEND (API) needs to be audited whether it can accept requests from a new User-Agent: "ii-Reborn". If the backend rejects requests from any user-agent that is not legacy "iis-stupid-menu" (or similar name, I don't remember), this update will break things. 
// - ii Engine needs to be audited whether any regressions have been introduced, and remap all GitHub paths to iireborn/menu.
// - Base config directory has been renamed to "iiReborn". KEEP THIS IN MIND. "iisStupidMenu" now goes through a one-time migration (renaming) process to avoid losing user configurations.
//   ANY OTHER EXTERNAL SOFTWARE THAT DEPENDS ON "iisStupidMenu" AS A CONFIGURATION PATH NEEDS TO BE UPDATED PRIOR TO PUBLISHING THIS UPDATE!

// FOR THE ONE WHO HAS ACCESS TO THE DISCORD DEVELOPERS ACCOUNT HOSTING ALL BOTS: ensure the logo/avatar/profile-picture uses no artwork belonging to Goldentrophy Software!

// ---

// Line 9 purposely contains invalid syntax for this file to generate a compilation error. 
// This is to bring your attention to this disclaimer. Please read it very thoroughly.
// Afterwards, this entire block, starting from line 9, and ending with line 35, can be deleted, along with the comment on line 43.
// - @corgisolutions; contact if any confusion

namespace iiMenu
{
    public class PluginInfo
    {
        public const string GUID = "corgi.gorillatag.iireborn"; // :)
        public const string Name = "ii Reborn";
        public const string Description = "A Gorilla Tag mod menu.";
        public const string BuildTimestamp = "2026-09-08T00:00:00Z";
        public const string Version = "1.0.3"; // <---- BUMP UP VERSION AS PER WARNING, THIS COMMENT CAN BE REMOVED. 1.0.4 OR 1.1 AT YOUR DISCRETION

        public const string BaseDirectory = "iiReborn";
        public const string LegacyBaseDirectory = "iisStupidMenu"; // pre-rename data directory, migrated one time. read NOTICE and LICENSE
        public const string ClientResourcePath = "iiMenu.Resources.Client";
        public const string ServerResourcePath = "https://raw.githubusercontent.com/iireborn/menu/main/Resources/Server";

        public const string DiscordAppId = "1550339122777030756";

        public const string DiscordLargeImageKey = "";
        public const string DiscordSmallImageKeyOnline = "";
        public const string DiscordSmallImageKeyOffline = "";
        
        // Identified as "Tmplr" ASCII font style
        public const string Logo = @"
••  ┳┓  ┓       
┓┓  ┣┫┏┓┣┓┏┓┏┓┏┓
┗┗  ┛┗┗ ┗┛┗┛┛ ┛┗
                ";

        public static bool BetaBuild = false;
    }
}
