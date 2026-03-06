using System.Windows;
using AppGreenRoots.ViewModels;

namespace AppGreenRoots.Views;

public partial class ShellWindow : Window
{
    public ShellWindow()
    {
        InitializeComponent();
        DataContext = new ShellViewModel();
    }
}