using System.Globalization;
using System.Text;

namespace Tarea
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnAgeTextChanged(object? sender, TextChangedEventArgs e)
        {
            if (sender is not Entry entry)
            {
                return;
            }

            var sanitizedValue = new string((e.NewTextValue ?? string.Empty).Where(char.IsAsciiDigit).ToArray());
            if (sanitizedValue != e.NewTextValue)
            {
                entry.Text = sanitizedValue;
            }
        }

        private void OnSalaryTextChanged(object? sender, TextChangedEventArgs e)
        {
            if (sender is not Entry entry)
            {
                return;
            }

            var decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            var sanitizedValue = new StringBuilder();
            var hasDecimalSeparator = false;

            foreach (var character in e.NewTextValue ?? string.Empty)
            {
                if (char.IsAsciiDigit(character))
                {
                    sanitizedValue.Append(character);
                }
                else if ((character is '.' or ',') && !hasDecimalSeparator)
                {
                    sanitizedValue.Append(decimalSeparator);
                    hasDecimalSeparator = true;
                }
            }

            var result = sanitizedValue.ToString();
            if (result != e.NewTextValue)
            {
                entry.Text = result;
            }
        }

        private async void OnCalculateClicked(object? sender, EventArgs e)
        {
            var name = NameEntry.Text?.Trim();
            var lastName = LastNameEntry.Text?.Trim();
            var ageText = AgeEntry.Text?.Trim();
            var salaryText = SalaryEntry.Text?.Trim();

            if (string.IsNullOrWhiteSpace(name)
                || string.IsNullOrWhiteSpace(lastName)
                || string.IsNullOrWhiteSpace(ageText)
                || string.IsNullOrWhiteSpace(salaryText))
            {
                await DisplayAlert("Datos incompletos", "Completa todos los campos antes de calcular.", "Aceptar");
                return;
            }

            if (!int.TryParse(ageText, NumberStyles.None, CultureInfo.InvariantCulture, out var age) || age is < 1 or > 120)
            {
                await DisplayAlert("Edad no válida", "La edad debe ser un número entero entre 1 y 120.", "Aceptar");
                return;
            }

            if (!decimal.TryParse(salaryText, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out var salary) || salary <= 0)
            {
                await DisplayAlert("Salario no válido", "Ingresa un salario numérico mayor que cero.", "Aceptar");
                return;
            }

            var contribution = salary * 0.0945m;
            var message = $"Bienvenido {name} {lastName}\nTienes {age} años\nTu aporte mensual es ${contribution.ToString("F2", CultureInfo.InvariantCulture)}";

            await DisplayAlert("Resultado del cálculo", message, "Aceptar");
        }
    }
}
