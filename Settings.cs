using System.Reflection.Metadata;
using CsvHelper.Configuration;

namespace GameTrackerEx01
{
    public sealed class SettingsMap : ClassMap<SettingsInfo>
    {
        public SettingsMap()
        {
            Map(m => m.dateLastUsed);
            Map(m => m.settingsID);
            Map(m => m.fileName);
            Map(m => m.nextSettingsID);
            Map(m => m.activeSettingsID);
            Map(m => m.nextGameID);
            Map(m => m.nextFranchiseID);
            Map(m => m.nextGenreID);
            Map(m => m.nextLogID);
            Map(m => m.genreFile);
            Map(m => m.franchiseFile);
            Map(m => m.logEntryFile);
            Map(m => m.currentGameFile);
            Map(m => m.completedGameFile);
            Map(m => m.droppedGameFile);
            Map(m => m.unpurchasedGameFile);
            Map(m => m.backloggedGameFile);
            Map(m => m.filterWaitingForPrequelSetting);
            Map(m => m.filterUnreleasedSetting);
            Map(m => m.filterReplaysSetting);
            Map(m => m.filterPlayedSetting);
            Map(m => m.filterPlayingSetting);
            Map(m => m.filterDroppedSetting);
            Map(m => m.filterNotPurchasedSetting);
            Map(m => m.filterDeckPlayableSetting);
            Map(m => m.avgLengthMattersSetting);
            Map(m => m.avgLengthBaseSetting);
            Map(m => m.avgLengthDeviationSetting);
            Map(m => m.requiresCertainExcitementSetting);
            Map(m => m.requiredExcitementLevelOrRatingSetting);
            Map(m => m.onlySessionGamesSetting);
            Map(m => m.ignoreSessionGamesSetting);
            Map(m => m.avgGameLengthRelative);
            Map(m => m.sessionGameDesired);
            Map(m => m.sessionGameRequired);
            Map(m => m.deckPlayableMatch);
            Map(m => m.removeSessionLengthReq);
            Map(m => m.maxSimilarGameOutput);
            Map(m => m.launchLandingPage);
            Map(m => m.landingShowCurrent);
            Map(m => m.landingShowNewRelease);
            Map(m => m.landingShowRecentRecommended);
            Map(m => m.autoDropCurrentAfterDuration);
            Map(m => m.daysRequiredToDropCurrent);
            Map(m => m.autoDropSession);
            Map(m => m.autoDropNotificationDays);
        }
    }

