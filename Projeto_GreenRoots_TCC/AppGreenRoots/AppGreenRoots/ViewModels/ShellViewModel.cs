using AppGreenRoots.Helpers;

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

    public ShellViewModel()
    {
        CurrentViewModel = new LoginViewModel(this);
    }

    public void NavigateDashboard()
    {
        CurrentViewModel = new DashboardViewModel(this);
    }

    public void NavigatePassaporte()
    {
        CurrentViewModel = new PassaporteWizardViewModel(
            usuario: null,
            onVoltar: () => NavigateDashboard()
        );
    }

    public void NavigateLogin()
    {
        CurrentViewModel = new LoginViewModel(this);
    }
}