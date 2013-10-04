using System;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Mc.Settings.Model.Concrete;


namespace WpfClient.ViewModel.Settings
{
    public class PasswordsSettingsVm
    {
        private RelayCommand _saveClickCommand;
        public ICommand SaveClick
        {
            get { return _saveClickCommand ?? (_saveClickCommand = new RelayCommand(SaveClickHandler)); }
        }

        private void SaveClickHandler()
        {
            if (CheckErrors())
            {
                int value = Int32.Parse(NewPassword);
                Config.Instance.RemotePassword = value.ToString();
                MessageBox.Show("Новый пароль успешно сохранен", "", MessageBoxButton.OKCancel,
                               MessageBoxImage.Information);
            }
        }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string RepeatPassword { get; set; }

        private bool CheckErrors()
        {
            if (OldPassword != Config.Instance.RemotePassword.ToString())
            {
                MessageBox.Show("Старый пароль введен неверно", "Ошибка", MessageBoxButton.OKCancel,
                                MessageBoxImage.Error);
                return false;
            }
            if(NewPassword=="")
            {
                MessageBox.Show("Введите новый пароль", "Ошибка", MessageBoxButton.OKCancel,
                                MessageBoxImage.Error);
                return false;
            }
            if (NewPassword != RepeatPassword)
            {
                MessageBox.Show("Повтор пароля введен неверно", "Ошибка", MessageBoxButton.OKCancel,
                                MessageBoxImage.Error);
                return false;
            }
            return true;
        }
    }
}
