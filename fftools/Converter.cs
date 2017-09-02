/*
 * 
 * Existing file from project managerCG Converted.cs
 * clas Converter, to convert file move to gif animate.
 * modificate for application thumblib, in automatication block gif
 * autor: Hernani
 * modification: 21/03/2017
 * 
 * */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.IO;
using System.Diagnostics;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace fftools
{
	public class Converter
	{
		#region Properties
		/// <summary>
		/// Localizacion del fichero ejecutable ffmpeg.exe
		/// </summary>
		private static string FFExe { get; set; }
		/// <summary>
		/// directorio de trabajo del fichero ejecutable si no 
		/// esta en el path de sistema.
		/// </summary>
		private string WorkingPath{ get; set; }
		/// <summary>
		/// Directorio temporal
		/// </summary>
		private string Temp{ get; set; }
		/// <summary>
		/// fichero de salida.
		/// </summary>
		private string outPutfile{ get; set; }
		/// <summary>
		/// Existe en el path del sistema??
		/// </summary>
		private bool OnPathExe{ get; set; }
		/// <summary>
		/// numero de frames por segundo al crear un fichero
		/// gif.
		/// </summary>
		public int FrameRate { get; set; }

		#endregion
		#region Constructors
		public Converter()
		{
			Initialize();
		}
        
		#endregion
		#region Initialization
		private void Initialize()
		{
			OnPathExe = false;
			FFExe = "ffmpeg.exe";
			FrameRate = 1;
			WorkingPath = Environment.CurrentDirectory;
			//desde la variable de entorno
			var environmentVariable = Environment.GetEnvironmentVariable("Path");
			//explorer las variables de entorno.
			if (environmentVariable != null) {
				string[] mpeg = environmentVariable.Split(Convert.ToChar(";"));
				foreach (string item in mpeg) {
					string cadena = Path.Combine(item, "ffmpeg.exe");
					//si exite el ejecutable
					if (File.Exists(cadena)) {
						OnPathExe = true; //true, está en la ruta
					}
				}
				if (!OnPathExe) { // is not in path - exception
					string ruta = Path.Combine(WorkingPath, "ffmpeg.exe"); //existe en directorio ejecutable?
					if (File.Exists(ruta)) { //si no existe en la ruta pero si en el directorio de trabajo
						FFExe = ruta; //asignar ruta ejecutable.
					} else {
						FFExe = null;
						throw new ConvertException("No existe el ejecutable ffmpeg la ruta Path del sistema ni en el directorio del ejecutable" +
						"\nAsegrese de que exista, instalando la aplicacion.");
					}
				}
			}
			//establecer la ruta de un directorio temporal de trabajo
			this.Temp = Path.GetTempPath();	
		}

		public string GetWorkingFile()
		{
			return FFExe;
		}
		
		#endregion
		#region Metodos-static
		/// <summary>
		/// return image load from file, must to exist file
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns>Image</returns>
		public static System.Drawing.Image LoadImageFromFile(string fileName)
		{
			System.Drawing.Image theImage = null;
			using (FileStream fileStream = new FileStream(fileName, FileMode.Open,
				                               FileAccess.Read)) {
				byte[] img;
				img = new byte[fileStream.Length];
				fileStream.Read(img, 0, img.Length);
				fileStream.Close();
				theImage = System.Drawing.Image.FromStream(new MemoryStream(img));
				img = null;
			}
			GC.Collect();
			return theImage;
		}
		
		/// <summary>
		/// return MemoryStream from file, must to exist
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns>MemoryStream</returns>
		public static MemoryStream LoadMemoryStreamFromFile(string fileName)
		{
			MemoryStream ms = null;
			using (FileStream fileStream = new FileStream(fileName, FileMode.Open,
				                               FileAccess.Read)) {
				byte[] fil;
				fil = new byte[fileStream.Length];
				fileStream.Read(fil, 0, fil.Length);
				fileStream.Close();
				ms = new MemoryStream(fil);
			}
			GC.Collect();
			return ms;
		}
		
		/// <summary>
		/// Save MemoryStream to file with a name.
		/// filename is a path.
		/// </summary>
		/// <param name="inputFile">MemoryStream</param>
		/// <param name="filename">name file</param>
		public static void SaveMemoryStreamToFile(MemoryStream inputFile, string filename)
		{		
			FileStream fs = File.Create(filename);
			inputFile.WriteTo(fs);
			fs.Flush();
			fs.Close();
			GC.Collect();
		}
		
		#endregion
		#region Run the process
		private string RunProcess(string Parameters)
		{
			//create a process info
			string ffexe = String.Format("\"{0}\"", FFExe);
			ProcessStartInfo oInfo = new ProcessStartInfo(ffexe, Parameters);
			oInfo.UseShellExecute = false;
			oInfo.CreateNoWindow = true;
			oInfo.RedirectStandardOutput = true;
			oInfo.RedirectStandardError = true;
			

			//Create the output and streamreader to get the output
			string output = null;
			StreamReader srOutput = null;

			//try the process
			try {
				//run the process
				Process proc = Process.Start(oInfo);

				proc.WaitForExit();

				//get the output
				srOutput = proc.StandardError;

				//now put it in a string
				output = srOutput.ReadToEnd();

				proc.Close();
				//proc.Dispose();
			} catch (Exception) {
				throw new ConvertException("Error en RunProcess");
			} finally {
				//now, if we succeded, close out the streamreader
				if (srOutput != null) {
					srOutput.Close();
					srOutput.Dispose();
				}
			}
			return output;
		}
		private void OnlyRunProcess(string Parameters)
		{
			//create a process info
			string ffexe = String.Format("\"{0}\"", FFExe);
			ProcessStartInfo oInfo = new ProcessStartInfo(ffexe, Parameters);
			oInfo.UseShellExecute = false;
			oInfo.CreateNoWindow = true;
			oInfo.RedirectStandardOutput = false;
			oInfo.RedirectStandardError = false;

			//try the process
			try {
				//run the process
				Process proc = Process.Start(oInfo);

				proc.WaitForExit();
				
				proc.Close();
				//proc.Dispose();
			} catch (Exception) {
				throw new ConvertException("Error en OnlyRunProcess");
			} finally {
				//now, if we succeded, close out the streamreader
				
			}
		}
		#endregion
		#region GetVideoInfo
		/// <summary>
		/// obtiene VideoFile info - con la informacion- de un MemoryString de video
		/// pasando un nombre de fichero temporal con extension, del cual a continuacion
		/// obtiene la informacion.
		/// </summary>
		/// <param name="inputFile">MemoryStream</param>
		/// <param name="Filename">string del fichero</param>
		/// <returns>VideoFile</returns>
		public VideoFile GetVideoInfo(MemoryStream inputFile, string Filename)
		{
			string tempfile = Path.Combine(Temp, System.Guid.NewGuid().ToString() + Path.GetExtension(Filename));
			FileStream fs = File.Create(tempfile);
			inputFile.WriteTo(fs);
			fs.Flush();
			fs.Close();
			GC.Collect();

			VideoFile vf = null;
			try {
				vf = new VideoFile(tempfile);
			} catch (Exception ex) {
				throw new ConvertException(ex.ToString());
			}
			//obten la informacion del fichero
			GetVideoInfo(vf);
			//eliminamos del fichero temporal creado
			try {
				File.Delete(tempfile);
			} catch (Exception ex) {
				throw new ConvertException(ex.ToString());
			}

			return vf;
		}
		/// <summary>
		/// Direccion completa de la ruta del fichero de video
		/// </summary>
		/// <param name="inputPath">path completo fichero video</param>
		/// <returns>VideoFile</returns>
		public VideoFile GetVideoInfo(string inputPath)
		{
			VideoFile vf = null;
			try {
				vf = new VideoFile(inputPath);
			} catch (Exception ex) {
				throw new ConvertException(ex.ToString());
			}
			GetVideoInfo(vf);
			return vf;
		}
		/// <summary>
		/// Pasando un VideoFile Info, creado solo con la ruta completa
		/// del fichero, realiza proceso para rrellenar con la informacion
		/// el objeto VideoFile, que pone a true su variable infoGathered
		/// </summary>
		/// <param name="input">VideoFile</param>
		public void GetVideoInfo(VideoFile input)
		{
			//la ruta del fichero de imagen
			string video = String.Format("\"{0}\"", input.Path);
			//set up the parameters for video info
			string Params = string.Format("-i {0}", video);

			string output = RunProcess(Params);
			input.RawInfo = output;

			//get duration
			Regex re = new Regex("[D|d]uration:.((\\d|:|\\.)*)");
			Match m = re.Match(input.RawInfo);

			if (m.Success) {
				string duration = m.Groups[1].Value;
				string[] timepieces = duration.Split(new char[] { ':', '.' });
				if (timepieces.Length == 4) {
					input.Duration = new TimeSpan(0, Convert.ToInt16(timepieces[0]), Convert.ToInt16(timepieces[1]), Convert.ToInt16(timepieces[2]), Convert.ToInt16(timepieces[3]));
				}
			}

			//get audio bit rate
			re = new Regex("[B|b]itrate:.((\\d|:)*)");
			m = re.Match(input.RawInfo);
			double kb = 0.0;
			if (m.Success) {
				Double.TryParse(m.Groups[1].Value, out kb);
			}
			input.BitRate = kb;

			//get the audio format
			re = new Regex("[A|a]udio:.*");
			m = re.Match(input.RawInfo);
			if (m.Success) {
				input.AudioFormat = m.Value;
			}

			//get the video format
			re = new Regex("[V|v]ideo:.*");
			m = re.Match(input.RawInfo);
			if (m.Success) {
				input.VideoFormat = m.Value;
			}

			//get the video format
			re = new Regex("(\\d{2,3})x(\\d{2,3})");
			m = re.Match(input.RawInfo);
			if (m.Success) {
				int width = 0;
				int height = 0;
				int.TryParse(m.Groups[1].Value, out width);
				int.TryParse(m.Groups[2].Value, out height);
				input.Width = width;
				input.Height = height;
			}
			input.infoGathered = true;
		}
		#endregion
		#region Convert to FLV
		/// <summary>
		/// make a OutputPackage with a MemoryStream of video and only
		/// a name de file with extension without path complete.
		/// </summary>
		/// <param name="inputFile">MemoryStream file video</param>
		/// <param name="Filename">only name file to save temporali without path</param>
		/// <returns></returns>
		public OutputPackage ConvertToFLV(MemoryStream inputFile, string Filename)
		{
			string tempfile = Path.Combine(Temp, System.Guid.NewGuid().ToString() + Path.GetExtension(Filename));
			FileStream fs = File.Create(tempfile);
			inputFile.WriteTo(fs);
			fs.Flush();
			fs.Close();
			GC.Collect();

			VideoFile vf = null;
			try {
				vf = new VideoFile(tempfile);
			} catch (Exception ex) {
				throw new ConvertException(ex.ToString());
			}

			OutputPackage oo = ConvertToFLV(vf);

			//borra el fichero final.
			try {
				File.Delete(tempfile);
			} catch (Exception) {

			}

			return oo;
		}
		/// <summary>
		/// return OutputPackage.
		/// </summary>
		/// <param name="inputPath">path complet of file video</param>
		/// <returns></returns>
		public OutputPackage ConvertToFLV(string inputPath)
		{
			VideoFile vf = null;
			try {
				vf = new VideoFile(inputPath);
			} catch (Exception ex) {
				throw new ConvertException(ex.ToString());
			}

			OutputPackage oo = ConvertToFLV(vf);
			return oo;
		}
		/// <summary>
		/// Retorna un OutPutPackage con un Memorystream, con el fichero .flv
		/// </summary>
		/// <param name="input">VideoFile info of video</param>
		/// <returns></returns>
		public OutputPackage ConvertToFLV(VideoFile input)
		{
			//si no tiene informacion, obtenerla
			if (!input.infoGathered) {
				GetVideoInfo(input);
			}
			OutputPackage ou = new OutputPackage();

			//set up the parameters for getting a previewimage
			string filename = System.Guid.NewGuid().ToString() + ".flv";
			
			string finalpath = Path.Combine(Temp, filename); 
			string Params = string.Format("-i \"{0}\" -ar 22050 -vf scale=230:-1 -f flv \"{1}\"", input.Path, finalpath);
			//-ar 22050 -f flv (o) -y -ar 22050 -ab 64 -f flv
			Debug.WriteLine("ConvertToFLV, Params:=> {0}" + Params);
			OnlyRunProcess(Params);

			if (File.Exists(finalpath)) {
				ou.VideoStream = LoadMemoryStreamFromFile(finalpath);
				try {
					File.Delete(finalpath);
				} catch (Exception ex) {
					throw new ConvertException(ex.ToString());
				}
			} else { 
				throw new ConvertException("No se pudo crear el fichero de video .flv");
			}
			return ou;
		}
		#endregion
		#region lista de Imagenes stackimages

		public OutputPackage StrackImages(string inputpath, int num)
		{
			VideoFile vf = null;
			try {
				vf = new VideoFile(inputpath);
			} catch (Exception ex) {
				throw new ConvertException(ex.ToString());
			}

			OutputPackage oo = StrackImages(vf, num);
			return oo;
		}

		public OutputPackage StrackImages(VideoFile input, int num)
		{
			if (!input.infoGathered) {
				GetVideoInfo(input);
			}
			OutputPackage ou = new OutputPackage();
			string temp = Path.GetTempPath();
			Debug.WriteLine(temp);
			int valor = input.DurationMs / (num + 1);//esto es en segundos.
			if (valor <= 0)
				valor = 60;
			string fil = System.Guid.NewGuid().ToString();
			string filename = fil + "%04d.jpg";
			string finalpath = Path.Combine(temp, filename);
			string Params = String.Format("-ss {1} -i \"{0}\" -vf fps=1/{1} -vframes {2} \"{3}\" -hide_banner", input.Path, valor, num, finalpath);
			Debug.WriteLine(Params);
			OnlyRunProcess(Params);

			string[] files = Directory.GetFiles(temp, fil + "*.jpg");
			if (files != null)
				Array.Sort(files, CompareDinosByLength);

			foreach (var file in files) {
				ou.ListImage.Add(LoadImageFromFile(file));
				try {
					File.Delete(file);
				} catch (Exception ex) {
					Debug.WriteLine(ex.Message);
					throw new ConvertException(ex.ToString());
				}
			}

			Debug.WriteLine("fin de proceso de extraccion");
			return ou;
		}

		/// <summary>
		/// condicion para el ordenado de ficheros. -ya no está en uso-
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		private static int CompareDinosByLength(string x, string y)
		{
			if (x == null) {
				if (y == null) {
					// If x is null and y is null, they're
					// equal. 
					return 0;
				} else {
					// If x is null and y is not null, y
					// is greater. 
					return -1;
				}
			} else {
				// If x is not null...
				//
				if (y == null) {                // ...and y is null, x is greater.
					return 1;
				} else {
					// ...and y is not null, compare the 
					// lengths of the two strings.
					//
					int retval = x.Length.CompareTo(y.Length);

					if (retval != 0) {
						// If the strings are not of equal length,
						// the longer string is greater.
						//
						return retval;
					} else {
						// If the strings are of equal length,
						// sort them with ordinary string comparison.
						//
						return x.CompareTo(y);
					}
				}
			}
		}

		#endregion
		#region make-gif thread

		public void MakeGifThread(string inputpath, int num)
		{
			t = Task.Factory.StartNew(() => ThreadMakeGif(inputpath, num));
		}

		public void MakeGifThread(VideoFile input, int num)
		{
			t = Task.Factory.StartNew(() => ThreadMakeGif(input, num));
		}

		public void ThreadMakeGif(string inputpath, int num)
		{
			OutputPackage pack = MakeGif(inputpath, num);
			if (MadeFilmGif != null)
				MadeFilmGif(this, pack);
			//MadeFilmGif?.Invoke(this, pack);
		}

		public void ThreadMakeGif(VideoFile input, int num)
		{
			OutputPackage pack = MakeGif(input, num);
			if (MadeFilmGif != null)
				MadeFilmGif(this, pack);
			//MadeFilmGif?.Invoke(this, pack);
		}
        #endregion
        #region funciones-makegif

        public OutputPackage MakeGif(string inputpath, int num)
		{
			VideoFile vf = null;
			try {
				vf = new VideoFile(inputpath);
			} catch (Exception ex) {
				Debug.WriteLine(ex.Message);
			}

			OutputPackage oo = MakeGif(vf, num);
			return oo;
		}

		/// <summary>
		/// Make a Gif animate with framerate num.
		/// one image for second.
		/// </summary>
		/// <param name="input">VideoFile data input</param>
		/// <param name="num">frames for each second</param>
		/// <returns></returns>
		public OutputPackage MakeGif(VideoFile input, int num)
		{
			if (!input.infoGathered) {
				GetVideoInfo(input);
			}
			OutputPackage ou = new OutputPackage();

			string filesearch = System.Guid.NewGuid().ToString();
			string filename = filesearch+"%04d.png";
            string dirwork = Path.Combine(Path.GetDirectoryName(input.Path), "Thumbails");  //directorio de trabajo.
            //comprobar que existe: si no crearlo.
            if (!Directory.Exists(dirwork)) Directory.CreateDirectory(dirwork); //lo creamos.
            string finalpath = Path.Combine(Path.Combine(Path.GetDirectoryName(input.Path), "Thumbails"), filename);
			string Params = String.Format("-y -i \"{0}\" -vf scale=220:-1,fps=1/{1} \"{2}\"",input.Path,num,finalpath);
			Debug.WriteLine(Params);
			OnlyRunProcess(Params);
			string videodir = Path.GetDirectoryName(input.Path);
			string videoname = Path.GetFileName(input.Path);
			string outfilegif = Path.Combine(Path.Combine(videodir, "Thumbails"), videoname);
			
			//ahora montamos el gif con las imagenes creadas en fichero.
			Params = String.Format("-y -framerate {0} -i \"{1}\"  \"{2}_thumbs_0000.gif\"", FrameRate, finalpath, outfilegif);
			Debug.WriteLine(Params);
			OnlyRunProcess(Params);
			//load image gif from file.
			if (File.Exists(outfilegif + "_thumbs_0000.gif")) {
				ou.PreviewImage = LoadImageFromFile(outfilegif + "_thumbs_0000.gif");
			}

			
			string[] files = Directory.GetFiles(Path.GetDirectoryName(outfilegif), filesearch + "*.png");
			//delete the files images.
			foreach (var file in files) {
				try {
					File.Delete(file);
				} catch (Exception ex) {
					Debug.WriteLine(ex.Message);
				}
			}

			Debug.WriteLine(String.Format("create gif {0}_thumbs_0000.gif from {1}",outfilegif,input.Path));
			return ou;
		}
		#endregion
		#region make film gif

		private Task t;

		public delegate void MakeFilmGifHandler(Object sender, OutputPackage package);

		public event MakeFilmGifHandler MadeFilmGif;

		public void MakeFilmGifThread(string inputpath)
		{
			t = Task.Factory.StartNew(() => ThreadMakeFilmGif(inputpath));
		}

		public void MakeFilmGifThread(VideoFile input)
		{
			t = Task.Factory.StartNew(() => ThreadMakeFilmGif(input));
		}

		private void ThreadMakeFilmGif(string inputpath)
		{
			VideoFile vf = null;
			try {
				vf = new VideoFile(inputpath);
			} catch (Exception ex) {
				Debug.WriteLine(ex.Message);
			}

			OutputPackage oo = MakeFilmGif(vf);
			if (MadeFilmGif != null)
				MadeFilmGif(this, oo);
			//MadeFilmGif?.Invoke(this, oo);
		}
		private void ThreadMakeFilmGif(VideoFile input)
		{
			OutputPackage oo = MakeFilmGif(input);
			if (MadeFilmGif != null)
				MadeFilmGif(this, oo);
			//MadeFilmGif?.Invoke(this, oo);
		}
		public OutputPackage MakeFilmGif(string inputpath)
		{
			VideoFile vf = null;
			try {
				vf = new VideoFile(inputpath);
			} catch (Exception ex) {
				Debug.WriteLine(ex.Message);
			}

			OutputPackage oo = MakeFilmGif(vf);
			return oo;
		}
		/// <summary>
		/// Make a Gif animate with framerate 1.
		/// one image for second.
		/// </summary>
		/// <param name="input">VideoFile data input</param>
		/// <param name="num">one frame for each second</param>
		/// <returns></returns>
		public OutputPackage MakeFilmGif(VideoFile input)
		{
			if (!input.infoGathered) {
				GetVideoInfo(input);
			}
			OutputPackage ou = new OutputPackage();      

			//set up the parameters for getting a previewimage
			//string filesearch = System.Guid.NewGuid().ToString();
			string filename = $"{input.Path}_thumbs_0000.gif";
			string finalpath = filename;
			string Params = $"-y -i \"{input.Path}\" -vf scale=220:-1 \"{finalpath}\"";
			Debug.WriteLine(Params);
			//string output = RunProcess(Params);

			//ou.RawOutput = output;
			////
			//create a process info
			string ffexe = $"\"{FFExe}\"";
			ProcessStartInfo oInfo = new ProcessStartInfo(ffexe, Params);
			oInfo.UseShellExecute = false;
			oInfo.CreateNoWindow = true;
			oInfo.RedirectStandardOutput = false;
			oInfo.RedirectStandardError = false;

			//Create the output and streamreader to get the output
			//string output = null; StreamReader srOutput = null;

			//try the process
			try {
				//run the process
				Process proc = System.Diagnostics.Process.Start(oInfo);

				proc.WaitForExit();

				//get the output
				//srOutput = proc.StandardError;

				//now put it in a string
				//output = srOutput.ReadToEnd();

				//ahora montamos el gif con las imagenes creadas en fichero.
				//Params = $"-y -framerate 1 -i \"{finalpath}\"  \"{input.Path}_thumbs_0000.gif\"";
				//oInfo = new ProcessStartInfo(ffexe, Params);
				//oInfo.UseShellExecute = false;
				//oInfo.CreateNoWindow = true;
				//oInfo.RedirectStandardOutput = false;
				//oInfo.RedirectStandardError = false;
				//proc = System.Diagnostics.Process.Start(oInfo);

				//proc.WaitForExit();

				if (File.Exists(finalpath)) {
					ou.VideoStream = LoadMemoryStreamFromFile(finalpath);
				}

				proc.Close();
				//proc.Dispose();
			} catch (Exception ex) {
				Debug.WriteLine(ex.Message);
				//output = string.Empty;
			}


			//Thread.Sleep(2000);
			//
			//string[] files = Directory.GetFiles(Path.GetDirectoryName(input.Path), $"{filesearch}*.png");
			//
			//Array.Sort(files, CompareDinosByLength);

			//foreach (var file in files)
			//{
			//    try
			//    {
			//        File.Delete(file);
			//    }
			//    catch (Exception ex)
			//    {
			//        Debug.WriteLine(ex.Message);
			//    }
			//}

			//if (File.Exists(finalpath))
			//{
			//    //ou.PreviewImage = LoadImageFromFile(finalpath);
			//    ou.ListImage.Add(LoadImageFromFile(finalpath));
			//    try
			//    {
			//        File.Delete(finalpath);
			//    }
			//    catch (Exception) { }
			//}
			Debug.WriteLine("create gif {finalpath} from {input.Path}");
			//MadeFilmGif?.Invoke(this, ou);
			return ou;
		}

		#endregion
		#region get-image
		public System.Drawing.Image getImage(string inputpath)
		{
			VideoFile vf = null;
			try {
				vf = new VideoFile(inputpath);
			} catch (Exception ex) {
				throw new ConvertException(ex.ToString());
			}

			System.Drawing.Image oo = getImage(vf);
			return oo;
		}
		public System.Drawing.Image getImage(VideoFile input)
		{
			if (!input.infoGathered) {
				GetVideoInfo(input);
			}
			System.Drawing.Image ou;
			string temp = Path.GetTempPath();
			Debug.WriteLine(temp + " Image thumbails from file");
			int valor = input.DurationMs / 4;//imagen a 1/3 del tiempo total.
			if (valor <= 0)
				valor = 60;
			string fil = System.Guid.NewGuid().ToString();
			string filename = fil + ".jpg";
			string finalpath = Path.Combine(temp, filename);
			string Params = String.Format("-ss {1} -i \"{0}\" -vframes {1} \"{2}\" -hide_banner", input.Path, valor, finalpath);
			Debug.WriteLine(Params);
			OnlyRunProcess(Params);

			ou=LoadImageFromFile(finalpath);
			try {
				File.Delete(finalpath);
			} catch (Exception ex) {
				Debug.WriteLine(ex.Message);
				throw new ConvertException(ex.ToString());
			}
			
			Debug.WriteLine("fin de proceso de extraccion");
			return ou;
		}
		#endregion
	}

	public class VideoFile
	{
		#region Properties
		private string _Path;
		// disable once ConvertToAutoProperty
		public string Path {
			get {
				return _Path;
			}
			set {
				_Path = value;
			}
		}
		
		public TimeSpan Duration { get; set; }
		public int DurationMs{ get { return (int)Math.Round(TimeSpan.FromTicks(
				Duration.Ticks).TotalSeconds, 3); } }
		public double BitRate { get; set; }
		public string AudioFormat { get; set; }
		public string VideoFormat { get; set; }
		public int Height { get; set; }
		public int Width { get; set; }
		public string RawInfo { get; set; }
		public bool infoGathered { get; set; }
		#endregion

		#region Constructors
		public VideoFile(string path)
		{
			_Path = path;
			Initialize();
		}
		#endregion

		#region Initialization
		private void Initialize()
		{
			this.infoGathered = false;
			//first make sure we have a value for the video file setting
			if (string.IsNullOrEmpty(_Path)) {
				throw new Exception("Could not find the location of the video file");
			}

			//Now see if the video file exists
			if (!File.Exists(_Path)) {
				throw new Exception("The video file " + _Path + " does not exist.");
			}
		}
		#endregion
	}

	public class OutputPackage
	{
		public OutputPackage()
		{
			ListImage = new List<System.Drawing.Image>();
		}
		public MemoryStream VideoStream { get; set; }
		public System.Drawing.Image PreviewImage { get; set; }
		public List<System.Drawing.Image> ListImage { get; set; }
		public string RawOutput { get; set; }
		public bool Success { get; set; }
	}
    
	public class ConvertException : ApplicationException
	{
		public ConvertException()
		{

		}

		public ConvertException(string message)
			: base(message)
		{

		}

		public ConvertException(string message, Exception innerException)
			: base(message, innerException)
		{

		}
	}
}
