using iiMenu.Classes.Menu;
using iiMenu.Menu;
using ExitGames.Client.Photon;
using GorillaNetworking;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Valve.Newtonsoft.Json.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using GJoinType = GorillaNetworking.JoinType;

namespace iiMenu.Managers
{
    public static class IiServersManager
    {
        public const string ApiUrl = "https://gtag.useless.best/v1/api/iiservers";
        public const string DefaultAppId = "4b0a8fa3-9ab4-4068-bb2a-b039ee8bd506";
        public const string DefaultAppVersion = "1.0";
        public const string DefaultRegion = "us";
        public static readonly string[] DefaultRoomCodes = { "II_BAN1", "II_BAN2", "II_BAN3", "II_BAN4" };
        public static readonly string LocalConfigPath = $"{PluginInfo.BaseDirectory}/iiServers.json";
        private static List<string> roomCodes = new List<string>(DefaultRoomCodes);

        public static List<string> RoomCodes => new List<string>(roomCodes);
        public static string RoomList => string.Join(" / ", roomCodes);
        public static string RoomCode(int index) => index >= 0 && index < roomCodes.Count ? roomCodes[index] : roomCodes.Count > 0 ? roomCodes[0] : null;

        public static bool IsOnIiServers { get; private set; }

        private static string savedAppIdRealtime;
        private static string savedVoiceAppId;
        private static string savedAppVersion;
        private static string savedFixedRegion;
        private static string savedServer;
        private static int savedPort;
        private static bool hasSaved;
        private static AuthenticationValues savedAuthValues;
        private static AuthenticationValues savedNetClientAuth;

        private static string iiAppId;
        private static string iiAppVersion = "1.0";
        private static string iiRegion = "us";
        private static string iiMotd;
        private static bool iiEnabled = true;
        private static bool fetching;
        private static bool connectRoutineRunning;

        public static string StatusText => IsOnIiServers ? "<color=green>iiSERVERS</color>" : "<color=grey>OFFICIAL</color>";

        public static string CloudLabel
        {
            get
            {
                string app = string.IsNullOrEmpty(iiAppId) ? "no-appid" : iiAppId.Substring(0, Math.Min(8, iiAppId.Length));
                string server = "";
                try { server = PhotonNetwork.ServerAddress; } catch { }
                string region = "";
                try { region = PhotonNetwork.CloudRegion; } catch { }
                return $"{app} {region} {server}".Trim();
            }
        }
        public static string Motd => string.IsNullOrEmpty(iiMotd) ? $"{RoomList} (10 each)" : iiMotd;

        public static void ShutdownForGameExit()
        {
            try
            {
                RestoreOfficialSettings();
                RestoreCustomAuth();
            }
            catch (Exception e)
            {
                LogManager.LogError($"iiServers shutdown restore failed: {e.Message}");
            }
            finally
            {
                IsOnIiServers = false;
            }
        }

        public static void EnterIiServers()
        {
            RefreshIiServersButtons();
            Buttons.CurrentCategoryName = "iiServers";
        }

        public static void RefreshIiServersButtons()
        {
            int cat = Buttons.GetCategory("iiServers");
            if (cat < 0) return;
            string status = IsOnIiServers ? $"<color=green>ON iiSERVERS</color> <color=grey>{CloudLabel}</color>" : "<color=grey>ON OFFICIAL</color>";
            string motdOverlap = Motd.Length > 32 ? Motd.Substring(0, 32) + "..." : Motd;
            List<ButtonInfo> codeList = new List<ButtonInfo>
            {
                new ButtonInfo { buttonText = "Exit iiServers", method = () => Buttons.CurrentCategoryName = "Room Mods", isTogglable = false, toolTip = "Back to Room Mods." },
                new ButtonInfo { buttonText = "Connect to iiServers", enableMethod = Connect, disableMethod = Disconnect, toolTip = $"Live swap to your private Photon Cloud. ON = fetch {ApiUrl} (AppId/AppVersion/Region/rooms), falling back to {LocalConfigPath} and then to the built-in defaults when the API cannot be reached -> disconnect official -> reconnect -> join {RoomList}, moving to the next code whenever one is full. OFF = restore official AppId/Version and reconnect. Banned users bypass PlayFab." },
            };

            for (int i = 0; i < roomCodes.Count && i < 4; i++)
            {
                string code = roomCodes[i];
                codeList.Add(new ButtonInfo { buttonText = $"Join {code}", method = () => JoinSpecific(code), isTogglable = false, toolTip = $"Joins {code} (10 players). Only works while on iiServers. If it is full, use the next code." });
            }

            codeList.Add(new ButtonInfo { buttonText = "iiServers Status", overlapText = $"Status <color=grey>[</color>{StatusText}<color=grey>]</color> <color=grey>{CloudLabel}</color> <color=grey>{motdOverlap}</color>", isTogglable = false, toolTip = $"API: {ApiUrl} | {Motd} | Codes: {RoomList} at 10 players each. Empty rooms are destroyed by Photon and re-created by the next player who joins.", label = true });
            codeList.Add(new ButtonInfo { buttonText = "Refresh iiServers Config", method = () => { if (CoroutineManager.instance != null) CoroutineManager.instance.StartCoroutine(FetchConfigRoutine((ok) => { RefreshIiServersButtons(); NotificationManager.SendNotification(ok ? $"<color=grey>[</color><color=green>iiSERVERS</color><color=grey>]</color> Config refreshed: {(string.IsNullOrEmpty(iiAppId) ? "no AppId yet" : iiAppId.Substring(0, Math.Min(8, iiAppId.Length)) + "...")} {iiAppVersion} {iiRegion} codes: {RoomList}" : "<color=grey>[</color><color=red>iiSERVERS</color><color=grey>]</color> Refresh failed."); })); }, isTogglable = false, toolTip = $"Re-fetches AppId/AppVersion/Region/room codes from {ApiUrl} without reconnecting. If the API is unreachable this falls back to {LocalConfigPath}, then to the built-in defaults." });

            Buttons.buttons[cat] = codeList.ToArray();
            try { var f = typeof(Buttons).GetField("cacheGetIndex", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static); var d = f?.GetValue(null) as System.Collections.IDictionary; d?.Clear(); } catch { }
        }

