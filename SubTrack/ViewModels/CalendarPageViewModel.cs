using System;
using System.Collections.Generic;
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
        private CalendarItemViewModel CalendarViewModel { get; set; }
        /// <summary>
        /// Obtient ou définit un ExpenseItemViewModel
        /// </summary>
        private ExpenseItemViewModel ExpenseItemViewModel { get; set; }
        #endregion

        #region Constructors 
        /// <summary>
        /// Constructeur du ViewModel de la Page CalendarPage
        /// </summary>
        public CalendarPageViewModel()
        {
            this.CalendarViewModel = new CalendarItemViewModel();
            this.ExpenseItemViewModel = new ExpenseItemViewModel();
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
