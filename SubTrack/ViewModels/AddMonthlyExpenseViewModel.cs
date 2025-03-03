using SubTrack.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SubTrack.ViewModels
{
    /// <summary>
    /// Représente le ViewModel de la fenêtre d'ajout des dépenses mensuelles.
    /// </summary>
    public class AddMonthlyExpenseViewModel
    {
        #region Events

        /// <summary>
        /// Événement déclenché lorsqu'une nouvelle dépense est ajoutée.
        /// </summary>
        public event EventHandler<Expense>? ExpenseAdded;

        #endregion

        #region Commands

        /// <summary>
        /// Commande pour valider l'ajout d'une dépense.
        /// </summary>
        public ICommand ValidateAddExpenseCommand { get; }

        #endregion

        #region Bindable Properties 

        /// <summary>
        /// Titre de la dépense.
        /// </summary>
        public string? ExpenseTitle { get; set; }

        /// <summary>
        /// Date sélectionnée de la dépense.
        /// </summary>
        public DateTime SelectedExpenseDate { get; set; }

        /// <summary>
        /// Montant de la dépense.
        /// </summary>
        public double ExpenseAmount { get; set; }

        /// <summary>
        /// Catégorie de la dépense.
        /// </summary>
        public string? ExpenseCategory { get; set; }

        /// <summary>
        /// Récurrence de la dépense.
        /// </summary>
        public bool IsExpenseRecurrent { get; set; }

        /// <summary>
        /// Catégories de dépenses disponibles.
        /// </summary>
        public ObservableCollection<string> Categories { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructeur de la classe AddMonthlyExpenseViewModel.
        /// </summary>
        public AddMonthlyExpenseViewModel()
        {
            // Initialisation des catégories de dépenses
            Categories = new ObservableCollection<string>
            {
                "Alimentation", "Transport", "Logement", "Divertissement", "Autre"
            };

            // Initialisation de la date sélectionnée pour éviter une valeur par défaut incorrecte
            SelectedExpenseDate = DateTime.Now;

            // Commande pour valider l'ajout d'une dépense
            ValidateAddExpenseCommand = new Command(ValidateAddExpense);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Valide et ajoute une nouvelle dépense si les conditions sont remplies.
        /// </summary>
        private void ValidateAddExpense()
        {
            if (string.IsNullOrWhiteSpace(ExpenseTitle) || ExpenseAmount <= 0)
            {
                return; // Évite d'ajouter une dépense invalide
            }

            var newExpense = new Expense
            {
                ExpenseTitle = this.ExpenseTitle,
                ExpenseAmount = this.ExpenseAmount,
                ExpenseDate = this.SelectedExpenseDate,
                ExpenseCategory = this.ExpenseCategory,
                IsRecurrent = this.IsExpenseRecurrent
            };

            // Déclenche l'événement ExpenseAdded pour informer le parent de la nouvelle dépense
            ExpenseAdded?.Invoke(this, newExpense);
        }

        #endregion
    }
}
