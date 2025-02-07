using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.DocObjects.Tables;

namespace Raven.Components
{
    public class UserTxtExport : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the UserTxtExport class.
        /// </summary>
        public UserTxtExport()
          : base("Export Document User Text", "Export",
              "Export document user text to a .csv or .txt file",
              "Rhino", "Raven")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("File Path", "F", "The file path to export to", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Export", "E", "Export the user text to a .csv file", GH_ParamAccess.item, false);
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Summary", "S", "Summary of the export operation", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Retrieve inputs
            string filePath = string.Empty;
            bool export = false;
            if (!DA.GetData(0, ref filePath)) return;
            if (!DA.GetData(1, ref export)) return;

            
            if(string.IsNullOrEmpty(filePath))
            {
                DA.SetData(0, "No file path provided.");
                return;
            }



            // Get the active Rhino document
            Rhino.RhinoDoc document = Rhino.RhinoDoc.ActiveDoc;
            if (document == null)
            {
                DA.SetData(0, "No active Rhino document.");
                return;
            }


            var userData = document.Strings;
            if (userData == null || userData.Count == 0)
            {
                DA.SetData(0, "No document user text found.");
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "No document user text found.");
                return;
            }

            List<string> csvLines = new List<string>();
            //csvLines.Add("Key,Value");

            string[] keyArray = new string[userData.Count];
            string[] valueArray = new string[userData.Count];
            for (int i = 0; i < userData.Count; i++)
            {
                string key = userData.GetKey(i);
                string value = userData.GetValue(i);

                // Properly escape any quotes in keys and values
                string formattedkey = "\"" + key.Replace("\"", "\"\"") + "\"";
                string formattedValue = "\"" + value.Replace("\"", "\"\"") + "\"";
                csvLines.Add($"{formattedkey},{formattedValue}");
            }


            if (filePath.EndsWith(".csv", StringComparison.OrdinalIgnoreCase) 
                || filePath.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
            {
                // Try to write the CSV file to the given file path
                if (export)
                {
                    try
                    {
                        System.IO.File.WriteAllLines(filePath, csvLines);
                        DA.SetData(0, $"Exported {userData.Count} items to CSV file: {filePath}");
                    }
                    catch (Exception ex)
                    {
                        DA.SetData(0, $"Error exporting CSV: {ex.Message}");
                    }
                }
                else
                {
                    DA.SetData(0, "Ready to export.");
                }
            }
            else
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "File path must end with .csv or .txt");
                DA.SetData(0, "File path must end with .csv or .txt");
                return;
            }
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("DD89BBD0-3447-436F-86C7-5347C62623AE"); }
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.quarternary; }
        }
    }
}