    public sealed class RetrieveSettingsMap : ClassMap<SettingsInfo>
    {
        public RetrieveSettingsMap()
        {
            Map(m => m.dateLastUsed);
            Map(m => m.settingsID);
            Map(m => m.fileName);
            Map(m => m.nextSettingsID);
            Map(m => m.activeSettingsID);
            Map(m => m.nextGameID);
            Map(m => m.nextFranchiseID);
            Map(m => m.nextGenreID);
            Map(m => m.nextLogID);
            Map(m => m.genreFile);
            Map(m => m.franchiseFile);
            Map(m => m.logEntryFile);
            Map(m => m.currentGameFile);
            Map(m => m.completedGameFile);
            Map(m => m.droppedGameFile);
            Map(m => m.unpurchasedGameFile);
            Map(m => m.backloggedGameFile);
            Map(m => m.filterWaitingForPrequelSetting);
            Map(m => m.filterUnreleasedSetting);
            Map(m => m.filterReplaysSetting);
            Map(m => m.filterPlayedSetting);
            Map(m => m.filterPlayingSetting);
            Map(m => m.filterDroppedSetting);
            Map(m => m.filterNotPurchasedSetting);
            Map(m => m.filterDeckPlayableSetting);
            Map(m => m.avgLengthMattersSetting);
            Map(m => m.avgLengthBaseSetting);
            Map(m => m.avgLengthDeviationSetting);
            Map(m => m.requiresCertainExcitementSetting);
            Map(m => m.requiredExcitementLevelOrRatingSetting);
            Map(m => m.onlySessionGamesSetting);
            Map(m => m.ignoreSessionGamesSetting);
            Map(m => m.avgGameLengthRelative);
            Map(m => m.sessionGameDesired);
            Map(m => m.sessionGameRequired);
            Map(m => m.deckPlayableMatch);
            Map(m => m.removeSessionLengthReq);
            Map(m => m.maxSimilarGameOutput);
            Map(m => m.launchLandingPage);
            Map(m => m.landingShowCurrent);
            Map(m => m.landingShowNewRelease);
            Map(m => m.landingShowRecentRecommended);
            Map(m => m.autoDropCurrentAfterDuration);
            Map(m => m.daysRequiredToDropCurrent);
            Map(m => m.autoDropSession);
            Map(m => m.autoDropNotificationDays);
        }
    }
    public class SettingsInfo {
        public DateOnly dateLastUsed {get; set;} = DateOnly.FromDateTime(DateTime.Now);
        public int settingsID {get; set;}
        public string fileName {get; set;}
        public int nextSettingsID {get; set;}
        public int activeSettingsID {get; set;}
        public int nextGameID {get; set;}
        public int nextFranchiseID {get; set;}
        public int nextGenreID {get; set;}
        public int nextLogID {get; set;}
        public string genreFile {get; set;}
        public string franchiseFile {get; set;}
        public string logEntryFile {get; set;}
        public string currentGameFile {get; set;}
        public string completedGameFile {get; set;}
        public string droppedGameFile {get; set;}
        public string unpurchasedGameFile {get; set;}
        public string backloggedGameFile {get; set;}
        public bool filterWaitingForPrequelSetting {get; set;} = true;
        public bool filterUnreleasedSetting {get; set;} = false; 
        public bool filterReplaysSetting {get; set;} = false; 
        public bool filterPlayedSetting {get; set;} = true;
        public bool filterPlayingSetting {get; set;} = false;
        public bool filterDroppedSetting {get; set;} = false;
        public bool filterNotPurchasedSetting {get; set;} = false;
        public bool filterDeckPlayableSetting {get; set;} = false;
        public bool avgLengthMattersSetting {get; set;} = false;
        public float avgLengthBaseSetting {get; set;} = 0;
        public float avgLengthDeviationSetting {get; set;} = 0;
        public bool requiresCertainExcitementSetting {get; set;} = false;
        public int requiredExcitementLevelOrRatingSetting {get; set;} = 8;
        public bool onlySessionGamesSetting {get; set;} = false;
        public bool ignoreSessionGamesSetting {get; set;} = false;
        public bool avgGameLengthRelative {get; set;} = false;
        public bool sessionGameDesired {get; set;} = false;
        public bool sessionGameRequired {get; set;} = false;
        public bool deckPlayableMatch {get; set;} = false;
        public bool removeSessionLengthReq {get; set;} = false;
        public int maxSimilarGameOutput {get; set;} = 5;
        public bool launchLandingPage {get; set;} = true;
        public bool landingShowCurrent {get; set;} = true;
        public bool landingShowNewRelease {get; set;} = true;
        public bool landingShowRecentRecommended {get; set;} = true;
        public bool autoDropCurrentAfterDuration {get; set;} = true;
        public int daysRequiredToDropCurrent {get; set;} = 60;
        public bool autoDropSession {get; set;} = false;
        public int autoDropNotificationDays {get; set;} = 15;
        public int maxLoadingRecommend {get; set;} = 5;

        public void OpenSettings()
        {
            Console.WriteLine("\n\nWelcome to your Settings Page!\nPlease select what you would like to edit below:\n");
            Console.WriteLine("1. Change similar game filters");
            Console.WriteLine("2. Manage backups");
            Console.WriteLine("3. Manage landing page");
            Console.WriteLine("4. Manage automatic game type changes");
            Console.WriteLine("0. Exit");
            int userInput = Format.GetSingleResponse(4, "What action would you like to take?");
            switch (userInput)
            {
                case 1:
                    ManageSimilarGamesSettings();
                    break;
                case 2:
                    ManageBackupsSettings();
                    break;
                case 3:
                    ManageLandingPageSettings();
                    break;
                case 4:
                    ManageTypeChangeSettings();
                    break;
                case 0:
                    return;
            }
            OpenSettings();
        }

        public void ManageSimilarGamesSettings()
        {
            Console.WriteLine("\n\nWhat game filtering category would you like to edit?\n");
            Console.WriteLine("1. Played Type Restrictions");
            Console.WriteLine("2. Game Prequels, Purchase Status & Deck Playability");
            Console.WriteLine("3. Average Game Duration");
            Console.WriteLine("4. Game Excitement/Rating");
            Console.WriteLine("5. Session Game Status");
            Console.WriteLine("6. Maximum Recommendation Count");
            Console.WriteLine("0. Exit");
            int userInput = Format.GetSingleResponse(6, "What action would you like to take?");
            switch (userInput)
            {
                case 1:
                    AlterPlayedTypeRestrictions();
                    break;
                case 2:
                    AlterMiscRestrictions();
                    break;
                case 3:
                    AlterDurationRestrictions();
                    break;
                case 4:
                    AlterExcitementRestrictions();
                    break;
                case 5:
                    AlterSessionGameRestrictions();
                    break;
                case 6:
                    AlterMaxSimilarGameCount();
                    break;
                case 0:
                    return;
            }
            ManageSimilarGamesSettings();
        }

