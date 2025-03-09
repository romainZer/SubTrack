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
    /// Représente une opération financière
    /// </summary>
    public class FinancialOperation : INotifyPropertyChanged
    {
        #region Attributes
        private int _operationId;
        private string? _operationTitle;
        private DateTime _operationDate;
        private double _operationAmount;
        private string? _operationCategory;
        private bool _isRecurrent;
        #endregion

        #region Properties

        /// <summary>
        /// Définit l'id de l'opération
        /// </summary>
        public int OperationId
        {
            get => this._operationId;
            set
            {
                if (this._operationId != value)
                {
                    this._operationId = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Définit le titre (libellé) de l'opération
        /// </summary>
        public string? OperationTitle
        {
            get => this._operationTitle;
            set
            {
                if (this._operationTitle != value)
                {
                    this._operationTitle = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Définit la date sélectionnée par l'utilisateur (l'utilisateur ajoute une opération à une date donnée)
        /// </summary>
        public DateTime OperationDate
        {
            get => this._operationDate;
            set
            {
                if (this._operationDate != value)
                {
                    this._operationDate = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Définit le montant de l'opération
        /// </summary>
        public double OperationAmount
        {
            get => this._operationAmount;
            set
            {
                if (this._operationAmount != value)
                {
                    this._operationAmount = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Définit la catégorie d'une opération (catégories personnalisables)
        /// </summary>
        public string? OperationCategory
        {
            get => this._operationCategory;
            set
            {
                if (this._operationCategory != value)
                {
                    this._operationCategory = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Définit si une opération est récurrente ou non
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
        /// Constructeur de la classe FinancialOperation
        /// </summary>
        /// <param name="title"></param>
        /// <param name="date"></param>
        /// <param name="amount"></param>
        /// <param name="category"></param>
        /// <param name="isRecurrent"></param>
        public FinancialOperation(string? title, DateTime date, double amount, string? category, bool isRecurrent = false)
        {
            this.OperationTitle = title;
            this.OperationDate = date;
            this.OperationAmount = amount;
            this.OperationCategory = category;
            this.IsRecurrent = isRecurrent;
        }

        /// <summary>
        /// Constructeur de la classe FinancialOperation (sans arguments -> Construction de la classe en utilisant les Propriétés dynamiques)
        /// </summary>
        public FinancialOperation() { }

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
