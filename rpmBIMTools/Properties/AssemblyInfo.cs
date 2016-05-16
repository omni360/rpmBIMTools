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
[assembly: AssemblyVersion("1.13.*")]
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