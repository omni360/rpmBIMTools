using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI.Events;
using Autodesk.Revit.Exceptions;

namespace rpmBIMTools {

    public static class rpmBIMExtensions
    {
        /// <summary>
        /// Checks if document is a valid NGB Template
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static bool isNGBTemplate(this Document doc)
        {
            DefinitionBindingMapIterator pIterator = doc.ParameterBindings.ForwardIterator();
            pIterator.Reset();
            List<string> list = new List<string>();
            bool isNGB = false;
            while (pIterator.MoveNext())
            {
                Definition def = null;
                def = pIterator.Key;
                if (def.Name == "Sub-Discipline")
                    isNGB = true;
            }
            return isNGB;
        }

        /// <summary>
        /// Checks is sheet has a valid NGB sheet number
        /// </summary>
        /// <param name="v">View Sheet</param>
        /// <returns></returns>
        public static bool IsValidNGBDwgNum(this ViewSheet v)
        {
            string regEx = @"^\w{2}-\w{2}-DR-[\w#]-[\d#]{2}[\d]{2}$";
            return Regex.IsMatch(v.SheetNumber, regEx);
        }

        /// <summary>
        /// Get NGB ViewSheet Status Parameter
        /// </summary>
        /// <param name="v">ViewSheet</param>
        /// <returns></returns>
        public static string GetStatus(this ViewSheet v)
        {
            Parameter p = v.LookupParameter("Status");
            return p != null ? p.AsString() : null;
        }

        /// <summary>
        /// Set NGB ViewSheet Status Parameter
        /// </summary>
        /// <param name="v">ViewSheet</param>
        /// <param name="value">Value to be set to parameter</param>
        public static void SetStatus (this ViewSheet v, string value)
        {
            Parameter p = v.LookupParameter("Status");
            if (p != null) p.Set(value);
        }

        /// <summary>
        /// Get NGB ViewSheet Sub-Discipline Parameter
        /// </summary>
        /// <param name="v">ViewSheet</param>
        /// <returns></returns>
        public static string GetSubDiscipline(this ViewSheet v)
        {
            Parameter p = v.LookupParameter("Sub-Discipline");
            return p != null ? p.AsString() : null;
        }

        /// <summary>
        /// Set NGB ViewSheet Sub-Discipline Parameter
        /// </summary>
        /// <param name="v">ViewSheet</param>
        /// <param name="value">Value to be set to parameter</param>
        public static void SetSubDiscipline(this ViewSheet v, string value)
        {
            Parameter p = v.LookupParameter("Sub-Discipline");
            if (p != null) p.Set(value);
        }

        /// <summary>
        /// Get NGB ViewSheet Drawn By Parameter
        /// </summary>
        /// <param name="v">ViewSheet</param>
        /// <returns></returns>
        public static string GetDrawnBy(this ViewSheet v)
        {
            Parameter p = v.LookupParameter("Drawn By");
            return p != null ? p.AsString() : null;
        }

        /// <summary>
        /// Set NGB ViewSheet Drawn By Parameter
        /// </summary>
        /// <param name="v">ViewSheet</param>
        /// <param name="value">Value to be set to parameter</param>
        public static void SetDrawnBy(this ViewSheet v, string value)
        {
            Parameter p = v.LookupParameter("Drawn By");
            if (p != null) p.Set(value);
        }

        public static List<TreeNode> GetAllNodes(this TreeView _self)
        {
            List<TreeNode> result = new List<TreeNode>();
            foreach (TreeNode child in _self.Nodes)
            {
                result.AddRange(child.GetAllNodes());
            }
            return result;
        }

        public static List<TreeNode> GetAllNodes(this TreeNode _self)
        {
            List<TreeNode> result = new List<TreeNode>();
            result.Add(_self);
            foreach (TreeNode child in _self.Nodes)
            {
                result.AddRange(child.GetAllNodes());
            }
            return result;
        }

