using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET.MapProviders;
using GMap.NET;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms;
using System.Runtime.InteropServices;
using System.IO;


namespace MapaProyecto1
{
    public partial class Form1 : Form
    {
        //Marcador
        GMarkerGoogle marker;
        //Capa de marcado
        GMapOverlay markerOverlay;

        DataTable dt;

        //Ruta
        bool trazarRuta = false;
        int ContadorIndicadoresRuta = 0;
        PointLatLng inicio;
        PointLatLng final;

        double lat;
        double lng;

        List<PointLatLng> puntos;

        int filaSeleccionada = 0;

        double LatInicial = 15.5041700;
        double LngInicial = -88.0250000;

        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //Agregar filas al datagrid view
            dt = new DataTable();
             dt.Columns.Add(new DataColumn("ID", typeof(double)));
            dt.Columns.Add(new DataColumn("Descripcion", typeof(string)));
            dt.Columns.Add(new DataColumn("Lat", typeof(double)));
            dt.Columns.Add(new DataColumn("Lng", typeof(double)));
           

            //Insertando datos al dt para mostrar en la lista 
         //   dt.Rows.Add(01,"Ubicacion 1", LatInicial, LngInicial);
            dataGridView1.DataSource = dt;

            //Desactivar las columnas lat y long
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[3].Visible = false;

            gMapControl1.DragButton = MouseButtons.Left;
            gMapControl1.CanDragMap = true;
            gMapControl1.MapProvider = GMapProviders.GoogleMap;
            gMapControl1.Position = new PointLatLng(LatInicial, LngInicial); //Ubicando el mapa
            gMapControl1.MinZoom = 0;
            gMapControl1.MaxZoom = 24;
            gMapControl1.Zoom = 15;

            gMapControl1.AutoScroll = true;
          AgregarMarcador("inicial",LatInicial, LngInicial);


        }
        public void AgregarMarcador(string nombre, double LatInicial, double LngInicial)
        {
            markerOverlay = new GMapOverlay("Marcador");
            marker = new GMarkerGoogle(new PointLatLng(LatInicial, LngInicial), GMarkerGoogleType.blue);//Genera Marcador
            markerOverlay.Markers.Add(marker);// le agrega al mapa

            //agregamos un tooltip de texto a los marcadores
            marker.ToolTipMode = MarkerTooltipMode.Always;
            marker.ToolTipText = string.Format("Ubicacion:{0} \n Latitud:{1} \n Longitud:{2}", nombre, LatInicial, LngInicial);

            //agregamos el mapa y el marcador control
            gMapControl1.Overlays.Add(markerOverlay);
            ///////////////////////////

        }

        private void SeleccionarRegistro(object sender, DataGridViewCellEventArgs e)
        {
            filaSeleccionada = e.RowIndex; // Fila seleccionada
           // recuperamos los datos del grid y los asignamos a los txtbox 
           // txtId.Text = dataGridView1.Rows[filaSeleccionada].Cells[0].Value.ToString();
           // txtDescripcion.Text = dataGridView1.Rows[filaSeleccionada].Cells[1].Value.ToString();
           // txtLatitud.Text = dataGridView1.Rows[filaSeleccionada].Cells[2].Value.ToString();
           // txtLongitud.Text = dataGridView1.Rows[filaSeleccionada].Cells[3].Value.ToString();
           
            // se asigna los valores del grid al marcador
            marker.Position = new PointLatLng(lat, lng);
            // se posiciona el foco del mapa en ese punto
            gMapControl1.Position = marker.Position;

        }


