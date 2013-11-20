using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.Example
{
    public partial class Apply_Imports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ExecuteTransformer();
        }

        private void ExecuteTransformer()
        {
            Minx.Xslt.TransformerSettings settings = new Minx.Xslt.TransformerSettings()
            {
                EnableDebug = true,
                EnableDocumentFunction = true
            };

            Minx.Xslt.Transformer transformer = new Minx.Xslt.Transformer(settings);

            transformer.Load(new System.IO.FileInfo(this.Context.Server.MapPath(
                "~/App_Data/Stylesheets/apply-imports/importing.xslt")));

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            Minx.Xslt.TransformArguments arguments = new Minx.Xslt.TransformArguments();

            arguments.FunctionProviders.Add(new WebApplication.ExtensionFunctions.UIExtensions());

            string data = this.Context.Server.MapPath("~/App_Data/Xml/apply-imports.xml");
            using (System.Xml.XmlReader reader = System.Xml.XmlReader.Create(data))
            {
                transformer.Transform(reader, sb, arguments);
            }

            this.LitTransformationResult.Text = sb.ToString();

        }
    }
}