        public void AlterPlayedTypeRestrictions()
        {
            Console.WriteLine("\n\nHere are your current settings preferences\n");
            Console.WriteLine($"1. Remove Played Games:                              {filterPlayedSetting}");
            if (!filterPlayedSetting)
            {
                Console.WriteLine($"2. Remove Games you are currently Playing:           {filterPlayingSetting}");
                Console.WriteLine($"3. Remove Games you have played and Dropped:         {filterDroppedSetting}");
                Console.WriteLine($"4. Remove Games you have shown desire to Replay:     {filterReplaysSetting}");
            }
            int userInput = 0;
            Console.WriteLine("\nPlease select the setting you would like to alter, or press 0 to go back");
            if (filterPlayedSetting)
            {
                userInput = Format.GetSingleResponse(1, "What filter would you like to change?");
            }
            else
            {
                userInput = Format.GetSingleResponse(4, "What filter would you like to change?");
            }
            switch (userInput)
            {
                case 1:
                    if (filterPlayedSetting && Format.GetClosedAnswer($"Are you sure you would like to alter your Similar Games Filters to have played games shown? (y/n)"))
                    {
                        filterPlayedSetting = false;
                    }
                    else if (!filterPlayedSetting && Format.GetClosedAnswer($"Are you sure you would like to alter your Similar Games Filters to have no played games shown? (y/n)"))
                    {
                        filterPlayedSetting = true;
                    }
                    break;
                case 2:
                    if (filterPlayingSetting && Format.GetClosedAnswer($"Are you sure you would like to alter your Similar Games Filters to include games you are currently playing? (y/n)"))
                    {
                        filterPlayingSetting = false;
                    }
                    else if (!filterPlayingSetting && Format.GetClosedAnswer($"Are you sure you would like to alter your Similar Games Filters to remove games you are currently playing? (y/n)"))
                    {
                        filterPlayingSetting = true;
                    }
                    break;
                case 3:
                    if (filterDroppedSetting && Format.GetClosedAnswer($"Are you sure you would like to alter your Similar Games Filters to include games you put down? (y/n)"))
                    {
                        filterDroppedSetting = false;
                    }
                    else if (!filterDroppedSetting && Format.GetClosedAnswer($"Are you sure you would like to alter your Similar Games Filters to remove games you put down? (y/n)"))
                    {
                        filterDroppedSetting = true;
                    }
                    break;
                case 4:
                    if (filterReplaysSetting && Format.GetClosedAnswer($"Are you sure you would like to alter your Similar Games Filters to include games you plan to replay? (y/n)"))
                    {
                        filterReplaysSetting = false;
                    }
                    else if (!filterReplaysSetting && Format.GetClosedAnswer($"Are you sure you would like to alter your Similar Games Filters to remove games you plan to replay? (y/n)"))
                    {
                        filterReplaysSetting = true;
                    }
                    break;
                case 0:
                    return;
            }
            SaveSettings();
            AlterPlayedTypeRestrictions();
        }

        public void AlterMiscRestrictions()
        {
            Console.WriteLine("\n\nHere are your current settings preferences\n");
            Console.WriteLine($"1. Remove Games that have uncompleted Prequels:                     {filterWaitingForPrequelSetting}");
            Console.WriteLine($"2. Remove Unpurchased Games:                                        {filterNotPurchasedSetting}");
            Console.WriteLine($"3. Remove Games Unplayable on the Steam Deck:                       {filterDeckPlayableSetting}");
            if (!filterDeckPlayableSetting)
            {
                Console.WriteLine($"4. Match Steam Deck Filter to Searched Game's Deck Playablility:    {deckPlayableMatch}");
            }
            Console.WriteLine("\nPlease select the setting you would like to alter, or press 0 to go back");
            int userInput = 0;
            if (!filterDeckPlayableSetting)
            {
                userInput = Format.GetSingleResponse(4, "What filter would you like to change?");
            }
            else
            {
                userInput = Format.GetSingleResponse(3, "What filter would you like to change?");
            }
            switch (userInput)
            {
                case 1:
                    if (filterWaitingForPrequelSetting && Format.GetClosedAnswer($"Are you sure you would like to alter your Similar Games Filters to include games that have previous entries uncompleted? (y/n)"))
                    {
                        filterWaitingForPrequelSetting = false;
                    }
                    else if (!filterWaitingForPrequelSetting && Format.GetClosedAnswer($"Are you sure you would like to alter your Similar Games Filters to remove games that have previous entries uncompleted? (y/n)"))
                    {
                        filterWaitingForPrequelSetting = true;
                    }
                    break;
                case 2:
                    if (filterNotPurchasedSetting && Format.GetClosedAnswer($"Are you sure you would like to alter your Similar Games Filters to include unpurchased games? (y/n)"))
                    {
                        filterNotPurchasedSetting = false;
                    }
                    else if (!filterNotPurchasedSetting && Format.GetClosedAnswer($"Are you sure you would like to alter your Similar Games Filters to remove unpurchased games? (y/n)"))
                    {
                        filterNotPurchasedSetting = true;
                    }
                    break;
                case 3:
                    if (filterDeckPlayableSetting && Format.GetClosedAnswer($"Are you sure you would like to alter your Similar Games Filters to include games that cannot be played on the Steam Deck? (y/n)"))
                    {
                        filterDeckPlayableSetting = false;
                    }
                    else if (!filterDeckPlayableSetting && Format.GetClosedAnswer($"Are you sure you would like to alter your Similar Games Filters to remove games that cannot be played on the Steam Deck? (y/n)"))
                    {
                        filterDeckPlayableSetting = true;
                    }
                    break;
                case 4:
                    if (deckPlayableMatch && Format.GetClosedAnswer($"Are you sure you would like to alter your Similar Games Filters to show games that cannot be played on the Steam Deck? (y/n)"))
                    {
                        deckPlayableMatch = false;
                    }
                    else if (!deckPlayableMatch && Format.GetClosedAnswer($"Are you sure you would like to alter your Similar Games Filters to remove games that cannot be played on the Steam Deck when the Searched Game is Steam Deck Playable? (y/n)"))
                    {
                        deckPlayableMatch = true;
                    }
                    break;
                case 0:
                    return;
            }
            SaveSettings();
            AlterMiscRestrictions();
        }