        public static void Connect()
        {
            if (IsOnIiServers)
            {
                NotificationManager.SendNotification("<color=grey>[</color><color=green>iiSERVERS</color><color=grey>]</color> Already on iiServers.");
                return;
            }
            if (connectRoutineRunning)
            {
                NotificationManager.SendNotification("<color=grey>[</color><color=yellow>iiSERVERS</color><color=grey>]</color> Connection already in progress.");
                return;
            }
            if (CoroutineManager.instance == null)
            {
                NotificationManager.SendNotification("<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> CoroutineManager not ready.");
                try { Buttons.GetIndex("Connect to iiServers").enabled = false; } catch { }
                return;
            }
            connectRoutineRunning = true;
            CoroutineManager.instance.StartCoroutine(ConnectRoutineGuarded());
        }

        private static IEnumerator ConnectRoutineGuarded()
        {
            try { yield return ConnectRoutine(); }
            finally { connectRoutineRunning = false; }
        }

        public static void Disconnect()
        {
            if (!IsOnIiServers)
            {
                NotificationManager.SendNotification("<color=grey>[</color><color=grey>OFFICIAL</color><color=grey>]</color> Already on official.");
                return;
            }
            if (CoroutineManager.instance == null) return;
            CoroutineManager.instance.StartCoroutine(DisconnectRoutine());
        }

        private static RoomConfig BuildIiRoomConfig()
        {
            string gm = "CASUAL";
            string queue = "DEFAULT";
            string lang = "English";
            string platform = "STANDALONE";
            string fan = "false";

            try
            {
                var trigger = PhotonNetworkController.Instance?.currentJoinTrigger ?? GorillaComputer.instance?.GetJoinTriggerForZone("forest");
                string full = null;
                try { full = trigger?.GetFullDesiredGameModeString(); } catch { }
                if (!string.IsNullOrEmpty(full))
                    gm = full;
            }
            catch { }

            try { queue = GorillaComputer.instance?.currentQueue ?? "DEFAULT"; if (string.IsNullOrEmpty(queue)) queue = "DEFAULT"; } catch { }
            try { lang = LocalisationManager.CurrentLanguage.ToString(); } catch { }
            try { platform = PhotonNetworkController.Instance?.platformTag ?? "STANDALONE"; if (string.IsNullOrEmpty(platform)) platform = "STANDALONE"; } catch { }
            try { fan = GorillaTagScripts.SubscriptionManager.IsLocalSubscribed() ? "true" : "false"; } catch { }

            Hashtable props = new Hashtable
            {
                { "gameMode", gm },
                { "platform", platform },
                { "queueName", queue },
                { "language", lang },
                { "fan_club", fan }
            };

            return new RoomConfig
            {
                createIfMissing = true,
                isJoinable = true,
                isPublic = true,
                MaxPlayers = 10,
                CustomProps = props
            };
        }

        private static RoomOptions BuildIiRoomOptions()
        {
            RoomConfig config = BuildIiRoomConfig();
            return new RoomOptions
            {
                MaxPlayers = 10,
                IsVisible = config.isPublic,
                IsOpen = config.isJoinable,
                CustomRoomProperties = config.CustomProps,
                CustomRoomPropertiesForLobby = new[] { "gameMode", "platform", "queueName", "language", "fan_club" }
            };
        }

        private static int lastPlayerCount = -1;

        public static void TickRoom()
        {
            if (!IsOnIiServers)
            {
                lastPlayerCount = -1;
                return;
            }

            if (!PhotonNetwork.InRoom)
                return;

            int players = PhotonNetwork.CurrentRoom.PlayerCount;
            if (players == lastPlayerCount)
                return;

            lastPlayerCount = players;
            LogManager.Log($"iiServers room {PhotonNetwork.CurrentRoom.Name} players={players}/{PhotonNetwork.CurrentRoom.MaxPlayers} open={PhotonNetwork.CurrentRoom.IsOpen} visible={PhotonNetwork.CurrentRoom.IsVisible} master={PhotonNetwork.IsMasterClient}");

            if (players > 1)
                NotificationManager.SendNotification($"<color=grey>[</color><color=green>iiSERVERS</color><color=grey>]</color> {players} players in {PhotonNetwork.CurrentRoom.Name}.");
        }

