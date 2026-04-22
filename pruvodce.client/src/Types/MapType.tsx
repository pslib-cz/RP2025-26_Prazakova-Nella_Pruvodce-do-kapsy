export const RoomType = {
    Classroom: 0,
    Specialized: 1,
    Office: 2,
    Toilets: 3,
    Buffet: 4,
    Corridor: 5,
    Other: 6
} as const;
export type RoomType = typeof RoomType[keyof typeof RoomType];


export const FieldType = {
    IT: 0,
    EL: 1,
    ST: 2,
    TL: 3,
    OD: 4,
    TE: 5
} as const;
export type FieldType = typeof FieldType[keyof typeof FieldType];


///


export interface Specialization {
    specializationId: string;
    name: string;
    description: string;
    type: FieldType;
}

export interface Subject {
    subjectId: string;
    name: string;
    acronym: string;
    note?: string;
    specialization?: Specialization[];
    roomId?: string;
}

export interface Teacher {
    teacherId: string;
    degree?: string;
    firstN: string;
    lastN: string;
    note?: string;
    subjects?: Subject[];
}

export interface Event {
    eventId: number;
    name: string;
    startDate: string;
    endDate: string;
    isActive: boolean;
    description?: string;
}

export interface Point {
    pointId: string;
    label: string;
    description?: string;
    subjects?: Subject[];
    labelX: number;
    labelY: number;
    note?: string;
    icon?: string;
    teacherId?: string;
    teacher?: Teacher;
    roomId: string;
    eventId?: number;
    event?: Event;
}

export interface RoomData {
    roomId: string;
    svgData: string;
    label: string;
    icon?: string;
    subjects?: Subject[];
    coordinateX?: number;
    coordinateY?: number;
    type: RoomType;
    note?: string;
    floorId: number;
    points?: Point[]; 
}

export interface FloorData {
    floorId: number;
    name: string;
    mapImageUrl: string;
    rooms: RoomData[];
}

export interface BuildingData {
    buildingId: number;
    name: string;
    address?: string;
    floors: FloorData[];
}