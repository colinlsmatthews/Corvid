using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino;
using Rhino.Geometry;

namespace Raven
{
    public class GHC_RhinoLicenseInfo : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GHC_RhinoLicenseInfo class.
        /// </summary>
        public GHC_RhinoLicenseInfo()
          : base("Rhino License Info", "License",
              "Get information about the current active license for Rhino",
              "Params", "Raven")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("License Owner", "L", "The name of the current license owner.", GH_ParamAccess.item);
            pManager.AddTextParameter("Organization", "O", "The organization of the current license owner", GH_ParamAccess.item);
            pManager.AddTextParameter("License Type", "T", "The type of license currently in use", GH_ParamAccess.item);
            pManager.AddTextParameter("Serial Number", "S", "The serial number of the current license", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string licenseOwner = RhinoApp.LicenseUserName;
            string licenseOrg = RhinoApp.LicenseUserOrganization;
            string install = RhinoApp.InstallationTypeString;
            string serialNumber = RhinoApp.SerialNumber;

            DA.SetData(0, licenseOwner);
            DA.SetData(1, licenseOrg);
            DA.SetData(2, install);
            DA.SetData(3, serialNumber);
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
            get { return new Guid("2549BC26-0159-4F8F-8356-4088516B1D91"); }
        }
    }
}