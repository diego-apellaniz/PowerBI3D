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
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using System.Linq;

namespace PowerBI3D_GH
{
    public class Component_GetCustomVisual : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public Component_GetCustomVisual()
          : base("GetCustomVisual", "CustomVisual",
          "Instructions to import the Rhino custom visual into Power BI",
            "Power BI", "Power BI")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Copy", "C", "Set to true to copy the file path to the clipboard", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Instruction", "I", "Instructions to import the visual into Power BI", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool copy = false;
            if (!DA.GetData(0, ref copy)) return;

            string instruction = "1) Connect button to component and press to copy file path.\n" +
                                 "2) Open Power BI, go to Visualizations panel, Get more visuals and select 'Import a visual from a file'.\n" +
                                 "3) Paste the copied file path and accept.";
            DA.SetData(0, instruction);

            if (copy)
            {
                string assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string folderPath = Path.GetDirectoryName(assemblyLocation);
                string filePath = Path.Combine(folderPath, "3DVisor.pbiviz");

                try
                {
                    System.Windows.Forms.Clipboard.SetText(filePath);
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, "File path copied to clipboard.");
                }
                catch (Exception ex)
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Failed to copy file path: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.logo_powerbi3d_small;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("bd8453e3-d5e6-4baa-a41c-e0005b7f61b1");

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.tertiary; }
        }

    }    
}