        private static void SetVoiceClientEnabled(bool enabled)
        {
            try
            {
                Photon.Voice.PUN.PhotonVoiceNetwork voice = Photon.Voice.PUN.PhotonVoiceNetwork.Instance;
                if (voice != null)
                    voice.enabled = enabled;
            }
            catch { }
        }

        private static IEnumerator VoiceProbeRoutine()
        {
            Photon.Voice.PUN.PhotonVoiceNetwork voice = null;
            try { voice = Photon.Voice.PUN.PhotonVoiceNetwork.Instance; } catch { }
            if (voice == null)
                yield break;

            try { voice.enabled = true; } catch { }

            float deadline = Time.time + 12f;
            while (Time.time < deadline)
            {
                ClientState state = ClientState.Disconnected;
                try { state = voice.ClientState; } catch { }

                if (state == ClientState.Joined)
                {
                    LogManager.Log("iiServers: voice client joined on the iiServers cloud");
                    yield break;
                }

                yield return new WaitForSeconds(0.5f);
            }

            SetVoiceClientEnabled(false);
            LogManager.LogError("iiServers: voice client did not join within 12s, voice disabled to stop the retry spam (the private Photon app must allow 'none' authentication for voice)");
            NotificationManager.SendNotification("<color=grey>[</color><color=yellow>iiSERVERS</color><color=grey>]</color> Voice cannot authenticate on this cloud, so it is off. Set the Photon app's voice authentication to none to use voice here.", 10000);
        }

