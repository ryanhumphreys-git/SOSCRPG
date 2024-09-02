using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using SOSCSRPG.Core;
using SOSCSRPG.Models;
using SOSCSRPG.ViewModels;

namespace WPFUI
{
    public partial class npcInteractionUI : Window
    {
        public GameSession Session => DataContext as GameSession;
        private readonly MessageBroker _messageBroker = MessageBroker.GetInstance();
        public npcInteractionUI()
        {
            InitializeComponent();

            btnAccept.Visibility = Visibility.Hidden;
            btnComplete.Visibility = Visibility.Hidden;
            lbQuestList.Visibility = Visibility.Visible;
                       
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _messageBroker.OnMessageRaised += OnGameMessageRaised;

        }
        private void OnGameMessageRaised(object sender, GameMessageEventArgs e)
        {
            InteractionMessages.Document.Blocks.Add(new Paragraph(new Run(e.Message)));
            InteractionMessages.ScrollToEnd();
        }
        private void OnClick_AcceptQuest(object sender, RoutedEventArgs e)
        {
            if(lbQuestList.SelectedItem == null) return;
            Session.AcceptQuest(lbQuestList.SelectedItem as Quest);
            btnAccept.Visibility = Visibility.Hidden;
        }
        private void OnClick_CompleteQuest(object sender, RoutedEventArgs e)
        {
            Quest selectedQuest = lbQuestList.SelectedItem as Quest;
            if(!Session.CurrentPlayer.Inventory.HasAllTheseItems(selectedQuest.ItemsToComplete)) return;
            Session.CompleteQuest(lbQuestList.SelectedItem as Quest);
            btnComplete.Visibility= Visibility.Hidden;
        }
        private void OnClick_Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void lbQuestList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lbQuestList.SelectedItem != null)
            {
                Quest selectedQuest = lbQuestList.SelectedItem as Quest;
                Session.SelectQuest(selectedQuest);
                // If has the quest show complete button
                if(Session.CurrentPlayer.Quests.FirstOrDefault(pq => pq.PlayerQuest.ID == selectedQuest.ID) != null)
                {
                    btnComplete.Visibility = Visibility.Visible;
                }
                // If doesnt have quest show accept button
                if(Session.CurrentPlayer.Quests.FirstOrDefault(pq => pq.PlayerQuest.ID == selectedQuest.ID) == null)
                {
                    btnAccept.Visibility = Visibility.Visible;
                }
                // Hide the list box
                lbQuestList.Visibility = Visibility.Hidden;
            }
        }
    }
}
