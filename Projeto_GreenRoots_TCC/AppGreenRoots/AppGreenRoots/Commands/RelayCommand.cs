using System;
using System.Windows.Input;

namespace AppGreenRoots.Helpers;

public class RelayCommand : ICommand
{
    // Variaveis que serão executadas
    private readonly Action<object?> execute;
    private readonly Func<object?, bool>? canExecute;

    // Contrutor que inicializa os comandos com a ação principal
    public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
        this.execute = execute;
        this.canExecute = canExecute;
    }

    // Evento que notifica caso a permissão do pode executar mude.
    public event EventHandler? CanExecuteChanged;

    // Verifica se o comando pode ser executado
    public bool CanExecute(object? parameter)
    {
        return canExecute == null || canExecute(parameter);
    }

    // Executa o comando
    public void Execute(object? parameter)
    {
        execute(parameter);
    }
}