        public void AlterDurationRestrictions()
        {
            Console.WriteLine("\n\nHere are your current game duration settings\n");
            Console.WriteLine($"1. Remove Games Too Long/Short in Length:                  {avgLengthMattersSetting}");
            Console.WriteLine($"2. Set Game Length Filter Based on Search Game Length:     {avgGameLengthRelative}");
            if (avgGameLengthRelative)
            {
                Console.WriteLine($"3. Avg Game Length Leeway:                                 +/-{avgLengthDeviationSetting * 100}%");
            } else
            {
                Console.WriteLine($"3. Required Avg Game Length:                               {avgLengthBaseSetting}");
                Console.WriteLine($"4. Avg Game Length Leeway:                                 +/-{avgLengthDeviationSetting * 100}%");
            }
            Console.WriteLine("\nPlease select the setting you would like to alter, or press 0 to go back");
            int userInput = 0;
            if (avgGameLengthRelative)
            {
                userInput = Format.GetSingleResponse(3, "What filter would you like to change?");
            }
            else
            {
                userInput = Format.GetSingleResponse(4, "What filter would you like to change?");
            }
            switch (userInput)
            {
                case 1:
                    if (avgLengthMattersSetting && Format.GetClosedAnswer($"Are you sure you would like to alter your Similar Games Filters to include games regardless of their avg duration? (y/n)"))
                    {
                        avgLengthMattersSetting = false;
                    }
                    else if (!avgLengthMattersSetting && Format.GetClosedAnswer($"Are you sure you would like to alter your Similar Games Filters to remove games that are outside your set game duration range? (y/n)"))
                    {
                        avgLengthMattersSetting = true;
                    }
                    break;
                case 2:
                    if (avgGameLengthRelative && Format.GetClosedAnswer($"Are you sure you would like to alter your Similar Games Filters to remove games that are too different in length compared to your set duration? (y/n)"))
                    {
                        avgGameLengthRelative = false;
                    }
                    else if (!avgGameLengthRelative && Format.GetClosedAnswer($"Are you sure you would like to alter your Similar Games Filters to remove games that are too different in length compared to the searched game? (y/n)"))
                    {
                        avgGameLengthRelative = true;
                    }
                    break;
                case 3:
                    if (avgGameLengthRelative)
                    {
                        Console.WriteLine($"Please input the new Avg Game Length Leeway Percentage for Similar Games Filter");
                        int avgLengthDevInput = Format.ConvertStringToInt(Console.ReadLine(), "Please input the new Avg Game Length Leeway Percentage for Similar Games Filter");
                        if (Format.GetClosedAnswer($"Are you sure you would like to change the Avg Game Length Leewat from +/-{avgLengthDeviationSetting * 100}% to +/-{avgLengthDevInput}%? (y/n)"))
                        {
                            avgLengthDeviationSetting = avgLengthDevInput;
                            avgLengthDeviationSetting /= 100;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Please input the new Avg Game Length for Similar Games Filter");
                        float avgLengthInput = Format.ConvertStringToFloat(Console.ReadLine(), "Please input the new Avg Game Length for Similar Games Filter");
                        if (Format.GetClosedAnswer($"Are you sure you would like to change the Avg Game Length from {avgLengthBaseSetting}hrs to {avgLengthBaseSetting}hrs? (y/n)"))
                        {
                            avgLengthBaseSetting = avgLengthInput;
                        }
                    }
                    break;
                case 4:
                        Console.WriteLine($"Please input the new Avg Game Length Leeway Percentage for Similar Games Filter");
                        int avgLengthDevInput2 = Format.ConvertStringToInt(Console.ReadLine(), "Please input the new Avg Game Length Leeway Percentage for Similar Games Filter");
                        if (Format.GetClosedAnswer($"Are you sure you would like to change the Avg Game Length Leeway from +/-{avgLengthDeviationSetting * 100}% to +/-{avgLengthDevInput2}%? (y/n)"))
                        {
                            avgLengthDeviationSetting = avgLengthDevInput2;
                            avgLengthDeviationSetting /= 100;
                        }
                    break;
                case 0:
                    return;
            }
            SaveSettings();
            AlterDurationRestrictions();
        }

        public void AlterExcitementRestrictions()
        {
            Console.WriteLine("\n\nHere are your current game excitement settings\n");
            Console.WriteLine($"1. Remove Games Below Required Excitement/Rating:    {requiresCertainExcitementSetting}");
            if (requiresCertainExcitementSetting)
            {
                Console.WriteLine($"2. Set Required Game Excitement/Rating:              {requiredExcitementLevelOrRatingSetting}");
            }
            Console.WriteLine("\nPlease select the setting you would like to alter, or press 0 to go back");
            int userInput = 0;
            if (!requiresCertainExcitementSetting)
            {
                userInput = Format.GetSingleResponse(1, "What filter would you like to change?");
            }
            else
            {
                userInput = Format.GetSingleResponse(2, "What filter would you like to change?");
            }
            switch (userInput)
            {
                case 1:
                    if (requiresCertainExcitementSetting && Format.GetClosedAnswer($"Are you sure you would like to alter your Similar Games Filters to include games regardless of their rating/excitement level? (y/n)"))
                    {
                        requiresCertainExcitementSetting = false;
                    }
                    else if (!requiresCertainExcitementSetting && Format.GetClosedAnswer($"Are you sure you would like to alter your Similar Games Filters to remove games that are below your set rating/excitement level? (y/n)"))
                    {
                        requiresCertainExcitementSetting = true;
                    }
                    break;
                case 2:
                    Console.WriteLine($"Please input the new Required Rating/Excitement Level");
                    int ratingInput = Format.ConvertStringToInt(Console.ReadLine(), "Please input the new Avg Game Length Leeway Percentage for Similar Games Filter");
                    if (Format.GetClosedAnswer($"Are you sure you would like to change the Required Rating/Excitement Level from {requiredExcitementLevelOrRatingSetting} to {ratingInput}? (y/n)"))
                    {
                        requiredExcitementLevelOrRatingSetting = ratingInput;
                    }
                    break;
                case 0:
                    return;
            }
            SaveSettings();
            AlterExcitementRestrictions();
        }

        public void AlterSessionGameRestrictions()
        {
            Console.WriteLine("\n\nHere are your current session game settings\n");
            Console.WriteLine($"1. Remove Session Games:                                    {ignoreSessionGamesSetting}");
            if (!ignoreSessionGamesSetting)
            {
                Console.WriteLine($"2. Show Only Session Games if Searched Is Session Game:     {sessionGameDesired}");
                Console.WriteLine($"3. Show No Session Games Unless Searched Is Session Game:   {sessionGameRequired}");
                Console.WriteLine($"4. Ignore Game Length For Session Games:                    {removeSessionLengthReq}");
            }
            int userInput = 0;
            if (ignoreSessionGamesSetting)
            {
                userInput = Format.GetSingleResponse(1, "What filter would you like to change?");
            }
            else
            {
                userInput = Format.GetSingleResponse(4, "What filter would you like to change?");
            }
            switch (userInput)
            {
                case 1:
                    if (ignoreSessionGamesSetting && Format.GetClosedAnswer($"Are you sure you would like to alter your Similar Games Filters to include session games? (y/n)"))
                    {
                        ignoreSessionGamesSetting = false;
                    }
                    else if (!ignoreSessionGamesSetting && Format.GetClosedAnswer($"Are you sure you would like to alter your Similar Games Filters to remove session games? (y/n)"))
                    {
                        ignoreSessionGamesSetting = true;
                    }
                    break;
                case 2:
                    if (sessionGameDesired && Format.GetClosedAnswer($"Are you sure you would like to alter your Similar Games Filters to remove all non session games when searched game is a session game? (y/n)"))
                    {
                        sessionGameDesired = false;
                    }
                    else if (!sessionGameDesired && Format.GetClosedAnswer($"Are you sure you would like to alter your Similar Games Filters to include non session games if searched game is a session game? (y/n)"))
                    {
                        sessionGameDesired = true;
                    }
                    break;
                case 3:
                    if (sessionGameRequired && Format.GetClosedAnswer($"Are you sure you would like to alter your Similar Games Filters to remove all session games unless searched game is a session game? (y/n)"))
                    {
                        sessionGameRequired = false;
                    }
                    else if (!sessionGameRequired && Format.GetClosedAnswer($"Are you sure you would like to alter your Similar Games Filters to include session games regardless of search game? (y/n)"))
                    {
                        sessionGameRequired = true;
                    }
                    break;
                case 4:
                    if (removeSessionLengthReq && Format.GetClosedAnswer($"Are you sure you would like to alter your Similar Games Filters to include Game Length Duration Filter regardless of if the Searched Game is a Session Game? (y/n)"))
                    {
                        removeSessionLengthReq = false;
                    }
                    else if (!removeSessionLengthReq && Format.GetClosedAnswer($"Are you sure you would like to alter your Similar Games Filters to remove Game Length Filter from Search Session Games? (y/n)"))
                    {
                        removeSessionLengthReq = true;
                    }
                    break;
                case 0:
                    return;
            }
            SaveSettings();
            AlterSessionGameRestrictions();
        }

        public void AlterMaxSimilarGameCount()
        {
            Console.WriteLine($"As per your settings, searching for similar games will only output up to {maxSimilarGameOutput} results\nHow many results would you like to show?");
            int newLimit = Format.ConvertStringToInt(Console.ReadLine(), $"What is the new limit for similar game recomendations you would like to receive?");
            while (newLimit < 1)
            {
                Console.WriteLine($"{newLimit} less than 1. Please select a positive full number");
                newLimit = Format.ConvertStringToInt(Console.ReadLine(), $"What is the new limit for similar game recomendations you would like to receive?");
            }
            if (Format.GetClosedAnswer($"Are you sure you want to update the limit of recommended similar games from {maxSimilarGameOutput} to {newLimit}? (y/n)"))
            {
                maxSimilarGameOutput = newLimit;
            }
            SaveSettings();
        }

        public void ManageBackupsSettings()
        {
            Console.WriteLine("\n\nWelcome to your File Manager!\nPlease select how you would like to interact with your save files below:\n");
            Console.WriteLine("1. Create a Backup");
            Console.WriteLine("2. Switch Save Files");
            Console.WriteLine("3. Create a Fresh Save");
            Console.WriteLine("4. Change File Name");
            Console.WriteLine("0. Exit");
            int userInput = Format.GetSingleResponse(4, "What action would you like to take?");
            switch (userInput)
            {
                case 1:
                    CreateBackup();
                    break;
                case 2:
                    SwitchSaveFiles();
                    break;
                case 3:
                    CreateFreshSave();
                    break;
                case 4:
                    ManageSaveNames();
                    break;
                case 0:
                    return;
            }
            ManageBackupsSettings();
        }

        public void CreateBackup()
        {
            SettingsInfo newSettingsInfo = CSVHandler.CreateBackup();
            string settingsName = Format.AskForInput("name for your backup save");
            newSettingsInfo.fileName = settingsName;
            newSettingsInfo.dateLastUsed = DateOnly.FromDateTime(DateTime.Now);
            Console.WriteLine($"A backup of your files has been created and is saved under the name {settingsName}");
            SaveSettings();
        }

        public void SwitchSaveFiles()
        {
            Console.WriteLine("\n\nAll your existing saves have been listed below\nPlease select which save you would like to use:\n");
            List<SettingsInfo> sortedSettings = Menu.backupFiles.OrderByDescending(settings => settings.dateLastUsed).ToList();
            for (int i = 0; i < sortedSettings.Count; i++)
            {
                if (sortedSettings[i].settingsID == sortedSettings[i].activeSettingsID)
                {
                    Console.WriteLine($"{i+1}. {sortedSettings[i].fileName} --> --> CURRENT USED SAVE FILE <-- <-- {sortedSettings[i].dateLastUsed.ToString("dd/MM/yyyy")}");
                }
                else
                {
                    Console.WriteLine($"{i+1}. {sortedSettings[i].fileName} <-- <-- LAST UPDATED --> --> {sortedSettings[i].dateLastUsed.ToString("dd/MM/yyyy")}");
                }
            }
            Console.WriteLine("0. Exit");
            int userInput = Format.GetSingleResponse(sortedSettings.Count, "What action would you like to take?");
            switch (userInput)
            {
                case 0:
                    return;
                default:
                    if (Format.GetClosedAnswer($"Are you sure you would like to set {sortedSettings[userInput-1].fileName}, last update on {sortedSettings[userInput-1].dateLastUsed.ToString("dd/MM/yyyy")}, as your current save? (y/n)"))
                    {
                        Menu.SetCurrentSettings(sortedSettings[userInput-1].settingsID);
                    }
                    break;
            }
            SaveSettings();
            SwitchSaveFiles();
        }

        public void CreateFreshSave()
        {
            if (!Format.GetClosedAnswer("Are you sure you would like to create a fresh save with NO saved data, INCLUDING games, genres, and franchises? (y/n)\nPlease note that previous saves will still be usable if switched to via the File Manager"))
            {
                return;
            }
            SettingsInfo newSettingsInfo = Menu.FindSettingsFile(CSVHandler.CreateRefresh());
            string settingsName = Format.AskForInput("name for your fresh save");
            newSettingsInfo.fileName = settingsName;
            newSettingsInfo.dateLastUsed = DateOnly.FromDateTime(DateTime.Now);
            Console.WriteLine($"A fresh save has been created and is saved under the name {settingsName}\nIf you would like to revert to a previous save with the entries removed from this save, please do so from the File Manager");
            SaveSettings();
        }

        public void ManageSaveNames()
        {
            Console.WriteLine("\n\nAll your existing saves have been listed below\nPlease select which save you would like to alter:\n");
            List<SettingsInfo> sortedSettings = Menu.backupFiles.OrderByDescending(settings => settings.dateLastUsed).ToList();
            for (int i = 0; i < sortedSettings.Count; i++)
            {
                Console.WriteLine($"{i+1}. {sortedSettings[i].fileName} --> --> LAST UPDATED --> {sortedSettings[i].dateLastUsed.ToString("dd/MM/yyyy")}");
            }
            Console.WriteLine("0. Exit");
            int userInput = Format.GetSingleResponse(sortedSettings.Count, "What action would you like to take?");
            switch (userInput)
            {
                case 0:
                    return;
                default:
                    string newName = Format.AskForInput("new name for your save file");
                    if (Format.GetClosedAnswer($"Are you sure you would like to set the name of {sortedSettings[userInput-1].fileName}, last update on {sortedSettings[userInput-1].dateLastUsed.ToString("dd/MM/yyyy")}, to {newName}? (y/n)"))
                    {
                       sortedSettings[userInput-1].fileName = newName;
                    }
                    break;
            }
            SaveSettings();
            SwitchSaveFiles();
        }

        public void ManageLandingPageSettings()
        {
            Console.WriteLine("\n\nWhat would you like to edit about your Landing Page?:\n");
            Console.WriteLine($"1. Launch App to Landing Page:       {launchLandingPage}");
            Console.WriteLine($"2. Show Current Games:               {landingShowCurrent}");
            Console.WriteLine($"3. Show Recently Released Games:     {landingShowNewRelease}");
            Console.WriteLine($"4. Show Recommended Games:           {landingShowRecentRecommended}");
            Console.WriteLine($"5. Max Recommended Titles:           {maxLoadingRecommend}");
            Console.WriteLine("0. Exit");
            int userInput = landingShowRecentRecommended ? Format.GetSingleResponse(5, "What setting would you like to edit?") : Format.GetSingleResponse(4, "What setting would you like to edit?");
            switch (userInput)
            {
                case 1:
                    launchLandingPage = launchLandingPage ? false : true;
                    break;
                case 2:
                    landingShowCurrent = landingShowCurrent ? false : true;
                    break;
                case 3:
                    landingShowNewRelease = landingShowNewRelease ? false : true;
                    break;
                case 4:
                    landingShowRecentRecommended = landingShowRecentRecommended ? false : true;
                    break;
                case 5:
                    Console.WriteLine("How many games would you like to have recommended to you on your Landing Page??");
                    int newMaxLoadingRec = Format.ConvertStringToInt(Console.ReadLine(), "How many days before dropping a game due to inactivity do you want to notify the user?");
                    while (newMaxLoadingRec < 1)
                    {
                        Console.WriteLine($"Your input is too low, please input a positive integer");
                        newMaxLoadingRec = Format.ConvertStringToInt(Console.ReadLine(), "How many days of inactivity do you want to require before auto dropping currently playing games?");
                    }
                    if (Format.GetClosedAnswer($"Are you sure you would like to edit the maximum number of recommended games on your loading screen from {maxLoadingRecommend} to {newMaxLoadingRec}? (y/n)"))
                    {
                        autoDropNotificationDays = newMaxLoadingRec;
                    }
                    break;
                case 0:
                    return;
            }
            ManageLandingPageSettings();
        }

        public void ManageTypeChangeSettings()
        {
            Console.WriteLine("\n\nWhat would you like to edit about your Landing Page?:\n");
            Console.WriteLine($"1. Automatically mark Current Games as Dropped after extended inactivity:       {autoDropCurrentAfterDuration}");
            if (autoDropCurrentAfterDuration)
            {
                Console.WriteLine($"2. Days of Inactivity Required before marking Current Game as Dropped:          {daysRequiredToDropCurrent}");
                Console.WriteLine($"3. Ignore Auto Drop functionality for Session Games:                            {!autoDropSession}");
                Console.WriteLine($"4. Notify user 'x' days before dropping:                                        x = {autoDropNotificationDays}");
            }
            Console.WriteLine("0. Exit");
            int userInput = autoDropCurrentAfterDuration ? Format.GetSingleResponse(4, "What setting would you like to edit?") : Format.GetSingleResponse(1, "What setting would you like to edit?");
            switch (userInput)
            {
                case 1:
                    autoDropCurrentAfterDuration = autoDropCurrentAfterDuration ? false : true;
                    break;
                case 2:
                    Console.WriteLine("How many days of inactivity do you want to require before auto dropping currently playing games?");
                    int newDaysToDrop = Format.ConvertStringToInt(Console.ReadLine(), "How many days of inactivity do you want to require before auto dropping currently playing games?");
                    while (newDaysToDrop < 1)
                    {
                        Console.WriteLine($"Your input is too low, please input a positive integer");
                        newDaysToDrop = Format.ConvertStringToInt(Console.ReadLine(), "How many days of inactivity do you want to require before auto dropping currently playing games?");
                    }
                    while (newDaysToDrop < 15 && !Format.GetClosedAnswer($"{newDaysToDrop} is an extremely short time of inactivity.\nAre you sure you would like to drop a current game after not playing for {newDaysToDrop} days? (y/n)"))
                    {
                        Console.WriteLine("How many days of inactivity do you want to require before auto dropping currently playing games?");
                        newDaysToDrop = Format.ConvertStringToInt(Console.ReadLine(), "How many days of inactivity do you want to require before auto dropping currently playing games?");
                    }
                    daysRequiredToDropCurrent = newDaysToDrop;
                    if (autoDropNotificationDays > daysRequiredToDropCurrent)
                    {
                        Console.WriteLine("Please change your Notification before Dropping settings to fit your new inactivity drop window");
                        goto case 4;
                    }
                    break;
                case 3:
                    autoDropSession = autoDropSession ? false : true;
                    break;
                case 4:
                    Console.WriteLine("How many days before dropping a game due to inactivity do you want to notify the user?");
                    int newNotifDays = Format.ConvertStringToInt(Console.ReadLine(), "How many days before dropping a game due to inactivity do you want to notify the user?");
                    while (newNotifDays < 0)
                    {
                        Console.WriteLine($"Your input is too low, please input a positive integer");
                        newNotifDays = Format.ConvertStringToInt(Console.ReadLine(), "How many days of inactivity do you want to require before auto dropping currently playing games?");
                    }
                    while (newNotifDays > daysRequiredToDropCurrent)
                    {
                        Console.WriteLine($"Your notification of dropping window must be smaller or equal to you inactivity before dropping duration.\nPlease input a number between 0 & {daysRequiredToDropCurrent}");
                        newNotifDays = Format.ConvertStringToInt(Console.ReadLine(), "How many days before dropping a game due to inactivity do you want to notify the user?");
                    }
                    autoDropNotificationDays = newNotifDays;
                    break;
                case 0:
                    return;
            }
            ManageTypeChangeSettings();
        }

        public bool CalculateAvgLengthFilter(bool searchedIsSession)
        {
            if (removeSessionLengthReq && searchedIsSession)
            {
                return false;
            }
            return avgLengthMattersSetting;
        }

        public float CalculateAvgLengthBase(float gameLength)
        {
            if (avgGameLengthRelative)
            {
                return gameLength;
            } 
            else
            {
                return avgLengthBaseSetting;
            }
        }

        public float CalculateAvgLengthDeviation(float gameLength)
        {
            if (avgGameLengthRelative)
            {
                return gameLength * avgLengthDeviationSetting;
            } 
            else
            {
                return avgLengthBaseSetting * avgLengthDeviationSetting;
            }
            
        }

        public (bool onlySession, bool ignoreSession) CalculateSessionGameStatus(bool searchedIsSession)
        {
            if (!ignoreSessionGamesSetting)
            {
                if (sessionGameRequired && !searchedIsSession)
                {
                    return (false, true);
                }
                else if (sessionGameDesired && searchedIsSession)
                {
                    return (true, false);
                }
            }
            else
            {
                return (false, true);
            }
            return (false, false);
        }

        public bool CalculateDeckPlayableFilter(bool searchGamePlayable)
        {
            if (deckPlayableMatch && !filterDeckPlayableSetting)
            {
                if (searchGamePlayable)
                {
                    return true;
                }
            }
            if (filterDeckPlayableSetting)
            {
                return true;
            }
            return false;
        }

        public void UpdateLastUsed()
        {
            dateLastUsed = DateOnly.FromDateTime(DateTime.Now);
            Console.WriteLine("Updating last used");
        }

        public void SaveSettings()
        {
            CSVHandler.UpdateInfoFile<SettingsInfo>(Menu.backupFiles, Menu.settingsPath);
        }
    }
}