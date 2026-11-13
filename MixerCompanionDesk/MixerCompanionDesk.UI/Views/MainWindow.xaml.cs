using System.Windows;
using MixerCompanionDesk.UI.ViewModels;

namespace MixerCompanionDesk.UI.Views;

/// <summary>
/// Main application window.
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// Initializes the main window.
    /// </summary>
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        ViewModel = viewModel;
        DataContext = viewModel;
    }

    /// <summary>Gets the view model displayed by this window.</summary>
    public MainViewModel ViewModel { get; }
}