        private static IEnumerator ConnectRoutine(bool autoJoin = true)
        {
            NotificationManager.SendNotification("<color=grey>[</color><color=cyan>iiSERVERS</color><color=grey>]</color> Checking iiServers config...");
            bool fetched = false;
            yield return FetchConfigRoutine((ok) => fetched = ok);
            if (!fetched || string.IsNullOrEmpty(iiAppId))
            {
                NotificationManager.SendNotification($"<color=grey>[</color><color=red>iiSERVERS</color><color=grey>]</color> No AppId available. Put {{\"appId\":\"...\"}} in {LocalConfigPath} and try again.", 8000);
                LogManager.LogError($"iiServers: no appId from API, {LocalConfigPath} or defaults");
                RefreshIiServersButtons();
                yield break;
            }
            if (!iiEnabled)
            {
                NotificationManager.SendNotification("<color=grey>[</color><color=yellow>iiSERVERS</color><color=grey>]</color> iiServers is disabled by API (enabled=false).");
                try { Buttons.GetIndex("Connect to iiServers").enabled = false; } catch { }
                yield break;
            }

            try { Buttons.GetIndex("Connect to iiServers").enabled = true; } catch { }

            SaveOriginalIfNeeded();

            NotificationManager.SendNotification($"<color=grey>[</color><color=cyan>iiSERVERS</color><color=grey>]</color> Leaving official and swapping to iiServers ({(string.IsNullOrEmpty(iiRegion) ? "us" : iiRegion)})...", 5000);

            try { NetworkSystem.Instance.ReturnToSinglePlayer(); } catch { }
            try { PhotonNetwork.Disconnect(); } catch { }

            float deadline = Time.time + 15f;
            while (Time.time < deadline && (PhotonNetwork.IsConnected || PhotonNetwork.NetworkClientState != ClientState.Disconnected))
                yield return null;
            deadline = Time.time + 12f;
            while (Time.time < deadline && NetworkSystem.Instance != null && NetworkSystem.Instance.netState != NetSystemState.Idle)
                yield return null;
            if (PhotonNetwork.IsConnected || PhotonNetwork.NetworkClientState != ClientState.Disconnected)
            {
                LogManager.LogError($"iiServers pre-swap still connected state={PhotonNetwork.NetworkClientState} netState={NetworkSystem.Instance?.netState}");
                try { PhotonNetwork.Disconnect(); } catch { }
                yield return new WaitForSeconds(0.8f);
            }
            yield return new WaitForSeconds(0.4f);

            try
            {
                var settings = PhotonNetwork.PhotonServerSettings.AppSettings;
                string voiceBefore = string.IsNullOrEmpty(settings.AppIdVoice) ? "(empty)" : settings.AppIdVoice.Substring(0, Math.Min(8, settings.AppIdVoice.Length)) + "...";
                settings.AppIdRealtime = iiAppId;
                settings.AppIdVoice = iiAppId;
                settings.AppVersion = string.IsNullOrEmpty(iiAppVersion) ? "1.0" : iiAppVersion;
                settings.FixedRegion = string.IsNullOrEmpty(iiRegion) ? "us" : iiRegion.ToLowerInvariant();
                LogManager.Log($"iiServers: swapped AppSettings -> {iiAppId.Substring(0, Math.Min(8, iiAppId.Length))}... {settings.AppVersion} {settings.FixedRegion}");
                LogManager.Log($"iiServers: voice appId {voiceBefore} -> {iiAppId.Substring(0, Math.Min(8, iiAppId.Length))}... (voice must follow the cloud, not stay on the official voice app)");
            }
            catch (Exception e)
            {
                NotificationManager.SendNotification($"<color=grey>[</color><color=red>ERROR</color><color=grey>]</color> Could not swap AppSettings: {e.Message}");
                LogManager.LogError($"iiServers swap failed: {e}");
                yield break;
            }

            ApplyIiServersAuth();

            NotificationManager.SendNotification($"<color=grey>[</color><color=cyan>iiSERVERS</color><color=grey>]</color> Connecting to iiServers {(string.IsNullOrEmpty(iiRegion) ? "" : $"({iiRegion})")}...", 5000);

            for (int attempt = 0; attempt < 4 && !PhotonNetwork.IsConnectedAndReady; attempt++)
            {
                try
                {
                    if (PhotonNetwork.IsConnected || PhotonNetwork.NetworkClientState != ClientState.Disconnected)
                        PhotonNetwork.Disconnect();
                }
                catch { }

                float disconnectDeadline = Time.time + 6f;
                while (Time.time < disconnectDeadline && PhotonNetwork.NetworkClientState != ClientState.Disconnected)
                    yield return null;

                if (!PhotonNetwork.IsConnected && PhotonNetwork.NetworkClientState == ClientState.Disconnected)
                {
                    try { PhotonNetwork.ConnectUsingSettings(); } catch (Exception e) { LogManager.LogError($"iiServers ConnectUsingSettings failed: {e.Message}"); }
                }

                deadline = Time.time + 14f;
                while (Time.time < deadline && !PhotonNetwork.IsConnectedAndReady)
                    yield return null;
            }

            LogManager.Log($"iiServers connect result: ready={PhotonNetwork.IsConnectedAndReady} state={PhotonNetwork.NetworkClientState} appId={PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime.Substring(0, Math.Min(8, PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime.Length))} version={PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion} server={PhotonNetwork.ServerAddress} region={PhotonNetwork.CloudRegion}");

            if (!PhotonNetwork.IsConnectedAndReady)
            {
                string err = PhotonNetwork.NetworkClientState.ToString();
                NotificationManager.SendNotification($"<color=grey>[</color><color=red>iiSERVERS</color><color=grey>]</color> Could not connect to iiServers ({err}). Check AppId is PUN (not Fusion) and CCU not full.");
                LogManager.LogError($"iiServers connect timeout state={PhotonNetwork.NetworkClientState} server={PhotonNetwork.Server} cloudRegion={PhotonNetwork.CloudRegion}");
                if (!string.IsNullOrEmpty(iiMotd))
                    NotificationManager.SendNotification($"<color=grey>[</color><color=cyan>MOTD</color><color=grey>]</color> {iiMotd}", 6000);
                IsOnIiServers = false;
                RestoreOfficialSettings();
                RestoreCustomAuth();
                try { PhotonNetwork.Disconnect(); } catch { }
                yield return new WaitForSeconds(0.5f);
                try { PhotonNetwork.ConnectUsingSettings(); } catch (Exception restoreError) { LogManager.LogError($"iiServers official rollback failed: {restoreError.Message}"); }
                RefreshIiServersButtons();
                yield break;
            }

            IsOnIiServers = true;
            SetVoiceClientEnabled(true);
            RefreshIiServersButtons();

            if (!string.IsNullOrEmpty(iiMotd))
                NotificationManager.SendNotification($"<color=grey>[</color><color=cyan>MOTD</color><color=grey>]</color> {iiMotd}", 6000);

            if (!autoJoin)
                yield break;

            NotificationManager.SendNotification($"<color=grey>[</color><color=green>iiSERVERS</color><color=grey>]</color> Connected to {CloudLabel}! Joining {RoomList}...", 6000);
            yield return JoinIiServersRooms();
        }

