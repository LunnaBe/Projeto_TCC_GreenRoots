using System.Windows;
using System.Windows.Controls;
using AppGreenRoots.ViewModels;

namespace AppGreenRoots.Views;

public partial class LoginView : UserControl
{
    public LoginView()
    {
        InitializeComponent();
    }

    private void PbSenha_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is LoginViewModel vm)
            vm.Senha = PbSenha.Password;
    }
}