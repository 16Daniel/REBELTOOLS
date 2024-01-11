using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics;

namespace TRW1
{
    public partial class PEDIDO : Form
    {
        //Conexion SQL
        SqlConnection con;
        DirConexion dirCon = new DirConexion();
        SqlDataAdapter adap = new SqlDataAdapter();
        DataSet ds = new DataSet();
        SqlDataAdapter adap1 = new SqlDataAdapter();
        DataSet ds1 = new DataSet();

        public string arquitectura;

        string SerieP;
        int NumeroP;
        string NumeroCompra;
        string SerieCompra;
        string supedidoCompra;
        //DateTime FechaPO;
        int Codproveedor;
        int UltimoZ;
        string Fecha = DateTime.Now.ToString("dd/MM/yyyy/HH:mm:tt");
        string Totalimpuestos;
        string Totalbruto;
        string Totalneto;
        public string acceso;
        List<AlbcompraLin> AClin = new List<AlbcompraLin>();
        List<Albcompratot> ACTot = new List<Albcompratot>();

        public PEDIDO(TRW1.Administrador.Pedido _pedido, int z)
        {
            InitializeComponent();
            con = dirCon.crearConexion();
            SerieP = _pedido.serie;
            NumeroP = _pedido.numero;
            labelProveedor.Text = _pedido.proveedor;
            labelFPedido.Text = _pedido.fecha.ToString("yyyy-MM-dd");
            labelTotal.Text = _pedido.total.ToString();
            labelPedido.Text = _pedido.serie + "-" + _pedido.numero;
            labelRFC.Text = _pedido.RFC;
            UltimoZ = z;
            supedidoCompra = _pedido.serie + "-" + _pedido.numero;

            CargarPedidos(SerieP, NumeroP);
            Procesador();
        }

        private void PEDIDO_Load(object sender, EventArgs e)
        {

        }

