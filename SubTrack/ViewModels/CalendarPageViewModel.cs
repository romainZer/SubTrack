using SubTrack.Controls;
using SubTrack.Models;
using SubTrack.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SubTrack.ViewModels
{
    /// <summary>
    /// Représente le ViewModel de la page de visualisation des dépenses mensuelles
    /// </summary>
    public class CalendarPageViewModel : INotifyPropertyChanged
    {
        #region Commands

        /// <summary>
        /// Commande lors du clic du bouton d'ajout des dépenses
        /// </summary>
        public ICommand AddExpenseCommand { get; }

        #endregion

        #region Properties

        /// <summary>
        /// Obtient ou définit un CalendarViewModel
        /// </summary>
        public CalendarItemViewModel CalendarViewModel { get; set; }

        /// <summary>
        /// Obtient ou définit les dépenses mensuelles brutes
        /// </summary>
        public ObservableCollection<Expense> RawExpenses { get; set; }

        /// <summary>
        /// Obtient ou définit une liste de tous les ExpenseItemsViewModel filtrés selon le mois courant
        /// </summary>
        public ObservableCollection<Expense> Expenses { get; set; }

        #endregion

        #region Constructors 

        /// <summary>
        /// Constructeur du ViewModel de la Page CalendarPage
        /// </summary>
        public CalendarPageViewModel()
        {
            this.CalendarViewModel = new CalendarItemViewModel();
            this.Expenses = new ObservableCollection<Expense>();
            this.RawExpenses = new ObservableCollection<Expense>();

            // Initialisation de la commande pour ajouter une dépense
            AddExpenseCommand = new Command(async () => await NavigateToAddExpensePage());

            // Ajout de dépenses factices
            RawExpenses.Add(new Expense { ExpenseTitle = "Netflix", ExpenseAmount = 15.99, ExpenseDate = new DateTime(2025, 1, 12), IsRecurrent = true });
            RawExpenses.Add(new Expense { ExpenseTitle = "Spotify", ExpenseAmount = 9.99, ExpenseDate = new DateTime(2025, 2, 12), IsRecurrent = true });
            RawExpenses.Add(new Expense { ExpenseTitle = "Amazon Prime", ExpenseAmount = 12.99, ExpenseDate = new DateTime(2025, 2, 12), IsRecurrent = false });
            RawExpenses.Add(new Expense { ExpenseTitle = "EDF", ExpenseAmount = 82.00, ExpenseDate = new DateTime(2025, 2, 12), IsRecurrent = false });

            // Abonnement aux changements de mois
            CalendarViewModel.PropertyChanged += CalendarViewModel_PropertyChanged;

            // Initialisation des dépenses visibles
            LoadExpenses();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Charge les dépenses du mois sélectionné
        /// </summary>
        private void LoadExpenses()
        {
            // Filtrer les dépenses en fonction du mois et de l'année courants
            Expenses.Clear();
            foreach (var expense in RawExpenses.Where(e => e.ExpenseDate.Year == CalendarViewModel.CurrentYear &&
                                                           e.ExpenseDate.Month == CalendarViewModel.CurrentMonth))
            {
                Expenses.Add(expense);
            }
            OnPropertyChanged(nameof(Expenses));
        }

        #endregion

        #region Actions

        /// <summary>
        /// Navigue vers la page d'ajout de dépense
        /// </summary>
        private async Task NavigateToAddExpensePage()
        {
            var addExpensePage = new AddMonthlyExpensePage();
            if (addExpensePage.BindingContext is AddMonthlyExpenseViewModel viewModel)
            {
                // S'abonner à l'événement pour ajouter une dépense
                viewModel.ExpenseAdded += OnExpenseAdded;
            }

            await Shell.Current.Navigation.PushAsync(addExpensePage);
        }

        /// <summary>
        /// Gestion de l'ajout d'une dépense à la liste et mise à jour de l'affichage
        /// </summary>
        private void OnExpenseAdded(object? sender, Expense newExpense)
        {
            if (newExpense != null)
            {
                RawExpenses.Add(newExpense);
                LoadExpenses();
            }
        }

        #endregion

        #region Event Handlers

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

        /// <summary>
        /// Met à jour les dépenses affichées lorsque l'utilisateur change de mois
        /// </summary>
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
