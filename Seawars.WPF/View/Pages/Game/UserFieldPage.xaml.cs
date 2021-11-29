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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Seawars.Domain.Models;
using Seawars.WPF.Services;
using Seawars.WPF.Services.GamePagesService;
using Seawars.WPF.ViewModels;

namespace Seawars.WPF.View.Pages.Game
{
    /// <summary>
    /// Interaction logic for UserFieldPage.xaml
    /// </summary>
    public partial class UserFieldPage : Page
    {
        public UserFieldPage()
        {
            InitializeComponent();

            Generate(ServicesLocator.UserFieldPageViewModel);
        }

        #region Private Methods
        private void Generate(UserFieldPageViewModel vm)
        {
            Button[,] button = new Button[11, 11];
            char[] Alphabet = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };

            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if (i is 0 && j is 0) continue;

                    CellsGenerate(Field);

                    if (i is 0)
                    {
                        ButtonSettings_ForUserField(i, j, button, vm);
                        button[i, j].Content = Alphabet[j - 1];
                        button[i, j].Foreground = Brushes.Black;
                        button[i, j].FontFamily = new FontFamily("MV Boli");
                        button[i, j].BorderThickness = new Thickness(0, 0, 1, 1);
                        button[i, j].IsEnabled = false;

                        ButtonAdd(i, j, button, Field);

                        continue;
                    }

                    if (j is 0)
                    {
                        ButtonSettings_ForUserField(i, j, button, vm);
                        button[i, j].Content = i;
                        button[i, j].Foreground = Brushes.Black;
                        button[i, j].FontFamily = new FontFamily("MV Boli");
                        button[i, j].BorderThickness = new Thickness(0, 0, 1, 1);
                        button[i, j].IsEnabled = false;

                        ButtonAdd(i, j, button, Field);

                        continue;
                    }


                    ButtonSettings_ForUserField(i, j, button, vm);

                    ButtonAdd(i, j, button, Field);

                }
            }
        }
        private static void CellsGenerate(Grid Field)
        {
            ColumnDefinition column = new ColumnDefinition();
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(40);
            column.Width = new GridLength(40);
            Field.ColumnDefinitions.Add(column);
            Field.RowDefinitions.Add(row);
        }
        private static void ButtonAdd(int i, int j, Button[,] button, Grid Field)
        {
            Grid.SetRow(button[i, j], i);
            Grid.SetColumn(button[i, j], j);
            Field.Children.Add(button[i, j]);
        }
        private static void ButtonSettings_ForUserField(int i, int j, Button[,] button, UserFieldPageViewModel vm)
        {
            button[i, j] = new Button();
            button[i, j].BorderBrush = Brushes.Gray;
            button[i, j].Background = Brushes.White;

            Binding BorderBinding = new Binding();
            BorderBinding.Source = vm;
            BorderBinding.Path = new PropertyPath($"Buttons[{i * 11 + j}].Border");
            BorderBinding.Mode = BindingMode.OneWay;    
            button[i, j].SetBinding(Button.BorderThicknessProperty, BorderBinding);

            Binding BackGroundBinding = new Binding();
            BackGroundBinding.Source = vm;
            BackGroundBinding.Path = new PropertyPath($"Color[{i * 11 + j}]");
            BackGroundBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BackGroundBinding.Mode = BindingMode.OneWay;
            button[i, j].SetBinding(Button.BackgroundProperty, BackGroundBinding);

            Binding ContentBinding = new Binding();
            ContentBinding.Source = vm;
            ContentBinding.Path = new PropertyPath($"Buttons[{i * 11 + j}].Content");
            ContentBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            ContentBinding.Mode = BindingMode.OneWay;
            button[i, j].SetBinding(Button.ContentProperty, ContentBinding);
            
            button[i, j].Name = $"C{i * 11 + j}";
            button[i, j].Width = 40;
            button[i, j].Height = 40;
            button[i, j].CommandParameter = button[i, j].Name;
            button[i, j].AllowDrop = true;
            button[i, j].PreviewMouseLeftButtonDown += vm.DragTheShipOnTheFieldAction;
            button[i, j].DragEnter += vm.DragEnter;
            button[i, j].DragLeave += vm.DragLeave;
            button[i, j].Drop += vm.DropAction;

        }
        #endregion

    }
}

