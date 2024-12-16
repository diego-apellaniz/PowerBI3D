using Grasshopper;
using Grasshopper.Kernel;
using PowerBI3D_GH.Properties;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace PowerBI3D_GH
{
    public class Component_Localhost : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public Component_Localhost()
          : base("Localhost", "Localhost",
            "Host the Rhino 3D model in localhost. The model is only accessible through your computer...",
            "Power BI", "Power BI")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Run", "Run", "Start the http listener.", GH_ParamAccess.item, false);
            pManager.AddIntegerParameter("Port", "Port Number", "Localhost port number.", GH_ParamAccess.item, 8888);
            pManager[1].Optional = true;
            pManager.AddTextParameter("Model path", "Model", "File path to the Rhino model.", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("URL", "URL", "URL to copy in the Power BI custom visual to load your model.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Get input
            bool run = false;
            int port = 0;
            string filepath = "";
            DA.GetData(0, ref run);
            if(!DA.GetData(1, ref port))
            {
                port = 8888;
            }
            DA.GetData(2, ref filepath);

            // Check input
            if(port < 0 || port >= 65535)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Select a valid port!");
                return;
            }
            if(string.IsNullOrEmpty(filepath))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "The provided file path is invalid!");
                return;
            }
            var fullPath = Path.GetFullPath(filepath);
            if (!Path.GetExtension(fullPath).Equals(".3dm", StringComparison.OrdinalIgnoreCase))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "The provided file path is not a rhino model!");
                return;
            }


            // Start listener
            if (!run)
            {
                return;
            }

            // Start the listener on a separate thread
            Thread listenerThread = new Thread(() => StartHttpListener(port, fullPath));
            listenerThread.IsBackground = true;
            listenerThread.Start();

            // Output
            var url_string = "http://localhost:" + port.ToString() + "/";
            DA.SetData(0, url_string); 
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.localhost_logo;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("6bda6cad-fcb7-4cae-9721-37072618ab7f");

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.hidden; }
        }

        static void StartHttpListener(int port, string filePath)
        {
            HttpListener listener = new HttpListener();
            string prefix = "http://localhost:" + port + "/";
            //string prefix = "https://localhost:" + port + "/"; // https not really working
            listener.Prefixes.Add(prefix);

            try
            {
                listener.Start();
                Console.WriteLine("Listening for connections on " + prefix);

                while (listener.IsListening) // Use a proper condition to allow graceful exit
                {
                    HttpListenerContext context = listener.GetContext();
                    ProcessRequest(context, filePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Listener error: " + ex.Message);
            }
            finally
            {
                listener.Close();
            }
        }

        static void ProcessRequest(HttpListenerContext context, string filePath)
        {
            HttpListenerResponse response = context.Response;
            try
            {
                if (File.Exists(filePath))
                {
                    //byte[] fileBytes = File.ReadAllBytes(filePath);
                    //response.ContentType = "application/octet-stream";
                    //response.ContentLength64 = fileBytes.Length;
                    //response.OutputStream.Write(fileBytes, 0, fileBytes.Length);

                    byte[] fileBytes = File.ReadAllBytes(filePath);
                    response.ContentType = "application/octet-stream";

                    // Extract the filename from the filePath
                    string fileName = Path.GetFileName(filePath);

                    // Set the Content-Disposition header with the filename
                    response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");

                    response.ContentLength64 = fileBytes.Length;
                    response.OutputStream.Write(fileBytes, 0, fileBytes.Length);
                }
                else
                {
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Console.WriteLine("Request processing error: " + ex.Message);
            }
            finally
            {
                response.OutputStream.Close();
            }
        }
    }    
}