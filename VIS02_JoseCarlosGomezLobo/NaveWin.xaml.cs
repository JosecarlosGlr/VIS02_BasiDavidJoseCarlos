using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using UltimateSupervisor;

namespace VIS02_JoseCarlosGomezLobo
{
    public partial class NaveWin : Window
    {
        // Variable para guardar la ruta actual
        private string rutaActual = "";

        public NaveWin()
        {
            InitializeComponent();
            // Cargo la carpeta Mis Imágenes por defecto
            CargarCarpetaMisImagenes();
        }

        private void CargarCarpetaMisImagenes()
        {
            rutaActual = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            txtRutaActual.Text = rutaActual;

            LlenarArbol(rutaActual);
        }

        // Evento para cambiar de carpeta
        private void btnCambiarCarpeta_Click(object sender, RoutedEventArgs e)
        {
            var dialogoCarpeta = new System.Windows.Forms.FolderBrowserDialog();
            dialogoCarpeta.Description = "Selecciona una nueva carpeta para explorar";

            // Si el usuario elige una carpeta y pulsa Aceptar
            if (dialogoCarpeta.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Actualizo la variable y el texto de la pantalla
                rutaActual = dialogoCarpeta.SelectedPath;
                txtRutaActual.Text = rutaActual;

                // Cargo el árbol con la nueva ruta
                LlenarArbol(rutaActual);
            }
        }

        // Evento cuando se hace clic en un elemento del TreeView
        private void treeArchivos_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            // 1. Obtengo el elemento que el usuario ha seleccionado
            TreeViewItem itemSeleccionado = (TreeViewItem)treeArchivos.SelectedItem;
            if (itemSeleccionado == null) return;

            // 2. Recupero la ruta completa
            string rutaElemento = itemSeleccionado.Tag.ToString();

            // 3. Compruebo si la ruta pertenece a una carpeta o a un archivo
            if (Directory.Exists(rutaElemento))
            {
                // Es una carpeta: actualizamos la ruta superior y volvemos a cargar el árbol
                rutaActual = rutaElemento;
                txtRutaActual.Text = rutaActual;
                LlenarArbol(rutaActual);
            }
            else if (File.Exists(rutaElemento))
            {
                // Llamo a DetailWin pasándole la ruta del archivo
                DetailWin visor = new DetailWin(rutaElemento);
                visor.Owner = this;
                visor.Show();
            }
        }

        private void LlenarArbol(string ruta)
        {
            // Limpio el árbol
            treeArchivos.Items.Clear();

            try
            {
                DirectoryInfo directorio = new DirectoryInfo(ruta);

                // 1. Primero cargo las carpetas
                foreach (DirectoryInfo subDir in directorio.GetDirectories())
                {
                    TreeViewItem itemCarpeta = new TreeViewItem();
                    itemCarpeta.Header = "📁 " + subDir.Name;
                    itemCarpeta.Tag = subDir.FullName;
                    itemCarpeta.FontWeight = FontWeights.Bold;

                    treeArchivos.Items.Add(itemCarpeta);
                }

                // 2. Luego cargo los archivos
                foreach (FileInfo archivo in directorio.GetFiles())
                {
                    TreeViewItem itemArchivo = new TreeViewItem();
                    itemArchivo.Header = "📄 " + archivo.Name;
                    itemArchivo.Tag = archivo.FullName; // Guardo la ruta completa del archivo

                    treeArchivos.Items.Add(itemArchivo);
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("No se pudo leer la carpeta: " + ex.Message);
            }
        }
    }
}