using Microsoft.Maui.Controls;
using SubTrack.ViewModels;
using System;

namespace SubTrack.Controls
{
    /// <summary>
    /// Composant de calendrier personnalisé pour afficher les jours et les mois.
    /// </summary>
    public partial class CalendarComponent : ContentView
    {
        #region Bindable Properties
        /// <summary>
        /// Propriété bindable pour l'année courante.
        /// </summary>
        public static readonly BindableProperty CurrentYearProperty =
            BindableProperty.Create(nameof(CurrentYear), typeof(int), typeof(CalendarComponent), DateTime.Now.Year, propertyChanged: OnDateChanged);

        /// <summary>
        /// Propriété bindable pour le mois courant.
        /// </summary>
        public static readonly BindableProperty CurrentMonthProperty =
            BindableProperty.Create(nameof(CurrentMonth), typeof(int), typeof(CalendarComponent), DateTime.Now.Month, propertyChanged: OnDateChanged);

        /// <summary>
        /// Propriété bindable pour le jour sélectionné.
        /// </summary>
        public static readonly BindableProperty SelectedDayProperty =
            BindableProperty.Create(nameof(SelectedDay), typeof(int?), typeof(CalendarComponent), null, propertyChanged: OnSelectedDayChanged);
        #endregion

        #region Properties
        /// <summary>
        /// Obtient ou définit l'année courante.
        /// </summary>
        public int CurrentYear
        {
            get => (int)GetValue(CurrentYearProperty);
            set => SetValue(CurrentYearProperty, value);
        }

        /// <summary>
        /// Obtient ou définit le mois courant.
        /// </summary>
        public int CurrentMonth
        {
            get => (int)GetValue(CurrentMonthProperty);
            set => SetValue(CurrentMonthProperty, value);
        }

        /// <summary>
        /// Obtient ou définit le jour sélectionné.
        /// </summary>
        public int? SelectedDay
        {
            get => (int?)GetValue(SelectedDayProperty);
            set => SetValue(SelectedDayProperty, value);
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="CalendarComponent"/>.
        /// </summary>
        public CalendarComponent()
        {
            InitializeComponent();
            BindingContext = new CalendarItemViewModel();
            GenerateCalendar();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Méthode appelée lorsque la date change.
        /// </summary>
        /// <param name="bindable">L'objet bindable.</param>
        /// <param name="oldValue">L'ancienne valeur.</param>
        /// <param name="newValue">La nouvelle valeur.</param>
        private static void OnDateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CalendarComponent control)
            {
                control.GenerateCalendar();
            }
        }

        /// <summary>
        /// Méthode appelée lorsque le jour sélectionné change.
        /// </summary>
        /// <param name="bindable">L'objet bindable.</param>
        /// <param name="oldValue">L'ancienne valeur.</param>
        /// <param name="newValue">La nouvelle valeur.</param>
        private static void OnSelectedDayChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CalendarComponent control)
            {
                control.GenerateCalendar();
            }
        }

        /// <summary>
        /// Génère le calendrier pour le mois et l'année courants.
        /// </summary>
        private void GenerateCalendar()
        {
            CalendarGrid.Children.Clear();
            CalendarGrid.ColumnDefinitions.Clear();
            CalendarGrid.RowDefinitions.Clear();

            // Ajouter 7 colonnes pour les jours de la semaine
            for (int i = 0; i < 7; i++)
            {
                CalendarGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            // Ajouter les lignes nécessaires
            int rows = GetNumberOfRowsForMonth(CurrentMonth, CurrentYear);
            for (int i = 0; i < rows; i++)
            {
                CalendarGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            // En-têtes des jours de la semaine
            string[] days = { "Lun", "Mar", "Mer", "Jeu", "Ven", "Sam", "Dim" };
            for (int i = 0; i < 7; i++)
            {
                var dayLabel = new Label
                {
                    Text = days[i],
                    FontSize = 14,
                    TextColor = Application.Current?.RequestedTheme == AppTheme.Dark ? Colors.White : Color.FromArgb("#666666"),
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };
                CalendarGrid.Children.Add(dayLabel);
                Grid.SetColumn(dayLabel, i);
                Grid.SetRow(dayLabel, 0);
            }

            // Remplir les jours du mois
            var firstDayOfMonth = new DateTime(CurrentYear, CurrentMonth, 1);
            var startDayOfWeek = (int)firstDayOfMonth.DayOfWeek == 0 ? 6 : (int)firstDayOfMonth.DayOfWeek - 1;
            int daysInMonth = DateTime.DaysInMonth(CurrentYear, CurrentMonth);

            for (int i = 0; i < daysInMonth; i++)
            {
                bool isActualDay = i + 1 == DateTime.Now.Day && CurrentMonth == DateTime.Now.Month && CurrentYear == DateTime.Now.Year;
                var dayButton = new Button
                {
                    Text = (i + 1).ToString(),
                    FontSize = 14,
                    BackgroundColor = i + 1 == SelectedDay ? Color.FromArgb("#2596be") : Colors.Transparent,
                    TextColor = i + 1 == SelectedDay ? Colors.White : (Application.Current.RequestedTheme == AppTheme.Dark ? Colors.White : Color.FromArgb("#333333")), // Texte blanc en mode sombre
                    CornerRadius = 20,
                    Padding = 0,
                    WidthRequest = 35,
                    HeightRequest = 35
                };

                int dayNumber = i + 1;
                dayButton.Clicked += (s, e) => SelectedDay = SelectedDay == dayNumber ? null : dayNumber;

                int row = (i + startDayOfWeek) / 7 + 1;
                int column = (i + startDayOfWeek) % 7;

                CalendarGrid.Children.Add(dayButton);
                Grid.SetColumn(dayButton, column);
                Grid.SetRow(dayButton, row);
            }
        }

        private int GetNumberOfRowsForMonth(int month, int year)
        {
            var daysInMonth = DateTime.DaysInMonth(year, month);
            var firstDayOfMonth = new DateTime(year, month, 1);
            var startDayOfWeek = (int)firstDayOfMonth.DayOfWeek == 0 ? 6 : (int)firstDayOfMonth.DayOfWeek - 1;

            return (int)Math.Ceiling((daysInMonth + startDayOfWeek) / 7.0) + 1;
        }
        #endregion
    }
}