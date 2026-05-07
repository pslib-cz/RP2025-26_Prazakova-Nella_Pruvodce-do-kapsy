using System.ComponentModel.DataAnnotations;

namespace pruvodce.server.Models
{
    public enum RoomType
    {
        Classroom,
        Specialized,
        Office,
        Toilets,
        Buffet,
        Corridor,
        Other
    }

    public enum FieldType
    {
        [Display(Name = "Informační technologie")]
        IT,
        [Display(Name = "Elektrotechnika")]
        EL,
        [Display(Name = "Strojírenství")]
        ST,
        [Display(Name = "Technické lyceum")]
        TL,
        [Display(Name = "Oděvnictví")]
        OD,
        [Display(Name = "Textnilnictví")]
        TE
    }

    public enum PointIcon
    {
        [Display(Name = "Přednáška")]
        Talk,

        [Display(Name = "Praktické stanoviště")]
        Hand,

        [Display(Name = "Ukázka učebny")]
        Ucebna,

        [Display(Name = "Jiné")]
        Jine
    }
}