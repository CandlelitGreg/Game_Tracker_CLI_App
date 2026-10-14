using System.Data;
using CsvHelper.Configuration;

namespace GameTrackerEx01
{
    public sealed class LogMessageMap : ClassMap<LogMessage>
    {
        public LogMessageMap()
        {
            Map(m => m.logMessageID);
            Map(m => m.logGameID);
            Map(m => m.logDate);
            Map(m => m.logData);
            Map(m => m.playDuration);
            Map(m => m.startedGame);
            Map(m => m.finishedGame);
            Map(m => m.droppedGame);
        }
    }
    public sealed class RetrieveLogMessageMap : ClassMap<LogMessage>
    {
        public RetrieveLogMessageMap()
        {
            Map(m => m.logMessageID);
            Map(m => m.logGameID);
            Map(m => m.logDate);
            Map(m => m.logData);
            Map(m => m.playDuration);
            Map(m => m.startedGame);
            Map(m => m.finishedGame);
            Map(m => m.droppedGame);
        }
    }
    public class LogMessage
    {
        public int logMessageID {get; set;} = -1;
        public int logGameID {get; set;} = -1;
        public DateOnly logDate {get; set;}
        public string logData {get; set;} = "";
        public float playDuration {get; set;} = 0;
        public bool startedGame {get; set;} = false;
        public bool finishedGame {get; set;} = false;
        public bool droppedGame {get; set;} = false;

        public void CreateLog(int gameID)
        {
            logMessageID = Menu.GetAndUpdateNextLogID();
            logGameID = gameID;
            logDate = GetLogDate();
            logData = GetLogData();
            playDuration = GetPlayDuration();
            Menu.existingLogMessages.Add(this);
            Menu.FindGameByID(gameID).AddLogToList(logMessageID);
            SaveLogChanges();
            CheckLogInputs();
        }
        public DateOnly GetLogDate()
        {
            if (logDate != DateOnly.FromDateTime(new DateTime()) && !Format.GetClosedAnswer($"Log date is already set to {logDate.ToString("dd/MM/yyyy")}\nAre you sure you would like to change the log date? (y/n)"))
            {
                return logDate;
            }
            return DateOnly.FromDateTime(Format.GetDateValue($"date this log entry is for in format dd/MM/yyyy"));
        }
        public string GetLogData()
        {
            if (logData != "" && !Format.GetClosedAnswer($"Log entry is already set to the following:\n ---   ---   ---\n{logData}\n ---   ---   ---\nAre you sure you would like to change the log entry? (y/n)"))
            {
                return logData;
            }
            Console.WriteLine($"Please enter log entry and comments");
            return Format.ReplaceCommasInString(Console.ReadLine());
        }
        public float GetPlayDuration()
        {
            if (playDuration != 0 && !Format.GetClosedAnswer($"Log duration is already set to {playDuration}\nAre you sure you would like to change the log duration? (y/n)"))
            {
                return playDuration;
            }
            Console.WriteLine($"Please input the approximate number of hours you spent during this play session of {Menu.FindGameByID(logGameID).gameName}");
            float playTime =  Format.ConvertStringToFloat(Console.ReadLine(), $"Please input the approximate number of hours you spent during this play session of {Menu.FindGameByID(logGameID).gameName}");
            if (Format.GetClosedAnswer($"Would you like to automatically update your time spent with {Menu.FindGameByID(logGameID).gameName} by {playTime-playDuration}hrs? (y/n)"))
            {
                Menu.FindGameByID(logGameID).AddPlayTime(playTime-playDuration);
            }
            return playTime;
        }
        public void CheckLogInputs()
        {
            Console.WriteLine($"Please find the details of your log entry below:\n\n");
            DisplayLogDetails();
            if (Format.GetClosedAnswer($"\n\nWould you like to edit any of the details in your log entry? (y/n)"))
            {
                EditLogInputs();
            }
        }
        public void DisplayLogDetails()
        {
            Console.WriteLine($"Displaying Data from Log Entry {Menu.FindGameByID(logGameID).RetrieveLogLocation(logMessageID)} for {Menu.FindGameByID(logGameID).gameName}\n");
            Console.WriteLine($"Log entry Date:           {logDate}");
            Console.WriteLine($"Log Play Duration:        {playDuration}hrs");
            Console.WriteLine($"Log Entry Text:\n ---   ---   ---\n{logData}\n ---   ---   ---");
        }
        public string ReadLog()
        {
            return Format.ReturnCommasToString(logData);
        }
        public string ReadLogPreview()
        {
            string returnLog = Format.ReturnCommasToString(logData);
            if (returnLog.Length > 45)
            {
                returnLog = $"{returnLog.Substring(0,45)}...";
            }
            return returnLog;
        }
        public void EditLogInputs()
        {
            Console.WriteLine("\n\nWhich information would you like to change?\n");
            Console.WriteLine($"1. Log Date:               {logDate.ToString("dd/MM/yyyy")}");
            Console.WriteLine($"2. Log Entry Text:         {ReadLogPreview()}");
            Console.WriteLine($"3. Log Play Duration:      {playDuration}hrs");
            Console.WriteLine("0. Exit");
            int userInput = Format.GetSingleResponse(3, "What action would you like to take?");
            switch (userInput)
            {
                case 1:
                    logDate = GetLogDate();
                    break;
                case 2:
                    logData = GetLogData();
                    break;
                case 3:
                    playDuration = GetPlayDuration();
                    break;
                case 0:
                    return;
            }
            SaveLogChanges();
            EditLogInputs();
        }
        public void SaveLogChanges()
        {
            CSVHandler.UpdateInfoFile<LogMessage>(Menu.existingLogMessages, Menu.mainFiles.logEntryFile);
        }
    }
}