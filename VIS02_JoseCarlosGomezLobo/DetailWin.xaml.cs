using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace UltimateSupervisor
{
    public partial class DetailWin : Window
    {
        public DetailWin(string rutaArchivo)
        {
            InitializeComponent();
            CargarArchivo(rutaArchivo);
        }

        private void CargarArchivo(string ruta)
        {
            if (string.IsNullOrEmpty(ruta) || !File.Exists(ruta)) return;

            string extension = Path.GetExtension(ruta).ToLower();

            // Ocultar todos los visores y paneles por defecto
            VisorImagen.Visibility = Visibility.Collapsed;
            VisorMedia.Visibility = Visibility.Collapsed;
            VisorTexto.Visibility = Visibility.Collapsed;
            VisorMarkdown.Visibility = Visibility.Collapsed;
            VisorMarkdownScroll.Visibility = Visibility.Collapsed;
            PanelBotonesImagen.Visibility = Visibility.Collapsed;
            PanelBotonesMedia.Visibility = Visibility.Collapsed;
            PanelBotonesTexto.Visibility = Visibility.Collapsed;
          

            // Mostrar el visor que corresponda según la extensión
            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".gif":
                case ".bmp":
                    VisorImagen.Source = new BitmapImage(new Uri(ruta));
                    VisorImagen.Visibility = Visibility.Visible;
                    PanelBotonesImagen.Visibility = Visibility.Visible;
                    break;

                case ".mp4":
                case ".mp3":
                case ".avi":
                case ".wav":
                    VisorMedia.Source = new Uri(ruta);
                    VisorMedia.Visibility = Visibility.Visible;
                    PanelBotonesMedia.Visibility = Visibility.Visible;
                    VisorMedia.Play();
                    break;

                case ".txt":
                    VisorTexto.Text = File.ReadAllText(ruta);
                    VisorTexto.Visibility = Visibility.Visible;
                    PanelBotonesTexto.Visibility = Visibility.Visible;
                    break;

                case ".md":
                    VisorMarkdown.Markdown = File.ReadAllText(ruta);
                    VisorMarkdown.Visibility = Visibility.Visible;
                    VisorMarkdownScroll.Visibility = Visibility.Visible;
                    break;

                default:
                    System.Windows.MessageBox.Show("Formato no soportado por el visor interno.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
            }
        }

        // --- CONTROLES DE IMAGEN ---
        private void BtnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            TransformacionZoom.ScaleX += 0.2;
            TransformacionZoom.ScaleY += 0.2;
        }

        private void BtnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (TransformacionZoom.ScaleX > 0.4)
            {
                TransformacionZoom.ScaleX -= 0.2;
                TransformacionZoom.ScaleY -= 0.2;
            }
        }

        // --- CONTROLES MULTIMEDIA ---
        private void BtnPlay_Click(object sender, RoutedEventArgs e) => VisorMedia.Play();
        private void BtnPause_Click(object sender, RoutedEventArgs e) => VisorMedia.Pause();

        // --- CONTROLES DE TEXTO Y MARKDOWN ---
        private void BtnFontUp_Click(object sender, RoutedEventArgs e)
        {
            VisorTexto.FontSize += 2;
        }

        private void BtnFontDown_Click(object sender, RoutedEventArgs e)
        {
            if (VisorTexto.FontSize > 8)
                VisorTexto.FontSize -= 2;
        }
        // --- CERRAR VENTANA ---
        private void BtnCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (VisorMedia.Visibility == Visibility.Visible)
            {
                VisorMedia.Stop();
                VisorMedia.Close();
            }
        }
    }
}