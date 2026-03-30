using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PharmacyApp.Features.Accounts.Logic;
using PharmacyApp.Features.Accounts.Views;
using PharmacyApp.Features.Period_Tracker.ViewModels;
using PharmacyApp.Models;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PharmacyApp.Features.Period_Tracker.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PeriodTrackerPage : Page
    {
        public PeriodTrackerViewModel ViewModel { get; } = new PeriodTrackerViewModel();
        public PeriodTrackerPage()
        {
            InitializeComponent();
            
        }

        private void OnCalculateCycleClicked(object sender, RoutedEventArgs e)
        {
            ViewModel.CalculatePeriodTracker(StartPeriodDatePicker.Date, CycleDaysNumberBox.Value,
                PeriodLastsNumberBox.Value, PMSRadioButtons.SelectedIndex);

            ViewModel.CalendarsVisibility = "Visible"; 
        }
    }
}
