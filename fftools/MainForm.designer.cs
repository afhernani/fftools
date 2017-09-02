/*
 * Creado por SharpDevelop.
 * Usuario: hernani
 * Fecha: 06/04/2017
 * Hora: 10:15
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
namespace fftools
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Button btnRun;
		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.TextBox textBoxfile;
		private System.Windows.Forms.TextBox textBoxdatos;
		private System.Windows.Forms.NumericUpDown numericValor;
		private System.Windows.Forms.Button btnConvert;
		private System.Windows.Forms.Button btnmakeGif;
		private System.Windows.Forms.Button btnImagen;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnRun = new System.Windows.Forms.Button();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.textBoxfile = new System.Windows.Forms.TextBox();
            this.textBoxdatos = new System.Windows.Forms.TextBox();
            this.numericValor = new System.Windows.Forms.NumericUpDown();
            this.btnConvert = new System.Windows.Forms.Button();
            this.btnmakeGif = new System.Windows.Forms.Button();
            this.btnImagen = new System.Windows.Forms.Button();
            this.btnExplore = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericValor)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRun.Location = new System.Drawing.Point(211, 378);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(45, 35);
            this.btnRun.TabIndex = 0;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.BtnRunClick);
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox.Location = new System.Drawing.Point(12, 35);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(380, 234);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 1;
            this.pictureBox.TabStop = false;
            this.pictureBox.Click += new System.EventHandler(this.PictureBoxClick);
            // 
            // textBoxfile
            // 
            this.textBoxfile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxfile.Location = new System.Drawing.Point(12, 9);
            this.textBoxfile.Name = "textBoxfile";
            this.textBoxfile.Size = new System.Drawing.Size(332, 22);
            this.textBoxfile.TabIndex = 2;
            // 
            // textBoxdatos
            // 
            this.textBoxdatos.Location = new System.Drawing.Point(12, 275);
            this.textBoxdatos.Multiline = true;
            this.textBoxdatos.Name = "textBoxdatos";
            this.textBoxdatos.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxdatos.Size = new System.Drawing.Size(380, 97);
            this.textBoxdatos.TabIndex = 3;
            // 
            // numericValor
            // 
            this.numericValor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numericValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericValor.Location = new System.Drawing.Point(12, 378);
            this.numericValor.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.numericValor.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericValor.Name = "numericValor";
            this.numericValor.Size = new System.Drawing.Size(43, 22);
            this.numericValor.TabIndex = 4;
            this.numericValor.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // btnConvert
            // 
            this.btnConvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnConvert.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConvert.Location = new System.Drawing.Point(112, 378);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(93, 35);
            this.btnConvert.TabIndex = 5;
            this.btnConvert.Text = "Convert-FLV";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.BtnConvertClick);
            // 
            // btnmakeGif
            // 
            this.btnmakeGif.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnmakeGif.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnmakeGif.Location = new System.Drawing.Point(61, 378);
            this.btnmakeGif.Name = "btnmakeGif";
            this.btnmakeGif.Size = new System.Drawing.Size(45, 35);
            this.btnmakeGif.TabIndex = 6;
            this.btnmakeGif.Text = "&Gif";
            this.btnmakeGif.UseVisualStyleBackColor = true;
            this.btnmakeGif.Click += new System.EventHandler(this.BtnmakeGifClick);
            // 
            // btnImagen
            // 
            this.btnImagen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnImagen.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImagen.ForeColor = System.Drawing.Color.Red;
            this.btnImagen.Location = new System.Drawing.Point(262, 378);
            this.btnImagen.Name = "btnImagen";
            this.btnImagen.Size = new System.Drawing.Size(61, 35);
            this.btnImagen.TabIndex = 7;
            this.btnImagen.Text = "&Image";
            this.btnImagen.UseVisualStyleBackColor = true;
            this.btnImagen.Click += new System.EventHandler(this.BtnImagenClick);
            // 
            // btnExplore
            // 
            this.btnExplore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExplore.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExplore.ForeColor = System.Drawing.Color.Red;
            this.btnExplore.Location = new System.Drawing.Point(350, 9);
            this.btnExplore.Name = "btnExplore";
            this.btnExplore.Size = new System.Drawing.Size(42, 22);
            this.btnExplore.TabIndex = 9;
            this.btnExplore.Text = "&>>";
            this.btnExplore.UseVisualStyleBackColor = true;
            this.btnExplore.Click += new System.EventHandler(this.btnExplore_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 425);
            this.Controls.Add(this.btnExplore);
            this.Controls.Add(this.btnImagen);
            this.Controls.Add(this.btnmakeGif);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.numericValor);
            this.Controls.Add(this.textBoxdatos);
            this.Controls.Add(this.textBoxfile);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.btnRun);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "ffutility";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
            this.Load += new System.EventHandler(this.MainFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericValor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        private System.Windows.Forms.Button btnExplore;
    }
}
