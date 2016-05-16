using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using Autodesk.Revit.Exceptions;

namespace rpmBIMTools {

    public static class rpmBIMExtensions
    {
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
        /// Checks if document is a valid NGB Template
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static bool isNGBTemplate(this Document doc)
        {
            DefinitionBindingMapIterator pIterator = doc.ParameterBindings.ForwardIterator();
            pIterator.Reset();
            List<string> list = new List<string>();
            bool notNGB = true;
            while (pIterator.MoveNext())
            {
                Definition def = null;
                def = pIterator.Key;
                if (def.Name == "Sub-Discipline")
                    notNGB = false;
            }
            return notNGB;
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
