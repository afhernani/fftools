/*
 * Creado por SharpDevelop.
 * Usuario: hernani
 * Fecha: 06/04/2017
 * Hora: 10:15
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace fftools
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
        /// <summary>
        /// inicializa el formulario.
        /// </summary>
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		void BtnRunClick(object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(textBoxfile.Text))
				return;
			Converter conv = new Converter();
			videofile = conv.GetVideoInfo(textBoxfile.Text);
			StringBuilder build = new StringBuilder();
			build.AppendLine(videofile.Duration.ToString());
			build.AppendLine(videofile.DurationMs.ToString());
			build.AppendLine(videofile.Path);
			build.AppendLine(videofile.Height.ToString());
			textBoxdatos.Text = build.ToString();
			outputpack = conv.StrackImages(videofile,(int)numericValor.Value);
			LoadImageToPicture(0);
			textBoxdatos.Text += newline + outputpack.ListImage.Count.ToString() + newline;
			btnConvert.Enabled = true;
			btnmakeGif.Enabled = true;
		}
        //variable almacena datos del video media
		VideoFile videofile;
        //salida de datos del video.
		OutputPackage outputpack;
		int Index { get; set; }
		private void LoadImageToPicture(int index){
			if (outputpack.ListImage.Count == 0)
				return;
			pictureBox.Image = outputpack.ListImage[index];
		}
		void PictureBoxClick(object sender, EventArgs e)
		{
			if (outputpack == null)
				return;
			Index++;
			if(Index < outputpack.ListImage.Count){
			}else{
				Index = 0;
			}
			LoadImageToPicture(Index);
		}
        /// <summary>
        /// Carga los parametros guardados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		void MainFormLoad(object sender, EventArgs e)
		{
			Index = 0;
			btnConvert.Enabled = false;
			btnmakeGif.Enabled = false;
			OpenDataInit();
		}
		void BtnConvertClick(object sender, EventArgs e)
		{
			Converter conv = new Converter();
			OutputPackage package = conv.ConvertToFLV(videofile);
			outputpack.VideoStream = package.VideoStream;
			string filename = Path.Combine(Environment.CurrentDirectory, 
			                               string.Format("{0}.flv", System.Guid.NewGuid().ToString()));
			Converter.SaveMemoryStreamToFile(outputpack.VideoStream, filename);
			Debug.WriteLine("Name-File:=> "+ filename);
			textBoxdatos.Text += newline + "Convert file: => " + filename + Environment.NewLine;
			btnConvert.Enabled = false;
		}
		string newline = Environment.NewLine;
		void BtnmakeGifClick(object sender, EventArgs e)
		{
			// Todo: aqui
			//
			Converter conv = new Converter(); //instancia converter.-inicializa.
			conv.FrameRate = 2;
			OutputPackage package = conv.MakeGif(videofile, (int)numericValor.Value);
			outputpack.ListImage.Add((Image)package.PreviewImage.Clone());
			pictureBox.Image = (Image)package.PreviewImage.Clone();
			textBoxdatos.Text += newline + "Make-gif from file: => " + Path.GetFileName(videofile.Path) + Environment.NewLine;
			//
			btnmakeGif.Enabled = false;
		}	
		/// <summary>
        /// guardar datos al fichero ini.
        /// </summary>
        private void SaveDataIni()
        {
            //escribir codigo aqui
            string dir = Environment.CurrentDirectory;
            string namefile = Path.Combine(dir, "dat.ini");
            IniFile inifile = new IniFile(namefile);
            inifile.Write("File", textBoxfile.Text);
            inifile.Write("File-Name", Path.GetFileName(textBoxfile.Text));
            inifile.Write("DefaultVolume", "100", "Audio");
            inifile.Write("HomePage", textBoxfile.Text, "Web");
            inifile.Write("name-Scraping-result",textBoxfile.Text,"temporal");
            inifile.Write("doc-html-down",textBoxfile.Text,"temporal");
            inifile.Write("DefaultVolume", "80", "Audio");
            //recuperamos una clave.
            //var res = inifile.Read("doc-html-down", "temporal");
            //txtBoxResult.Text = res;
            Debug.WriteLine("Proceso de guardado en fichero ini terminado...");
        }
        /// <summary>
        /// open data ini
        /// </summary>
        private void OpenDataInit()
        {
            string namefile = Path.Combine(Environment.CurrentDirectory, "dat.ini");
            if (!File.Exists(namefile)) return; //si no existe salimos para evitar errores.
            IniFile inifile = new IniFile(namefile);
            var res = inifile.Read("File");
            textBoxfile.Text = res;
			res = inifile.Read("File-Name");
            textBoxdatos.Text = res;
            //res = inifile.Read("HomePage");
            //txtUrl.Text = res;
            //ClearCachedSWFFiles();
            //webBrowser1.Navigate(res); //= new Uri(res);
            /*if (inifile.KeyExists("name-Scraping-result", "temporal"))
            {
                var script = inifile.Read("name-Scraping-result", "temporal");
                btnHtml.Tag = script;
                txtBoxResult.Text = @"asignado el fichero temporal de scraping de la" +
                                    $"ultima secion {script}";
            }*/
        }
		/// <summary>
		/// save data ini to closing form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			SaveDataIni();
		}
		void BtnImagenClick(object sender, EventArgs e)
		{
            string res = "No se ha podido cargar la imagen ";
            if(loadImagen()==false)
                textBoxdatos.Text += res;
        }
        /// <summary>
        /// general carga la imagen.
        /// </summary>
        /// <returns></returns>
        public bool loadImagen()
        {
            bool res = false;
            Converter conv = new Converter();
            if (videofile == null && File.Exists(textBoxfile.Text))
            {
                pictureBox.Image = conv.getImage(textBoxfile.Text);
                res = true;
                return res;
            }
            if (videofile != null)
            {
                pictureBox.Image = conv.getImage(videofile);
                res = true;
                return res;
            }
            return res;
        }

        string name_Only;
        private void btnExplore_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog()
            {
                Filter = "flv file(*.flv*)|*.flv|mp4 file(*.mp4*)|*.mp4|All files(*.*)|*.*",
                Title = @"Open gif to load",
                //InitialDirectory = Environment.CurrentDirectory,
                //RestoreDirectory = true,
                //Multiselect = true
            };

            if (openfile.ShowDialog() == DialogResult.OK)
            {
                textBoxfile.Text = openfile.FileName;
                name_Only = openfile.SafeFileName;
                loadImagen();
            }
        }
    }
}
