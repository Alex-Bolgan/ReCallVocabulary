﻿using Microsoft.EntityFrameworkCore;
using ReCallVocabulary.Data_Access;

namespace ReCallVocabulary
{
    public partial class App : Application
    {
        public static string FileWithCurrentDBName { get; private set; } = null!;

        private static DictionaryContext activeContext = null!;
        public static StatsContext statsContext { get; set; } = null!;

        public static DictionaryContext ActiveContext
        {
            get => activeContext;
            set
            {
                File.WriteAllText(FileWithCurrentDBName, Path.GetFileName(value.MyPath));
                activeContext = value;
            }

        }

        public App()
        {
            InitializeComponent();
#if WINDOWS
            FileWithCurrentDBName = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "currentDBName.txt");
            if (!File.Exists(FileWithCurrentDBName))
            {
                var myFile = File.Create(FileWithCurrentDBName);
                myFile.Close();
                File.WriteAllText(FileWithCurrentDBName,"autogenerated.db");
            }
#elif ANDROID

            FileWithCurrentDBName = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            if (!Directory.Exists(FileWithCurrentDBName))
            {
                Directory.CreateDirectory(FileWithCurrentDBName);               
            }
            FileWithCurrentDBName = Path.Combine(
                    FileWithCurrentDBName, "currentDBName.txt");

            if(!File.Exists((FileWithCurrentDBName)))
            {
var myFile = File.Create(FileWithCurrentDBName);
                myFile.Close();
                File.WriteAllText(FileWithCurrentDBName,"autogenerated.db");
            }
#endif
            ActiveContext = new DictionaryContext(File.ReadAllText(FileWithCurrentDBName));
            statsContext = new StatsContext("Stats.db");
            MainPage = new AppShell();
        }
    }
}
