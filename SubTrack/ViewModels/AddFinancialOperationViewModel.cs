using SubTrack.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SubTrack.ViewModels
{
    /// <summary>
    /// Représente le ViewModel de la fenêtre d'ajout des opérations financières mensuelles.
    /// </summary>
    public class AddFinancialOperationViewModel
    {
        #region Navigation
        private readonly INavigation _navigation;
        #endregion

        #region Events

        /// <summary>
        /// Événement déclenché lorsqu'une nouvelle opération financière est ajoutée.
        /// </summary>
        public event EventHandler<FinancialOperation>? OperationAdded;

        #endregion

        #region Commands

        /// <summary>
        /// Commande pour valider l'ajout d'une opération financière.
        /// </summary>
        public ICommand ValidateAddOperationCommand { get; }

        #endregion

        #region Bindable Properties 

        /// <summary>
        /// Titre de l'opération financière.
        /// </summary>
        public string? OperationTitle { get; set; }

        /// <summary>
        /// Date sélectionnée de l'opération financière.
        /// </summary>
        public DateTime SelectedOperationDate { get; set; }

        /// <summary>
        /// Montant de l'opération financière.
        /// </summary>
        public double OperationAmount { get; set; }

        /// <summary>
        /// Catégorie de l'opération financière.
        /// </summary>
        public string? OperationCategory { get; set; }

        /// <summary>
        /// Récurrence de l'opération financière.
        /// </summary>
        public bool IsOperationRecurrent { get; set; }

        /// <summary>
        /// Catégories d'opérations financières disponibles.
        /// </summary>
        public ObservableCollection<string> Categories { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructeur de la classe AddFinancialOperationViewModel.
        /// </summary>
        public AddFinancialOperationViewModel(INavigation navigation)
        {
            _navigation = navigation;

            // Initialisation des catégories d'opérations financières
            Categories = new ObservableCollection<string>
                {
                    "Alimentation", "Transport", "Logement", "Divertissement", "Autre"
                };

            // Initialisation de la date sélectionnée pour éviter une valeur par défaut incorrecte
            SelectedOperationDate = DateTime.Now;

            // Commande pour valider l'ajout d'une opération financière
            ValidateAddOperationCommand = new Command(async () => await ValidateAddOperation());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Valide et ajoute une nouvelle opération financière si les conditions sont remplies.
        /// </summary>
        private async Task ValidateAddOperation()
        {
            if (string.IsNullOrWhiteSpace(OperationTitle) || OperationAmount <= 0)
            {
                return; // Évite d'ajouter une opération financière invalide
            }

            var newOperation = new FinancialOperation
            {
                OperationTitle = this.OperationTitle,
                OperationAmount = this.OperationAmount,
                OperationDate = this.SelectedOperationDate,
                OperationCategory = this.OperationCategory,
                IsRecurrent = this.IsOperationRecurrent
            };

            // Déclenche l'événement OperationAdded pour informer le parent de la nouvelle opération financière
            OperationAdded?.Invoke(this, newOperation);

            await this._navigation.PopAsync();
        }

        #endregion
    }
}
