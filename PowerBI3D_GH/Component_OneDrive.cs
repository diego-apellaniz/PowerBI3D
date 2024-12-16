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
    public class Component_OneDrive : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public Component_OneDrive()
          : base("One Drive", "One Drive",
            "Gets the direct link to a Rhino model hosted in priavte One Drive.",
            "Power BI", "Power BI")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Share link", "Link", "Share link of your Rhino model in PRIVATE One Drive.", GH_ParamAccess.item);
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
            //// Get input;
            //string filepath = "";
            //DA.GetData(0, ref filepath);

            //// Do stuff
            //string base64Value = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(filepath));
            //string encodedUrl = "u!" + base64Value.TrimEnd('=').Replace('/', '_').Replace('+', '-');
            //string resultUrl = string.Format("Converted URL: " + "https://api.onedrive.com/v1.0/shares/{0}/root/content", encodedUrl);

            //// Output
            //DA.SetData(0, resultUrl);

            // Get input
            string filepath = "";
            DA.GetData(0, ref filepath);

            string resultUrl;

            // Check if the link is a personal OneDrive link
            if (filepath.Contains("1drv.ms"))
            {
                // Convert personal OneDrive link to direct download link
                string base64Value = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(filepath));
                string encodedUrl = "u!" + base64Value.TrimEnd('=').Replace('/', '_').Replace('+', '-');
                resultUrl = $"https://api.onedrive.com/v1.0/shares/{encodedUrl}/root/content";
            }
            else if (filepath.Contains("sharepoint.com") && filepath.Contains("."))
            {
                // Check for presence of a file extension by looking for a period in the last segment of the URL
                int lastSlashIndex = filepath.LastIndexOf('/');
                string lastSegment = filepath.Substring(lastSlashIndex + 1);
                if (lastSegment.Contains("."))
                {
                    int queryIndex = filepath.IndexOf('?');
                    if (queryIndex != -1)
                    {
                        resultUrl = filepath.Substring(0, queryIndex) + "?download=1";
                    }
                    else
                    {
                        resultUrl = filepath + "?download=1";
                    }
                }
                else
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Invalid link format. When creating the file link, use \"Get link\", then use \"People with existing access\" to get the direct link to the file.");
                    return;
                }
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Link format not supported.");
                return;
            }

            // Output
            DA.SetData(0, resultUrl);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.one_drive;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("c8d32dad-bd6e-44de-8cdf-29583b03ba1b");

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.quarternary; }
        }

    }    
}