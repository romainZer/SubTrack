using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SubTrack.ViewModels
{
    /// <summary>
    /// ViewModel pour le calendrier, gère l'année et le mois courants.
    /// </summary>
    public class CalendarItemViewModel : INotifyPropertyChanged
    {
        #region Attributes
        private int _currentYear;
        private int _currentMonth;
        private string? _currentMonthName;
        private List<string> _months = new List<string>
        {
            "Janvier", "Février", "Mars", "Avril", "Mai", "Juin",
            "Juillet", "Août", "Septembre", "Octobre", "Novembre", "Décembre"
        };
        #endregion

        #region Properties
        /// <summary>
        /// Obtient ou définit l'année courante.
        /// </summary>
        public int CurrentYear
        {
            get => _currentYear;
            set
            {
                if (_currentYear != value)
                {
                    _currentYear = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Obtient ou définit le mois courant.
        /// </summary>
        public int CurrentMonth
        {
            get => _currentMonth;
            set
            {
                if (_currentMonth != value)
                {
                    _currentMonth = value;
                    CurrentMonthName = _months[_currentMonth - 1]; // Met à jour le nom du mois
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Obtient le nom du mois courant.
        /// </summary>
        public string? CurrentMonthName
        {
            get => _currentMonthName;
            private set
            {
                if (_currentMonthName != value)
                {
                    _currentMonthName = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Commande pour passer au mois précédent.
        /// </summary>
        public ICommand PreviousMonthCommand { get; }

        /// <summary>
        /// Commande pour passer au mois suivant.
        /// </summary>
        public ICommand NextMonthCommand { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="CalendarItemViewModel"/>.
        /// </summary>
        public CalendarItemViewModel()
        {
            var now = DateTime.Now;
            CurrentYear = now.Year;
            CurrentMonth = now.Month;

            // Initialise les commandes
            PreviousMonthCommand = new Command(OnPreviousMonth);
            NextMonthCommand = new Command(OnNextMonth);
        }
        #endregion

        #region Command Handlers
        /// <summary>
        /// Méthode appelée lors du clic sur le bouton précédent.
        /// </summary>
        private void OnPreviousMonth()
        {
            if (CurrentMonth == 1)
            {
                CurrentMonth = 12;
                CurrentYear--;
            }
            else
            {
                CurrentMonth--;
            }
        }

        /// <summary>
        /// Méthode appelée lors du clic sur le bouton suivant.
        /// </summary>
        private void OnNextMonth()
        {
            if (CurrentMonth == 12)
            {
                CurrentMonth = 1;
                CurrentYear++;
            }
            else
            {
                CurrentMonth++;
            }
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