        /// <summary>
        /// Returns the parameter linked to the ScheduleField or null
        /// </summary>
        /// <param name="elem">Element</param>
        /// <param name="sf">ScheduelField to return parameter from</param>
        /// <returns></returns>
        public static Parameter GetScheduleParameter(this Element elem, ScheduleField sf)
        {
            FamilyInstance fi = elem as FamilyInstance;
            Parameter rp = (sf.GetName().StartsWith("Room: ") && fi != null && fi.Room != null) ? fi.Room.LookupParameter(sf.GetName().Substring(6)) : null;
            Parameter rbip = (sf.GetName().StartsWith("Room: ") && fi != null && fi.Room != null) ? fi.Room.get_Parameter((BuiltInParameter)sf.ParameterId.IntegerValue) : null;
            Parameter sp = (sf.GetName().StartsWith("Space: ") && fi != null && fi.Space != null) ? fi.Space.LookupParameter(sf.GetName().Substring(7)) : null;
            Parameter sbip = (sf.GetName().StartsWith("Space: ") && fi != null && fi.Space != null) ? fi.Space.get_Parameter((BuiltInParameter)sf.ParameterId.IntegerValue) : null;
            Parameter trp = (sf.GetName().StartsWith("To Room: ") && fi != null && fi.ToRoom != null) ? fi.ToRoom.LookupParameter(sf.GetName().Substring(9)) : null;
            Parameter trbip = (sf.GetName().StartsWith("To Room: ") && fi != null && fi.ToRoom != null) ? fi.ToRoom.get_Parameter((BuiltInParameter)sf.ParameterId.IntegerValue) : null;
            Parameter frp = (sf.GetName().StartsWith("From Room: ") && fi != null && fi.FromRoom != null) ? fi.FromRoom.LookupParameter(sf.GetName().Substring(11)) : null;
            Parameter frbip = (sf.GetName().StartsWith("From Room: ") && fi != null && fi.FromRoom != null) ? fi.FromRoom.get_Parameter((BuiltInParameter)sf.ParameterId.IntegerValue) : null;
            Parameter p = elem.LookupParameter(sf.GetName());
            Parameter tp = elem.GetTypeId() != ElementId.InvalidElementId ? elem.Document.GetElement(elem.GetTypeId()).LookupParameter(sf.GetName()) : null;
            Parameter bip = elem.get_Parameter((BuiltInParameter)sf.ParameterId.IntegerValue);
            Parameter tbip = elem.GetTypeId() != ElementId.InvalidElementId ? elem.Document.GetElement(elem.GetTypeId()).get_Parameter((BuiltInParameter)sf.ParameterId.IntegerValue) : null;
            return
                rp != null ? rp : // Room Parameter Name Found
                rbip != null ? rbip : // Room BuiltInParameter Found
                sp != null ? sp : // Space Parameter Name Found
                sbip != null ? sbip : // Space BuiltInParameter Found
                trp != null ? trp : // To Room Parameter Name Found
                trbip != null ? trbip : // Tom Room BuiltInParameter Found
                frp != null ? frp : // From Room Parameter Name Found
                frbip != null ? frbip : // From Room BuiltInParameter Found
                p != null ? p : // Parameter Name Found
                tp != null ? tp : // Type Parameter Name Found
                bip != null ? bip : // BuiltInParamter Found
                tbip != null ? tbip : // Type BuiltInParameter Found
                null;
        }

