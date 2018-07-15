using NJsonSchema.CodeGeneration;
using NSwag;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SmartPlugin.ApiClient.CodeGen.Templates
{
    /// <summary>The default template factory which loads templates from embedded resources.</summary>
    public class TemplateFactory : NJsonSchema.CodeGeneration.DefaultTemplateFactory
    {      
        private readonly string TemplateDirectory;
        private readonly string TemplateAssemblyName;
        private readonly List<Assembly> Assemblies;

        /// <summary>Initializes a new instance of the <see cref="TemplateFactory" /> class.</summary>
        /// <param name="settings">The settings.</param>
        /// <param name="assemblies">The assemblies.</param>
        public TemplateFactory(CodeGeneratorSettingsBase settings, string templateAssemblyName , List<Assembly> assemblies)
            : base(settings, assemblies?.ToArray())
        {
            Assemblies = assemblies;
            TemplateAssemblyName = templateAssemblyName;
            TemplateDirectory = settings.TemplateDirectory;
        }

        public TemplateFactory(CodeGeneratorSettingsBase settings)
          : base(settings, null)
        {
            TemplateDirectory = settings.TemplateDirectory;
        }

        /// <summary>Gets the current toolchain version.</summary>
        /// <returns>The toolchain version.</returns>
        protected override string GetToolchainVersion()
        {
            return SwaggerDocument.ToolchainVersion + " (NJsonSchema v" + base.GetToolchainVersion() + ")";
        }

        protected new Assembly GetLiquidAssembly(string name)
        {
          //  var d = Assemblies?.Select(a => a.FullName).ToList();

            Assembly assembly = Assemblies?.FirstOrDefault(a => a.FullName.Contains(name));

            if (assembly == null)
                assembly = Assembly.Load(TemplateAssemblyName);

            if (assembly != null)
                return assembly;

            throw new InvalidOperationException("The assembly '" + name + "' containting liquid templates could not be found.");
        }

        /// <summary>Tries to load an embedded Liquid template.</summary>
        /// <param name="language">The language.</param>
        /// <param name="template">The template name.</param>
        /// <returns>The template.</returns>
        protected override string GetEmbeddedLiquidTemplate(string language, string template)
        {
            var assembly = GetLiquidAssembly(TemplateAssemblyName);
            var resourceName = $"{TemplateAssemblyName}.Templates.{language}.{template}.liquid";

            var resource = assembly.GetManifestResourceStream(resourceName);
            if (resource != null)
            {
                using (var reader = new StreamReader(resource))
                    return reader.ReadToEnd();
            }

            return base.GetEmbeddedLiquidTemplate(language, template);
        }

        ///// <summary>
        ///// Gets the liquid template.
        ///// </summary>
        ///// <param name="language">The language.</param>
        ///// <param name="template">The template.</param>
        ///// <returns></returns>
        //public string GetLiquidTemplate(string language, string template)
        //{
        //    if (!template.EndsWith("!") && !string.IsNullOrEmpty(TemplateDirectory))
        //    {
        //        var templateFilePath = Path.Combine(TemplateDirectory, template + ".liquid");
        //        if (File.Exists(templateFilePath))
        //            return File.ReadAllText(templateFilePath);
        //    }

        //    return GetEmbeddedLiquidTemplate(language, template);
        //}
    }
}
