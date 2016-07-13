using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;

namespace rpmBIMTools
{
    [TransactionAttribute(TransactionMode.Manual)]

    public partial class toggleSectionBox : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            Document doc = rpmBIMTools.Load.liveDoc;
            UIApplication uiApp = rpmBIMTools.Load.uiApp;
            if (rpmBIMTools.Load.liveDoc.IsFamilyDocument)
            {
                TaskDialog.Show("Toggle Section Box", "Cannot be used in family editor");
            }
            else
            {
                IList<Element> sectionBoxVisable = new FilteredElementCollector(doc, doc.ActiveView.Id)
                .OfCategory(BuiltInCategory.OST_SectionBox).ToList();
                Element sectionBoxHidden = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_SectionBox).Where(x => x.IsHidden(doc.ActiveView))
                .FirstOrDefault();
                if (sectionBoxVisable.Count != 0)
                {
                    using (Transaction t = new Transaction(doc, "Hide Section Boxes"))
                    {
                        t.Start();
                        foreach (Element elem in sectionBoxVisable)
                        {
                            if (elem.CanBeHidden(doc.ActiveView)) {
                                ICollection<ElementId> col = new List<ElementId>();
                                col.Add(elem.Id);
                                doc.ActiveView.HideElements(col);
                            }
                        }
                        t.Commit();
                    }
                }
                if (sectionBoxHidden != null)
                {
                    using (Transaction t = new Transaction(doc, "Show Section Boxes"))
                    {
                        t.Start();
                        ICollection<ElementId> col = new List<ElementId>();
                        col.Add(sectionBoxHidden.Id);
                        doc.ActiveView.UnhideElements(col);
                        t.Commit();
                    }
                }
            }
            return Result.Succeeded;
        }
    }
}