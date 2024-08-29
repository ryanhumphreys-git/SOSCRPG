﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SOSCSRPG.Core;
using SOSCSRPG.Models;
using SOSCSRPG.ViewModels;
using System;
using System.Collections.Generic;
using SOSCSRPG.Services;
using System.ComponentModel;
using Microsoft.Win32;
using WPFUI.Windows;
using SOSCSRPG.Services.Factories;

namespace WPFUI
{
    public partial class MainWindow : Window
    {
        private const string SAVE_GAME_FILE_EXTENSION = "soscrpg";

        private readonly MessageBroker _messageBroker = MessageBroker.GetInstance();
        private readonly Dictionary<Key, Action> _userInputActions = new Dictionary<Key, Action>();

        private GameSession _gameSession;

        public MainWindow(Player player, int xLocation = 0, int yLocation = -1)
        {
            InitializeComponent();

            InitializeUserInputActions();

            SetActiveGameSessionTo(new GameSession(player, xLocation, yLocation));
        }
        private void OnClick_MoveNorth(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveNorth();
        }
        private void OnClick_MoveEast(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveEast();
        }
        private void OnClick_MoveWest(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveWest();
        }
        private void OnClick_MoveSouth(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveSouth();
        }
        private void OnClick_AttackMonster(object sender, RoutedEventArgs e)
        {
            _gameSession.AttackCurrentMonster();
        }
        private void OnClick_UseCurrentConsumable(object sender, RoutedEventArgs e)
        {
            _gameSession.UseCurrentConsumable();
        }
        private void OnClick_Craft(object sender, RoutedEventArgs e)
        {
            Recipe recipe = ((FrameworkElement)sender).DataContext as Recipe;
            _gameSession.CraftItemUsing(recipe);
        }
        private void OnClick_Interact(object sender, RoutedEventArgs e)
        {
            npcInteractionUI interactionWindow = new npcInteractionUI();
            interactionWindow.Owner = this;
            interactionWindow.DataContext = _gameSession;
            _messageBroker.OnMessageRaised -= OnGameMessageRaised;
            interactionWindow.Closed += InteractionWindow_Closed;
            interactionWindow.ShowDialog();  
        }
        private void OnClick_DisplayVendorScreen(object sender, RoutedEventArgs e)
        {
            if (_gameSession.CurrentVendor != null)
            {
                TradeScreen tradeScreen = new TradeScreen();
                tradeScreen.Owner = this;
                tradeScreen.DataContext = _gameSession;
                tradeScreen.ShowDialog();
            }
        }
        private void InitializeUserInputActions()
        {
            _userInputActions.Add(Key.W, () => _gameSession.MoveNorth());
            _userInputActions.Add(Key.A, () => _gameSession.MoveWest());
            _userInputActions.Add(Key.S, () => _gameSession.MoveSouth());
            _userInputActions.Add(Key.D, () => _gameSession.MoveEast());
            _userInputActions.Add(Key.Z, () => _gameSession.AttackCurrentMonster());
            _userInputActions.Add(Key.C, () => _gameSession.UseCurrentConsumable());
            _userInputActions.Add(Key.I, () => SetTabFocusTo("InventoryTabItem"));
            _userInputActions.Add(Key.Q, () => SetTabFocusTo("QuestTabItem"));
            _userInputActions.Add(Key.R, () => SetTabFocusTo("RecipesTabItem"));
            _userInputActions.Add(Key.T, () => OnClick_DisplayVendorScreen(this, new RoutedEventArgs()));
        }
        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if(_userInputActions.ContainsKey(e.Key))
            {
                _userInputActions[e.Key].Invoke();
            }
        }
        private void SetTabFocusTo(string tabName)
        {
            foreach(object item in PlayerDataTabControl.Items)
            {
                if(item is TabItem tabItem)
                {
                    if(tabItem.Name == tabName)
                    {
                        tabItem.IsSelected = true;
                        return;
                    }
                }
            }
        }
        private void InteractionWindow_Closed(object? sender, EventArgs e)
        {
            _messageBroker.OnMessageRaised += OnGameMessageRaised;
        }
        private void SetActiveGameSessionTo(GameSession gameSession)
        {
            _messageBroker.OnMessageRaised -= OnGameMessageRaised;

            _gameSession = gameSession;
            DataContext = _gameSession;

            GameMessages.Document.Blocks.Clear();

            _messageBroker.OnMessageRaised += OnGameMessageRaised;
        }
        private void StartNewGame_OnClick(object sender, RoutedEventArgs e)
        {
            Startup startup = new Startup();
            startup.Show();
            Close();
        }
        private void LoadGame_OnClick(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog openFileDialog =
            //    new OpenFileDialog
            //    {
            //        InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
            //        Filter = $"Saved games (*.{SAVE_GAME_FILE_EXTENSION})|*.{SAVE_GAME_FILE_EXTENSION}"
            //    };

            //if(openFileDialog.ShowDialog() == true)
            //{
            //    SetActiveGameSessionTo(SaveGameService.LoadLastSaveOrCreateNew(openFileDialog.FileName));
            //}
        }
        private void SaveGame_OnClick(object sender, RoutedEventArgs e)
        {
            SaveGame();
        }
        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void OnGameMessageRaised(object sender, GameMessageEventArgs e)
        {
            GameMessages.Document.Blocks.Add(new Paragraph(new Run(e.Message)));
            GameMessages.ScrollToEnd();
        }
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            AskToSaveGame();
        }
        private void AskToSaveGame()
        {
            YesNoWindow message =
                new YesNoWindow("Save Game", "Do you want to save your game?");
            message.Owner = GetWindow(this);
            message.ShowDialog();

            if (message.ClickedYes)
            {
                SaveGame();
            }
        }
        private void SaveGame()
        {
            SaveFileDialog saveFileDialog =
                new SaveFileDialog
                {
                    InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                    Filter = $"Saved games (*.{SAVE_GAME_FILE_EXTENSION})|*.{SAVE_GAME_FILE_EXTENSION}"
                };

            if (saveFileDialog.ShowDialog() == true)
            {
                SaveGameService.Save(new GameState(_gameSession.CurrentPlayer, _gameSession.CurrentLocation.XCoordinate,
                                                   _gameSession.CurrentLocation.YCoordinate), saveFileDialog.FileName);
            }
        }
    }
}