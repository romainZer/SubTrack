using SubTrack.Models;
using SubTrack.ViewModels;

namespace SubTrack.Controls;

public partial class OperationItemComponent : ContentView
{
    #region Constructors
    public OperationItemComponent()
	{
        InitializeComponent();
        BindingContext = new FinancialOperation();
	}
    #endregion
}