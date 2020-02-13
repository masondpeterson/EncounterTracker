﻿using EncounterTracker.DataBase;
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
            if(encounters.Count == 0)
            {
                var label = new Label
                {
                    Text = "No Results Found",
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                };
                resultsSection.Children.Add(label);
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
                            new Label
                            {
                                Text = e.EncounterName,
                                HorizontalOptions = LayoutOptions.CenterAndExpand

                            },
                            new Label
                            {
                                Text = _conn.GetCharacterById(e.CharId).CharName,
                                HorizontalOptions = LayoutOptions.CenterAndExpand
                            },
                            new StackLayout
                            {
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