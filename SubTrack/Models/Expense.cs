using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SubTrack.Models
{
    /// <summary>
    /// Représente une dépense
    /// </summary>
    public class Expense : INotifyPropertyChanged
    {
        #region Attributes
        private string? _expenseTitle;
        private DateTime _expenseDate;
        private double _expenseAmount;
        private string? _expenseCategory;
        private bool _isRecurrent;
        #endregion

        #region Properties

        /// <summary>
        /// Définit le titre (libellé) de la dépense
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
        /// Définit la date sélectionnée par l'utilisateur (l'utilisateur ajoute une dépense à une date donnée)
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
        /// Définit le montant de la dépense mensuelle
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
        /// Définit la catégorie d'une dépense mensuelle (catégories personnalisables)
        /// </summary>
        public string? ExpenseCategory
        {
            get => this._expenseCategory;
            set
            {
                if (this._expenseCategory != value)
                {
                    this._expenseCategory = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Définit si un paiement est récurrent ou non
        /// </summary>
        public bool IsRecurrent
        {
            get => this._isRecurrent;
            set
            {
                if (this._isRecurrent != value)
                {
                    this._isRecurrent = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructeur de la classe Expense
        /// </summary>
        /// <param name="date"></param>
        /// <param name="amount"></param>
        /// <param name="category"></param>
        /// <param name="isRecurrent"></param>
        public Expense(string? title, DateTime date, double amount, string? category, bool isRecurrent = false)
        {
            this.ExpenseTitle = title;
            this.ExpenseDate = date;
            this.ExpenseAmount = amount;
            this.ExpenseCategory = category;
            this.IsRecurrent = isRecurrent;
        }

        /// <summary>
        /// Constructeur de la classe Expense (sans arguments -> Construction de la classe en utilisant les Propriétés dynamiques)
        /// </summary>
        public Expense() { }

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
