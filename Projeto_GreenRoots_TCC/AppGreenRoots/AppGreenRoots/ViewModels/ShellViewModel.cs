using AppGreenRoots.Helpers;
using AppGreenRoots.Models;

namespace AppGreenRoots.ViewModels;

public class ShellViewModel : BaseViewModel
{
    private object ? currentViewModel;

    public object CurrentViewModel
    {
        get => currentViewModel;
        set
        {
            currentViewModel = value;
            OnPropertyChanged();
        }
    }

    // Define a tela de login como o ponto de entrada da aplicação
    public ShellViewModel()
    {
        CurrentViewModel = new LoginViewModel(this);
    }

    public void NavigateDashboard(Usuario usuario)
    {
        CurrentViewModel = new TelaInicialViewModel(this, usuario);
    }

    public void NavigatePassaporte(Usuario usuario)
    {
        CurrentViewModel = new PassaporteWizardViewModel(
            usuario: null,
            onVoltar: () => NavigateDashboard(usuario)
        );
    }

    public void NavigateLogin()
    {
        CurrentViewModel = new LoginViewModel(this);
    }
}