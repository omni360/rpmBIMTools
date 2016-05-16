using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;

using Autodesk.Revit.DB;

namespace rpmBIMTools.Create
{
    //------------------------------------------------------------------------------------------
    // Element Classes

    /// <summary>
    /// Set of Revit elements for UI Display
    /// </summary>
    public class ElementItem
    {
        public string Name { get; set; }
        public ElementId Id { get; set; }
    }

    //------------------------------------------------------------------------------------------
    // LV Schematic Classes

    [XmlRoot(ElementName = "schematic")]
    public class LVSchematic
    {
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "settings")]
        public Settings Settings { get; set; }
        [XmlElement(ElementName = "bus1")]
        public Bus1 Bus1 { get; set; }
        [XmlElement(ElementName = "bus2")]
        public Bus2 Bus2 { get; set; }
        [XmlElement(ElementName = "lifeSafetySection")]
        public LifeSafetySection LifeSafetySection { get; set; }
        [XmlElement(ElementName = "notes")]
        public Notes Notes { get; set; }
    }

    [XmlRoot(ElementName = "settings")]
    public class Settings
    {
        [XmlElement(ElementName = "includeSheet")]
        public bool IncludeSheet { get; set; }
        [XmlElement(ElementName = "includeTerminations")]
        public bool IncludeTerminations { get; set; }
        [XmlElement(ElementName = "includeSchedule")]
        public bool IncludeSchedule { get; set; }
        [XmlElement(ElementName = "includeSurgeSuppression")]
        public bool IncludeSurgeSuppression { get; set; }
        [XmlElement(ElementName = "includeExternalEarthBar")]
        public bool IncludeExternalEarthBar { get; set; }
        [XmlElement(ElementName = "includeInternalEarthBar")]
        public bool IncludeInternalEarthBar { get; set; }
        [XmlElement(ElementName = "includeInterTripping")]
        public bool IncludeInterTripping { get; set; }
        [XmlElement(ElementName = "includeMeteringOnMain")]
        public bool IncludeMeteringOnMain { get; set; }
        [XmlElement(ElementName = "includeRestrictedEarthFaultRelay")]
        public bool IncludeRestrictedEarthFaultRelay { get; set; }
        [XmlElement(ElementName = "includeOvercurrentRelay")]
        public bool IncludeOvercurrentRelay { get; set; }
        [XmlElement(ElementName = "includeEarthFaultRelay")]
        public bool IncludeEarthFaultRelay { get; set; }
        [XmlElement(ElementName = "includeNeutralEarthLink")]
        public bool IncludeNeutralEarthLink { get; set; }
        [XmlElement(ElementName = "includeCastellInterlock")]
        public bool IncludeCastellInterlock { get; set; }
        [XmlElement(ElementName = "includeElectricalInterlock")]
        public bool IncludeElectricalInterlock { get; set; }
        [XmlElement(ElementName = "includeMechanicalInterlock")]
        public bool IncludeMechanicalInterlock { get; set; }
    }

    [XmlRoot(ElementName = "device")]
    public class Device
    {
        [XmlElement(ElementName = "type")]
        public string Type { get; set; }
        [XmlElement(ElementName = "ref")]
        public string Ref { get; set; }
        [XmlElement(ElementName = "frame")]
        public string Frame { get; set; }
        [XmlElement(ElementName = "rating")]
        public string Rating { get; set; }
        [XmlElement(ElementName = "poles")]
        public string Poles { get; set; }
    }

    [XmlRoot(ElementName = "connectedLoad")]
    public class ConnectedLoad
    {
        [XmlElement(ElementName = "type")]
        public string Type { get; set; }
        [XmlElement(ElementName = "dbReference")]
        public string DbReference { get; set; }
        [XmlElement(ElementName = "dbWays")]
        public string DbWays { get; set; }
        [XmlElement(ElementName = "dbRating")]
        public string DbRating { get; set; }
        [XmlElement(ElementName = "dbPhase")]
        public string DbPhase { get; set; }
        [XmlElement(ElementName = "dbIF")]
        public string DbIF { get; set; }
        [XmlElement(ElementName = "dbIB")]
        public string DbIB { get; set; }
        [XmlElement(ElementName = "dbZS")]
        public string DbZS { get; set; }
    }

    [XmlRoot(ElementName = "circuit")]
    public class Circuit
    {
        [XmlElement(ElementName = "ref")]
        public string Ref { get; set; }
        [XmlElement(ElementName = "size")]
        public string Size { get; set; }
        [XmlElement(ElementName = "type")]
        public string Type { get; set; }
        [XmlElement(ElementName = "length")]
        public string Length { get; set; }
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }
        [XmlElement(ElementName = "metered")]
        public bool Metered { get; set; }
        [XmlElement(ElementName = "device")]
        public Device Device { get; set; }
        [XmlElement(ElementName = "connectedLoad")]
        public ConnectedLoad ConnectedLoad { get; set; }
    }

    [XmlRoot(ElementName = "linkBridge")]
    public class LinkBridge
    {
        [XmlElement(ElementName = "ref")]
        public string Ref { get; set; }
        [XmlElement(ElementName = "frame")]
        public string Frame { get; set; }
        [XmlElement(ElementName = "rating")]
        public string Rating { get; set; }
        [XmlElement(ElementName = "poles")]
        public string Poles { get; set; }
    }

    [XmlRoot(ElementName = "supplyTransformer")]
    public class SupplyTransformer
    {
        [XmlElement(ElementName = "ref")]
        public string Ref { get; set; }
        [XmlElement(ElementName = "rating")]
        public string Rating { get; set; }
        [XmlElement(ElementName = "voltage")]
        public string Voltage { get; set; }
        [XmlElement(ElementName = "vectorGroup")]
        public string VectorGroup { get; set; }
        [XmlElement(ElementName = "type")]
        public string Type { get; set; }
        [XmlElement(ElementName = "cooling")]
        public string Cooling { get; set; }
        [XmlElement(ElementName = "linkBridge")]
        public LinkBridge LinkBridge { get; set; }
        [XmlAttribute(AttributeName = "location")]
        public string Location { get; set; }
    }

    [XmlRoot(ElementName = "supplyGenerator")]
    public class SupplyGenerator
    {
        [XmlElement(ElementName = "ref")]
        public string Ref { get; set; }
        [XmlElement(ElementName = "rating")]
        public string Rating { get; set; }
        [XmlElement(ElementName = "linkBridge")]
        public LinkBridge LinkBridge { get; set; }
        [XmlAttribute(AttributeName = "location")]
        public string Location { get; set; }
    }

    [XmlRoot(ElementName = "powerFactorCorrection")]
    public class PowerFactorCorrection
    {
        [XmlElement(ElementName = "type")]
        public string Type { get; set; }
        [XmlElement(ElementName = "total")]
        public int Total { get; set; }
        [XmlElement(ElementName = "steps")]
        public int Steps { get; set; }
    }

    [XmlRoot(ElementName = "bus1")]
    public class Bus1
    {
        [XmlElement(ElementName = "circuit")]
        public List<Circuit> Circuit { get; set; }
        [XmlElement(ElementName = "supplyTransformer")]
        public SupplyTransformer SupplyTransformer { get; set; }
        [XmlElement(ElementName = "supplyGenerator")]
        public SupplyGenerator SupplyGenerator { get; set; }
        [XmlElement(ElementName = "powerFactorCorrection")]
        public PowerFactorCorrection PowerFactorCorrection { get; set; }
    }

    [XmlRoot(ElementName = "bus2")]
    public class Bus2
    {
        [XmlElement(ElementName = "circuit")]
        public List<Circuit> Circuit { get; set; }
        [XmlElement(ElementName = "supplyTransformer")]
        public SupplyTransformer SupplyTransformer { get; set; }
        [XmlElement(ElementName = "supplyGenerator")]
        public SupplyGenerator SupplyGenerator { get; set; }
        [XmlElement(ElementName = "powerFactorCorrection")]
        public PowerFactorCorrection PowerFactorCorrection { get; set; }
    }

    [XmlRoot(ElementName = "lsCircuit")]
    public class LsCircuit
    {
        [XmlElement(ElementName = "ref")]
        public string Ref { get; set; }
        [XmlElement(ElementName = "size")]
        public string Size { get; set; }
        [XmlElement(ElementName = "type")]
        public string Type { get; set; }
        [XmlElement(ElementName = "length")]
        public string Length { get; set; }
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }
        [XmlElement(ElementName = "device")]
        public Device Device { get; set; }
    }

    [XmlRoot(ElementName = "lifeSafetySection")]
    public class LifeSafetySection
    {
        [XmlElement(ElementName = "lsCircuit")]
        public List<LsCircuit> LsCircuit { get; set; }
    }

    [XmlRoot(ElementName = "notes")]
    public class Notes
    {
        [XmlElement(ElementName = "note")]
        public List<string> Note { get; set; }
    }
}