using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using SOSCSRPG.Core;
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
                       
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _messageBroker.OnMessageRaised += OnGameMessageRaised;

            if (Session.CurrentPlayer.Quests.FirstOrDefault(pq => pq.PlayerQuest.ID == Session.CurrentLocation.QuestGiverHere.QuestAvailableHere[0].ID) != null)
            {
                if (Session.CurrentPlayer.Quests.FirstOrDefault(pq => pq.PlayerQuest.ID == Session.CurrentLocation.QuestGiverHere.QuestAvailableHere[0].ID).IsCompleted)
                {
                    btnQuestOne.Visibility = Visibility.Hidden;
                }
            }
            if (Session.CurrentLocation.QuestGiverHere.QuestAvailableHere.Count > 1)
            {
                if (Session.CurrentPlayer.Quests.FirstOrDefault(pq => pq.PlayerQuest.ID == Session.CurrentLocation.QuestGiverHere.QuestAvailableHere[1].ID) != null)
                {
                    if (Session.CurrentPlayer.Quests.FirstOrDefault(pq => pq.PlayerQuest.ID == Session.CurrentLocation.QuestGiverHere.QuestAvailableHere[1].ID).IsCompleted)
                    {
                        btnQuestOne.Visibility = Visibility.Hidden;
                    }
                }
            }
            if (Session.CurrentLocation.QuestGiverHere.QuestAvailableHere.Count > 1)
            {
                if (btnQuestOne.Visibility == Visibility.Hidden && btnQuestTwo.Visibility == Visibility.Hidden)
                {
                    Session.AllQuestsAtQuestGiverAreCompleted(Session.CurrentLocation.QuestGiverHere.Name);
                }
            }
            else
            {
                if (btnQuestOne.Visibility == Visibility.Hidden)
                {
                    Session.AllQuestsAtQuestGiverAreCompleted(Session.CurrentLocation.QuestGiverHere.Name);
                }
            }
               
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
            if (!Session.CurrentPlayer.Inventory.HasAllTheseItems(Session.SelectedQuest.ItemsToComplete)) return;
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
            
            Session.SelectedQuest = Session.CurrentLocation.QuestGiverHere.QuestAvailableHere[0];

            Button button = sender as Button;

            Session.SelectQuest(Session.SelectedQuest);

            if (Session.CurrentPlayer.Quests.FirstOrDefault(pq => pq.PlayerQuest.ID == Session.SelectedQuest.ID) == null)
            {
                btnAccept.Visibility = Visibility.Visible;
            }
            else
            {
                if (Session.CurrentPlayer.Inventory.HasAllTheseItems(Session.SelectedQuest.ItemsToComplete))
                {
                    btnComplete.Visibility = Visibility.Visible;
                }
            }

            button.Visibility = Visibility.Hidden;
        }
        private void OnClick_ChooseQuestTwo(object sender, RoutedEventArgs e)
        {
            Session.SelectedQuest = Session.CurrentLocation.QuestGiverHere.QuestAvailableHere[1];

            Button button = sender as Button;

            Session.SelectQuest(Session.SelectedQuest);

            if (Session.CurrentPlayer.Quests.FirstOrDefault(pq => pq.PlayerQuest.ID == Session.SelectedQuest.ID) == null)
            {
                btnAccept.Visibility = Visibility.Visible;
            }
            else
            {
                if (Session.CurrentPlayer.Inventory.HasAllTheseItems(Session.SelectedQuest.ItemsToComplete))
                {
                    btnComplete.Visibility = Visibility.Visible;
                }
            }

            button.Visibility = Visibility.Hidden;
        }
    }
}