        private void CargarPedidos(string Serie, int Numero)
        {

            con.Open();
            //SqlDataAdapter datos = new SqlDataAdapter("SELECT  TIQUETSLIN.CODARTICULO, TIQUETSLIN.DESCRIPCION, SUM(TIQUETSLIN.UNIDADES) AS UDS FROM TIQUETSLIN INNER JOIN ARTICULOSCAMPOSLIBRES ON TIQUETSLIN.CODARTICULO = ARTICULOSCAMPOSLIBRES.CODARTICULO WHERE (TIQUETSLIN.HORA BETWEEN CONVERT(DATETIME, '"+dtpInicio.Value.ToString("yyyy-MM-dd")+ " 00:00:00', 102) AND CONVERT(DATETIME, '"+dtpFinal.Value.ToString("yyyy-MM-dd") + " 23:59:59', 102)) AND (ARTICULOSCAMPOSLIBRES.INDUCIDA = 'T') GROUP BY TIQUETSLIN.CODARTICULO, TIQUETSLIN.DESCRIPCION ORDER BY UDS DESC", con);
            SqlDataAdapter datos = new SqlDataAdapter("SELECT PEDCOMPRALIN.DESCRIPCION, PEDCOMPRALIN.UNIDADESTOTAL AS UDS, PEDCOMPRALIN.PRECIO AS PRECIO_UNITARIO, CONVERT(varchar(100), CAST(IVA AS decimal(38,0)))+ '%' AS IVA, (CASE WHEN IVA = 16 THEN (PEDCOMPRALIN.TOTALLINEA * 1.16) ELSE PEDCOMPRALIN.TOTALLINEA END) AS TOTAL_NETO FROM PEDCOMPRACAB INNER JOIN PEDCOMPRALIN ON PEDCOMPRACAB.NUMSERIE = PEDCOMPRALIN.NUMSERIE AND PEDCOMPRACAB.NUMPEDIDO = PEDCOMPRALIN.NUMPEDIDO AND PEDCOMPRACAB.N = PEDCOMPRALIN.N WHERE(PEDCOMPRACAB.TODORECIBIDO = N'F') AND (PEDCOMPRACAB.NUMSERIE = N'" + Serie + "') AND (PEDCOMPRACAB.NUMPEDIDO = " + Numero + ")", con);
            DataSet data = new DataSet();

            datos.Fill(data);
            if (data.Tables[0].Rows.Count > 0)
            {
                DGWArticulos.Enabled = true;
                DGWArticulos.DataSource = data.Tables[0];
                con.Close();
            }
            else { DGWArticulos.Enabled = false; con.Close(); }

            this.DGWArticulos.Columns["UDS"].DefaultCellStyle.Format = "N2";
            this.DGWArticulos.Columns["PRECIO_UNITARIO"].DefaultCellStyle.Format = "N2";
            this.DGWArticulos.Columns["TOTAL_NETO"].DefaultCellStyle.Format = "N2";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (textBoxRemision.Text.Length != 0)
            {
                ConsultaCompra();
            }
            else {
                //MessageBox.Show("FAVOR DE CAPTURAR EL CAMPO REMISION/FACTURA");
                MessageBox.Show("FAVOR DE CAPTURAR EL CAMPO REMISION/FACTURA", "ALERTA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void recibir(){
        
        }

        //CONSULTA ULTIMA COMPRA
        public void ConsultaCompra()
        {

            con.Open();
            string query = "SELECT NUMSERIE, MAX(NUMALBARAN) AS NUMERO FROM  ALBCOMPRACAB GROUP BY NUMSERIE ORDER BY NUMERO DESC";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader reg = cmd.ExecuteReader();
            try
            {
                if (reg.Read())
                {
                    NumeroCompra = reg["NUMERO"].ToString();
                    SerieCompra = reg["NUMSERIE"].ToString();

                    con.Close();

                }

                else
                {
                    con.Close();


                }
            }
            catch {
                con.Close();
                MessageBox.Show("Error al consultar funcion ConsultaCompra()");

            }
            NumeroCompra = ""+(int.Parse(NumeroCompra) + 1);
            ConsultaProveedor(SerieP,NumeroP);

        }
        //CONSULTA CODPROVEEDOR
        public void ConsultaProveedor(string serie, int numero)
        {

            con.Open();
            string query = "SELECT   CODPROVEEDOR, TOTBRUTO, TOTIMPUESTOS, TOTNETO FROM  PEDCOMPRACAB WHERE   (TODORECIBIDO = N'F') AND (NUMSERIE = N'" + serie+"') AND (NUMPEDIDO = "+numero+")";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader reg = cmd.ExecuteReader();
            try
            {
                if (reg.Read())
                {
                    Codproveedor = int.Parse(reg["CODPROVEEDOR"].ToString());
                    Totalimpuestos = reg["TOTIMPUESTOS"].ToString();
                    Totalbruto = reg["TOTBRUTO"].ToString();
                    Totalneto = reg["TOTNETO"].ToString();

                    con.Close();

                }

                else
                {
                    con.Close();


                }
            }
            catch
            {
                con.Close();
                MessageBox.Show("Error al consultar funcion ConsuktaProveedor()");

            }
            llenaArticulos(SerieP,NumeroP);
        }

        //CALCULA TOTAL SOBRE IMPUESTOS
        public void ConsuktaTotal(string serie, int numero)
        {

            con.Open();
            string query = "SELECT   CODPROVEEDOR, TOTBRUTO, TOTIMPUESTOS, TOTNETO FROM  PEDCOMPRACAB WHERE   (TODORECIBIDO = N'F') AND (NUMSERIE = N'" + serie + "') AND (NUMPEDIDO = " + numero + ")";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader reg = cmd.ExecuteReader();
            try
            {
                if (reg.Read())
                {
                    Codproveedor = int.Parse(reg["CODPROVEEDOR"].ToString());
                    Totalimpuestos = reg["TOTIMPUESTOS"].ToString();
                    Totalbruto = reg["TOTBRUTO"].ToString();
                    Totalneto = reg["TOTNETO"].ToString();

                    con.Close();

                }

                else
                {
                    con.Close();


                }
            }
            catch
            {
                con.Close();
                MessageBox.Show("Error al consultar funcion ConsuktaProveedor()");

            }

        }
        private void AlbcompracabInsert()
        {

            try
            {
                con.Open();
                SqlDataAdapter query = new SqlDataAdapter();

                query.InsertCommand = new SqlCommand("INSERT INTO [dbo].[ALBCOMPRACAB]([NUMSERIE],[NUMALBARAN],[N],[SUALBARAN],[FACTURADO],[NUMSERIEFAC],[NUMFAC],[NFAC],[ESUNDEPOSITO],[ESDEVOLUCION],[CODPROVEEDOR],[FECHAALBARAN],[ENVIOPOR],[PORTESPAG],[DTOCOMERCIAL],[TOTDTOCOMERCIAL],[DTOPP],[TOTDTOPP],[TOTALBRUTO],[TOTALIMPUESTOS],[TOTALNETO],[SELECCIONADO],[CODMONEDA],[FACTORMONEDA],[IVAINCLUIDO],[FECHAENTRADA],[TIPODOC],[TIPODOCFAC],[VIENECENTRAL],[CAJA],[Z],[CHEQUEADO])"
                                                                + "VALUES(@SerieCompra,@NumeroCompra,'B',@Sualbaran,'F','','0','','F','F',@Codproveedor,@Fecha,'','T','0','0','0','0',@Totalbruto,@Totalimpuestos,@Totalneto,'F','1','1','F',@Fecha,'4','0','0','1',@z,'F')", con);
                query.InsertCommand.Parameters.Add("@z", SqlDbType.Int).Value = UltimoZ+1;
                query.InsertCommand.Parameters.Add("@SerieCompra", SqlDbType.NVarChar).Value = SerieCompra;
                query.InsertCommand.Parameters.Add("@NumeroCompra", SqlDbType.Int).Value = NumeroCompra;
                query.InsertCommand.Parameters.Add("@Sualbaran", SqlDbType.NVarChar).Value = textBoxRemision.Text;
                query.InsertCommand.Parameters.Add("@Codproveedor", SqlDbType.Int).Value = Codproveedor;
                query.InsertCommand.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = DateTime.Now.Date;
                query.InsertCommand.Parameters.Add("@Totalbruto", SqlDbType.NVarChar).Value = Totalbruto;
                query.InsertCommand.Parameters.Add("@Totalimpuestos", SqlDbType.NVarChar).Value = Totalimpuestos;
                query.InsertCommand.Parameters.Add("@Totalneto", SqlDbType.NVarChar).Value = Totalneto;
                query.InsertCommand.ExecuteNonQuery();


                con.Close();
            }
            catch {
                con.Close();
                MessageBox.Show("Error al consultar funcion AlbcompracabInsert()");
            }

           AlbcompralinInsert(AClin);

        }
        private void AlbcompralinInsert(List<AlbcompraLin> albcompraLins)
        {

            try
            {

                SqlDataAdapter query = new SqlDataAdapter();
                foreach (var i in albcompraLins)
                {
                    con.Open();
                    query = new SqlDataAdapter();

                    query.InsertCommand = new SqlCommand("INSERT INTO [dbo].[ALBCOMPRALIN]([NUMSERIE],[NUMALBARAN],[N],[NUMLIN],[CODARTICULO],[REFERENCIA],[DESCRIPCION],[COLOR],[TALLA],[UNID1],[UNID2],[UNID3],[UNID4],[UNIDADESTOTAL],[UNIDADESPAGADAS],[PRECIO],[DTO],[TOTAL],[TIPOIMPUESTO],[IVA],[REQ],[NUMKG],[CODALMACEN],[DEPOSITO],[PRECIOVENTA],[USARCOLTALLAS],[IMPORTEGASTOS],[SUPEDIDO])"
                                                                                 + "VALUES(@numserie,@numalbaran,@n,@numlin,@codarticulo,@referencia,@descripcion,@color,@talla,@unid1,@unid2,@unid3,@unid4,@unidadestotal,NULL,@precio,@dto,@total,@tipoimpuesto,@iva,@req,@numkg,@codalmacen,@deposito,@precioventa,@usarcoltallas,@importegastos,@supedido)", con);

                    query.InsertCommand.Parameters.Add("@numserie", SqlDbType.NVarChar).Value = i.NUMSERIE;
                    query.InsertCommand.Parameters.Add("@numalbaran", SqlDbType.Int).Value = int.Parse(i.NUMALBARAN);
                    query.InsertCommand.Parameters.Add("@n", SqlDbType.NChar).Value = i.N;
                    query.InsertCommand.Parameters.Add("@numlin", SqlDbType.Int).Value = int.Parse(i.NUMLIN);
                    query.InsertCommand.Parameters.Add("@codarticulo", SqlDbType.Int).Value = int.Parse(i.CODARTICULO);
                    query.InsertCommand.Parameters.Add("@referencia", SqlDbType.NVarChar).Value = i.REFERENCIA;
                    query.InsertCommand.Parameters.Add("@descripcion", SqlDbType.NVarChar).Value = i.DESCIPCION;
                    query.InsertCommand.Parameters.Add("@color", SqlDbType.NVarChar).Value = i.COLOR;
                    query.InsertCommand.Parameters.Add("@talla", SqlDbType.NVarChar).Value = i.TALLA;
                    query.InsertCommand.Parameters.Add("@unid1", SqlDbType.Float).Value = float.Parse(i.UNID1);
                    query.InsertCommand.Parameters.Add("@unid2", SqlDbType.Float).Value = float.Parse(i.UNID2);
                    query.InsertCommand.Parameters.Add("@unid3", SqlDbType.Float).Value = 1;
                    query.InsertCommand.Parameters.Add("@unid4", SqlDbType.Float).Value = 1;
                    query.InsertCommand.Parameters.Add("@unidadestotal", SqlDbType.Float).Value = float.Parse(i.UNIDADESTOTAL);
                    //query.InsertCommand.Parameters.Add("@unidadespagadas", SqlDbType.Float).Value = null;
                    query.InsertCommand.Parameters.Add("@precio", SqlDbType.Float).Value = float.Parse(i.PRECIO);
                    query.InsertCommand.Parameters.Add("@dto", SqlDbType.Float).Value = float.Parse(i.DTO);
                    query.InsertCommand.Parameters.Add("@total", SqlDbType.Float).Value = float.Parse(i.TOTAL);
                    query.InsertCommand.Parameters.Add("@tipoimpuesto", SqlDbType.SmallInt).Value = int.Parse(i.TIPOIMPUESTO);
                    query.InsertCommand.Parameters.Add("@iva", SqlDbType.Float).Value = float.Parse(i.IVA);
                    query.InsertCommand.Parameters.Add("@req", SqlDbType.Float).Value = float.Parse(i.REQ);
                    query.InsertCommand.Parameters.Add("@numkg", SqlDbType.Float).Value = 0;
                    query.InsertCommand.Parameters.Add("@codalmacen", SqlDbType.NVarChar).Value = i.CODALMACEN;
                    query.InsertCommand.Parameters.Add("@deposito", SqlDbType.NVarChar).Value = "F";
                    query.InsertCommand.Parameters.Add("@precioventa", SqlDbType.Float).Value = 0;
                    query.InsertCommand.Parameters.Add("@usarcoltallas", SqlDbType.NVarChar).Value = "F";
                    query.InsertCommand.Parameters.Add("@importegastos", SqlDbType.Float).Value = 0;
                    query.InsertCommand.Parameters.Add("@supedido", SqlDbType.NVarChar).Value = i.SUPEDIDO;
                    query.InsertCommand.Parameters.Add("@abonode_numserie", SqlDbType.NVarChar).Value = i.ABONODE_NUMSERIE;
                    query.InsertCommand.Parameters.Add("@abonode_numalbaran", SqlDbType.NVarChar).Value = i.ABONODE_NUMALBARAN;
                    query.InsertCommand.Parameters.Add("@abonode_n", SqlDbType.NVarChar).Value = i.ABONODE_N;

                    query.InsertCommand.ExecuteNonQuery();
                    con.Close();
            }


            }
            catch
            {
                con.Close();
                MessageBox.Show("Error al consultar funcion AlbcompralinInsert()");

            }
            llenaTotalesComp(SerieCompra, int.Parse(NumeroCompra));
        }

        private void AlbcompratotInsert(List<Albcompratot> albcompraTots)
        {

            try
            {
                SqlDataAdapter query = new SqlDataAdapter();

                foreach (var i in albcompraTots)
                {
                    con.Open();
                    query = new SqlDataAdapter();

                    query.InsertCommand = new SqlCommand("INSERT INTO [dbo].[ALBCOMPRATOT]([SERIE],[NUMERO],[N],[NUMLINEA],[BRUTO],[DTOCOMERC],[TOTDTOCOMERC],[DTOPP],[TOTDTOPP],[BASEIMPONIBLE],[IVA],[TOTIVA],[REQ],[TOTREQ],[TOTAL],[ESGASTO],[DESCRIPCION])"
                                                         + "VALUES(@serie,@numero,@n,@numlinea,@bruto,@dtocomerc,@totdtocomerc,@dtopp,@totdtopp,@baseimponible,@iva,@totiva,@req,@totreq,@total,@esgasto,@descripcion)", con);

                    query.InsertCommand.Parameters.Add("@serie", SqlDbType.NVarChar).Value = i.SERIE;
                    query.InsertCommand.Parameters.Add("@numero", SqlDbType.Int).Value = int.Parse(i.NUMERO);
                    query.InsertCommand.Parameters.Add("@n", SqlDbType.NVarChar).Value = i.N;
                    query.InsertCommand.Parameters.Add("@numlinea", SqlDbType.Int).Value = int.Parse(i.NUMLINEA);
                    query.InsertCommand.Parameters.Add("@bruto", SqlDbType.Float).Value = float.Parse(i.BRUTO);
                    query.InsertCommand.Parameters.Add("@dtocomerc", SqlDbType.Float).Value = float.Parse(i.DTOCOMERC);
                    query.InsertCommand.Parameters.Add("@totdtocomerc", SqlDbType.Float).Value = float.Parse(i.TOTDTOCOMERC);
                    query.InsertCommand.Parameters.Add("@dtopp", SqlDbType.Float).Value = float.Parse(i.DTOPP);
                    query.InsertCommand.Parameters.Add("@totdtopp", SqlDbType.Float).Value = float.Parse(i.TOTDTOPP);
                    query.InsertCommand.Parameters.Add("@baseimponible", SqlDbType.Float).Value = float.Parse(i.BASEIMPONIBLE);
                    query.InsertCommand.Parameters.Add("@iva", SqlDbType.Float).Value = float.Parse(i.IVA);
                    query.InsertCommand.Parameters.Add("@totiva", SqlDbType.Float).Value = float.Parse(i.TOTIVA);
                    query.InsertCommand.Parameters.Add("@req", SqlDbType.Float).Value = float.Parse(i.REQ);
                    query.InsertCommand.Parameters.Add("@totreq", SqlDbType.Float).Value = float.Parse(i.TOTREQ);
                    query.InsertCommand.Parameters.Add("@total", SqlDbType.Float).Value = float.Parse(i.TOTAL);
                    query.InsertCommand.Parameters.Add("@esgasto", SqlDbType.NVarChar).Value = i.ESGASTO;
                    query.InsertCommand.Parameters.Add("@descripcion", SqlDbType.NVarChar).Value = i.DESCRIPCION;

                    query.InsertCommand.ExecuteNonQuery();
                    con.Close();
                }

                
            }
            catch
            {
                con.Close();
                MessageBox.Show("Error al consultar funcion AlbcompratotInsert()");

            }
            ActualizaPedidoCompra();

        }

        private void AccionREM()
        {

            int intzeta = UltimoZ + 1;

            con.Open();
            SqlDataAdapter query = new SqlDataAdapter();

            query.InsertCommand = new SqlCommand("INSERT INTO REM_TRANSACCIONES (TERMINAL,CAJA,CAJANUM,Z,TIPO,ACCION,SERIE,NUMERO,N,FECHA,HORA,FO,IDCENTRAL,TALLA,COLOR,CODIGOSTR,SUBTIPO,FECHA2,FECHA3,CAMPOBIT,NUMERO2) VALUES (HOST_NAME(),'','1',@z,'4','0',@Serie,@Ticket,'B',CONVERT(datetime, '30-12-1899',105),CONVERT(datetime, '30-12-1899',105),'0','1','.','.','',NULL,NULL,NULL,NULL,NULL)", con);
            query.InsertCommand.Parameters.Add("@z", SqlDbType.Int).Value = intzeta;
            query.InsertCommand.Parameters.Add("@Serie", SqlDbType.NVarChar).Value = SerieCompra;
            query.InsertCommand.Parameters.Add("@Ticket", SqlDbType.Int).Value = NumeroCompra;
            query.InsertCommand.ExecuteNonQuery();


            con.Close();

        }
        private void AccionREM1()
        {

            //int intzeta = UltimoZ + 1;

            con.Open();
            SqlDataAdapter query = new SqlDataAdapter();

            query.InsertCommand = new SqlCommand("INSERT INTO REM_TRANSACCIONES (TERMINAL,CAJA,CAJANUM,Z,TIPO,ACCION,SERIE,NUMERO,N,FECHA,HORA,FO,IDCENTRAL,TALLA,COLOR,CODIGOSTR,SUBTIPO,FECHA2,FECHA3,CAMPOBIT,NUMERO2) VALUES (HOST_NAME(),'','0','0','5','0',@Serie,@Ticket,'B',CONVERT(datetime, '30-12-1899',105),CONVERT(datetime, '30-12-1899',105),'0','1','.','.','',NULL,NULL,NULL,NULL,NULL)", con);
            //query.InsertCommand.Parameters.Add("@z", SqlDbType.Int).Value = intzeta;
            query.InsertCommand.Parameters.Add("@Serie", SqlDbType.NVarChar).Value = SerieP;
            query.InsertCommand.Parameters.Add("@Ticket", SqlDbType.Int).Value = NumeroP;
            query.InsertCommand.ExecuteNonQuery();


            con.Close();

        }

        public class Albcompratot {
            public string SERIE;
            public string NUMERO;
            public string N;
            public string NUMLINEA;
            public string BRUTO;
            public string DTOCOMERC;
            public string TOTDTOCOMERC;
            public string DTOPP;
            public string TOTDTOPP;
            public string BASEIMPONIBLE;
            public string IVA;
            public string TOTIVA;
            public string REQ;
            public string TOTREQ;
            public string TOTAL;
            public string ESGASTO;
            public string DESCRIPCION;

        }
        public class AlbcompraLin {
            public string NUMSERIE;
            public string NUMALBARAN;
            public string N;
            public string NUMLIN;
            public string CODARTICULO;
            public string REFERENCIA;
            public string DESCIPCION;
            public string COLOR;
            public string TALLA;
            public string UNID1;
            public string UNID2;
            public string UNID3;
            public string UNID4;
            public string UNIDADESTOTAL;
            public string UNIDADESPAGADAS;
            public string PRECIO;
            public string DTO;
            public string TOTAL;
            public string TIPOIMPUESTO;
            public string IVA;
            public string REQ;
            public string NUMKG;
            public string CODALMACEN;
            public string DEPOSITO;
            public string PRECIOVENTA;
            public string USARCOLTALLAS;
            public string IMPORTEGASTOS;
            public string SUPEDIDO;
            public string ABONODE_NUMSERIE;
            public string ABONODE_NUMALBARAN;
            public string ABONODE_N;
            public string CODFORMATO;


        }
        //LLENA OBJETO DE ARTICULOS DE PEDIDOS PARA COMPRA
        public void llenaArticulos(string serie, int numero)
        {

            con.Open();
            string query = "SELECT NUMSERIE, NUMPEDIDO, N, NUMLINEA, CODARTICULO, REFERENCIA, TALLA, COLOR, DESCRIPCION, UNID1, UNID2, UNID3, UNID4, UNIDADESTOTAL, UNIDADESREC, UNIDADESPEN, PRECIO, DTO, TIPOIMPUESTO, IVA, REQ, TOTALLINEA, CODALMACEN, DEPOSITO, PRECIOVENTA FROM  PEDCOMPRALIN WHERE   (NUMSERIE = N'"+serie+"') AND (NUMPEDIDO = "+numero+")";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader reg = cmd.ExecuteReader();
            try
            {
                //if (reg.Read())
                //{
                //    Codproveedor = int.Parse(reg["CODPROVEEDOR"].ToString());
                //    Totalimpuestos = reg["TOTIMPUESTOS"].ToString();
                //    Totalbruto = reg["TOTBRUTO"].ToString();
                //    Totalneto = reg["TOTNETO"].ToString();

                //    con.Close();

                //}

                //else
                //{
                //    con.Close();


                //}

                if (reg.HasRows)
                {
                    while (reg.Read())
                    {
                        AClin.Add(
                            new AlbcompraLin { 
                               NUMSERIE = SerieCompra,
                               NUMALBARAN = NumeroCompra,
                               N="B",
                               NUMLIN= reg["NUMLINEA"].ToString(),
                               CODARTICULO= reg["CODARTICULO"].ToString(),
                               REFERENCIA= reg["REFERENCIA"].ToString(),
                               DESCIPCION= reg["DESCRIPCION"].ToString(),
                               COLOR=".",
                               TALLA=".",
                               UNID1= reg["UNID1"].ToString(),
                               UNID2= reg["UNID2"].ToString(),
                               UNID3="",
                               UNID4="",
                               UNIDADESTOTAL= reg["UNIDADESTOTAL"].ToString(),
                               UNIDADESPAGADAS="NULL",
                               PRECIO= reg["PRECIO"].ToString(),
                               DTO= reg["DTO"].ToString(),
                               TOTAL= reg["TOTALLINEA"].ToString(),
                               TIPOIMPUESTO= reg["TIPOIMPUESTO"].ToString(),
                               IVA= reg["IVA"].ToString(),
                               REQ= reg["REQ"].ToString(),
                               NUMKG="0",
                               CODALMACEN= reg["CODALMACEN"].ToString(),
                               DEPOSITO="F",
                               PRECIOVENTA="0",
                               USARCOLTALLAS="F",
                               IMPORTEGASTOS="0",
                               SUPEDIDO=supedidoCompra,
                               ABONODE_NUMSERIE="NULL",
                               ABONODE_NUMALBARAN="NULL",
                               ABONODE_N="NULL",
                               CODFORMATO="0",                            
                            }                          
                        );
                        
                    }
                    con.Close();
                }
                else
                {
                    con.Close();


                }
            }
            catch
            {
                con.Close();
                MessageBox.Show("Error al consultar funcion llenaArticulos()");

            }
            AlbcompracabInsert();
        }

        //LLENA OBJETO DE TOTALES PARA COMPRA
        public void llenaTotalesComp(string serie, int numero)
        {

            con.Open();
            string query = "SELECT  ALBCOMPRALIN.NUMSERIE, ALBCOMPRALIN.NUMALBARAN, ALBCOMPRALIN.N, SUM(ALBCOMPRALIN.TOTAL) AS BRUTO, ALBCOMPRACAB.DTOCOMERCIAL, ALBCOMPRACAB.TOTDTOCOMERCIAL, ALBCOMPRACAB.DTOPP,(ALBCOMPRACAB.DTOCOMERCIAL+ ALBCOMPRACAB.TOTDTOCOMERCIAL+ ALBCOMPRACAB.DTOPP) AS TOTDTOPP, SUM(ALBCOMPRALIN.TOTAL) AS BASEIMPONIBLE, ALBCOMPRALIN.IVA, CASE WHEN IVA = 0 THEN 0 ELSE(SUM(ALBCOMPRALIN.TOTAL) * IVA) / 100 END AS TOTIVA, ALBCOMPRALIN.REQ, CASE WHEN REQ = 0 THEN 0 ELSE(SUM(ALBCOMPRALIN.TOTAL) * REQ) / 100 END AS TOTREQ, SUM(ALBCOMPRALIN.TOTAL) + CASE WHEN IVA = 0 THEN 0 ELSE(SUM(ALBCOMPRALIN.TOTAL) * IVA) / 100 END + CASE WHEN REQ = 0 THEN 0 ELSE(SUM(ALBCOMPRALIN.TOTAL) * REQ) / 100 END AS TOTAL FROM ALBCOMPRALIN INNER JOIN ALBCOMPRACAB ON ALBCOMPRALIN.NUMSERIE = ALBCOMPRACAB.NUMSERIE AND ALBCOMPRALIN.NUMALBARAN = ALBCOMPRACAB.NUMALBARAN AND ALBCOMPRALIN.N = ALBCOMPRACAB.N WHERE(ALBCOMPRALIN.NUMSERIE = N'"+serie+"') AND(ALBCOMPRALIN.NUMALBARAN = "+numero+") GROUP BY ALBCOMPRALIN.NUMSERIE, ALBCOMPRALIN.NUMALBARAN, ALBCOMPRALIN.N, ALBCOMPRACAB.DTOCOMERCIAL, ALBCOMPRACAB.TOTDTOCOMERCIAL, ALBCOMPRACAB.DTOPP, ALBCOMPRALIN.IVA, ALBCOMPRALIN.REQ, ALBCOMPRALIN.TIPOIMPUESTO";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader reg = cmd.ExecuteReader();
            try
            {
                //if (reg.Read())
                //{
                //    Codproveedor = int.Parse(reg["CODPROVEEDOR"].ToString());
                //    Totalimpuestos = reg["TOTIMPUESTOS"].ToString();
                //    Totalbruto = reg["TOTBRUTO"].ToString();
                //    Totalneto = reg["TOTNETO"].ToString();

                //    con.Close();

                //}

                //else
                //{
                //    con.Close();


                //}

                if (reg.HasRows)
                {
                    int cont = 0;
                    while (reg.Read())
                    {
                        ACTot.Add(
                            new Albcompratot
                            {
                                SERIE=serie,
                                NUMERO=numero.ToString(),
                                N="B",
                                NUMLINEA= cont.ToString(),
                                BRUTO= reg["BRUTO"].ToString(),
                                DTOCOMERC= reg["DTOCOMERCIAL"].ToString(),
                                TOTDTOCOMERC= reg["TOTDTOCOMERCIAL"].ToString(),
                                DTOPP= reg["DTOPP"].ToString(),
                                TOTDTOPP= reg["TOTDTOPP"].ToString(),
                                BASEIMPONIBLE= reg["BASEIMPONIBLE"].ToString(),
                                IVA= reg["IVA"].ToString(),
                                TOTIVA= reg["TOTIVA"].ToString(),
                                REQ= reg["REQ"].ToString(),
                                TOTREQ= reg["TOTREQ"].ToString(),
                                TOTAL= reg["TOTAL"].ToString(),
                                ESGASTO="T",
                                DESCRIPCION="",
                                
                            }
                        );
                        cont++;

                    }
                    con.Close();
                }
                else
                {
                    con.Close();


                }
            }
            catch
            {
                con.Close();
                MessageBox.Show("Error al consultar funcion llenaTotalesComp()");

            }
            AlbcompratotInsert(ACTot);
        }

        private void ActualizaPedidoCompra()
        {


            con.Open();
            SqlDataAdapter query = new SqlDataAdapter();
            query.UpdateCommand = new SqlCommand("UPDATE PEDCOMPRACAB SET  SERIEALBARAN =@serie, NUMEROALBARAN =@numero, FECHAENTREGA =@fecha, TODORECIBIDO ='T' WHERE  (NUMSERIE = N'"+SerieP+"') AND (NUMPEDIDO = "+NumeroP+")", con);

            query.UpdateCommand.Parameters.Add("@serie", SqlDbType.NVarChar).Value = SerieCompra;
            query.UpdateCommand.Parameters.Add("@numero", SqlDbType.NVarChar).Value = NumeroCompra;
            query.UpdateCommand.Parameters.Add("@fecha", SqlDbType.DateTime).Value = DateTime.Now.Date;






            try
            {

                query.UpdateCommand.ExecuteNonQuery();
                //MessageBox.Show("Cambio de Cliente exitoso", "Cambio Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                con.Close();

                

            }
            catch
            {
                con.Close();
                MessageBox.Show("Error ActualizaPedidoCompra()");
            }
            ActualizaPedidoLin(AClin);
        }

        private void ejecuta()
        {



            ProcessStartInfo psi = new ProcessStartInfo();
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            if (arquitectura == "64 bits")
            {
                psi.FileName = "C:\\Windows\\REBELTOOL\\FrontRest64.bat";

            }
            else
            {
                psi.FileName = "C:\\Windows\\REBELTOOL\\FrontRest32.bat";

            }
            Process.Start(psi);

        }
        private void Procesador()
        {
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

        private void ActualizaPedidoLin(List<AlbcompraLin> albcompraLins)
        {


            try
            {
                SqlDataAdapter query = new SqlDataAdapter();

                foreach (var i in albcompraLins)
                {
                  con.Open();
                  query = new SqlDataAdapter();
                  query.UpdateCommand = new SqlCommand("UPDATE  PEDCOMPRALIN SET  UNIDADESREC =@unidadesrec, UNIDADESPEN =@unidadespen  WHERE (NUMSERIE = N'" + SerieP + "') AND (NUMPEDIDO = " + NumeroP + ") AND (CODARTICULO = "+i.CODARTICULO+")", con);

                  query.UpdateCommand.Parameters.Add("@unidadesrec", SqlDbType.Float).Value = float.Parse(i.UNIDADESTOTAL);
                  query.UpdateCommand.Parameters.Add("@unidadespen", SqlDbType.Float).Value = 0;


                

                    query.UpdateCommand.ExecuteNonQuery();
                    //MessageBox.Show("Cambio de Cliente exitoso", "Cambio Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    con.Close();


                    
               
                }
                string mess = "SE RECIBIO EL PEDIDO EL CUAL SE REGISTRO CON EL SIGUIENTE ALBARAN DE COMPRA: " + SerieCompra + "-" + NumeroCompra;
                MessageBox.Show(mess, "ALERTA", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //MessageBox.Show("SE RECIBIO EL PEDIDO EL CUAL SE REGISTRO CON EL SIGUIENTE ALBARAN DE COMPRA: " + SerieCompra + "-" + NumeroCompra);
                AccionREM();
                AccionREM1();
                ActualizaSerieCompra();

                Administrador frm = new Administrador();
                frm.Usuario = acceso;

                frm.Show(this);
                this.Hide();
                ejecuta();
            }
            catch
            {
                con.Close();
                MessageBox.Show("Error ActualizaPedidoLin()");
            }
        }

        private void ActualizaSerieCompra()
        {


            con.Open();
            SqlDataAdapter query = new SqlDataAdapter();
            query.UpdateCommand = new SqlCommand("UPDATE SERIES SET NUMALBCB = @numero WHERE (SERIE = N'"+ SerieCompra + "')", con);

            query.UpdateCommand.Parameters.Add("@numero", SqlDbType.NVarChar).Value = NumeroCompra;







            try
            {

                query.UpdateCommand.ExecuteNonQuery();
                //MessageBox.Show("Cambio de Cliente exitoso", "Cambio Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                con.Close();



            }
            catch
            {
                con.Close();
                MessageBox.Show("Error ActualizaPedidoCompra()");
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Administrador frm = new Administrador();
            frm.Usuario = acceso;

            frm.Show(this);
            this.Hide();
        }
    }
}
