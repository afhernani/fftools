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
using System.Threading.Tasks;
using System.Drawing.Imaging;

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
            ShowDataFromVideofile();
            outputpack = conv.StrackImages(videofile,(int)numericValor.Value);
			LoadImageToPicture(0);
			textBoxdatos.Text += outputpack.ListImage.Count.ToString() + newline;
			btnConvert.Enabled = true;
			btnmakeGif.Enabled = true;
		}
        private void ShowDataFromVideofile()
        {
            StringBuilder build = new StringBuilder();
            build.AppendLine(videofile.Duration.ToString());
            build.AppendLine(videofile.DurationMs.ToString());
            build.AppendLine(videofile.Path);
            build.AppendLine(videofile.Height.ToString());
            textBoxdatos.Text = build.ToString();
        }
        //variable almacena datos del video media
        Converter conv;
		VideoFile videofile = null;
        //salida de datos del video.
		OutputPackage outputpack = null;
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
            conv = new Converter();
            conv.MadeFilmGif += Conv_MadeFilmGif;
			OpenDataInit();
		}

        private void Conv_MadeFilmGif(object sender, OutputPackage package)
        {
            if (InvokeRequired)
                Invoke(new Action(() => {
                    btnmakeGif.Enabled = true;
                    outputpack.ListImage.Add((Image)package.PreviewImage.Clone());
                    pictureBox.Image = (Image)package.PreviewImage.Clone();
                    textBoxdatos.Text += newline + "Make-gif from file: => " + 
                        Path.GetFileName(videofile.Path) + Environment.NewLine;
                }));
        }

        void BtnConvertClick(object sender, EventArgs e)
		{
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
			//instancia converter.-inicializa.
			conv.FrameRate = 2;
            int num = videofile.DurationMs / (int)numericValor.Value;
            conv.MakeGifThread(videofile, num);
            //OutputPackage package = conv.MakeGif(videofile, (int)numericValor.Value);
			
			//
			btnmakeGif.Enabled = false;
		}	
		/// <summary>
        /// guardar datos al fichero ini.
        /// </summary>
        private void SaveDataIni()
        {
            Properties.Settings.Default.Filename = textBoxfile.Text;
            Properties.Settings.Default.Comentarios = textBoxdatos.Text;
            Properties.Settings.Default.Save();
            
        }
        /// <summary>
        /// open data ini
        /// </summary>
        private void OpenDataInit()
        {
            string filename = Properties.Settings.Default.Filename;
            string comments = Properties.Settings.Default.Comentarios;
            if (!String.IsNullOrEmpty(filename))
            {
                textBoxfile.Text = filename;
                loadImagen();
            }
            if (!String.IsNullOrEmpty(comments))
            {
                textBoxdatos.Text = comments;
            }
            
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
            SaveFileToDisk();
        }
        Task t;
        /// <summary>
        /// devuelve true si carga la imagen sin problemas
        /// </summary>
        /// <returns></returns>
        public bool loadImagen()
        {
            bool res = false;
            if (File.Exists(textBoxfile.Text))
            { 
                t = Task.Factory.StartNew(new Action(() => {

                    videofile = conv.GetVideoInfo(textBoxfile.Text);
                    outputpack = conv.StrackImages(videofile, 1);
                }));
                pictureBox.Image = conv.getImage(textBoxfile.Text);
                //pictureBox.Image = conv.getImage(videofile);
                ShowDataFromVideofile();
                res = true;
                return res;
            }
            return res;
        }

        string name_Only = String.Empty;

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
                if (loadImagen()==false)
                {
                    textBoxdatos.Text += "A sido imposible cargar "
                        +$"la imgen del fichero {name_Only}";
                }
            }
        }
        private void SaveFileToDisk()
        {
            SaveFileDialog savefile = new SaveFileDialog()
            {
                Filter = "Gif file(*.gif*)|*.gif",
                Title = @"Save gif to disk",
                //InitialDirectory = Environment.CurrentDirectory,
                //RestoreDirectory = true,
                ValidateNames = false,
                FileName = textBoxfile.Text + "_thumbss_0000.gif"
            };
            if (savefile.ShowDialog() == DialogResult.OK)
            {
                if (pictureBox.Image != null )
                {
                    pictureBox.Image.Save(savefile.FileName, ImageFormat.Gif);
                    //using (FileStream file = new FileStream(savefile.FileName, FileMode.Create, System.IO.FileAccess.Write))
                    //{
                    //    outputpack.VideoStream.WriteTo(file);
                    //}
                }
            }
        }
    }
}
