using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace SubTrack.ViewModels
{
    /// <summary>
    /// Représente le ViewModel du composant ExpenseItem
    /// </summary>
    public class ExpenseItemViewModel : INotifyPropertyChanged
    {
        #region Attributes
        private DateTime _expenseDate;
        private string? _expenseTitle;
        private double _expenseAmount;
        private bool _isRecurrent;
        #endregion

        #region Properties
        /// <summary>
        /// Obtient ou définit la date à laquelle une dépense est faite
        /// </summary>
        public DateTime ExpenseDate
        {
            get => this._expenseDate;
            set
            {
                if (this._expenseDate != value)
                {
                    this._expenseDate = value;
                    OnPropertyChanged();
                }
            }
        }
        
        /// <summary>
        /// Obtient ou définit le titre/motif d'une dépense
        /// </summary>
        public string? ExpenseTitle
        {
            get => this._expenseTitle;
            set
            {
                if (this._expenseTitle != value)
                {
                    this._expenseTitle = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Obtient ou définit le montant d'une dépense
        /// </summary>
        public double ExpenseAmount
        {
            get => this._expenseAmount;
            set
            {
                if (this._expenseAmount != value)
                {
                    this._expenseAmount = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Obtient ou définit si une dépense est menuelle ou unique
        /// </summary>
        public bool IsRecurrent
        {
            get => this._isRecurrent;
            set
            {
                if (this._isRecurrent != value)
                {
                    this._isRecurrent= value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Constructors
        public ExpenseItemViewModel()
        {
            //Appeler la méthode LoadData() dans le but d'initialiser les données depuis la base
            this.ExpenseDate = DateTime.Now;
            this.ExpenseTitle = "Netflix";
            this.ExpenseAmount = 100;
            this.IsRecurrent = true;
        }
        #endregion

        #region Event Handler
        /// <summary>
        /// Se produit lorsque la valeur d'une propriété change.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Déclenche l'événement <see cref="PropertyChanged"/>.
        /// </summary>
        /// <param name="propertyName">Nom de la propriété qui a changé.</param>
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
