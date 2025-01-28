using Microsoft.Maui.Controls;
using SubTrack.ViewModels;

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
        #endregion

        #region Constructors
        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="CalendarComponent"/>.
        /// </summary>
        public CalendarComponent()
        {
            InitializeComponent();
            BindingContext = new CalendarViewModel(); // Lie la View au ViewModel
            GenerateCalendar();
        }
        #endregion

        #region Frames
        /// <summary>
        /// Génère un cadre pour afficher le nom du jour.
        /// </summary>
        /// <param name="text">Le texte à afficher dans le cadre.</param>
        /// <returns>Un objet <see cref="Frame"/> contenant le nom du jour.</returns>
        private Frame GenerateDayFrame(string text)
        {
            return new Frame
            {
                Content = new Label
                {
                    Text = text,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Colors.White,
                    FontSize = 14,
                    FontAttributes = FontAttributes.Bold,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                },

                WidthRequest = 45,
                HeightRequest = 45,
                Padding = 10,
                CornerRadius = 0,
                BorderColor = Colors.Transparent,
                BackgroundColor = Colors.Transparent,
                HasShadow = false,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
        }

        /// <summary>
        /// Génère un cadre pour afficher le numéro du jour.
        /// </summary>
        /// <param name="number">Le numéro du jour à afficher.</param>
        /// <param name="actualDay">Indique si le jour est le jour actuel.</param>
        /// <returns>Un objet <see cref="Frame"/> contenant le numéro du jour.</returns>
        private Frame GenerateDayNumberFrame(int number, bool actualDay = false)
        {
            Color borderColor = actualDay ? Colors.LightGray : Colors.Transparent;

            return new Frame
            {
                Content = new Label
                {
                    Text = number.ToString(),
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
                BackgroundColor = Colors.Transparent,
                HasShadow = false,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
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
            var control = (CalendarComponent)bindable;
            control.GenerateCalendar();
        }

        /// <summary>
        /// Génère le calendrier pour le mois et l'année courants.
        /// </summary>
        private void GenerateCalendar()
        {
            // Réinitialise la grille existante
            CalendarGrid.Children.Clear();
            CalendarGrid.ColumnDefinitions.Clear();
            CalendarGrid.RowDefinitions.Clear();

            // Ajoute les colonnes (7 jours)
            for (int i = 0; i < 7; i++)
            {
                CalendarGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            // Calcule le nombre de lignes nécessaires
            int rows = GetNumberOfRowsForMonth(CurrentMonth, CurrentYear);
            for (int i = 0; i < rows; i++)
            {
                CalendarGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            // Ajoute les labels des jours (Lun, Mar, etc.) avec dimanche en dernier
            string[] days = { "Lun", "Mar", "Mer", "Jeu", "Ven", "Sam", "Dim" };
            for (int i = 0; i < 7; i++)
            {
                Frame dayFrame = GenerateDayFrame(days[i]);
                CalendarGrid.Children.Add(dayFrame);
                Grid.SetColumn(dayFrame, i);
                Grid.SetRow(dayFrame, 0);
            }

            // Ajoute les numéros des jours
            var firstDayOfMonth = new DateTime(CurrentYear, CurrentMonth, 1);
            var startDayOfWeek = (int)firstDayOfMonth.DayOfWeek;
            startDayOfWeek = startDayOfWeek == 0 ? 6 : startDayOfWeek - 1; // Dimanche est maintenant dernier

            int daysInMonth = GetNumberOfDaysInMonth(CurrentMonth, CurrentYear);

            for (int i = 0; i < daysInMonth; i++)
            {
                bool isActualDay = i + 1 == DateTime.Now.Day && CurrentMonth == DateTime.Now.Month && CurrentYear == DateTime.Now.Year;
                Frame dayNumberFrame = GenerateDayNumberFrame(i + 1, isActualDay); // Si jour actuel, on entoure

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
        /// Obtient le nombre de lignes nécessaires pour afficher un mois donné.
        /// </summary>
        /// <param name="month">Le mois.</param>
        /// <param name="year">L'année.</param>
        /// <returns>Le nombre de lignes nécessaires.</returns>
        private int GetNumberOfRowsForMonth(int month, int year)
        {
            var daysInMonth = GetNumberOfDaysInMonth(month, year);
            var firstDayOfMonth = new DateTime(year, month, 1);
            var startDayOfWeek = (int)firstDayOfMonth.DayOfWeek;
            startDayOfWeek = startDayOfWeek == 0 ? 6 : startDayOfWeek - 1;

            return (int)Math.Ceiling((daysInMonth + startDayOfWeek) / 7.0) + 1;
        }
        #endregion
    }
}