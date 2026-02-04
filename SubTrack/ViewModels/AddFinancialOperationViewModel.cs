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
        /// Types d'opérations financières (ajout, retrait)
        /// </summary>
        public ObservableCollection<string> OperationTypes { get; set; }

        private string? _selectedOperationType;
        /// <summary>
        /// Opération financière sélectionnée
        /// </summary>
        public string? SelectedOperationType
        {
            get => _selectedOperationType;
            set
            {
                if (_selectedOperationType != value)
                {
                    _selectedOperationType = value;
                    UpdateCategories();
                }
            }
        }

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
        public string? SelectedOperationCategory { get; set; }

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

            // Initialisation des types d'opérations
            OperationTypes = new ObservableCollection<string>
                {
                    "Ajout", "Retrait"
                };

            // Initialisation de la collection des catégories.
            Categories = new ObservableCollection<string>();

            // Affectation d'une valeur par défaut pour le SelectedOperationType.
            SelectedOperationType = "Retrait"; // Par défaut retrait (modifiable)

            // Initialisation de la date sélectionnée pour éviter une valeur par défaut incorrecte
            SelectedOperationDate = DateTime.Now;

            // Commande pour valider l'ajout d'une opération financière
            ValidateAddOperationCommand = new Command(async () => await ValidateAddOperation());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Met à jour la collection des catégories en fonction du type d'opération sélectionné.
        /// </summary>
        private void UpdateCategories()
        {
            Categories.Clear();
            if (SelectedOperationType == "Ajout")
            {
                // Liste pour les ajouts d'argent.
                Categories.Add("Salaires");
                Categories.Add("Allocations sociales");
                // Vous pouvez ajouter d'autres catégories spécifiques aux ajouts d'argent ici.
            }
            else if (SelectedOperationType == "Retrait")
            {
                // Liste pour les retraits.
                Categories.Add("Alimentation");
                Categories.Add("Transport");
                Categories.Add("Logement");
                Categories.Add("Divertissement");
                Categories.Add("Autre");
            }
        }

        /// <summary>
        /// Valide et ajoute une nouvelle opération financière si les conditions sont remplies.
        /// Gère correctement l'opération d'ajout et de retrait.
        /// </summary>
        private async Task ValidateAddOperation()
        {
            if (string.IsNullOrWhiteSpace(OperationTitle) || OperationAmount <= 0)
            {
                return; // Évite d'ajouter une opération financière invalide
            }

            double finalAmount = (SelectedOperationType == "Retrait") ? -OperationAmount : OperationAmount;

            var newOperation = new FinancialOperation
            {
                OperationTitle = this.OperationTitle,
                OperationAmount = finalAmount,
                OperationDate = this.SelectedOperationDate,
                OperationCategory = this.SelectedOperationCategory,
                IsRecurrent = this.IsOperationRecurrent
            };

            // Déclenche l'événement OperationAdded pour informer le parent de la nouvelle opération financière
            OperationAdded?.Invoke(this, newOperation);

            await this._navigation.PopAsync();
        }

        #endregion
    }
}
