namespace SubTrack.Controls;

public partial class CalendarComponent : ContentView
{
    private readonly List<string> _days = new() { "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi", "Dimanche" };
    private int _currentYear;
    private int _currentMonth;

    public CalendarComponent()
    {
        InitializeComponent();
        SetCurrentDate();
        GenerateCalendar();
    }

    private void SetCurrentDate()
    {
        var now = DateTime.Now;
        _currentYear = now.Year;
        _currentMonth = now.Month;
    }

    private int GetNumberOfDaysInMonth(int month, int year)
    {
        return DateTime.DaysInMonth(year, month);
    }

    private int GetNumberOfRowsForMonth(int month, int year)
    {
        var daysInMonth = GetNumberOfDaysInMonth(month, year);
        var firstDayOfMonth = new DateTime(year, month, 1);
        var startDayOfWeek = (int)firstDayOfMonth.DayOfWeek;
        startDayOfWeek = startDayOfWeek == 0 ? 6 : startDayOfWeek - 1;

        return (int)Math.Ceiling((daysInMonth + startDayOfWeek) / 7.0) + 1;
    }

    private void GenerateCalendar()
    {
        // Supprime le contenu actuel avant de regénérer le calendrier
        this.Content = null;

        var calendarGrid = CreateCalendarGrid();
        AddDayLabels(calendarGrid);
        AddDayNumbers(calendarGrid);

        var navigationButtons = CreateNavigationButtons();
        var mainLayout = new StackLayout { Children = { navigationButtons, calendarGrid } };

        this.Content = mainLayout;
    }

    private Grid CreateCalendarGrid()
    {
        var grid = new Grid();

        for (int i = 0; i < 7; i++)
        {
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        }

        for (int i = 0; i < GetNumberOfRowsForMonth(_currentMonth, _currentYear); i++)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        }

        return grid;
    }

    private void AddDayLabels(Grid calendarGrid)
    {
        for (int i = 0; i < _days.Count; i++)
        {
            var dayLabel = new Label
            {
                Text = _days[i].Substring(0, 3),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                BackgroundColor = Colors.Blue,
                TextColor = Colors.White
            };

            calendarGrid.Children.Add(dayLabel);
            Grid.SetColumn(dayLabel, i);
            Grid.SetRow(dayLabel, 0);
        }
    }

    private void AddDayNumbers(Grid calendarGrid)
    {
        var firstDayOfMonth = new DateTime(_currentYear, _currentMonth, 1);
        var startDayOfWeek = (int)firstDayOfMonth.DayOfWeek;
        startDayOfWeek = startDayOfWeek == 0 ? 6 : startDayOfWeek - 1;

        var daysInMonth = GetNumberOfDaysInMonth(_currentMonth, _currentYear);

        for (int i = 0; i < daysInMonth; i++)
        {
            var dayNumber = new Label
            {
                Text = (i + 1).ToString(),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                BackgroundColor = Colors.Red,
                TextColor = Colors.White
            };

            int row = (i + startDayOfWeek) / 7 + 1;
            int column = (i + startDayOfWeek) % 7;

            calendarGrid.Children.Add(dayNumber);
            Grid.SetColumn(dayNumber, column);
            Grid.SetRow(dayNumber, row);
        }
    }

    private StackLayout CreateNavigationButtons()
    {
        var previousButton = new Button
        {
            Text = "Précédent",
            HorizontalOptions = LayoutOptions.Start
        };
        previousButton.Clicked += OnPreviousButtonClicked;

        var nextButton = new Button
        {
            Text = "Suivant",
            HorizontalOptions = LayoutOptions.End
        };
        nextButton.Clicked += OnNextButtonClicked;

        return new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            Children = { previousButton, nextButton }
        };
    }

    private void OnPreviousButtonClicked(object sender, EventArgs e)
    {
        if (_currentMonth == 1)
        {
            _currentMonth = 12;
            _currentYear--;
        }
        else
        {
            _currentMonth--;
        }
        GenerateCalendar();
    }

    private void OnNextButtonClicked(object sender, EventArgs e)
    {
        if (_currentMonth == 12)
        {
            _currentMonth = 1;
            _currentYear++;
        }
        else
        {
            _currentMonth++;
        }
        GenerateCalendar();
    }
}