        private static IEnumerator DisconnectRoutine()
        {
            NotificationManager.SendNotification("<color=grey>[</color><color=cyan>iiSERVERS</color><color=grey>]</color> Restoring official servers...");

            SetVoiceClientEnabled(true);

            try { NetworkSystem.Instance.ReturnToSinglePlayer(); } catch { }
            try { PhotonNetwork.Disconnect(); } catch { }

            float deadline = Time.time + 10f;
            while (Time.time < deadline && PhotonNetwork.IsConnected)
                yield return null;
            deadline = Time.time + 8f;
            while (Time.time < deadline && NetworkSystem.Instance != null && NetworkSystem.Instance.netState != NetSystemState.Idle)
                yield return null;
            yield return new WaitForSeconds(0.5f);

            try
            {
                if (hasSaved)
                {
                    var s = PhotonNetwork.PhotonServerSettings.AppSettings;
                    s.AppIdRealtime = savedAppIdRealtime;
                    s.AppIdVoice = savedVoiceAppId;
                    s.AppVersion = savedAppVersion;
                    s.FixedRegion = savedFixedRegion;
                    if (!string.IsNullOrEmpty(savedServer)) s.Server = savedServer; else s.Server = "";
                    if (savedPort != 0) s.Port = savedPort;
                    LogManager.Log($"iiServers: restored official {savedAppIdRealtime?.Substring(0, Math.Min(8, savedAppIdRealtime.Length))}... {savedAppVersion} {savedFixedRegion}");
                }
            }
            catch (Exception e) { LogManager.LogError($"iiServers restore failed: {e.Message}"); }

            RestoreCustomAuth();

            yield return new WaitForSeconds(0.2f);
            try { PhotonNetwork.ConnectUsingSettings(); } catch (Exception e) { LogManager.LogError($"iiServers official reconnect failed: {e.Message}"); }

            deadline = Time.time + 12f;
            while (Time.time < deadline && !PhotonNetwork.IsConnectedAndReady)
                yield return null;

            IsOnIiServers = false;
            RefreshIiServersButtons();
            if (PhotonNetwork.IsConnectedAndReady)
                NotificationManager.SendNotification("<color=grey>[</color><color=green>OFFICIAL</color><color=grey>]</color> Back on official servers.");
            else
                NotificationManager.SendNotification("<color=grey>[</color><color=yellow>OFFICIAL</color><color=grey>]</color> Restored settings. Reconnecting...", 6000);
        }

        private static IEnumerator JoinIiServersRooms()
        {
            float connectedDeadline = Time.time + 10f;
            while (Time.time < connectedDeadline && !PhotonNetwork.IsConnectedAndReady)
                yield return null;
            if (!PhotonNetwork.IsConnectedAndReady)
            {
                NotificationManager.SendNotification("<color=grey>[</color><color=red>iiSERVERS</color><color=grey>]</color> Not connected, cannot join room.");
                yield break;
            }

            foreach (string code in roomCodes.ToArray())
            {
                if (PhotonNetwork.InRoom)
                    yield break;

                NotificationManager.SendNotification($"<color=grey>[</color><color=cyan>iiSERVERS</color><color=grey>]</color> Joining {code}...", 3000);
                yield return JoinRoom(code);

                if (PhotonNetwork.InRoom)
                {
                    NotificationManager.SendNotification($"<color=grey>[</color><color=green>iiSERVERS</color><color=grey>]</color> Joined {PhotonNetwork.CurrentRoom.Name}! ({PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}) {CloudLabel}", 5000);
                    yield break;
                }

                NotificationManager.SendNotification($"<color=grey>[</color><color=yellow>iiSERVERS</color><color=grey>]</color> {code} is full or unavailable ({JoinFailureReason(PhotonNetwork.NetworkClientState.ToString())}), trying the next code...", 3000);
            }

            NotificationManager.SendNotification($"<color=grey>[</color><color=red>iiSERVERS</color><color=grey>]</color> Every iiServers code is full or unavailable: {RoomList}", 9000);
            LogManager.Log($"iiServers join failed for every code state={PhotonNetwork.NetworkClientState} reason={lastJoinError}");
        }

        private static string lastJoinError;

        private static void AttachJoinErrorListener()
        {
            try
            {
                if (PhotonNetwork.NetworkingClient == null)
                    return;

                PhotonNetwork.NetworkingClient.OpResponseReceived -= OnJoinOperationResponse;
                PhotonNetwork.NetworkingClient.OpResponseReceived += OnJoinOperationResponse;
            }
            catch { }
        }

        private static void DetachJoinErrorListener()
        {
            try
            {
                if (PhotonNetwork.NetworkingClient == null)
                    return;

                PhotonNetwork.NetworkingClient.OpResponseReceived -= OnJoinOperationResponse;
            }
            catch { }
        }

        private static void OnJoinOperationResponse(OperationResponse response)
        {
            if (response.ReturnCode == 0)
                return;

            lastJoinError = $"{response.OperationCode}/{response.ReturnCode} {response.DebugMessage}";
        }

        private static string JoinFailureReason(string state) =>
            string.IsNullOrEmpty(lastJoinError) ? $"no reason reported by Photon ({state})" : $"{lastJoinError} ({state})";

