using SubTrack.Controls;
using SubTrack.Data;
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
        #region Attributes
        private double _currentBalance;
        #endregion

        #region Commands

        /// <summary>
        /// Commande lors du clic du bouton d'ajout des dépenses
        /// </summary>
        public ICommand AddExpenseCommand { get; }

        /// <summary>
        /// Commande lors du swipe d'un ExpenseItem vers la gauche
        /// </summary>
        public ICommand DeleteExpenseCommand { get; }
        #endregion

        #region Properties

        /// <summary>
        /// Obtient ou définit un CalendarViewModel
        /// </summary>
        public CalendarItemViewModel CalendarViewModel { get; set; }

        /// <summary>
        /// Obtient ou définit une liste de tous les ExpenseItemsViewModel filtrés selon le mois courant
        /// </summary>
        public ObservableCollection<Expense> Expenses { get; set; }

        /// <summary>
        /// Obtient ou définit la balance actuelle sur le compte en banque
        /// </summary>
        public double CurrentBalance
        {
            get => _currentBalance;
            set
            {
                if (_currentBalance != value)
                {
                    _currentBalance = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructeur du ViewModel de la Page CalendarPage
        /// </summary>
        public CalendarPageViewModel()
        {
            this.CurrentBalance = 10000;

            this.CalendarViewModel = new CalendarItemViewModel();
            this.Expenses = new ObservableCollection<Expense>();

            // Initialisation de la commande pour ajouter une dépense
            AddExpenseCommand = new Command(async () => await NavigateToAddExpensePage());
            DeleteExpenseCommand = new Command<int>(async (id) => await DeleteExpense(id));

            // Abonnement aux changements de mois
            CalendarViewModel.PropertyChanged += CalendarViewModel_PropertyChanged;

            // Initialisation des dépenses visibles
            _ = LoadExpenses();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Charge les dépenses du mois sélectionné
        /// </summary>
        private async Task LoadExpenses()
        {
            // Filtrer les dépenses en fonction du mois et de l'année courants
            Expenses.Clear();
            var expenses = await Database.Instance.GetAllExpensesAsync();
            foreach (var expense in expenses)
            {
                if (expense.ExpenseDate.Month == CalendarViewModel.CurrentMonth &&
                    expense.ExpenseDate.Year == CalendarViewModel.CurrentYear)
                {
                    Expenses.Add(expense);
                }
            }
            UpdateCurrentBalance();
            OnPropertyChanged(nameof(Expenses));
        }

        /// <summary>
        /// Met à jour la balance courante
        /// </summary>
        private void UpdateCurrentBalance()
        {
            CurrentBalance = Expenses.Sum(e =>  -e.ExpenseAmount);
        }

        /// <summary>
        /// Supprime une dépense par son id
        /// </summary>
        /// <param name="id">L'id de la dépense</param>
        /// <returns>Une Task</returns>
        private async Task DeleteExpense(int id)
        {
            var expenseToDelete = Expenses.FirstOrDefault(e => e.ExpenseId == id);
            if (expenseToDelete != null)
            {
                Expenses.Remove(expenseToDelete);
                await Database.Instance.DeleteExpenseByIdAsync(id);
                UpdateCurrentBalance();
                OnPropertyChanged(nameof(Expenses));
            }
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
                viewModel.ExpenseAdded += OnExpenseAdded;
            }

            await Shell.Current.Navigation.PushAsync(addExpensePage);
        }

        /// <summary>
        /// Gestion de l'ajout d'une dépense à la liste et mise à jour de l'affichage
        /// </summary>
        private async void OnExpenseAdded(object? sender, Expense newExpense)
        {
            if (newExpense != null)
            {
                // Vérifiez que toutes les propriétés de l'objet Expense sont définies
                if (string.IsNullOrEmpty(newExpense.ExpenseTitle) ||
                    newExpense.ExpenseAmount <= 0 ||
                    newExpense.ExpenseDate == default ||
                    string.IsNullOrEmpty(newExpense.ExpenseCategory))
                {
                    throw new InvalidOperationException("Tous les champs de la dépense doivent être remplis.");
                }

                await Database.Instance.AddExpenseAsync(newExpense);
                await LoadExpenses();
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
        protected async void CalendarViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CalendarViewModel.CurrentMonth) ||
                e.PropertyName == nameof(CalendarViewModel.CurrentYear))
            {
                await LoadExpenses();
            }
        }
        #endregion
    }
}
