export const RoomType = {
  Classroom: 0,
  Specialized: 1,
  Office: 2,
  Toilets: 3,
  Elevator: 4,
  Other: 5
} as const;

export type RoomType = typeof RoomType[keyof typeof RoomType];

export const RoomTypeLabels: Record<number, string> = {
  0: 'Učebna',
  1: 'Specializovaná',
  2: 'Specializovaná',
  3: 'WC',
  4: 'Výtah',
  5: 'Jiné'
};

export const FieldTypeLabels: Record<number, string> = {
  0: 'IT',
  1: 'Elektro',
  2: 'Strojírenství',
  3: 'Technické lyceum',
  4: 'Odborné',
  5: 'Teorie'
};

export interface Specialization {
  specializationId: string;
  name: string;
  description?: string;
  type?: number | string;
  icon?: number | string;
}

export interface Teacher {
  teacherId: string;
  degree?: string;
  firstN: string;
  lastN: string;
  note?: string;
}

export interface Subject {
  subjectId: string;
  name: string;
  acronym: string;
  note?: string;
}

export interface Event {
  eventId: number;
  name: string;
  startDate: string;
  endDate: string;
  isActive: boolean;
  description?: string;
  buildingId?: number | null;
}

export interface Point {
  pointId: string;
  label: string;
  description?: string;
  note?: string;
  icon?: string;

  specializationId?: string | null;
  specialization?: Specialization | null;

  subjects?: Subject[];
  teachers?: Teacher[];

  roomId: string;

  eventId?: number | null;
  event?: Event | null;
}

export interface RoomData {
  roomId: string;
  label?: string;
  type: RoomType | number | string;
  note?: string;
  floorId: number;

  svgOutline?: string;
  svgData?: string;
  clipPathId?: string | null;

  interiorX?: number;
  interiorY?: number;
  interiorImageUrl?: string;
  coordinateX?: number;
  coordinateY?: number;
  interiorWidth?: number;
  interiorHeight?: number;

  icon?: string;

  points?: Point[];
}

export interface FloorData {
  floorId: number;
  name: string;
  floorNumber?: number;
  mapImageUrl?: string;
  backgroundUrl?: string;
  detailUrl?: string;
  roomsUrl?: string;
  rooms: RoomData[];
}

export interface BuildingData {
  buildingId: number;
  name: string;
  address?: string;
  floors: FloorData[];
}