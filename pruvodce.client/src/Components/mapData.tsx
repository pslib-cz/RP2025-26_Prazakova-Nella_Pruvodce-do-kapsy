import { RoomType } from '../Types/MapType';
import { firstFloorRoomsM } from '../Data/M-floor1';

export const SHARED_BACKGROUND = "M0 0 H 540 V 600 H 0 Z";

export const BUILDINGS = [
  {
    buildingId: 1,
    name: "Masarykova",
    floors: [
      {
        floorId: 1,
        name: "1. Patro",
        mapImageUrl: "/Mfirst.svg",
        rooms: firstFloorRoomsM
      },
      {
        floorId: 2,
        name: "2. Patro",
        mapImageUrl: "/Msecond.svg",
        rooms: []
      }
    ]
  },
  {
    buildingId: 2,
    name: "Tyršova",
    floors: [
      {
        floorId: 3,
        name: "1. Patro",
        mapImageUrl: "/Tfirst.svg",
        rooms: []
      }
    ]
  }
];