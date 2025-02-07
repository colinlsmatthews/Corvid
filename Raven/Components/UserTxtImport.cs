using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Rhino.DocObjects.Tables;

namespace Raven.Components
{
    public class UserTxtImport : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the UserTxtImport class.
        /// </summary>
        public UserTxtImport()
          : base("Import Document User Text", "Import",
              "Import document user text from a .csv or .txt file",
              "Rhino", "Raven")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("File Path", "F", "The file path to import from", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Import", "I", "Import the user text to the Rhino document", GH_ParamAccess.item, false);
            pManager[1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Summary", "S", "Summary of the import operation", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Retrieve inputs
            string filePath = string.Empty;
            bool doImport = false;
            if (!DA.GetData(0, ref filePath)) return;
            DA.GetData(1, ref doImport);

            if (string.IsNullOrEmpty(filePath))
            {
                DA.SetData(0, "No file path provided.");
                return;
            }

            // Validate file extension
            if (!(filePath.EndsWith(".csv", StringComparison.OrdinalIgnoreCase) ||
                  filePath.EndsWith(".txt", StringComparison.OrdinalIgnoreCase)))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "File path must end with .csv or .txt");
                DA.SetData(0, "File path must end with .csv or .txt");
                return;
            }

            // Check if the file exists
            if (!File.Exists(filePath))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "File not found.");
                DA.SetData(0, "File not found.");
                return;
            }

            // If import flag is false, then do nothing
            if (!doImport)
            {
                DA.SetData(0, "Ready to import.");
                return;
            }

            // Read all lines from the file
            string[] lines;
            try
            {
                lines = File.ReadAllLines(filePath);
            }
            catch (Exception ex)
            {
                DA.SetData(0, $"Error reading file: {ex.Message}");
                return;
            }

            // Get the active Rhino document
            Rhino.RhinoDoc document = Rhino.RhinoDoc.ActiveDoc;
            if (document == null)
            {
                DA.SetData(0, "No active Rhino document.");
                return;
            }

            // Prepare a regex pattern to match the CSV line format
            // Each line is assumed to be in the format: "key","value"
            string pattern = "^\"(?<key>(?:[^\"]|\"\")*)\",\"(?<value>(?:[^\"]|\"\")*)\"$";
            Regex regex = new Regex(pattern);

            int importedCount = 0;
            List<string> failedLines = new List<string>();

            // Process each line in the file
            foreach (string line in lines)
            {
                // Skip empty lines
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                Match match = regex.Match(line);
                if (match.Success)
                {
                    // Extract the key and value; unescape double quotes (i.e. "" -> ")
                    string key = match.Groups["key"].Value.Replace("\"\"", "\"");
                    string value = match.Groups["value"].Value.Replace("\"\"", "\"");

                    // Set the document user text.
                    // This call overwrites an existing key if it already exists.
                    try
                    {
                        document.Strings.SetString(key, value);
                        importedCount++;
                    }
                    catch (Exception ex)
                    {
                        // If there's an error, add the line to the failed lines list
                        failedLines.Add(line);
                    }

                }
                else
                {
                    // The line didn't match the expected CSV format
                    failedLines.Add(line);
                }
            }

            // Prepare the summary message
            string summary = $"Imported {importedCount} item(s) from file: {filePath}";
            if (failedLines.Count > 0)
            {
                summary += $". Failed to import {failedLines.Count} line(s).";
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Some lines could not be parsed.");
            }
            DA.SetData(0, summary);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("F3B2C1D8-1234-5678-ABCD-9876543210EF"); }
        }

        public override GH_Exposure Exposure
        {
            get { return GH_Exposure.quarternary; }
        }
    }
}
