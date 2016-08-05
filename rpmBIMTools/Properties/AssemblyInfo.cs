using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Resources;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("rpmBIM Tools")]
[assembly: AssemblyDescription("Utilities used within revit")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("NG Bailey")]
[assembly: AssemblyProduct("rpmBIM Tools")]
[assembly: AssemblyCopyright("Copyright © NG Bailey")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(true)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("039bb53b-b79f-40ab-8708-dc8cb8bcbae2")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.18.*")]
//[assembly: AssemblyFileVersion("1.0.0")]
[assembly: NeutralResourcesLanguageAttribute("en-GB")]

// Version Update Information
//
// 1.0.0 - 01/01/2016 - Starting Version Released
// 1.7.5926 - 23/03/2016 - (Updated) Export Model - Added Landing Page Selection
// 1.7.5935 - 01/04/2016 - (Updated) QuickSelect - Corrected Exception
// 1.8.5946 - 12/04/2016 - (New) Project Setup - Utility Added
// 1.8.5947 - 13/04/2016 - (Updated) Drawing Number Calculator - New bailey service codes
// 1.8.5948 - 14/04/2016 - (Updated) Drawing Number Calculator - Corrected drawing type issue and zone type codes
// 1.9.5962 - 27/04/2016 - (New) Project Sheet Duplicator - Utility Added
//                         (Updated) Drawing Number Calculator - Added sheet size selection, included master and offsite service types, also fixed title block issue
// 1.9.5963 - 29/04/2016 - (Updated) Project Sheet Duplicator - Corrected discipline read only and sheet numbering issues
//                         (Updated) Others - Set all form UI's to autoscale based on DPI
// 1.10.5969 - 05/05/2016 - (New) Generate GUID - Utility Added
//                          (Updated) Solution Build - Dropped Revit 2014 Support
//                          (Updated) rpmBIMTools Ribbon - Relocating to its own ribbon tab
// 1.10.5970 - 06/05/2016 - (Updated) Others - All Icons throughout have been updated and improved
// 1.11.5975 - 11/05/2016 - (New) Purge Scope Box - Utility Added
//                          (Updated) rpmBIMTools Utilities - Removed All support for Revit 2014 Code
// 1.12.5977 - 13/05/2016 - (New) Create Section Box - Utility Added
// 1.13.5980 - 16/05/2016 - (New) Toggle Section Box - Utililty Added
//                          (Update) Create Section Box - Fixed issue with selection of objects the first time the utility was ran
//                          (Update) rpmBIMTools Solution - Support for Revit 2017 added
//                          (Update) Family Name Editor - Set Uniclass list to be in alphanumetic order
//                          (Update) Model Expoter - Increased width of landing page dropdown list for improved visability
// 1.13.5995 - 31/05/2016   (Update) Family Name Editor - Added the standard BS 8541-2012 and set as default (BS 8541-2012)
// 1.13.5997 - 02/06/2016   (Update) Family Name Editor - Updated uniclass to latest version (March 2016 release)
// 1.14.6004 - 09/06/2016   (New) Export / Import Schedule - Producing application for  company use, major testing still required, only the export feature is semi complete at this time.
// 1.14.6008 - 13/06/2016   (Update) rpmBIMTools - Updated application for changes to STATUS parameter to Status on viewSheets
// 1.15.6010 - 15/06/2016   (Update) Export / Import Schedule - Development completed, testing script for any issues
// 1.16.6022 - 27/06/2016   (New) Special Character tab added for Text Notes for easy character insertion
// 1.16.6023 - 28/06/2016   (Update) Drawing Number Calculator - Increase project number character limit from 7 to 8 characters for longer project numbers
// 1.17.6038 - 13/07/2016   (New) Family Library - Utility Added
//                          (Update) QuickSelect - Fixed issue that would allow the select button to be clickable when no elements were detected in the model or view
// 1.17.6044 - 19/07/2016   (Update) Project Setup - Corrected issue due to the starting view was changed in the NGB Templates
// 1.17.6045 - 20/07/2016   (Update) Project Setup - Included NGB Location parameter set to the landing page
// 1.17.6046 - 21/07/2016   (Update) Ribbon - Added Wiki Guide and About Buttons
//                          (Update) Other - Added Help (F1) Support for all dialog boxes
//                          (Update) Family Library - Corrected Scrolling support for family icon panel and allow width change to give more icon columns in grid
// 1.18.6059 - 03/08/2016   (New) Family Library - Addon reworked into a dockable pane version that is intigrated with revit
// 1.18.6061 - 05/08/2016   (Update) Family Library - Addtional changes to improve functionality