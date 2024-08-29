using System.Windows;
using SOSCSRPG.Models;
using SOSCSRPG.ViewModels;
using Microsoft.Win32;
using SOSCSRPG.Services;
namespace WPFUI
{
    public partial class Startup : Window
    {
        private const string SAVE_GAME_FILE_EXTENSION = "soscrpg";
        public Startup()
        {
            InitializeComponent();

            DataContext = GameDetailsService.ReadGameDetails();
        }
        private void StartNewGame_OnClick(object sender, RoutedEventArgs e)
        {
            CharacterCreation characterCreationWindow = new CharacterCreation();
            characterCreationWindow.Show();
            Close();
        }
        private void LoadSavedGame_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog =
                new OpenFileDialog
                {
                    InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                    Filter = $"Saved games(*.{SAVE_GAME_FILE_EXTENSION})|*.{SAVE_GAME_FILE_EXTENSION}"
                };
            if(openFileDialog.ShowDialog() == true)
            {
                GameState gameState = SaveGameService.LoadLastSaveOrCreateNew(openFileDialog.FileName);

                MainWindow mainWindow =
                    new MainWindow(gameState.Player,
                                   gameState.XCoordinate,
                                   gameState.YCoordinate);

                mainWindow.Show();
                Close();
            }
        }
        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
