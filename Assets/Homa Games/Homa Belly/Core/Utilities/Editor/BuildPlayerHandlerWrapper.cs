using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;


// ReSharper disable once CheckNamespace
namespace HomaGames.HomaBelly {
    public static class BuildPlayerHandlerWrapper {

        public delegate bool BuildPlayerHandlerFilter(BuildPlayerOptions options);

        private static readonly List<BuildPlayerHandlerFilter> Filters = new List<BuildPlayerHandlerFilter>();

        public static Action<BuildPlayerOptions> BuildPlayerHandler = BuildPlayerWindow.DefaultBuildMethods.BuildPlayer;

        [PublicAPI]
        public static void AddBuildPlayerHandlerFilter(BuildPlayerHandlerFilter filter) {
            Filters.Add(filter);
        }

        [PublicAPI]
        public static void RemoveBuildPlayerHandlerFilter(BuildPlayerHandlerFilter filter) {
            Filters.Remove(filter);
        }

        [InitializeOnLoadMethod]
        private static void RegisterMainBuildPlayerHandler() {
            BuildPlayerWindow.RegisterBuildPlayerHandler(MainBuildPlayerHandler);
        }

        private static void MainBuildPlayerHandler(BuildPlayerOptions options) {
            if (Filters.Any(filter => !filter.Invoke(options))) {
                return;
            }

            BuildPlayerHandler(options);
        }
    }
}