        private static IEnumerator JoinRoom(string room)
        {
            if (PhotonNetwork.InRoom)
                yield break;

            lastJoinError = null;
            AttachJoinErrorListener();

            if (PhotonNetwork.InRoom)
                yield return ReturnToIdle();

            bool announcedCreate = false;
            float deadline = Time.time + 20f;
            float nextReconnect = 0f;

            while (Time.time < deadline && !PhotonNetwork.InRoom)
            {
                if (PhotonNetwork.NetworkClientState == ClientState.ConnectedToMasterServer)
                {
                    try
                    {
                        PhotonNetworkController.Instance.currentJoinType = GJoinType.Solo;
                        PhotonNetwork.JoinOrCreateRoom(room, BuildIiRoomOptions(), TypedLobby.Default);

                        if (!announcedCreate)
                        {
                            announcedCreate = true;
                            LogManager.Log($"iiServers: joining or creating {room} on the iiServers cloud");
                        }
                    }
                    catch (Exception e)
                    {
                        LogManager.LogError($"iiServers join {room} failed: {e.Message}");
                    }

                    yield return new WaitForSeconds(1.5f);
                    continue;
                }

                if (Time.time >= nextReconnect && !PhotonNetwork.IsConnected)
                {
                    nextReconnect = Time.time + 3f;
                    LogManager.Log($"iiServers: reconnecting to the iiServers cloud before joining {room}");
                    try { PhotonNetwork.ConnectUsingSettings(); } catch (Exception e) { LogManager.LogError($"iiServers reconnect failed: {e.Message}"); }
                }

                yield return new WaitForSeconds(0.5f);
            }

            DetachJoinErrorListener();

            if (!PhotonNetwork.InRoom)
                yield break;

            try { NetworkSystem.Instance.netState = NetSystemState.InGame; } catch { }

            lastPlayerCount = -1;

            LogManager.Log($"iiServers joined {PhotonNetwork.CurrentRoom.Name} players={PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers} open={PhotonNetwork.CurrentRoom.IsOpen} visible={PhotonNetwork.CurrentRoom.IsVisible} master={PhotonNetwork.IsMasterClient} netState={NetworkSystem.Instance?.netState} photon={PhotonNetwork.NetworkClientState} region={PhotonNetwork.CloudRegion} error={lastJoinError}");

            yield return EnsureMapLoaded();

            if (CoroutineManager.instance != null)
                CoroutineManager.instance.StartCoroutine(VoiceProbeRoutine());
        }

        private static IEnumerator ReturnToIdle()
        {
            try { NetworkSystem.Instance.ReturnToSinglePlayer(); } catch { }

            float idleDeadline = Time.time + 6f;
            while (Time.time < idleDeadline && NetworkSystem.Instance != null && NetworkSystem.Instance.netState != NetSystemState.Idle)
                yield return null;

            yield return new WaitForSeconds(0.5f);
        }

        private static IEnumerator EnsureMapLoaded()
        {
            yield return new WaitForSeconds(1.5f);

            try
            {
                if (iiMenu.Menu.Main.GetObject("Environment Objects/LocalObjects_Prefab") != null)
                    yield break;
            }
            catch { }

            LogManager.Log($"iiServers: joined {PhotonNetwork.CurrentRoom.Name} with no map loaded, reloading the zone");
            iiMenu.Mods.Important.FixMap();
        }

        public static void JoinSpecific(string room)
        {
            if (string.IsNullOrEmpty(room))
            {
                NotificationManager.SendNotification("<color=grey>[</color><color=red>iiSERVERS</color><color=grey>]</color> No iiServers code is set. Check the API or connect first.");
                return;
            }

            if (CoroutineManager.instance == null) return;

            if (!IsOnIiServers || !PhotonNetwork.IsConnectedAndReady)
            {
                NotificationManager.SendNotification($"<color=grey>[</color><color=cyan>iiSERVERS</color><color=grey>]</color> Connecting to iiServers, then joining {room}...", 4000);
                CoroutineManager.instance.StartCoroutine(ConnectThenJoin(room));
                return;
            }

            CoroutineManager.instance.StartCoroutine(JoinSpecificRoutine(room));
        }

        private static IEnumerator ConnectThenJoin(string room)
        {
            yield return ConnectRoutine(false);

            if (IsOnIiServers && PhotonNetwork.IsConnectedAndReady)
                yield return JoinSpecificRoutine(room);
            else
                NotificationManager.SendNotification($"<color=grey>[</color><color=red>iiSERVERS</color><color=grey>]</color> Could not reach iiServers, so {room} was not joined.", 8000);
        }

        private static IEnumerator JoinSpecificRoutine(string room)
        {
            if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.Name == room)
            {
                NotificationManager.SendNotification($"<color=grey>[</color><color=green>iiSERVERS</color><color=grey>]</color> Already in {room}.");
                yield break;
            }
            NotificationManager.SendNotification($"<color=grey>[</color><color=cyan>iiSERVERS</color><color=grey>]</color> Joining {room}...", 3000);
            yield return JoinRoom(room);

            if (PhotonNetwork.InRoom)
                NotificationManager.SendNotification($"<color=grey>[</color><color=green>iiSERVERS</color><color=grey>]</color> Joined {PhotonNetwork.CurrentRoom.Name}! ({PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}) {CloudLabel}", 5000);
            else
                NotificationManager.SendNotification($"<color=grey>[</color><color=red>iiSERVERS</color><color=grey>]</color> Could not join {room}: {JoinFailureReason(PhotonNetwork.NetworkClientState.ToString())}", 7000);
        }

