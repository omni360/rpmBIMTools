using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;

namespace rpmBIMTools.InsertSpecialCharacter
{
    /// <summary>
    /// Common Command to Insert a character into a Text Note
    /// </summary>
    public static class InsertCharacter
    {
        /// <summary>
        /// Insert character into a text note
        /// </summary>
        /// <param name="character">Character</param>
        public static void Insert(string character)
        {
            //UIApplication uiApp = Load.uiApp;
            Document doc = Load.liveDoc;
            ICollection<Element> elements = Load.uiApp.ActiveUIDocument.Selection.GetElementIds().Select(eId => doc.GetElement(eId)).ToList();
            using (Transaction t = new Transaction(doc, "Inserting Special Character"))
            {
                t.Start();
                foreach (Element element in elements)
                {
                    TextNote textNote = element as TextNote;
                    if (textNote != null)
                    {
                        textNote.Text = textNote.Text + character;
                    }
                }
                t.Commit();
            }
        }
    }
    [TransactionAttribute(TransactionMode.Manual)]
    public class Degree : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            InsertCharacter.Insert("°");
            return Result.Succeeded;
        }
    }
    [TransactionAttribute(TransactionMode.Manual)]
    public class Radius : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            InsertCharacter.Insert("Ø");
            return Result.Succeeded;
        }
    }
    [TransactionAttribute(TransactionMode.Manual)]
    public class Ohm : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            InsertCharacter.Insert("Ω");
            return Result.Succeeded;
        }
    }
    [TransactionAttribute(TransactionMode.Manual)]
    public class Superscript2 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            InsertCharacter.Insert("²");
            return Result.Succeeded;
        }
    }
    [TransactionAttribute(TransactionMode.Manual)]
    public class Superscript3 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            InsertCharacter.Insert("³");
            return Result.Succeeded;
        }
    }
    [TransactionAttribute(TransactionMode.Manual)]
    public class FractionOneFour : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            InsertCharacter.Insert("¼");
            return Result.Succeeded;
        }
    }
    [TransactionAttribute(TransactionMode.Manual)]
    public class FractionOneTwo : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            InsertCharacter.Insert("½");
            return Result.Succeeded;
        }
    }
    [TransactionAttribute(TransactionMode.Manual)]
    public class FractionThreeFour : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            InsertCharacter.Insert("¾");
            return Result.Succeeded;
        }
    }
    [TransactionAttribute(TransactionMode.Manual)]
    public class Nabia : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            InsertCharacter.Insert("∇");
            return Result.Succeeded;
        }
    }
    [TransactionAttribute(TransactionMode.Manual)]
    public class PlusMinus : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            InsertCharacter.Insert("±");
            return Result.Succeeded;
        }
    }
    [TransactionAttribute(TransactionMode.Manual)]
    public class Squareroot : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            InsertCharacter.Insert("√");
            return Result.Succeeded;
        }
    }
    [TransactionAttribute(TransactionMode.Manual)]
    public class Infinity : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            InsertCharacter.Insert("∞");
            return Result.Succeeded;
        }
    }
    [TransactionAttribute(TransactionMode.Manual)]
    public class Angle : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            InsertCharacter.Insert("∠");
            return Result.Succeeded;
        }
    }
    [TransactionAttribute(TransactionMode.Manual)]
    public class Pie : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            InsertCharacter.Insert("π");
            return Result.Succeeded;
        }
    }
    [TransactionAttribute(TransactionMode.Manual)]
    public class NotEqual : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            InsertCharacter.Insert("≠");
            return Result.Succeeded;
        }
    }
    [TransactionAttribute(TransactionMode.Manual)]
    public class LessThan : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            InsertCharacter.Insert("≤");
            return Result.Succeeded;
        }
    }
    [TransactionAttribute(TransactionMode.Manual)]
    public class GreaterThan : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            InsertCharacter.Insert("≥");
            return Result.Succeeded;
        }
    }
    [TransactionAttribute(TransactionMode.Manual)]
    public class Female : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            InsertCharacter.Insert("♀");
            return Result.Succeeded;
        }
    }
    [TransactionAttribute(TransactionMode.Manual)]
    public class Male : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            InsertCharacter.Insert("♂");
            return Result.Succeeded;
        }
    }
    [TransactionAttribute(TransactionMode.Manual)]
    public class Register : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            InsertCharacter.Insert("®");
            return Result.Succeeded;
        }
    }
    [TransactionAttribute(TransactionMode.Manual)]
    public class Euro : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            InsertCharacter.Insert("€");
            return Result.Succeeded;
        }
    }
    [TransactionAttribute(TransactionMode.Manual)]
    public class Trademark : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            InsertCharacter.Insert("™");
            return Result.Succeeded;
        }
    }
    [TransactionAttribute(TransactionMode.Manual)]
    public class Copyright : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            InsertCharacter.Insert("©");
            return Result.Succeeded;
        }
    }
    [TransactionAttribute(TransactionMode.Manual)]
    public class Numero : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            rpmBIMTools.Load.uiApp = commandData.Application;
            rpmBIMTools.Load.liveDoc = commandData.Application.ActiveUIDocument.Document;
            InsertCharacter.Insert("№");
            return Result.Succeeded;
        }
    }
}