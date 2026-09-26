/*
 * ii Reborn
 * Portions Copyright (C) 2025–2026 Goldentrophy Software
 * Licensed under GNU GPL v3.0-or-later — see LICENSE and NOTICE.
 * This file is part of a derivative work; see NOTICE for attribution
 * and modification history. Do not remove this notice.
 */

using GorillaTagScripts;

namespace iiMenu.Utilities
{
    public class ManagerRegistry
    {
        #region Public Properties
        public class GhostReactor
        {
            public static GhostReactorManager GhostReactorManager
            {
                get => global::GhostReactor.instance.grManager;
            }

            public static GameEntityManager GameEntityManager
            {
                get => GameEntityManager.GetManagerForZone(global::GhostReactor.instance.zone);
            }
        }
        
        public class SuperInfection
        {
            public static SuperInfectionManager SuperInfectionManager
            {
                get => SuperInfectionManager.activeSuperInfectionManager;
            }

            public static global::SuperInfection ZoneSuperInfection
            {
                get => SuperInfectionManager.zoneSuperInfection;
            }

            public static GameEntityManager GameEntityManager
            {
                get => SuperInfectionManager.gameEntityManager;
            }
        }

        public class CustomMaps
        {
            public static CustomMapsGameManager CustomMapsGameManager
            {
                get => CustomMapsGameManager.instance;
            }

            public static GameEntityManager GameEntityManager
            {
                get => CustomMapsGameManager.gameEntityManager;
            }
        }

        public static BuilderTable BuilderTable
        {
            get => GetBuilderTable();
        }

        private static LightningManager _lightningManager;
        public static LightningManager LightningManager
        {
            get
            {
                if (_lightningManager == null)
                    _lightningManager = Menu.Main.GetObject("Environment Objects/05Maze_PersistentObjects/2025_Halloween1_PersistentObjects/LightningManager").GetComponent<LightningManager>();

                return _lightningManager;
            }
            set => _lightningManager = value;
        }
        #endregion

        #region Private Methods
        private static BuilderTable GetBuilderTable()
        {
            BuilderTable.TryGetBuilderTableForZone(VRRig.LocalRig.zoneEntity.currentZone, out BuilderTable table);
            return table;
        }
        #endregion
    }
}
