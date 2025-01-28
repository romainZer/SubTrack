using SubTrack.ViewModels;

namespace SubTrack.Controls;

public partial class ExpenseItemComponent : ContentView
{
    #region Constructors
    public ExpenseItemComponent()
	{
        InitializeComponent();
        BindingContext = new ExpenseItemViewModel();
	}
    #endregion
}