        private void btnAgregar_Click(object sender, EventArgs e)
        {
            /*   dt.Rows.Add(txtId.Text,txtDescripcion.Text, txtLatitud.Text, txtLongitud.Text); //agregar a la tabla
               AgregarMarcador(txtDescripcion.Text, lat, lng);
               txtDescripcion.Text = " ";
               txtId.Text = " ";
   */


            //declarrar de variables
            string codigo, desc;

            // entrada de datos

            codigo = Microsoft.VisualBasic.Interaction.InputBox("Ingrese el codigo", "Registro de Datos", "", 300, 300);

            if (codigo.Length <= 0)
            {
                MessageBox.Show("Ingrese valores");
                return;
            }

            else
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (codigo == dataGridView1.Rows[i].Cells[0].Value.ToString())
                    {
                        MessageBox.Show("Este codigo ya existe , vuelva ingresar el codigo");
                        return;


                    }

                }
            }
         
            desc = Microsoft.VisualBasic.Interaction.InputBox("Ingrese la descripcion", "Registro de Datos", "", 300, 300);
            if (desc.Length <= 0)
            {
                MessageBox.Show("Ingrese valores");
                return;
            }
            else
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (desc == dataGridView1.Rows[i].Cells[3].Value.ToString())
                    {
                        MessageBox.Show("Este punto ya existe vuelva a ingresar el punto");
                        return;
                    }

                }

            }
            MessageBox.Show("Se guardo el punto", "Sistema de Datos", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            AgregarMarcador(codigo,lat, lng);

            dt.Rows.Add(codigo, desc,lat.ToString(), lng.ToString());
      

        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {

            string codigo;

            codigo = Microsoft.VisualBasic.Interaction.InputBox("Ingrese el ID del punto a borrar", "Registro de Datos", "", 300, 300);

            MessageBox.Show("Se ha Borrardo el punto", "Sistema de Datos", MessageBoxButtons.OK, MessageBoxIcon.Information);

            
            dataGridView1.Rows.RemoveAt(filaSeleccionada); //revomer de la tabla

        }


        private void gMapControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Se obtiene los de latitud y longitud del mapa donde el usuario presiono
            lat = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lat;
            lng = gMapControl1.FromLocalToLatLng(e.X, e.Y).Lng;
   
            //se posiciona en el txt de la latitud y longitud
         //   txtLatitud.Text = lat.ToString();
           // txtLongitud.Text = lng.ToString();

            //creamos el marcador para moverlo al lugar indicado
             marker.Position = new PointLatLng(lat, lng);

            // tambien se agrega el mensaje al marcador(tooltip)
            marker.ToolTipText = string.Format("Ubicacion \n Latitud:{0} \n Longitud{1}", lat, lng);

   
        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {

        }

            private void btnRuta_Click(object sender, EventArgs e)
        {
            
            string codigo;
            string desde;
            string hasta;

            codigo = Microsoft.VisualBasic.Interaction.InputBox("Ingrese el Codigo de Ruta", "Registro de Datos", "", 300, 300);

            desde = Microsoft.VisualBasic.Interaction.InputBox("Codigo de Punto ACTUAL", "Registro de Datos", "", 300, 300);

            hasta = Microsoft.VisualBasic.Interaction.InputBox("Codigo de Punto donde se dirije", "Registro de Datos", "", 300, 300);

            GMapOverlay Ruta = new GMapOverlay("GuiaRuta");
            Pen estilo = new Pen(Brushes.DarkBlue, 2);
            System.Drawing.Drawing2D.AdjustableArrowCap flecha = new System.Drawing.Drawing2D.AdjustableArrowCap(10, 10);
            estilo.StartCap = System.Drawing.Drawing2D.LineCap.RoundAnchor;
            estilo.CustomEndCap = flecha;
            puntos = new List<PointLatLng>();
            puntos.Add(new PointLatLng(Desde(desde).Item1, Desde(desde).Item2));
            puntos.Add(new PointLatLng(Hasta(hasta).Item1, Hasta(hasta).Item2));
            GMapRoute puntosRuta = new GMapRoute(puntos, "Ruta");
            puntosRuta.Stroke = estilo;
            Ruta.Routes.Add(puntosRuta);
            gMapControl1.Overlays.Add(Ruta);

            /////////////////////////////

            //actualizar el mapa
            gMapControl1.Zoom = gMapControl1.Zoom + 1;
            gMapControl1.Zoom = gMapControl1.Zoom - 1;


        }

        public Tuple<double, double> Desde(string nombre)
        {
            double lat, lng;

            for (int filas = 0; filas < dataGridView1.Rows.Count; filas++)

            {
                if (dataGridView1.Rows[filas].Cells[0].Value.ToString() == nombre)
                {
                    lat = Convert.ToDouble(dataGridView1.Rows[filas].Cells[2].Value);
                    lng = Convert.ToDouble(dataGridView1.Rows[filas].Cells[3].Value);

                    return Tuple.Create(lat, lng);

                }

            }
            return Tuple.Create(0.0, 0.0);

        }

        public Tuple<double, double> Hasta(string nombre)
        {
            double lat, lng;

            double[] datos = new double[1];

            for (int filas = 0; filas < dataGridView1.Rows.Count; filas++)

            {
                if (dataGridView1.Rows[filas].Cells[0].Value.ToString() == nombre)
                {
                    lat = Convert.ToDouble(dataGridView1.Rows[filas].Cells[2].Value);
                    lng = Convert.ToDouble(dataGridView1.Rows[filas].Cells[3].Value);


                    return Tuple.Create(lat, lng);

                }

            }
            return Tuple.Create(0.0, 0.0);

        }

        
    }
}
