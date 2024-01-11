namespace TRW1
{
    partial class MostrarProveedores
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridViewmostrarProveedor = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewmostrarProveedor)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewmostrarProveedor
            // 
            this.dataGridViewmostrarProveedor.AllowUserToAddRows = false;
            this.dataGridViewmostrarProveedor.AllowUserToDeleteRows = false;
            this.dataGridViewmostrarProveedor.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewmostrarProveedor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewmostrarProveedor.Location = new System.Drawing.Point(66, 24);
            this.dataGridViewmostrarProveedor.Name = "dataGridViewmostrarProveedor";
            this.dataGridViewmostrarProveedor.ReadOnly = true;
            this.dataGridViewmostrarProveedor.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewmostrarProveedor.Size = new System.Drawing.Size(535, 281);
            this.dataGridViewmostrarProveedor.TabIndex = 21;
            this.dataGridViewmostrarProveedor.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewProveedor_CellContentClick);
            // 
            // MostrarProveedores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 328);
            this.Controls.Add(this.dataGridViewmostrarProveedor);
            this.Name = "MostrarProveedores";
            this.Text = "MostrarProveedores";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewmostrarProveedor)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewmostrarProveedor;
    }
}