        private static IEnumerator FetchConfigRoutine(Action<bool> callback)
        {
            if (fetching) { callback?.Invoke(!string.IsNullOrEmpty(iiAppId)); yield break; }
            fetching = true;

            string json = null;
            string lastError = null;

            UnityWebRequest req = null;
            UnityWebRequestAsyncOperation op = null;

            try
            {
                req = UnityWebRequest.Get(ApiUrl);
                req.SetRequestHeader("User-Agent", "iis-Stupid-Menu");
                req.SetRequestHeader("Accept", "application/json");
                req.timeout = 12;
                op = req.SendWebRequest();
            }
            catch (Exception e)
            {
                lastError = $"{ApiUrl} -> {e.GetType().Name}: {e.Message}";
                LogManager.LogError($"iiServers fetch {lastError}");
            }

            if (op != null)
            {
                yield return op;

                if (req.result != UnityWebRequest.Result.Success)
                {
                    lastError = $"{ApiUrl} -> {req.error}";
                    LogManager.LogError($"iiServers fetch {lastError}");
                }
                else
                {
                    json = req.downloadHandler.text;
                }
            }

            req?.Dispose();
            req = null;

            if (!string.IsNullOrEmpty(json) && ApplyConfigJson(json, "api"))
            {
                fetching = false;
                callback?.Invoke(true);
                yield break;
            }

            if (ApplyLocalConfigFile())
            {
                LogManager.Log($"iiServers: API unreachable ({lastError ?? "empty response"}), using {LocalConfigPath} -> {iiAppId.Substring(0, Math.Min(8, iiAppId.Length))}... {iiAppVersion} {iiRegion} codes={RoomList}");
                fetching = false;
                callback?.Invoke(true);
                yield break;
            }

            ApplyDefaultConfig();
            LogManager.LogError($"iiServers: API unreachable ({lastError ?? "empty response"}) and no usable {LocalConfigPath}, using built-in defaults -> {iiAppId.Substring(0, Math.Min(8, iiAppId.Length))}... {iiAppVersion} {iiRegion} codes={RoomList}");
            fetching = false;
            callback?.Invoke(true);
        }

        private static bool ApplyConfigJson(string json, string source)
        {
            try
            {
                JObject obj = JObject.Parse(json);
                string appId = (string)obj["appId"] ?? (string)obj["appIdRealtime"] ?? (string)obj["AppId"] ?? (string)obj["AppIdRealtime"];
                string appVersion = (string)obj["appVersion"] ?? (string)obj["AppVersion"] ?? (string)obj["app_version"] ?? "1.0";
                string region = (string)obj["region"] ?? (string)obj["Region"] ?? (string)obj["fixedRegion"] ?? (string)obj["FixedRegion"] ?? "us";
                string motd = (string)obj["motd"] ?? (string)obj["MOTD"] ?? (string)obj["message"] ?? null;
                JToken enabledTok = obj["enabled"] ?? obj["Enabled"];
                bool enabled = enabledTok == null ? true : enabledTok.Type == JTokenType.Boolean ? (bool)enabledTok : true;

                List<string> codes = ParseRoomCodes(obj);

                if (string.IsNullOrEmpty(appId))
                {
                    LogManager.LogError($"iiServers parse ({source}): missing appId in {json.Substring(0, Math.Min(300, json.Length))}");
                    return false;
                }

                if (codes.Count > 0)
                    roomCodes = codes;

                iiAppId = appId.Trim();
                iiAppVersion = string.IsNullOrWhiteSpace(appVersion) ? "1.0" : appVersion.Trim();
                iiRegion = string.IsNullOrWhiteSpace(region) ? "us" : region.Trim();
                iiMotd = motd;
                iiEnabled = enabled;
                LogManager.Log($"iiServers config ({source}): {iiAppId.Substring(0, Math.Min(8, iiAppId.Length))}... {iiAppVersion} {iiRegion} enabled={iiEnabled} codes={RoomList}");
                return true;
            }
            catch (Exception e)
            {
                LogManager.LogError($"iiServers parse ({source}) failed: {e.Message} json={json?.Substring(0, Math.Min(400, json.Length))}");
                return false;
            }
        }

        private static bool ApplyLocalConfigFile()
        {
            try
            {
                if (!File.Exists(LocalConfigPath))
                {
                    WriteLocalConfigTemplate();
                    return false;
                }

                return ApplyConfigJson(File.ReadAllText(LocalConfigPath), "local file");
            }
            catch (Exception e)
            {
                LogManager.LogError($"iiServers local config read failed: {e.Message}");
                return false;
            }
        }

        private static void WriteLocalConfigTemplate()
        {
            try
            {
                Directory.CreateDirectory(PluginInfo.BaseDirectory);
                JObject obj = new JObject
                {
                    ["appId"] = DefaultAppId,
                    ["appVersion"] = DefaultAppVersion,
                    ["region"] = DefaultRegion,
                    ["rooms"] = new JArray(DefaultRoomCodes),
                    ["motd"] = "Welcome to iiServers!",
                    ["enabled"] = true
                };
                File.WriteAllText(LocalConfigPath, obj.ToString());
                LogManager.Log($"iiServers: wrote {LocalConfigPath}, edit it to change the AppId or room codes without relying on the API");
            }
            catch (Exception e)
            {
                LogManager.LogError($"iiServers local config write failed: {e.Message}");
            }
        }

        private static void ApplyDefaultConfig()
        {
            iiAppId = DefaultAppId;
            iiAppVersion = DefaultAppVersion;
            iiRegion = DefaultRegion;
            iiEnabled = true;
            roomCodes = new List<string>(DefaultRoomCodes);
        }

