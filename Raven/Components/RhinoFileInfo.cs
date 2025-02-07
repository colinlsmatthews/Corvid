using System;
using System.IO;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino;
using Rhino.Geometry;

namespace Raven.Components
{
    public class RhinoFileInfo : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the RhinoFileInfo class.
        /// </summary>
        public RhinoFileInfo()
          : base("Rhino File Info", "File Info",
              "Get information about the currently opened Rhino file.",
              "Rhino", "Raven")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("File Name", "N", "The name of the currently opened Rhino file.", GH_ParamAccess.item);
            pManager.AddTextParameter("File Path", "P", "The path of the currently opened Rhino file.", GH_ParamAccess.item);
            pManager.AddTextParameter("Date Created", "C", "The date the currently opened Rhino file was created.", GH_ParamAccess.item);
            pManager.AddTextParameter("Date Modified", "M", "The date the currently opened Rhino file was last modified.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            RhinoDoc document = RhinoDoc.ActiveDoc;
            string docName = string.Empty;
            string path = document.Path;
            DateTime dateCreated = new DateTime(2000, 01, 01);
            DateTime dateModified = new DateTime(2000, 01, 01);


            if (path != null && File.Exists(path))
            {
                docName = document.Name.Split('.')[0];
                FileInfo file = new FileInfo(path);
                dateCreated = file.CreationTime;
                dateModified = file.LastWriteTime;
            }

            if (dateCreated == new DateTime(2000, 01, 01))
            {
                DA.SetData(0, "Not saved yet!"); // Set warning for name
                DA.SetData(1, "Not saved yet!"); // Set warning for path
                DA.SetData(2, "Not saved yet!"); // Set warning for date created
                DA.SetData(3, "Not saved yet!"); // Set warning for date modified
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning,
                                        "This document is brand new and has not been saved yet.");
            }

            else
            {
                DA.SetData(0, docName);
                DA.SetData(1, path);
                DA.SetData(2, dateCreated.GetDateTimeFormats()[17]);
                DA.SetData(3, dateModified.GetDateTimeFormats()[17]);
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
            get { return new Guid("94CB6D21-98C3-408E-A602-E4B6FB4F9C47"); }
        }
    }
}