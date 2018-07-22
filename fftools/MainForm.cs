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
using System.Threading;
using System.Globalization;

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
        CancellationTokenSource cs;
        public void TreadShowImagens(CancellationTokenSource cs)
        {
            int minimo = 1;
            int maximo = trackBar.Maximum;
            
            VideoFile video = videofile;
            OutputPackage outpackge = outputpack;
            while(minimo < maximo)
            {
                cs.Token.ThrowIfCancellationRequested();//cancelamos si es requerido
                Image img = conv.getImage(video, minimo);
                if (InvokeRequired)
                {
                    Invoke(new Action(()=>pictureBox.Image = img));
                    Invoke(new Action(()=> trackBar.Value = minimo)); //da error x doble llamada
                }
                Thread.Sleep(10);
                minimo++;
            }
        }
        void BtnRunClick(object sender, EventArgs e)
		{
            cs = new CancellationTokenSource();
            t = Task.Factory.StartNew(new Action(() => {
                TreadShowImagens(cs);
            }));
		}
        private void ShowDataFromVideofile()
        {
            StringBuilder build = new StringBuilder();
            double milisegundos = videofile.Duration.TotalMilliseconds;
            build.AppendLine($"duracion total miliseconds: {milisegundos}");
            build.AppendLine($"duracion total miliseconds: {videofile.DurationMs}");
            build.AppendLine($"Origen datos: {videofile.Path}");
            build.AppendLine($"video ancho: {videofile.Width} x alto: {videofile.Height}");
            textBoxdatos.Text = build.ToString();
        }
        //variable almacena datos del video media
        Converter conv;
		VideoFile videofile = null;
        //salida de datos del video.
		OutputPackage outputpack = null;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.G))
            {
                this.Text = ("<CTRL> + G Save Imagen");
                SaveFileToDisk();
            }
            if (keyData == (Keys.Escape))
            {
                this.Text = ("<ESC>:  Cancela tarea.");
                cs.Cancel();
            }
            if (keyData == (Keys.Control | Keys.H))
            {
                this.Text = ("<CTRL> + H help comands");
                MessageBox.Show(" shotkut:\n" +
                "<CTRL> + G Save Imagen.\n" +
                "<ESC>:  Cancela tarea.\n" );
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
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
        private void SaveData()
        {
            Properties.Settings.Default.Filename = textBoxfile.Text;
            Properties.Settings.Default.Comentarios = textBoxdatos.Text;
            Properties.Settings.Default.Save();
            KillProcess("ffmpeg");
            
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
			SaveData();
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
                trackBar.Enabled = false;
                t = Task.Factory.StartNew(new Action(() => {

                    videofile = conv.GetVideoInfo(textBoxfile.Text);
                    outputpack = conv.StrackImages(videofile, 1);
                    if (InvokeRequired)
                        Invoke(new Action(() => {
                            trackBar.Maximum = videofile.DurationMs-1;
                            trackBar.Enabled = true;
                            btnRun.Enabled = true;
                        }));
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
        string dirfault = string.Empty;
        private void SaveFileToDisk()
        {
            SaveFileDialog savefile = new SaveFileDialog()
            {
                Filter = "Gif file(*.gif*)|*.gif|Png file(*.png*)|*.png|Jpg file(*.jpg*)|*.jpg",
                Title = @"Save to disk",
                FilterIndex =2,
                //InitialDirectory = Environment.CurrentDirectory,
                //RestoreDirectory = true,
                ValidateNames = false,
                //FileName = Path.GetFileName(textBoxfile.Text)
            };
            
            if (String.IsNullOrEmpty(dirfault)) dirfault = Path.GetDirectoryName(textBoxfile.Text);
            savefile.FileName = AnalizarFileName(Path.Combine(dirfault, Path.GetFileName(textBoxfile.Text)));

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                //todo: pendiente - habria que analizar si existe ya un fichero con este nombre
                //inicial y cambiarle la numeracion.
                if (pictureBox.Image != null )
                {
                    dirfault = Path.GetDirectoryName(savefile.FileName);
                    pictureBox.Image.Save(savefile.FileName, ImageFormat.Gif);
                    //using (FileStream file = new FileStream(savefile.FileName, FileMode.Create, System.IO.FileAccess.Write))
                    //{
                    //    outputpack.VideoStream.WriteTo(file);
                    //}
                }
            }
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            if (!InvokeRequired)//asi cuando estamos en automático no extrae la imagen de aqui
            {
                pictureBox.Image = conv.getImage(videofile, trackBar.Value);
            }
        }
        private void KillProcess(string Nameprocess)
        {
            foreach (var process in Process.GetProcesses())
            {
                if (process.ProcessName != null)
                    if (process.ProcessName == Nameprocess)
                    {
                        //Debug.WriteLine($"Kiled process {process}");
                        process.Kill();
                    }
            }
        }
        public static string AnalizarFileName(string pathfilename)
        {
            //combrueba si existe un fichero en el directorio.
            string namefile = Path.GetFileName(pathfilename);
            //obtenemos el directorio
            DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(pathfilename));
            int iter = 0; //una variable pivote
            foreach (var item in dir.GetFiles(namefile+"*"))
            {
                iter++;
            }
            CultureInfo ci = CultureInfo.InvariantCulture;
            string Nuevoname = namefile+"_thumbs_"+iter.ToString("D4",ci);
            return Nuevoname;
            
        }
    }
}