        /// <summary>
        /// Returns the parameter linked to parameter Id & Name or null
        /// </summary>
        /// <param name="elem">Element</param>
        /// <param name="parameterId">Parameter Id</param>
        /// <param name="parameterName">Parameter Name</param>
        /// <returns></returns>
        public static Parameter GetScheduleParameter(this Element elem, int parameterId, string parameterName)
        {
            FamilyInstance fi = elem as FamilyInstance;
            Parameter rp = (parameterName.StartsWith("Room: ") && fi != null && fi.Room != null) ? fi.Room.LookupParameter(parameterName.Substring(6)) : null;
            Parameter rbip = (parameterName.StartsWith("Room: ") && fi != null && fi.Room != null) ? fi.Room.get_Parameter((BuiltInParameter)parameterId) : null;
            Parameter sp = (parameterName.StartsWith("Space: ") && fi != null && fi.Space != null) ? fi.Space.LookupParameter(parameterName.Substring(7)) : null;
            Parameter sbip = (parameterName.StartsWith("Space: ") && fi != null && fi.Space != null) ? fi.Space.get_Parameter((BuiltInParameter)parameterId) : null;
            Parameter trp = (parameterName.StartsWith("To Room: ") && fi != null && fi.ToRoom != null) ? fi.ToRoom.LookupParameter(parameterName.Substring(9)) : null;
            Parameter trbip = (parameterName.StartsWith("To Room: ") && fi != null && fi.ToRoom != null) ? fi.ToRoom.get_Parameter((BuiltInParameter)parameterId) : null;
            Parameter frp = (parameterName.StartsWith("From Room: ") && fi != null && fi.FromRoom != null) ? fi.FromRoom.LookupParameter(parameterName.Substring(11)) : null;
            Parameter frbip = (parameterName.StartsWith("From Room: ") && fi != null && fi.FromRoom != null) ? fi.FromRoom.get_Parameter((BuiltInParameter)parameterId) : null;
            Parameter p = elem.LookupParameter(parameterName);
            Parameter tp = elem.GetTypeId() != ElementId.InvalidElementId ? elem.Document.GetElement(elem.GetTypeId()).LookupParameter(parameterName) : null;
            Parameter bip = elem.get_Parameter((BuiltInParameter)parameterId);
            Parameter tbip = elem.GetTypeId() != ElementId.InvalidElementId ? elem.Document.GetElement(elem.GetTypeId()).get_Parameter((BuiltInParameter)parameterId) : null;
            return
                rp != null ? rp : // Room Parameter Name Found
                rbip != null ? rbip : // Room BuiltInParameter Found
                sp != null ? sp : // Space Parameter Name Found
                sbip != null ? sbip : // Space BuiltInParameter Found
                trp != null ? trp : // To Room Parameter Name Found
                trbip != null ? trbip : // Tom Room BuiltInParameter Found
                frp != null ? frp : // From Room Parameter Name Found
                frbip != null ? frbip : // From Room BuiltInParameter Found
                p != null ? p : // Parameter Name Found
                tp != null ? tp : // Type Parameter Name Found
                bip != null ? bip : // BuiltInParamter Found
                tbip != null ? tbip : // Type BuiltInParameter Found
                null;
        }

