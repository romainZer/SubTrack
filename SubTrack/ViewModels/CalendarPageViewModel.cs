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
    /// Représente le ViewModel de la page de visualisation des opérations financières mensuelles
    /// </summary>
    public class CalendarPageViewModel : INotifyPropertyChanged
    {
        #region Attributes
        private double _currentBalance;
        #endregion

        #region Commands

        /// <summary>
        /// Commande lors du clic du bouton d'ajout d'opération
        /// </summary>
        public ICommand AddOperationCommand { get; }

        /// <summary>
        /// Commande lors du swipe d'un OperationItem vers la gauche pour supprimer une opération
        /// </summary>
        public ICommand DeleteOperationCommand { get; }
        #endregion

        #region Properties

        /// <summary>
        /// Obtient ou définit un CalendarViewModel
        /// </summary>
        public CalendarItemViewModel CalendarViewModel { get; set; }

        /// <summary>
        /// Obtient ou définit une liste de toutes les opérations financières filtrées selon le mois courant
        /// </summary>
        public ObservableCollection<FinancialOperation> Operations { get; set; }

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
            this.CalendarViewModel = new CalendarItemViewModel();
            this.Operations = new ObservableCollection<FinancialOperation>();

            // Initialisation de la commande pour ajouter une opération financière
            AddOperationCommand = new Command(async () => await NavigateToAddOperationPage());
            DeleteOperationCommand = new Command<int>(async (id) => await DeleteOperation(id));

            // Abonnement aux changements de mois
            CalendarViewModel.PropertyChanged += CalendarViewModel_PropertyChanged;

            // Initialisation des opérations visibles
            _ = LoadOperations();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Met à jour la balance courante
        /// </summary>
        private void UpdateCurrentBalance()
        {
            double totalIncome = Operations.Where(operation => operation.OperationAmount >= 0).Sum(operation => operation.OperationAmount); // Revenus
            double totalExpense = Operations.Where(operation => operation.OperationAmount < 0).Sum(operation => operation.OperationAmount); // Dépenses
            this.CurrentBalance = totalIncome + totalExpense;
        }

        /// <summary>
        /// Charge les opérations du mois sélectionné
        /// </summary>
        private async Task LoadOperations()
        {
            // Filtrer les opérations en fonction du mois et de l'année courants
            Operations.Clear();
            // TODO : Modifier la requete pour ne pas TOUT récupérer
            var operations = await Database.Instance.GetAllFinancialOperationsAsync();
            foreach (var operation in operations)
            {
                if (operation.IsRecurrent == false
                    && operation.OperationDate.Month == CalendarViewModel.CurrentMonth
                    && operation.OperationDate.Year == CalendarViewModel.CurrentYear)
                {
                    Operations.Add(operation);
                }
                else if (operation.IsRecurrent == true)
                {
                    Operations.Add(operation);
                }
            }
            UpdateCurrentBalance();
            OnPropertyChanged(nameof(Operations));
        }

        /// <summary>
        /// Supprime une opération par son id
        /// </summary>
        /// <param name="id">L'id de l'opération</param>
        /// <returns>Une Task</returns>
        private async Task DeleteOperation(int id)
        {
            var operationToDelete = Operations.FirstOrDefault(e => e.OperationId == id);
            if (operationToDelete != null)
            {
                Operations.Remove(operationToDelete);
                await Database.Instance.DeleteFinancialOperationByIdAsync(id);
                UpdateCurrentBalance();
                OnPropertyChanged(nameof(Operations));
            }
        }
        #endregion

        #region Actions

        /// <summary>
        /// Navigue vers la page d'ajout d'opération financière
        /// </summary>
        private async Task NavigateToAddOperationPage()
        {
            var addOperationPage = new AddFinancialOperationPage();
            if (addOperationPage.BindingContext is AddFinancialOperationViewModel viewModel)
            {
                viewModel.OperationAdded += OnOperationAdded;
            }

            await Shell.Current.Navigation.PushAsync(addOperationPage);
        }

        /// <summary>
        /// Gestion de l'ajout d'une opération financière à la liste et mise à jour de l'affichage
        /// </summary>
        private async void OnOperationAdded(object? sender, FinancialOperation newOperation)
        {
            if (newOperation != null)
            {
                // Vérifiez que toutes les propriétés de l'objet FinancialOperation sont définies
                if (string.IsNullOrEmpty(newOperation.OperationTitle) ||
                    newOperation.OperationDate == default ||
                    string.IsNullOrEmpty(newOperation.OperationCategory))
                {
                    throw new InvalidOperationException("Tous les champs de l'opération financière doivent être remplis.");
                }

                await Database.Instance.AddFinancialOperationAsync(newOperation);
                await LoadOperations();
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
        /// Met à jour les opérations affichées lorsque l'utilisateur change de mois
        /// </summary>
        protected async void CalendarViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CalendarViewModel.CurrentMonth) ||
                e.PropertyName == nameof(CalendarViewModel.CurrentYear))
            {
                await LoadOperations();
            }
        }
        #endregion
    }
}
