using Seawars.Domain.Entities;
using Seawars.Infrastructure.Validation;
using Seawars.WPF.Common;
using Seawars.WPF.View.Pages;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Seawars.WPF.View.UserControls;
using Seawars.WPF.Common.Commands.Base;
using Seawars.WPF.Services;


namespace Seawars.WPF.ViewModels
{
    internal class AuthorizationPageViewModel : ViewModelBase
    {
        #region Commands
        public ICommand RegisterCommand { get; }
        public ICommand GoToLoginWindowCommand { get; }
        public ICommand LoginCommand { get; }
        public ICommand BackCommand { get; }

        #endregion

        #region Data
        private string _name;
        private string _username;
        private string _password;
        private string _repeatedPassword;

        public string RepeatedPassword
        {
            get => _repeatedPassword is not null ? new string('*', _repeatedPassword.Length) : _repeatedPassword;
            set => AddLastSymbol(ref _repeatedPassword, value);
        }
        public string Passwrod
        {
            get => _password is not null? new string('*', _password.Length):string.Empty;
            set => AddLastSymbol(ref _password, value);
        }

        public string Username
        {
            get => _username;
            set => Set(ref _username, value);
        }
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }
        #endregion

        public AuthorizationPageViewModel()
        {
            RegisterCommand = new Command(RegisterCommandAction, x=> true);
            GoToLoginWindowCommand = new Command(GoToLoginWindowAction, x => true);
            LoginCommand = new Command(LoginCommandAction, x => true);
            BackCommand = new Command(BackCommandAction, x => true);
        }

        private void BackCommandAction(object obj) => ServicesLocator.PageService.SetPage<AuthorizationPage>(new AuthorizationPage());
        private void GoToLoginWindowAction(object obj) => ServicesLocator.PageService.SetPage<LoginPage>(new LoginPage());

        private MessageBoxResult ErrorMessage(string message) => MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

        private void LoginCommandAction(object obj)
        {
            var Users = ServicesLocator.UserRepository.GetAll();

             _ = Validator.NotNullElementsExist(Username, _password) is true

             ? ErrorMessage("Please input all fields!") : Users.Exists(x => x.UserName == Username) is false

             ? ErrorMessage($"Username '{Username}' doesnt exist!") : Users.Exists(x => x.Password == _password) is false

             ? ErrorMessage($"Incorrect password!") : SuccessLogin($"Welcome, {Username}!");

        }
        private void RegisterCommandAction(object obj)
        {
            _ = Validator.NotNullElementsExist(Name, Username, _password, _repeatedPassword) is true

            ? ErrorMessage("Please input all fields!") : ServicesLocator.UserRepository.GetAll().Exists(x => x.UserName == Username) is true

            ? ErrorMessage($"This Username '{Username}' is already used... Try another") : _password != _repeatedPassword 

            ? ErrorMessage("Passwords are different. . .") : SuccessRegister("Account has been created!");           

        }

        private MessageBoxResult SuccessLogin(string message)
        {
            UserProfileSettings();

            return MessageBoxResult.OK;
        }
         
        private MessageBoxResult SuccessRegister(string message)
        {
            ServicesLocator.UserRepository.Add<User>(new User(Username, Name, _password));

            UserProfileSettings();

            return MessageBoxResult.OK;
        }

        private void UserProfileSettings()
        {
            var currentUser = ServicesLocator.UserRepository.GetAll().FirstOrDefault(x => x.UserName == Username);

            ServicesLocator.PageService.SetPage(new UserPage(currentUser));

            ServicesLocator.UserPageViewModel.CurrentViewControl = new ProfileControl();

            App.UserProfileWindow = App.WindowCurrent;
        }

    }  
}
