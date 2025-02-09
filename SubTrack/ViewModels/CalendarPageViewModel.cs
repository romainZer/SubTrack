using SubTrack.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SubTrack.ViewModels
{
    public class CalendarPageViewModel : INotifyPropertyChanged
    {
        #region Properties
        /// <summary>
        /// Obtient ou définit un CalendarViewModel
        /// </summary>
        public CalendarItemViewModel CalendarViewModel { get; set; }

        public ObservableCollection<ExpenseItemViewModel> RawExpenses { get; set; }

        /// <summary>
        /// Obtient ou définit une liste de tous les ExpenseItemsViewModel
        /// </summary>
        public ObservableCollection<ExpenseItemViewModel> Expenses { get; set; }
        #endregion

        #region Constructors 
        /// <summary>
        /// Constructeur du ViewModel de la Page CalendarPage
        /// </summary>
        public CalendarPageViewModel()
        {
            this.CalendarViewModel = new CalendarItemViewModel();
            this.Expenses = new ObservableCollection<ExpenseItemViewModel>();
            this.RawExpenses = new ObservableCollection<ExpenseItemViewModel>();

            RawExpenses.Add(new ExpenseItemViewModel
            {
                ExpenseTitle = "Netflix",
                ExpenseAmount = 15.99,
                ExpenseDate = new DateTime(2025, 1, 12),
                IsRecurrent = true
            });

            RawExpenses.Add(new ExpenseItemViewModel
            {
                ExpenseTitle = "Spotify",
                ExpenseAmount = 9.99,
                ExpenseDate = new DateTime(2025, 2, 12),
                IsRecurrent = true
            });

            RawExpenses.Add(new ExpenseItemViewModel
            {
                ExpenseTitle = "Dealer",
                ExpenseAmount = 9.99,
                ExpenseDate = new DateTime(2025, 2, 12),
                IsRecurrent = true
            });

            RawExpenses.Add(new ExpenseItemViewModel
            {
                ExpenseTitle = "Bible.com",
                ExpenseAmount = 9.99,
                ExpenseDate = new DateTime(2025, 2, 12),
                IsRecurrent = true
            });

            RawExpenses.Add(new ExpenseItemViewModel
            {
                ExpenseTitle = "Amazon Prime",
                ExpenseAmount = 12.99,
                ExpenseDate = new DateTime(2025, 2, 12),
                IsRecurrent = false
            });

            RawExpenses.Add(new ExpenseItemViewModel
            {
                ExpenseTitle = "EDF",
                ExpenseAmount = 82.00,
                ExpenseDate = new DateTime(2025, 2, 12),
                IsRecurrent = false
            });

            RawExpenses.Add(new ExpenseItemViewModel
            {
                ExpenseTitle = "Test2",
                ExpenseAmount = 82.00,
                ExpenseDate = new DateTime(2025, 3, 12),
                IsRecurrent = true
            });

            //Abonnement au suivi des mois
            CalendarViewModel.PropertyChanged += CalendarViewModel_PropertyChanged;

            //Initialiser les dépenses (factices pour le moment)
            this.LoadExpenses();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Charge des dépenses factices
        /// </summary>
        private void LoadExpenses()
        {
            var filteredExpenses = RawExpenses
                .Where(expense =>
                    expense.ExpenseDate.Year == CalendarViewModel.CurrentYear &&
                    expense.ExpenseDate.Month == CalendarViewModel.CurrentMonth)
                .ToList();

            Expenses.Clear();
            foreach (var expense in filteredExpenses)
            {
                Expenses.Add(expense);
            }

            OnPropertyChanged(nameof(Expenses));
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

        protected void CalendarViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CalendarViewModel.CurrentMonth))
            {
                LoadExpenses();
            }
        }
        #endregion
    }
}
