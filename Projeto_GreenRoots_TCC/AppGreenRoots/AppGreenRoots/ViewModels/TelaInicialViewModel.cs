using AppGreenRoots.Helpers;
using System.Windows.Input;
using AppGreenRoots.Models;

namespace AppGreenRoots.ViewModels;

public class TelaInicialViewModel : BaseViewModel
{
    // Referência ao Shell para controlar a navegação entre telas
    private readonly ShellViewModel shell;

    public string NomeUsuario { get; set; }

    public ICommand AbrirPassaporteCommand { get; }
    public ICommand LogoutCommand { get; }

    public TelaInicialViewModel(ShellViewModel shell , Usuario usuario)
    {
        this.shell = shell;
        this.NomeUsuario = usuario.Nome;

        AbrirPassaporteCommand = new RelayCommand(_ =>
        {
            shell.NavigatePassaporte(usuario);
        });

        LogoutCommand = new RelayCommand(_ =>
        {
            shell.NavigateLogin();
        });
    }
    
}