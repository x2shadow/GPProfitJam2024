using UnityEngine;

namespace GamePush.ConsoleController
{
    public class GP_ConsoleController : MonoBehaviour
    {
        public static GP_ConsoleController Instance;
        private void Awake() => Instance = this;

        [SerializeField] public bool AchievementsConsoleLogs = true;
        [SerializeField] public bool AdsConsoleLogs = true;
        [SerializeField] public bool AnalyticsConsoleLogs = true;
        [SerializeField] public bool AppConsoleLogs = true;
        [SerializeField] public bool AvatarGeneratorConsoleLogs = true;
        [SerializeField] public bool ChannelsConsoleLogs = true;
        [SerializeField] public bool DeviceConsoleLogs = true;
        [SerializeField] public bool DocumentsConsoleLogs = true;
        [SerializeField] public bool EventsConsoleLogs = true;
        [SerializeField] public bool ExperimentsConsoleLogs = true;
        [SerializeField] public bool FilesConsoleLogs = true;
        [SerializeField] public bool FullscreenConsoleLogs = true;
        [SerializeField] public bool GameConsoleLogs = true;
        [SerializeField] public bool GamesCollectionsConsoleLogs = true;
        [SerializeField] public bool LanguageConsoleLogs = true;
        [SerializeField] public bool LeaderboardConsoleLogs = true;
        [SerializeField] public bool LeaderboardScopedConsoleLogs = true;
        [SerializeField] public bool PaymentsConsoleLogs = true;
        [SerializeField] public bool PlatformConsoleLogs = true;
        [SerializeField] public bool PlayerConsoleLogs = true;
        [SerializeField] public bool PlayersConsoleLogs = true;
        [SerializeField] public bool RewardsConsoleLogs = true;
        [SerializeField] public bool SchedulersConsoleLogs = true;
        [SerializeField] public bool SegmentsConsoleLogs = true;
        [SerializeField] public bool ServerConsoleLogs = true;
        [SerializeField] public bool SocialsConsoleLogs = true;
        [SerializeField] public bool SystemConsoleLogs = true;
        [SerializeField] public bool VariablesConsoleLogs = true;

        [SerializeField] public bool TriggersConsoleLogs = true;
        [SerializeField] public bool UniquesConsoleLogs = true;
        [SerializeField] public bool StorageConsoleLogs = true;

        public void SwitchAll(bool value)
        {
            AchievementsConsoleLogs = value;
            AdsConsoleLogs = value;
            AnalyticsConsoleLogs = value;
            AppConsoleLogs = value;
            AvatarGeneratorConsoleLogs = value;
            ChannelsConsoleLogs = value;
            DeviceConsoleLogs = value;
            DocumentsConsoleLogs = value;
            EventsConsoleLogs = value;
            ExperimentsConsoleLogs = value;
            FilesConsoleLogs = value;
            FullscreenConsoleLogs = value;
            GameConsoleLogs = value;
            GamesCollectionsConsoleLogs = value;
            LanguageConsoleLogs = value;
            LeaderboardConsoleLogs = value;
            LeaderboardScopedConsoleLogs = value;
            PaymentsConsoleLogs = value;
            PlatformConsoleLogs = value;
            PlayerConsoleLogs = value;
            PlayersConsoleLogs = value;
            RewardsConsoleLogs = value;
            SchedulersConsoleLogs = value;
            SegmentsConsoleLogs = value;

            ServerConsoleLogs = value;
            SocialsConsoleLogs = value;
            SystemConsoleLogs = value;
            VariablesConsoleLogs = value;

            TriggersConsoleLogs = value;
            UniquesConsoleLogs = value;
            StorageConsoleLogs = value;
        }

        public bool IsModuleLogs(ModuleName name)
        {
            if (!GP_Settings.instance.viewLogs)
                return false;

            return name switch
            {
                ModuleName.Achievements => AchievementsConsoleLogs,
                ModuleName.Ads => AdsConsoleLogs,
                ModuleName.Analytics => AnalyticsConsoleLogs,
                ModuleName.App => AppConsoleLogs,
                ModuleName.AvatarGenerator => AvatarGeneratorConsoleLogs,
                ModuleName.Channels => ChannelsConsoleLogs,
                ModuleName.Device => DeviceConsoleLogs,
                ModuleName.Documents => DocumentsConsoleLogs,
                ModuleName.Events => EventsConsoleLogs,
                ModuleName.Experiments => ExperimentsConsoleLogs,
                ModuleName.Files => FilesConsoleLogs,
                ModuleName.Fullscreen => FullscreenConsoleLogs,
                ModuleName.Game => GameConsoleLogs,
                ModuleName.Triggers => TriggersConsoleLogs,
                ModuleName.Uniques => UniquesConsoleLogs,
                ModuleName.Storage => StorageConsoleLogs,
                ModuleName.Socials => SocialsConsoleLogs,
                _ => false
            };
        }
    }


}