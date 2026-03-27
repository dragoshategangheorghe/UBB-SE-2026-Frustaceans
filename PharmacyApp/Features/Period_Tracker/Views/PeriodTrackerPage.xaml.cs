using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using PharmacyApp.Features.Period_Tracker.ViewModels;
using Syncfusion.UI.Xaml.Calendar;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PharmacyApp.Features.Period_Tracker.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PeriodTrackerPage : Page
    {
        public PeriodTrackerViewModel ViewModel { get; set; } = new PeriodTrackerViewModel();
        public PeriodTrackerPage()
        {
            InitializeComponent();
            //CalendarsStackPanel.Visibility=Visibility.Collapsed;
        }
        private void UpdateCalendar()
        {
            
        }
    }
}