        private static List<string> ParseRoomCodes(JObject obj)
        {
            List<string> codes = new List<string>();

            try
            {
                JToken rooms = obj["rooms"] ?? obj["roomCodes"] ?? obj["codes"] ?? obj["Rooms"] ?? obj["RoomCodes"];

                if (rooms != null)
                {
                    if (rooms.Type == JTokenType.Array)
                    {
                        foreach (JToken token in rooms)
                        {
                            string code = (string)token;
                            if (!string.IsNullOrWhiteSpace(code))
                                codes.Add(code.Trim().ToUpperInvariant());
                        }
                    }
                    else
                    {
                        string text = (string)rooms;
                        if (!string.IsNullOrWhiteSpace(text))
                            codes.AddRange(text.Split(new[] { ',', '|', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(piece => piece.Trim().ToUpperInvariant()));
                    }
                }

                if (codes.Count == 0)
                {
                    string first = (string)obj["room1"] ?? (string)obj["room"];
                    string second = (string)obj["room2"];

                    if (!string.IsNullOrWhiteSpace(first)) codes.Add(first.Trim().ToUpperInvariant());
                    if (!string.IsNullOrWhiteSpace(second)) codes.Add(second.Trim().ToUpperInvariant());
                }
            }
            catch { }

            return codes.Where(code => !string.IsNullOrEmpty(code)).Distinct().Take(6).ToList();
        }

        private static void SaveOriginalIfNeeded()
        {
            if (hasSaved) return;
            try
            {
                var s = PhotonNetwork.PhotonServerSettings.AppSettings;
                savedAppIdRealtime = s.AppIdRealtime;
                savedVoiceAppId = s.AppIdVoice;
                savedAppVersion = s.AppVersion;
                savedFixedRegion = s.FixedRegion;
                savedServer = s.Server;
                savedPort = s.Port;
                try { savedAuthValues = PhotonNetwork.AuthValues; } catch { }
                try
                {
                    var client = PhotonNetwork.NetworkingClient;
                    if (client != null)
                    {
                        var p = client.GetType().GetProperty("AuthValues");
                        if (p != null) savedNetClientAuth = p.GetValue(client) as AuthenticationValues;
                    }
                }
                catch { }
                hasSaved = true;
                LogManager.Log($"iiServers saved official {savedAppIdRealtime?.Substring(0, Math.Min(8, savedAppIdRealtime.Length))}... {savedAppVersion} {savedFixedRegion} voice={savedVoiceAppId ?? "(empty)"}");
            }
            catch (Exception e) { LogManager.LogError($"iiServers save original failed: {e.Message}"); }
        }

        private static string iiUserId;

        private static void ApplyIiServersAuth()
        {
            string userId = null;

            try
            {
                if (savedAuthValues != null && !string.IsNullOrEmpty(savedAuthValues.UserId))
                    userId = savedAuthValues.UserId;
            }
            catch { }

            if (string.IsNullOrEmpty(userId))
            {
                if (string.IsNullOrEmpty(iiUserId))
                    iiUserId = Guid.NewGuid().ToString("N");
                userId = iiUserId;
            }

            try
            {
                AuthenticationValues auth = new AuthenticationValues(userId);
                auth.AuthType = CustomAuthenticationType.None;

                var prop = typeof(PhotonNetwork).GetProperty("AuthValues");
                if (prop != null && prop.CanWrite)
                    prop.SetValue(null, auth);

                var client = PhotonNetwork.NetworkingClient;
                if (client != null)
                {
                    var p = client.GetType().GetProperty("AuthValues");
                    if (p != null && p.CanWrite)
                        p.SetValue(client, auth);
                }

                LogManager.Log($"iiServers: auth set to none with userId {userId.Substring(0, Math.Min(8, userId.Length))}... so Gorilla Tag gets a non-null player id");
            }
            catch (Exception e)
            {
                LogManager.LogError($"iiServers auth setup failed: {e.Message}");
            }
        }

        private static void RestoreOfficialSettings()
        {
            if (!hasSaved) return;
            try
            {
                var settings = PhotonNetwork.PhotonServerSettings.AppSettings;
                settings.AppIdRealtime = savedAppIdRealtime;
                settings.AppIdVoice = savedVoiceAppId;
                settings.AppVersion = savedAppVersion;
                settings.FixedRegion = savedFixedRegion;
                settings.Server = savedServer ?? "";
                settings.Port = savedPort;
            }
            catch (Exception e)
            {
                LogManager.LogError($"iiServers official settings rollback failed: {e.Message}");
            }
        }

        private static void RestoreCustomAuth()
        {
            try
            {
                if (savedAuthValues != null)
                {
                    var prop = typeof(PhotonNetwork).GetProperty("AuthValues");
                    if (prop != null && prop.CanWrite) prop.SetValue(null, savedAuthValues);
                }
            }
            catch { }
            try
            {
                var client = PhotonNetwork.NetworkingClient;
                if (client != null && savedNetClientAuth != null)
                {
                    var p = client.GetType().GetProperty("AuthValues");
                    if (p != null && p.CanWrite) p.SetValue(client, savedNetClientAuth);
                }
            }
            catch { }
        }
    }
}
