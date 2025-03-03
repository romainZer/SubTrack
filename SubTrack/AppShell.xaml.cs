using SubTrack.Views;

namespace SubTrack
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AddMonthlyExpensePage), typeof(AddMonthlyExpensePage));
        }
    }
}
