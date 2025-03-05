using SubTrack.ViewModels;

namespace SubTrack.Views;

public partial class AddMonthlyExpensePage : ContentPage
{
    public AddMonthlyExpensePage()
    {
        InitializeComponent();
        BindingContext = new AddMonthlyExpenseViewModel(Navigation);
    }
}