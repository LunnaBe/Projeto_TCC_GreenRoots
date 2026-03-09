
using AppGreenRoots.Helpers;
using AppGreenRoots.Models;
using AppGreenRoots.Services;
using System;
using System.Windows.Input;

namespace AppGreenRoots.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private readonly ShellViewModel _shell;
    private readonly UsuarioService _usuarioService = new();

    public LoginViewModel(ShellViewModel shell)
    {
        _shell = shell;

        // Inicialização dos comandos que a View irá chamar através de Binding
        LoginCommand = new RelayCommand(_ => Login());
        CadastrarCommand = new RelayCommand(_ => Cadastrar());
        AlternarModoCommand = new RelayCommand(_ => AlternarModo());
        ExitCommand = new RelayCommand(_ => Environment.Exit(0));
    }

    private bool _isCadastro;
    public bool IsCadastro
    {
        get => _isCadastro;
        set { _isCadastro = value; OnPropertyChanged(); }
    }

    private string _nome = "";
    public string Nome
    {
        get => _nome;
        set { _nome = value; OnPropertyChanged(); }
    }

    private string _email = "";
    public string Email
    {
        get => _email;
        set { _email = value; OnPropertyChanged(); }
    }

    private string _senha = "";
    public string Senha
    {
        get => _senha;
        set { _senha = value; OnPropertyChanged(); }
    }

    private string _mensagem = "";
    public string Mensagem
    {
        get => _mensagem;
        set { _mensagem = value; OnPropertyChanged(); }
    }

    public ICommand LoginCommand { get; }
    public ICommand CadastrarCommand { get; }
    public ICommand AlternarModoCommand { get; }
    public ICommand ExitCommand { get; }

    // Alterna entre modo Login e modo Cadastro
    private void AlternarModo()
    {
        IsCadastro = !IsCadastro;
        Mensagem = "";
    }

    private void Login()
    {
        var usuario = _usuarioService.Login(Email, Senha);

        if (usuario == null)
        {
            Mensagem = "Usuário ou senha inválidos.";
            return;
        }

        _shell.NavigateDashboard(usuario);
    }

    private void Cadastrar()
    {
        if (string.IsNullOrWhiteSpace(Nome))
        {
            Mensagem = "Informe o nome.";
            return;
        }

        if (string.IsNullOrWhiteSpace(Email))
        {
            Mensagem = "Informe o email.";
            return;
        }

        if (string.IsNullOrWhiteSpace(Senha))
        {
            Mensagem = "Informe a senha.";
            return;
        }

        var ok = _usuarioService.Cadastrar(Nome, Email, Senha);

        if (!ok)
        {
            Mensagem = "E-mail já cadastrado.";
            return;
        }

        Mensagem = "Conta criada com sucesso!";
        IsCadastro = false;
    }
}