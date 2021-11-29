using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Seawars.Domain.Enums;
using Seawars.Interfaces.Game;
using Seawars.WPF.Services;
using Seawars.WPF.ViewModels;

namespace Seawars.WPF.View.Pages.Game
{
    public partial class BattleGroundPage : Page
    {
        public BattleGroundPage(GameMode gameMode)
        {
            InitializeComponent();

            IBattleGroundData vm = gameMode is GameMode.User
                ? ServicesLocator.BattleGroundDataWithUserViewModel
                : ServicesLocator.BattleGroundDataWithComputerViewModel;

            DataContext = vm;

            CreateField<UserFieldPageViewModel>(UserField, ServicesLocator.UserFieldPageViewModel, null);
            CreateField<EnemyFieldViewModel>(ComputerField, ServicesLocator.EnemyFieldViewModel, vm.AttackCommand);

        }
        #region Private Methods
        private void CreateField<T>(Grid Field, T vm, ICommand cmd) where T : class
        {
            var button = new Button[11, 11];
            char[] Alphabet = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };

            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if (i is 0 && j is 0) continue;

                    CellsGenerate(Field);

                    if (i is 0)
                    {
                        ButtonSettings(i, j, button, vm, cmd);
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
                        ButtonSettings(i, j, button, vm, cmd);
                        button[i, j].Content = i;
                        button[i, j].Foreground = Brushes.Black;
                        button[i, j].FontFamily = new FontFamily("MV Boli");
                        button[i, j].BorderThickness = new Thickness(0, 0, 1, 1);
                        button[i, j].IsEnabled = false;

                        ButtonAdd(i, j, button, Field);

                        continue;
                    }

                    ButtonSettings(i, j, button, vm, cmd);

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
        private static void ButtonSettings<T>(int i, int j, Button[,] button, T vm, ICommand Command)
        {
            button[i, j] = new Button();
            button[i, j].BorderBrush = Brushes.Gray;
            button[i, j].Background = Brushes.White;

            Binding BorderBinding = new Binding();
            BorderBinding.Source = vm;
            BorderBinding.Path = new PropertyPath($"Buttons[{i * 11 + j}].Border");
            BorderBinding.Mode = BindingMode.OneWay;
            button[i, j].SetBinding(Button.BorderThicknessProperty, BorderBinding);

            Binding ContentBinding = new Binding();
            ContentBinding.Source = vm;
            ContentBinding.Path = new PropertyPath($"Buttons[{i * 11 + j}].Content");
            ContentBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            ContentBinding.Mode = BindingMode.OneWay;
            button[i, j].SetBinding(Button.ContentProperty, ContentBinding);

            Binding EnebledBinding = new Binding();
            EnebledBinding.Source = vm;
            EnebledBinding.Path = new PropertyPath($"Buttons[{i * 11 + j}].CanUse");
            EnebledBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            EnebledBinding.Mode = BindingMode.TwoWay;
            button[i, j].SetBinding(Button.IsEnabledProperty, EnebledBinding);

            button[i, j].Name = $"C{i * 11 + j}";
            button[i, j].Width = 40;
            button[i, j].Height = 40;
            button[i, j].Command = Command;
            button[i, j].CommandParameter = button[i, j].Name;
        }
        #endregion
    }
}
