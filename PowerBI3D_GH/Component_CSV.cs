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
    public class Component_CSV : GH_Component
    {
        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public Component_CSV()
          : base("CSV for Power BI", "CsvPBI",
          "Converts data to CSV format for Power BI",
            "Power BI", "Power BI")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Column Headings", "Headings", "List of column headings for the CSV.", GH_ParamAccess.list);
            pManager.AddTextParameter("Data Tree", "Data", "DataTree<string> with the values for each column.", GH_ParamAccess.tree);
            pManager.AddTextParameter("Filename", "Filename", "The full path to the file where the CSV will be saved", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Save to File", "Save", "Set to true to save the CSV output to a file", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("CSV Output", "CSV", "CSV formatted string", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<string> headings = new List<string>();
            GH_Structure<GH_String> dataTree = new GH_Structure<GH_String>();
            string filename = "";
            bool saveToFile = false;

            if (!DA.GetDataList(0, headings)) return;
            if (!DA.GetDataTree(1, out dataTree)) return;
            if (!DA.GetData(2, ref filename)) return;
            if (!DA.GetData(3, ref saveToFile)) return;

            // Check if the number of headings matches the number of branches
            if (headings.Count != dataTree.Branches.Count)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Number of headings does not match the number of branches in the data tree.");
                return;
            }

            // Check if all branches have the same number of items
            int itemCount = dataTree.Branches[0].Count;
            if (!dataTree.Branches.All(branch => branch.Count == itemCount))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Not all branches have the same number of items.");
                return;
            }

            // Check for semicolons in the data
            if (dataTree.Branches.Any(branch => branch.Any(item => item.Value.Contains(";"))))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Data contains semicolons which are not allowed in CSV values.");
                return;
            }

            // Construct CSV string using semicolon as separator
            var csv = new System.Text.StringBuilder();
            csv.AppendLine(string.Join(";", headings)); // Add headings with semicolon

            for (int i = 0; i < itemCount; i++) // For each item in branches
            {
                var row = dataTree.Branches.Select(branch => branch[i].Value).ToList();
                csv.AppendLine(string.Join(";", row)); // Add row values with semicolon
            }

            string csvContent = csv.ToString();
            DA.SetData(0, csvContent);

            // Check if user wants to save the CSV to a file
            if (saveToFile)
            {
                try
                {
                    File.WriteAllText(filename, csvContent);
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, "CSV file saved successfully.");
                }
                catch (Exception ex)
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Failed to save CSV file: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// You can add image files to your project resources and access them like this:
        /// return Resources.IconForThisComponent;
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.csv;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("31b84ebb-e544-4c38-9ebf-28aaf4a13d73");

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.secondary; }
        }

    }    
}