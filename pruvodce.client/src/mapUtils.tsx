import type { BuildingData, Point } from './Types/MapType';

export function mergePointsIntoRooms(
  buildings: BuildingData[],
  points: Point[]
): BuildingData[] {
  const pointsByRoom = new Map<string, Point[]>();

  for (const point of points) {
    if (!point.roomId) continue;

    const list = pointsByRoom.get(point.roomId) ?? [];
    list.push(point);
    pointsByRoom.set(point.roomId, list);
  }

  return buildings.map(building => ({
    ...building,
    floors: building.floors.map(floor => ({
      ...floor,
      rooms: (floor.rooms ?? []).map(room => ({
        ...room,
        points: pointsByRoom.get(room.roomId) ?? [],
      })),
    })),
  }));
}