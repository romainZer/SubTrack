using SubTrack.ViewModels;

namespace SubTrack.Views;

public partial class AddFinancialOperationPage : ContentPage
{
    public AddFinancialOperationPage()
    {
        InitializeComponent();
        BindingContext = new AddFinancialOperationViewModel(Navigation);
    }
}