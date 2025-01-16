using System;
using System.Collections.Generic;
using System.Drawing;
using Grasshopper.Kernel;
using Rhino;
using Rhino.Geometry;

namespace Raven
{
    public class GHC_RhinoUserInfo : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GHC_RhinoUserInfo class.
        /// </summary>
        public GHC_RhinoUserInfo()
          : base("Rhino User Info", "User Info",
              "Gets info for the current user",
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
            pManager.AddTextParameter("Full Name", "N", "The full name of the current user.", GH_ParamAccess.item);
            pManager.AddTextParameter("First Name", "F", "The first name of the current user.", GH_ParamAccess.item);
            pManager.AddTextParameter("Last Name", "L", "The last name of the current user.", GH_ParamAccess.item);
            pManager.AddTextParameter("Email Address", "E", "The email address for the current user.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Avatar", "A", "The avatar image for the current user.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string user = RhinoApp.LoggedInUserName;
            Image avatar = RhinoApp.LoggedInUserAvatar;
            string[] userList = user.Split(new char[2] { ' ', ',' });

            DA.SetData(0, userList[2] + " " + userList[0]);
            DA.SetData(1, userList[2]);
            DA.SetData(2, userList[0]);
            DA.SetData(3, userList[4]);
            DA.SetData(4, avatar);
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
            get { return new Guid("7E7A7238-ABCC-4DD7-AB7B-CD4EDD89B432"); }
        }
    }
}