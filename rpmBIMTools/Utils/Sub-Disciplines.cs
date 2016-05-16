using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;

namespace rpmBIMTools
{
    public enum SubDiscipline
    {
        [DescriptionAttribute("### - Drafting Views")] DraftingViews,
        [DescriptionAttribute("### - Working Views")] WorkingViews,
        [DescriptionAttribute("### - Working Sections")] WorkingSections,
        [DescriptionAttribute("### - SnagR")] SnagR,
        [DescriptionAttribute("CS0 - Coord Services")] CoordinatedFloor,
        [DescriptionAttribute("CS0 - Reflected Ceiling Plan")] CoordinatedCeiling,
        [DescriptionAttribute("BS0 - Builderswork")] Builderswork,
        [DescriptionAttribute("M50 - Coordinated Services")] CoordinatedMech,
        [DescriptionAttribute("M52 - Drainage")] Drainage,
        [DescriptionAttribute("M53 - Domestics")] Domestics,
        [DescriptionAttribute("M54 - Gas")] Gas,
        [DescriptionAttribute("M55 - Chilled")] Chilled,
        [DescriptionAttribute("M56 - Heating")] Heating,
        [DescriptionAttribute("M56 - Heating & Chilled")] HeatingChilled,
        [DescriptionAttribute("M57 - Ductwork")] Ductwork,
        [DescriptionAttribute("E60 - Coordinated Services")] CoordinatedElec,
        [DescriptionAttribute("E61 - Main HV/LV Distribution")] MainHVLV,
        [DescriptionAttribute("E62 - Small Power")] SmallPower,
        [DescriptionAttribute("E63 - Lighting")] Lighting,
        [DescriptionAttribute("E64 - CCTV/TV")] CCTV,
        [DescriptionAttribute("E65 - Communications")] Communications,
        [DescriptionAttribute("E66 - Containment")] Containment,
        [DescriptionAttribute("E67 - Fire Alarm")] FireAlarm,
        [DescriptionAttribute("E68 - Security")] Security,
        [DescriptionAttribute("E69 - Lightning Protection/Earthing")] LightningProtection
    }

}