namespace pruvodce.server.Models
{
    public enum RoomType
    {
            Classroom,
            Specialized, //1
            Office,
            Toilets, //3
            Buffet,
            Corridor, //5
            Other
    }
    public enum FieldType //pravdìpodobnì jen pro barvièky, jinak vše pøes specializace
    {
            IT,
            EL,
            ST, //strojíretsví 
            TL, //technické lyceum
            OD,
            TE
    }
}