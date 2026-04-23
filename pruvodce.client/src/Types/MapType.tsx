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

export const RoomTypeLabels: Record<number, string> = {
    0: "Učebna",
    1: "Specializovaná",
    2: "Kancelář",
    3: "WC",
    4: "Bufet",
    5: "Chodba",
    6: "Jiné"
};

export const FieldTypeLabels: Record<number, string> = {
    0: "IT",
    1: "Elektro",
    2: "Strojírenství",
    3: "Technické lyceum",
    4: "Odborné",
    5: "Teorie"
};


export interface Specialization {
    specializationId: string;
    name: string;
    description: string;
    type: number;
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
    type: number;
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