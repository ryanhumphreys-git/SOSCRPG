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

            btnAccept.Visibility = Visibility.Hidden;
            btnComplete.Visibility = Visibility.Hidden;
                       
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
            btnAccept.Visibility = Visibility.Hidden;
        }

        private void OnClick_CompleteQuest(object sender, RoutedEventArgs e)
        {
            if (!Session.CurrentPlayer.HasAllTheseItems(Session.SelectedQuest.ItemsToComplete)) return;
            Session.CompleteQuest(Session.SelectedQuest);
            Session.SelectedQuest = null;
            btnComplete.Visibility= Visibility.Hidden;
        }

        private void OnClick_Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnClick_ChooseQuestOne(object sender, RoutedEventArgs e)
        {
            Session.OnMessageRaised += OnGameMessageRaised;

            Session.SelectedQuest = Session.CurrentLocation.QuestGiverHere.QuestAvailableHere[0];

            Button button = sender as Button;

            if (Session.CurrentPlayer.Quests.FirstOrDefault(pq => pq.PlayerQuest.ID == Session.SelectedQuest.ID) == null)
            {
                btnAccept.Visibility = Visibility.Visible;
                Session.RaiseMessage("");
                Session.RaiseMessage($"Would you like to accept {Session.SelectedQuest.Name}?");
                Session.RaiseMessage($"You must {Session.SelectedQuest.Description}");
                Session.RaiseMessage($"And will be rewarded with {Session.SelectedQuest.RewardExperiencePoints} experience, ");
                Session.RaiseMessage($"{Session.SelectedQuest.RewardGold} gold.");
                if (Session.SelectedQuest.RewardItems != null)
                {
                    Session.RaiseMessage($"You will also receive " +
                        $"{ItemFactory.GetGameItemName(Session.CurrentLocation.QuestGiverHere.QuestAvailableHere[0].RewardItems[0].ItemID)}.");
                }
            }
            else
            {
                
                Session.RaiseMessage("");
                Session.RaiseMessage($"Would you like to complete {Session.SelectedQuest.Name}?");
                Session.RaiseMessage($"You must {Session.SelectedQuest.Description}");

                if (Session.CurrentPlayer.HasAllTheseItems(Session.SelectedQuest.ItemsToComplete))
                {
                    btnComplete.Visibility = Visibility.Visible;
                }
            }

            if (Session.CurrentPlayer.Quests.FirstOrDefault(pq => pq.PlayerQuest.ID == Session.SelectedQuest.ID) != null)
            {
                if (Session.CurrentPlayer.Quests.FirstOrDefault(pq => pq.PlayerQuest.ID == Session.SelectedQuest.ID).IsCompleted)
                {
                    Session.RaiseMessage("You have already completed this quest");
                }
            }

            button.Visibility = Visibility.Hidden;
        }

        private void OnClick_ChooseQuestTwo(object sender, RoutedEventArgs e)
        {
            Session.OnMessageRaised += OnGameMessageRaised;

            Session.SelectedQuest = Session.CurrentLocation.QuestGiverHere.QuestAvailableHere[1];

            Button button = sender as Button;

            if (Session.CurrentPlayer.Quests.FirstOrDefault(pq => pq.PlayerQuest.ID == Session.SelectedQuest.ID) == null)
            {
                btnAccept.Visibility = Visibility.Visible;
                Session.RaiseMessage("");
                Session.RaiseMessage($"Would you like to accept {Session.SelectedQuest.Name}?");
                Session.RaiseMessage($"You must {Session.SelectedQuest.Description}");
                Session.RaiseMessage($"And will be rewarded with {Session.SelectedQuest.RewardExperiencePoints} experience, ");
                Session.RaiseMessage($"{Session.SelectedQuest.RewardGold} gold.");
                if (Session.SelectedQuest.RewardItems != null)
                {
                    Session.RaiseMessage($"You will also receive " +
                        $"{ItemFactory.GetGameItemName(Session.CurrentLocation.QuestGiverHere.QuestAvailableHere[0].RewardItems[0].ItemID)}.");
                }
            }
            else 
            {
                
                Session.RaiseMessage("");
                Session.RaiseMessage($"Would you like to complete {Session.SelectedQuest.Name}?");
                Session.RaiseMessage($"You must {Session.SelectedQuest.Description}");

                if (Session.CurrentPlayer.HasAllTheseItems(Session.SelectedQuest.ItemsToComplete))
                {
                    btnComplete.Visibility = Visibility.Visible;
                }
            }

            if (Session.CurrentPlayer.Quests.FirstOrDefault(pq => pq.PlayerQuest.ID == Session.SelectedQuest.ID) != null)
            {
                if (Session.CurrentPlayer.Quests.FirstOrDefault(pq => pq.PlayerQuest.ID == Session.SelectedQuest.ID).IsCompleted)
                {
                    Session.RaiseMessage("You have already completed this quest");
                }
            }            

            button.Visibility = Visibility.Hidden;
        }
    }
}
