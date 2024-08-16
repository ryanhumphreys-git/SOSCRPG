using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Engine.EventArgs;
using Engine.Factories;
using Engine.ViewModels;

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for npcInteractionUI.xaml
    /// </summary>
    public partial class npcInteractionUI : Window
    {
        public GameSession Session => DataContext as GameSession;
        public npcInteractionUI()
        {
            InitializeComponent();
                       
        }
        private void OnGameMessageRaised(object sender, GameMessageEventArgs e)
        {
            InteractionMessages.Document.Blocks.Add(new Paragraph(new Run(e.Message)));
            InteractionMessages.ScrollToEnd();
        }

        private void OnClick_AcceptQuest(object sender, RoutedEventArgs e)
        {
            if(Session.SelectedQuest == null) return;
            Session.AcceptQuest(Session.SelectedQuest);
            Session.SelectedQuest = null;
        }

        private void OnClick_Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnClick_ChooseQuestOne(object sender, RoutedEventArgs e)
        {
            Session.OnMessageRaised += OnGameMessageRaised;
            Session.RaiseMessage("");
            Session.RaiseMessage($"Would you like to accept {Session.CurrentLocation.QuestGiverHere.QuestAvailableHere[0].Name}?");
            Session.RaiseMessage($"You must {Session.CurrentLocation.QuestGiverHere.QuestAvailableHere[0].Description}");
            Session.RaiseMessage($"And will be rewarded with {Session.CurrentLocation.QuestGiverHere.QuestAvailableHere[0].RewardExperiencePoints} experience, ");
            Session.RaiseMessage($"{Session.CurrentLocation.QuestGiverHere.QuestAvailableHere[0].RewardGold} gold.");
            if(Session.CurrentLocation.QuestGiverHere.QuestAvailableHere[0].RewardItems != null)
            {
                Session.RaiseMessage($"You will also receive " +
                    $"{ItemFactory.GetGameItemName(Session.CurrentLocation.QuestGiverHere.QuestAvailableHere[0].RewardItems[0].ItemID)}.");
            }

            Session.SelectedQuest = Session.CurrentLocation.QuestGiverHere.QuestAvailableHere[0];
            Button button = sender as Button;

            button.Visibility=Visibility.Hidden;

        }

        private void OnClick_ChooseQuestTwo(object sender, RoutedEventArgs e)
        {
            Session.OnMessageRaised += OnGameMessageRaised;
            Session.RaiseMessage("");
            Session.RaiseMessage($"Would you like to accept {Session.CurrentLocation.QuestGiverHere.QuestAvailableHere[1].Name}?");
            Session.RaiseMessage($"You must {Session.CurrentLocation.QuestGiverHere.QuestAvailableHere[1].Description}");
            Session.RaiseMessage($"And will be rewarded with {Session.CurrentLocation.QuestGiverHere.QuestAvailableHere[1].RewardExperiencePoints} experience, ");
            Session.RaiseMessage($"And will be rewarded with {Session.CurrentLocation.QuestGiverHere.QuestAvailableHere[1].RewardGold} gold, and");
            if (Session.CurrentLocation.QuestGiverHere.QuestAvailableHere[1].RewardItems != null)
            {
                Session.RaiseMessage($"You will also receive " +
                    $"{ItemFactory.GetGameItemName(Session.CurrentLocation.QuestGiverHere.QuestAvailableHere[1].RewardItems[0].ItemID)}.");
            }

            Session.SelectedQuest = Session.CurrentLocation.QuestGiverHere.QuestAvailableHere[1];
            Button button = sender as Button;

            button.Visibility = Visibility.Hidden;
        }
    }
}
