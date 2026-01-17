using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpensesApp.Core.Controllers;
using ExpensesApp.Core.Models;
using ExpensesApp.MAUI.Views;
using FilterMode = ExpensesApp.MAUI.Views.FilterMode;

namespace ExpensesApp.MAUI.PageModels;

public partial class SpendingPageModel : ObservableObject
{
    private readonly ExpenseController _expenseController;
    private List<Expense> _allExpensesCache = new();

    [ObservableProperty] private ObservableCollection<int> _availableYears = new();
    [ObservableProperty] private ObservableCollection<int> _availableMonths = new();
    [ObservableProperty] private int _selectedYear;
    [ObservableProperty] private int _selectedMonth;

    public SpendingPageModel(ExpenseController expenseController)
    {
        _expenseController = expenseController;
        FilterMode = FilterMode.Month;
        SelectedDate = DateTime.Now;
    }

    [ObservableProperty] private FilterMode _filterMode;


    [ObservableProperty] private DateTime _selectedDate;

    [ObservableProperty] private ObservableCollection<Expense> _expenses = new();

    [ObservableProperty] private bool _isDayViewVisible = false;
    [ObservableProperty] private bool _isMonthViewVisible = false;
    [ObservableProperty] private bool _isYearViewVisible = false;
    public List<FilterMode> FilterModes => Enum.GetValues(typeof(FilterMode)).Cast<FilterMode>().ToList();

    public async Task LoadExpensesAsync()
    {
        try
        {
            var list = await _expenseController.GetExpensesAsync();
            _allExpensesCache = list.ToList();
            InitilizeFilters();
            ApplyFilter();
        }
        catch (Exception ex)
        {
            Expenses = new ObservableCollection<Expense>();
        }
    }

    public void InitilizeFilters()
    {
        // Get unique years and months from expenses
        var years = _allExpensesCache
            .Select(e => e.Date.Year)
            .Distinct()
            .OrderBy(y => y)
            .ToList();

        var months = _allExpensesCache
            .Select(e => e.Date.Month)
            .Distinct()
            .OrderBy(m => m)
            .ToList();

        AvailableYears = new ObservableCollection<int>(years);
        //AvailableMonths = new ObservableCollection<int>([1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]);

        if (years.Any()) SelectedYear = years.Last();
        //if (months.Any()) SelectedMonth = months.Last();
    }

    public void UpdateMonthsForYear(int year)
    {
        if (_allExpensesCache == null) return;

        var filtered = _allExpensesCache
            .Where(e => e.Date.Year == year)
            .Select(e => e.Date.Month)
            .Distinct()
            .OrderBy(m => m)
            .ToList();

        AvailableMonths.Clear();

        AvailableMonths = new ObservableCollection<int>(filtered);

        if (AvailableMonths.Any() && !AvailableMonths.Contains(SelectedMonth))
        {
            SelectedMonth = AvailableMonths.Last();
        }
        // Optional: If the new year has NO data at all, maybe reset to 0 or 1
        else if (!AvailableMonths.Any())
        {
            SelectedMonth = 1;
        }
    }

    [RelayCommand]
    public void ApplyFilter()
    {
        if (_allExpensesCache == null) return;

        IEnumerable<Expense> filtered = _allExpensesCache;

        switch (FilterMode)
        {
            case FilterMode.Year:
                filtered = filtered.Where(e => e.Date.Year == SelectedYear);
                break;
            case FilterMode.Month:
                filtered = filtered.Where(e => e.Date.Year == SelectedYear &&
                                               e.Date.Month == SelectedMonth);
                break;
            case FilterMode.Day:
                filtered = filtered.Where(e => e.Date.Date == SelectedDate.Date);
                break;
        }

        Expenses = new ObservableCollection<Expense>(filtered);
    }

    partial void OnFilterModeChanged(FilterMode value)
    {
        IsDayViewVisible = value == FilterMode.Day;
        IsMonthViewVisible = value == FilterMode.Month;
        IsYearViewVisible = value == FilterMode.Year;
        SyncChartDate();
        ApplyFilter();
    }

    partial void OnSelectedYearChanged(int value) 
    {
        UpdateMonthsForYear(value);
        SyncChartDate();
        ApplyFilter();
    }

    partial void OnSelectedMonthChanged(int value) 
    {
        SyncChartDate();
        ApplyFilter();
    }
    
    partial void OnSelectedDateChanged(DateTime value)
    {
        // Reverse Sync: If using DatePicker (Day Mode), update dropdowns to match
        if (FilterMode == FilterMode.Day)
        {
            if (SelectedYear != value.Year) SelectedYear = value.Year;
            if (SelectedMonth != value.Month) SelectedMonth = value.Month;
        }
        ApplyFilter();
    }
    
    private void SyncChartDate()
    {
        int y = SelectedYear > 0 ? SelectedYear : DateTime.Now.Year;
        int m = SelectedMonth > 0 ? SelectedMonth : DateTime.Now.Month;

        if (SelectedDate.Year != y || SelectedMonth != m)
        {
            SelectedDate = new DateTime(y, m, 1);
        }
    }
}