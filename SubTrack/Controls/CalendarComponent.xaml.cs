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
            set
            {
                SetValue(CurrentYearProperty, value);
                SelectedDay = null;
            }
        }

        /// <summary>
        /// Obtient ou définit le mois courant.
        /// </summary>
        public int CurrentMonth
        {
            get => (int)GetValue(CurrentMonthProperty);
            set
            {
                SetValue(CurrentMonthProperty, value);
                SelectedDay = null;
            }
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

        #region Frames
        /// <summary>
        /// Génère un cadre pour afficher un jour de la semaine.
        /// </summary>
        /// <param name="text">Le texte à afficher.</param>
        /// <param name="textColor">La couleur du texte.</param>
        /// <param name="backgroundColor">La couleur de fond.</param>
        /// <param name="cornerRadius">Le rayon des coins.</param>
        /// <returns>Un objet <see cref="Frame"/>.</returns>
        private Frame GenerateDayFrame(string text, Color textColor, Color backgroundColor, double cornerRadius)
        {
            return new Frame
            {
                Content = new Label
                {
                    Text = text,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = textColor,
                    FontSize = 14,
                    FontAttributes = FontAttributes.Bold,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                },
                WidthRequest = 45,
                HeightRequest = 45,
                Padding = 10,
                CornerRadius = (int)cornerRadius,
                BorderColor = Colors.Transparent,
                BackgroundColor = backgroundColor,
                HasShadow = false,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
        }

        /// <summary>
        /// Génère un cadre pour afficher un numéro de jour.
        /// </summary>
        /// <param name="dayNumber">Le numéro du jour.</param>
        /// <param name="actualDay">Indique si c'est le jour actuel.</param>
        /// <returns>Un objet <see cref="Frame"/>.</returns>
        private Frame GenerateDayNumberFrame(int dayNumber, bool actualDay = false)
        {
            Color borderColor = actualDay ? Colors.LightGray : Colors.Transparent;
            Color backgroundColor = dayNumber == SelectedDay ? Colors.Blue : Colors.Transparent;

            var frame = new Frame
            {
                Content = new Label
                {
                    Text = dayNumber.ToString(),
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Colors.White,
                    FontSize = 16,
                    FontAttributes = FontAttributes.Bold,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                },
                WidthRequest = 45,
                HeightRequest = 45,
                Padding = 10,
                CornerRadius = 50,
                BorderColor = borderColor,
                BackgroundColor = backgroundColor,
                HasShadow = false,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                if (SelectedDay == dayNumber)
                {
                    SelectedDay = null;
                }
                else
                {
                    SelectedDay = dayNumber;
                }
            };
            frame.GestureRecognizers.Add(tapGestureRecognizer);

            return frame;
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

            for (int i = 0; i < 7; i++)
            {
                CalendarGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            int rows = GetNumberOfRowsForMonth(CurrentMonth, CurrentYear);
            for (int i = 0; i < rows; i++)
            {
                CalendarGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            string[] days = { "Lun", "Mar", "Mer", "Jeu", "Ven", "Sam", "Dim" };
            for (int i = 0; i < 7; i++)
            {
                Frame dayFrame = GenerateDayFrame(days[i], Colors.White, Colors.Transparent, 0);
                CalendarGrid.Children.Add(dayFrame);
                Grid.SetColumn(dayFrame, i);
                Grid.SetRow(dayFrame, 0);
            }

            var firstDayOfMonth = new DateTime(CurrentYear, CurrentMonth, 1);
            var startDayOfWeek = (int)firstDayOfMonth.DayOfWeek == 0 ? 6 : (int)firstDayOfMonth.DayOfWeek - 1;

            int daysInMonth = DateTime.DaysInMonth(CurrentYear, CurrentMonth);

            for (int i = 0; i < daysInMonth; i++)
            {
                bool isActualDay = i + 1 == DateTime.Now.Day && CurrentMonth == DateTime.Now.Month && CurrentYear == DateTime.Now.Year;
                Frame dayNumberFrame = GenerateDayNumberFrame(i + 1, isActualDay);

                int row = (i + startDayOfWeek) / 7 + 1;
                int column = (i + startDayOfWeek) % 7;

                CalendarGrid.Children.Add(dayNumberFrame);
                Grid.SetColumn(dayNumberFrame, column);
                Grid.SetRow(dayNumberFrame, row);
            }
        }

        /// <summary>
        /// Obtient le nombre de jours dans un mois donné.
        /// </summary>
        /// <param name="month">Le mois.</param>
        /// <param name="year">L'année.</param>
        /// <returns>Le nombre de jours dans le mois.</returns>
        private int GetNumberOfDaysInMonth(int month, int year)
        {
            return DateTime.DaysInMonth(year, month);
        }

        /// <summary>
        /// Obtient le nombre de lignes nécessaires pour afficher le mois.
        /// </summary>
        /// <param name="month">Le mois.</param>
        /// <param name="year">L'année.</param>
        /// <returns>Le nombre de lignes nécessaires.</returns>
        private int GetNumberOfRowsForMonth(int month, int year)
        {
            var daysInMonth = GetNumberOfDaysInMonth(month, year);
            var firstDayOfMonth = new DateTime(year, month, 1);
            var startDayOfWeek = (int)firstDayOfMonth.DayOfWeek == 0 ? 6 : (int)firstDayOfMonth.DayOfWeek - 1;

            return (int)Math.Ceiling((daysInMonth + startDayOfWeek) / 7.0) + 1;
        }
        #endregion
    }
}