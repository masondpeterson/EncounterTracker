using EncounterTracker.DataBase;
using EncounterTracker.DBObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EncounterTracker.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchResultsPage : ContentPage
    {
        private SQLConn _conn = new SQLConn();
        private int _userId;
        private List<Encounter> _elist;

        public SearchResultsPage(string searchTerm, int userId)
        {
            _userId = userId;
            Title = "Encounter Search Results";
            InitializeComponent();

            _elist = SearchEncounters(searchTerm, userId);
            BuildResultFrames(_elist);
        }

        private List<Encounter> SearchEncounters(string searchTerm, int userId)
        {            
            var elist = _conn.GetEncountersByPlayer(userId);
            var encounters = new List<Encounter>();
            if (searchTerm == null || searchTerm == "")
            {
                return elist;
            }
            else
            {
                foreach (var e in elist)
                {
                    var name = e.EncounterName.ToLower();
                    if (name.Contains(searchTerm.ToLower()))
                    {
                        encounters.Add(e);
                    }
                }
            }
            return encounters;
        }

        private void BuildResultFrames(List<Encounter> encounters)
        {
            var backButton = new Button
            {
                Text = "Back"
            };
            backButton.Clicked += backButton_Clicked;
            if(encounters.Count == 0)
            {
                var label = new Label
                {
                    Text = "No Results Found",
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                };
                resultsSection.Children.Add(label);
                resultsSection.Children.Add(backButton);
            }
            else
            {
                //Builds the Frames and Adds them to the page
                foreach (var e in encounters)
                {
                    string dropped;
                    if(e.Dropped == 1)
                    {
                        dropped = "yes";
                    }
                    else
                    {
                        dropped = "no";
                    }
                    var frame = new Frame
                    {
                        BorderColor = Color.Gray,
                        HasShadow = true,
                        Content = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Children =
                        {
                            new StackLayout
                            {
                                HorizontalOptions = LayoutOptions.StartAndExpand,
                                Children =
                                {
                                    new Label
                                    {
                                        Text = e.EncounterName,

                                    },
                                    new Label
                                    {
                                        Text = _conn.GetCharacterById(e.CharId).CharName,
                                    },
                                    new Label
                                    {
                                        Text = "Session: " + e.Session.ToString("MMM dd @ h:mm tt")
                                    }
                                }
                            },
                            new StackLayout
                            {
                                HorizontalOptions = LayoutOptions.EndAndExpand,
                                Children =
                                {
                                    new Label
                                    {
                                        Text = "Kills: " + e.Kills
                                    },
                                    new Label
                                    {
                                        Text = "Assists: " + e.Assist
                                    },
                                    new Label
                                    {
                                        Text = "Dropped: " + dropped
                                    }
                                }
                            },
                            new StackLayout
                            {
                                HorizontalOptions = LayoutOptions.EndAndExpand,
                                Children =
                                {
                                    new Label
                                    {
                                        Text = "Dealt: " + e.DmgDealt
                                    },
                                    new Label
                                    {
                                        Text = "Taken: " + e.DmgTaken
                                    },
                                    new Label
                                    {
                                        Text = "Healing: " + e.Healing
                                    }
                                }
                            }
                        }
                        }
                    };
                    resultsSection.Children.Add(frame);
                }
                resultsSection.Children.Add(backButton);
            }
        }

        async void searchButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchResultsPage(searchInput.Text, _userId));
        }

        async void backButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HomePage(_userId));
        }
    }
}