        /// <summary>
        /// Returns the base value of the parameter based by it's StorageType
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static object AsBaseValue(this Parameter p)
        {
            if (p == null) return string.Empty;
            switch (p.StorageType)
            {
                case StorageType.None:
                    return "N/A";
                case StorageType.Integer:
                    return p.AsInteger();
                case StorageType.Double:
                    return
                        p.Definition.ParameterType == ParameterType.Area ? (p.AsDouble() * 0.092903) :
                        p.Definition.ParameterType == ParameterType.Length ? (p.AsDouble() * 304.8) :
                        p.AsDouble();
                case StorageType.String:
                    return p.AsString();
                case StorageType.ElementId:
                    return p.AsElementId() != ElementId.InvalidElementId ? p.Element.Document.GetElement(p.AsElementId()).Name : p.AsValueString();
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Sets the base value of the parameter based by it's value type
        /// </summary>
        /// <param name="p">Parameter</param>
        /// <param name="value">Object Value</param>
        /// <returns></returns>
        public static bool SetBaseValue(this Parameter p, object value)
        {
            if (p == null) return false;
            if (value == null) value = string.Empty;
            if (p.IsReadOnly) return false;
            //TaskDialog.Show("Test", "Parameter Name: " + p.Definition.Name + "\nParameter Type: " + p.StorageType.ToString() + "\nValue Set To: " + value.ToString());
            if (value is double)
            {
                //TaskDialog.Show("Test", "Value is a double");
                double doubleValue = p.Definition.ParameterType == ParameterType.Area ? Convert.ToDouble(value) * 10.7639 :
                    p.Definition.ParameterType == ParameterType.Length ? Convert.ToDouble(value) * 0.00328084 : Convert.ToDouble(value);
                p.Set(doubleValue);
                //TaskDialog.Show("Test", "value is set");
                return true;
            }
            if (value is int)
            {
                //TaskDialog.Show("Test", "Value is a integer");
                p.Set(Convert.ToInt32(value));
                //TaskDialog.Show("Test", "value is set");
                return true;
            }
            if (value is string)
            {
                //TaskDialog.Show("Test", "Value is a string");
                p.Set(Convert.ToString(value));
                //TaskDialog.Show("Test", "value is set");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Splits camel case strings with spaces to make them readable.
        /// </summary>
        /// <param name="str">The String</param>
        /// <returns></returns>
        public static string SplitCamelCase(this string str)
        {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );
        }

        /// <summary>
        /// Checks if the file is being used by another resource.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsFileLocked(this FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (System.IO.IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return false;
        }

        /// <summary>
        /// Returns a ViewFamilyType used for creating a view.
        /// </summary>
        /// <param name="doc">The document to check.</param>
        /// <param name="viewFamily">viewFamily type to return.</param>
        /// <returns></returns>
        public static ViewFamilyType GetViewFamilyType(this Document doc, ViewFamily viewFamily)
        {
            return new FilteredElementCollector(doc)
                    .OfClass(typeof(ViewFamilyType))
                    .Cast<ViewFamilyType>()
                    .FirstOrDefault(q => q.ViewFamily == viewFamily);
        }

        /// <summary>
        /// Purges all elements unused from the document.
        /// </summary>
        /// <param name="doc">The Document.</param>
        public static IList<ElementId> PurgeUnused(this Document doc)
        {
            // Deleted Counter
            List<ElementId> deleted = new List<ElementId>();

            // Get Family Data
            List<ElementId> familyInstances = new FilteredElementCollector(doc)
               .OfClass(typeof(FamilyInstance))
               .Cast<FamilyInstance>()
               .Select<FamilyInstance, ElementId>(i => i.Symbol.Id)
               .ToList<ElementId>();
            List<FamilySymbol> familySymbols = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilySymbol))
                .Cast<FamilySymbol>()
                .ToList<FamilySymbol>();
            
            // Collect Family Data
            List<ElementId> symbolsUsed = new List<ElementId>();
            foreach (ElementId symbolId in familyInstances)
            {
                if (!symbolsUsed.Contains(symbolId)) { symbolsUsed.Add(symbolId); }
            }

            // Purge Families
            using (Transaction t = new Transaction(doc, "Purging Unused Families"))
            {
                t.Start();
                foreach (FamilySymbol symbol in familySymbols)
                {
                    // TaskDialog.Show("test", "Family: " + test.FamilyName + "\nFamily Symbol: " + test.Name + "\nNumber of symbols :" + test.Family.GetFamilySymbolIds().Count());
                    if (!symbolsUsed.Contains(symbol.Id))
                    {
                        int symbolCount = symbol.Family.GetFamilySymbolIds().Count();
                        if (symbolCount == 1) {
                            deleted.AddRange(doc.Delete(symbol.Family.Id));
                        } else {
                            deleted.AddRange(doc.Delete(symbol.Id));
                        }
                    }
                }
                t.Commit();
            }    
            return deleted;

        }

        /// <summary>
        /// Closes all views, but the current active view.
        /// </summary>
        /// <param name="doc">The document to check.</param>
        public static void CloseViews(this Document doc)
        {
            foreach (UIView uiv in rpmBIMTools.Load.uiApp.ActiveUIDocument.GetOpenUIViews())
            {
                if (uiv.ViewId != doc.ActiveView.Id) { uiv.Close(); }
            }

        }

        /// <summary>
        /// Closes all views, but the selected view
        /// </summary>
        /// <param name="doc">The document to check.</param>
        /// <param name="view">View Id to keep open.</param>
        public static void CloseViews(this Document doc, ElementId viewId)
        {
            foreach (UIView uiv in rpmBIMTools.Load.uiApp.ActiveUIDocument.GetOpenUIViews())
            {
                if (uiv.ViewId != viewId) { uiv.Close(); }
            }
        }

        /// <summary>
        /// Returns the value or defaults to an empty string
        /// </summary>
        /// <param name="p">Parameter to be returned</param>
        /// <returns></returns>
        public static string AsStringOrDefault(this Parameter p)
        {
            return p != null ? p.AsString() : string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey> 
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
