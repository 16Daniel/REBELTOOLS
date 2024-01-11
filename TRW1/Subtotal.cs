using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data.SqlClient;

namespace TRW1
{
    public partial class Subtotal : Form
    {
        NotifyIcon NotifyIcon1 = new NotifyIcon();
        public string command;
        SqlConnection con;
        DirConexion dirCon = new DirConexion();
        public string arquitectura;

        public Subtotal()
        {
            con = dirCon.crearConexion();
            InitializeComponent();
            Procesador();
             
        }
        private void ejecuta() {
            


            ProcessStartInfo psi = new ProcessStartInfo();
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            if (arquitectura == "64 bits") {
                psi.FileName = "C:\\Windows\\REBELTOOL\\FrontRest64.bat";
                
            }
            else {
                psi.FileName = "C:\\Windows\\REBELTOOL\\FrontRest32.bat";
                
            }
            Process.Start(psi);
      
        }

        private void Procesador() {
            // La información sobre la arquitectura de 32 o 64 bits se obtiene mejor desde la clave
            // [HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment]
            // En la clave PROCESSOR_ARCHITECTURE que puede ser AMD64 o x86
            string Clave2 = @"SYSTEM\CurrentControlSet\Control\Session Manager\Environment";
            //
            Microsoft.Win32.RegistryKey principal = Microsoft.Win32.Registry.LocalMachine; //rama LocalMachine
            Microsoft.Win32.RegistryKey subclave2 = principal.OpenSubKey(Clave2); //clave SYSTEM ...  Environment
            
            // valor de la cadena PROCESSOR_ARCHITECTURE de Environment
            arquitectura = subclave2.GetValue("PROCESSOR_ARCHITECTURE").ToString();
            if (arquitectura.Equals("AMD64"))
                arquitectura = "64 bits";
            else
                arquitectura = "32 bits";
            


        }

        private void cambioModifica() {

            con.Open();


            SqlDataAdapter query = new SqlDataAdapter();
            query.UpdateCommand = new SqlCommand("UPDATE PARAMETROS SET VALOR = 'False' WHERE  (CLAVE = 'PrintOneS') OR (CLAVE = 'BLOQSUBT')", con);
            
            try
            {

                query.UpdateCommand.ExecuteNonQuery();
                //MessageBox.Show("Movimiento de mesa TOTAL exitoso...", "Cambio Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                con.Close();



                // Application.Exit();

            }
            catch
            {
                con.Close();
                MessageBox.Show("Error Habilita mesa");
            }
            ejecuta();

        }
        private void cambioBloquea()
        {

            con.Open();


            SqlDataAdapter query = new SqlDataAdapter();
            query.UpdateCommand = new SqlCommand("UPDATE PARAMETROS SET VALOR = 'True' WHERE  (CLAVE = 'PrintOneS') OR (CLAVE = 'BLOQSUBT')", con);

            try
            {

                query.UpdateCommand.ExecuteNonQuery();
                //MessageBox.Show("Movimiento de mesa TOTAL exitoso...", "Cambio Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                con.Close();



                // Application.Exit();

            }
            catch
            {
                con.Close();
                MessageBox.Show("Error Bloquea mesa");
            }
            ejecuta();

        }

        private void Subtotal_Load(object sender, EventArgs e)
        {
            this.ShowInTaskbar = false;
            StartSub();
            cambioModifica();
            
        }

        private int duration = 120;
        private void StartSub()
        {
            labelTiempo.Text = "" +duration;
            timerSub = new System.Windows.Forms.Timer();
            timerSub.Tick += new EventHandler(count_down);
            timerSub.Interval = 1000;
            timerSub.Start();

        }
        private void count_down(object sender, EventArgs e)
        {

            if (duration == 0)
            {
                //actua evento
                timerSub.Stop();

                cambioBloquea();
               // MessageBox.Show("Termino Timer", "Termino");
                Application.Exit();
                

            }
            else if (duration > 0)
            {
                duration--;
                labelTiempo.Text = duration.ToString();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
