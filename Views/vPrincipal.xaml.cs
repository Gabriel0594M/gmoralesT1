using System.IO;
using System.Text.RegularExpressions;

namespace gmoralesS1.Views;

public partial class vPrincipal : ContentPage
{
    private static readonly Regex SoloLetras = new(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$");
    private static readonly Regex SoloNumeros = new(@"^[0-9]+$");

	public vPrincipal()
	{
		InitializeComponent();
	}

    private async void btnEnviar_Clicked(object sender, EventArgs e)
    {
        if (pkrIdentificacion.SelectedItem is null ||
            string.IsNullOrWhiteSpace(entApellidoPaterno.Text) ||
            string.IsNullOrWhiteSpace(entApellidoMaterno.Text) ||
            string.IsNullOrWhiteSpace(entNombres.Text) ||
            string.IsNullOrWhiteSpace(entTelefono.Text) ||
            string.IsNullOrWhiteSpace(entEmail.Text) ||
            string.IsNullOrWhiteSpace(entConfirmarEmail.Text) ||
            pkrCarrera.SelectedItem is null ||
            string.IsNullOrWhiteSpace(entModalidad.Text) ||
            string.IsNullOrWhiteSpace(entCampus.Text))
        {
            await DisplayAlertAsync("Campos incompletos", "Por favor completa todos los campos obligatorios.", "OK");
            return;
        }

        if (!SoloLetras.IsMatch(entApellidoPaterno.Text) ||
            !SoloLetras.IsMatch(entApellidoMaterno.Text) ||
            !SoloLetras.IsMatch(entNombres.Text) ||
            !SoloLetras.IsMatch(entModalidad.Text) ||
            !SoloLetras.IsMatch(entCampus.Text))
        {
            await DisplayAlertAsync("Dato inválido", "Apellidos, nombres, modalidad y campus solo deben contener letras.", "OK");
            return;
        }

        if (!SoloNumeros.IsMatch(entTelefono.Text))
        {
            await DisplayAlertAsync("Dato inválido", "El teléfono solo debe contener números.", "OK");
            return;
        }

        if (entEmail.Text != entConfirmarEmail.Text)
        {
            await DisplayAlertAsync("Correo inválido", "El correo electrónico y su confirmación no coinciden.", "OK");
            return;
        }

        string resumen =
            $"Nombre: {entNombres.Text} {entApellidoPaterno.Text} {entApellidoMaterno.Text}\n" +
            $"Identificación: {pkrIdentificacion.SelectedItem}\n" +
            $"Teléfono: {entTelefono.Text}\n" +
            $"Correo: {entEmail.Text}\n" +
            $"Carrera: {pkrCarrera.SelectedItem}\n" +
            $"Modalidad: {entModalidad.Text}\n" +
            $"Campus: {entCampus.Text}";

        string nombreArchivo = $"Inscripcion_{entNombres.Text}_{entApellidoPaterno.Text}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
        string rutaArchivo = Path.Combine(FileSystem.AppDataDirectory, nombreArchivo);

        try
        {
            await File.WriteAllTextAsync(rutaArchivo, resumen);
            lblRutaArchivo.Text = $"Archivo guardado en: {rutaArchivo}";
            await DisplayAlertAsync("Inscripción enviada", resumen, "OK");
        }
        catch (Exception ex)
        {
            lblRutaArchivo.Text = string.Empty;
            await DisplayAlertAsync("Error al guardar", $"No se pudo guardar el archivo:\n{ex.Message}", "OK");
        }
    }

    private void btnLimpiar_Clicked(object sender, EventArgs e)
    {
        pkrIdentificacion.SelectedItem = null;
        entApellidoPaterno.Text = string.Empty;
        entApellidoMaterno.Text = string.Empty;
        entNombres.Text = string.Empty;
        entTelefono.Text = string.Empty;
        entEmail.Text = string.Empty;
        entConfirmarEmail.Text = string.Empty;
        pkrCarrera.SelectedItem = null;
        entModalidad.Text = string.Empty;
        entCampus.Text = string.Empty;
        lblRutaArchivo.Text = string.Empty;
    }
}
