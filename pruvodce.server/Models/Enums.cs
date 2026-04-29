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

    public enum FieldType // pravdìpodobnì jen pro barvièky, jinak vše pøes specializace
    {
        IT,
        EL,
        ST, // strojírenství
        TL, // technické lyceum
        OD,
        TE
    }

    public enum SpecializationIcon
    {
        Computer,
        Code,
        Network,
        Electricity,
        Machine,
        Design,
        Health,
        Business
    }
}