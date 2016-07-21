using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.IO;

using Autodesk.Revit.DB;

namespace rpmBIMTools
{

    /// <summary>
    /// A collection of utilities used for rpmBIMTools Projects.
    /// </summary>
    public class rpmBIMUtils
    {
        /// <summary>
        /// Returns a valid NGB title block if it exists
        /// </summary>
        /// <param name="doc">The Document.</param>
        /// <param name="titleBlockName">Find by title block name.</param>
        /// <returns></returns>
            public static FamilySymbol findNGBTitleBlock(Document doc, string titleBlockName)
            {
                IEnumerable<FamilySymbol> titleBlocks = from elem in new FilteredElementCollector(doc)
                                                       .OfClass(typeof(FamilySymbol))
                                                       .OfCategory(BuiltInCategory.OST_TitleBlocks)
                                                    let type = elem as FamilySymbol
                                                    where type.Name.Contains(titleBlockName) && type.Family.Name.Contains("NGB")
                                                    select type;
                return titleBlocks.FirstOrDefault();
            }
        /// <summary>
        /// Returns a collection of ElementIds for all line styles used in a document.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <returns></returns>
        public static ICollection<ElementId> GetLineStyles(Document doc)
        {
            Transaction t = new Transaction(doc, "Get Line Styles");
            t.Start();
            ViewDrafting vd = ViewDrafting.Create(doc, doc.GetViewFamilyType(ViewFamily.Drafting).Id);
            DetailCurve dc = doc.Create.NewDetailCurve(vd, Line.CreateBound(new XYZ(0, 0, 0), new XYZ(1, 0, 0)));
            ICollection<ElementId> lineStyles = dc.GetLineStyleIds();
            t.RollBack();
            return lineStyles;
        }
        /// <summary>
        /// Returns a Element within a document based on type and name.
        /// </summary>
        /// <param name="doc">The document to check.</param>
        /// <param name="targetType">Type of element, use typeof(Type).</param>
        /// <param name="targetName">Name of element.</param>
        /// <returns></returns>
        public static Element FindElementByName(Document doc, Type targetType, string targetName)
        {
            return new FilteredElementCollector(doc)
              .OfClass(targetType)
              .FirstOrDefault<Element>(
                e => e.Name.Equals(targetName));
        }
        /// <summary>
        /// Returns a Element within a document based on type, category and name.
        /// </summary>
        /// <param name="doc">The document to check.</param>
        /// <param name="targetType">Type of element, use typeof(Type).</param>
        /// <param name="category">Category of element, use BuiltInCategory class</param>
        /// <param name="targetName">Name of element.</param>
        /// <returns></returns>
        public static Element FindElementByName(Document doc, Type targetType, BuiltInCategory category, string targetName)
        {
            return new FilteredElementCollector(doc)
              .OfClass(targetType)
               .OfCategory(category)
              .FirstOrDefault<Element>(
                e => e.Name.Equals(targetName));
        }
        /// <summary>
        /// Clean filename so it can be used as a valid system path
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        public static string GetSafeFilename(string filename)
        {
            filename = string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
            filename = filename.Replace(" ", "_");
            return filename;
        }
        /// <summary>
        /// Clean family name so it can be used as a valid string for naming
        /// </summary>
        /// <param name="familyName">The filename.</param>
        /// <returns></returns>
        public static string GetSafeFamilyName(string familyName)
        {
            familyName = Regex.Replace(familyName, @"[^0-9a-zA-Z-]", "_");
            return familyName;
        }
        /// <summary>
        /// Clean family name so it can be used as a valid string for naming
        /// </summary>
        /// <param name="familyName">The filename.</param>
        /// <returns></returns>
        public static string GetSafeFamilyName2(string familyName)
        {
            familyName = Regex.Replace(familyName, @"[^0-9a-zA-Z-]", "-");
            return familyName;
        }
        /// <summary>
        /// Add a timestamp at the end of a file name
        /// </summary>
        /// <param name="fileName">String </param>
        /// <returns></returns>
        public static string AppendTimeStamp(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return string.Concat(
                Path.GetFileNameWithoutExtension(fileName),
                DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                Path.GetExtension(fileName)
                );
        }
        /// <summary>
        /// Searches the document for the given family name or loads from the location given.
        /// </summary>
        /// <param name="doc">The document to find or load.</param>
        /// <param name="familyName">The Family Name to check for.</param>
        /// <param name="familyLocation">The Family locaytion to load from</param>
        /// <returns></returns>
        public static FamilySymbol findOrLoadFamilySymbol(Document doc, string familyName, string familyLocation)
        {
            Family family = FindElementByName(rpmBIMTools.Load.liveDoc, typeof(Family), familyName) as Family;
            using (Transaction t = new Transaction(doc, "Loading Family"))
            {
                t.Start();
                if (family == null) doc.LoadFamily(familyLocation, out family);
                FamilySymbol fs = doc.GetElement(family.GetFamilySymbolIds().FirstOrDefault()) as FamilySymbol;
                fs.Activate();
                t.Commit();
                return fs;
            }
        }
        /// <summary>
        /// Returns the center reference line in a Family Instance.
        /// </summary>
        /// <param name="fi">Family Instance to be used.</param>
        /// <param name="useYAxis"><b>(Optionable)</b> Return Horizontal reference line, Default is to use verticle reference.</param>
        /// <returns></returns>
        public static Reference findReference(FamilyInstance fi, bool useYAxis = false)
        {
            Reference Reference = null;
            Options opt = new Options();
            opt.ComputeReferences = true;
            opt.IncludeNonVisibleObjects = true;
            opt.View = fi.Document.ActiveView;
            GeometryInstance geoElement = fi.get_Geometry(opt).First() as GeometryInstance;
            foreach (var obj in geoElement.GetSymbolGeometry())
            {
                Line lineObj = obj as Line;
                if (lineObj != null)
                {
                    if (useYAxis == false)
                    {
                        if (lineObj.GetEndPoint(0).X == 0 && lineObj.GetEndPoint(1).X == 0)
                        {
                            Reference = lineObj.Reference; break;
                        }
                    }
                    else
                    {
                        if (lineObj.GetEndPoint(0).Y == 0 && lineObj.GetEndPoint(1).Y == 0)
                        {
                            Reference = lineObj.Reference; break;
                        }
                    }
                }
            }
            return Reference;
        }
    }
}
