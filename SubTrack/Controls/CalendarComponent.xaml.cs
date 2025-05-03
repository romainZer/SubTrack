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
        #region Static Fields
        private static readonly string[] WeekDayNames = { "Lun", "Mar", "Mer", "Jeu", "Ven", "Sam", "Dim" };
        #endregion

        #region Bindable Properties
        public static readonly BindableProperty CurrentYearProperty =
            BindableProperty.Create(nameof(CurrentYear), typeof(int), typeof(CalendarComponent), DateTime.Now.Year, propertyChanged: OnDateChanged);

        public static readonly BindableProperty CurrentMonthProperty =
            BindableProperty.Create(nameof(CurrentMonth), typeof(int), typeof(CalendarComponent), DateTime.Now.Month, propertyChanged: OnMonthChanged);

        public static readonly BindableProperty SelectedDayProperty =
            BindableProperty.Create(nameof(SelectedDay), typeof(int?), typeof(CalendarComponent), null, propertyChanged: OnSelectedDayChanged);
        #endregion

        #region Properties
        public int CurrentYear
        {
            get => (int)GetValue(CurrentYearProperty);
            set => SetValue(CurrentYearProperty, value);
        }

        public int CurrentMonth
        {
            get => (int)GetValue(CurrentMonthProperty);
            set => SetValue(CurrentMonthProperty, value);
        }

        public int? SelectedDay
        {
            get => (int?)GetValue(SelectedDayProperty);
            set => SetValue(SelectedDayProperty, value);
        }
        #endregion

        #region Fields
        // Pool de boutons pour réutilisation (31 boutons maximum)
        private readonly List<Button> _dayButtonPool = new List<Button>();
        #endregion

        #region Constructors
        public CalendarComponent()
        {
            InitializeComponent();
            BindingContext = new CalendarItemViewModel();

            // Initialiser le pool de 31 boutons
            for (int i = 0; i < 31; i++)
            {
                var btn = new Button
                {
                    FontSize = 14,
                    CornerRadius = 6,
                    Padding = 4,
                    WidthRequest = 45,
                    HeightRequest = 45
                };
                btn.Clicked += OnDayButtonClicked;
                _dayButtonPool.Add(btn);
            }

            GenerateCalendar();
        }
        #endregion

        #region Methods
        private static void OnDateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CalendarComponent control)
            {
                control.GenerateCalendar();
            }
        }

        private static void OnMonthChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CalendarComponent control)
            {
                if (!Equals(oldValue, newValue))
                {
                    control.SelectedDay = null;
                }
                control.GenerateCalendar();
            }
        }

        private static void OnSelectedDayChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CalendarComponent control)
            {
                control.GenerateCalendar();
            }
        }

        private void OnDayButtonClicked(object? sender, EventArgs e)
        {
            if (sender is Button btn && btn.BindingContext is int day)
            {
                SelectedDay = SelectedDay == day ? null : day;
            }
        }

        private void GenerateCalendar()
        {
            // Réinitialisation de la grille
            CalendarGrid.Children.Clear();
            CalendarGrid.ColumnDefinitions.Clear();
            CalendarGrid.RowDefinitions.Clear();

            // Création de 7 colonnes
            for (int i = 0; i < 7; i++)
            {
                CalendarGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            // Calcul du nombre de lignes (première ligne pour les entêtes)
            int rows = GetNumberOfRowsForMonth(CurrentMonth, CurrentYear);
            for (int i = 0; i < rows; i++)
            {
                CalendarGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            // Ajout des entêtes des jours
            for (int i = 0; i < 7; i++)
            {
                var dayLabel = new Label
                {
                    Text = WeekDayNames[i],
                    FontSize = 14,
                    TextColor = Application.Current?.RequestedTheme == AppTheme.Dark ? Colors.White : Color.FromArgb("#666666"),
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                };
                CalendarGrid.Children.Add(dayLabel);
                Grid.SetColumn(dayLabel, i);
                Grid.SetRow(dayLabel, 0);
            }

            // Détermination du décalage pour le premier jour du mois
            var firstDayOfMonth = new DateTime(CurrentYear, CurrentMonth, 1);
            int startDayOfWeek = firstDayOfMonth.DayOfWeek == DayOfWeek.Sunday ? 6 : ((int)firstDayOfMonth.DayOfWeek - 1);
            int daysInMonth = DateTime.DaysInMonth(CurrentYear, CurrentMonth);

            // Remplissage de la grille avec les boutons déjà créés
            for (int i = 0; i < daysInMonth; i++)
            {
                var dayButton = _dayButtonPool[i];
                int dayNumber = i + 1;

                dayButton.Text = dayNumber.ToString();
                dayButton.BindingContext = dayNumber;
                dayButton.BackgroundColor = dayNumber == SelectedDay ? Color.FromArgb("#2596be") : Color.FromArgb("#6E6E6E");
                dayButton.TextColor = dayNumber == SelectedDay
                    ? Colors.White
                    : (Application.Current?.RequestedTheme == AppTheme.Dark ? Colors.White : Color.FromArgb("#333333"));

                int row = (i + startDayOfWeek) / 7 + 1;
                int column = (i + startDayOfWeek) % 7;

                CalendarGrid.Children.Add(dayButton);
                Grid.SetColumn(dayButton, column);
                Grid.SetRow(dayButton, row);
            }
        }

        private int GetNumberOfRowsForMonth(int month, int year)
        {
            int daysInMonth = DateTime.DaysInMonth(year, month);
            var firstDayOfMonth = new DateTime(year, month, 1);
            int startDayOfWeek = firstDayOfMonth.DayOfWeek == DayOfWeek.Sunday ? 6 : ((int)firstDayOfMonth.DayOfWeek - 1);
            return (int)Math.Ceiling((daysInMonth + startDayOfWeek) / 7.0) + 1;
        }
        #endregion
    }
}