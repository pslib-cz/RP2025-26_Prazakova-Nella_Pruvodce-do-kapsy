import { useState, useEffect } from 'react';
import type { BuildingData } from '../Types/MapType';
import staticData from '../Data/staticData.json';
import floor1Rooms from '../Data/rooms/masarykova-1.json';

export const useStaticData = () => {
  const [buildings, setBuildings] = useState<BuildingData[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
  console.log('=== useStaticData ===');
  console.log('staticData:', staticData);
  console.log('floor1Rooms:', floor1Rooms);
  
  const data = staticData as unknown as { buildings: BuildingData[] };
  
  const buildingsWithRooms = data.buildings.map(building => ({
    ...building,
    floors: building.floors.map(floor => {
      console.log('floor.floorId:', floor.floorId);
      console.log('floor.name:', floor.name);
      
      return {
        ...floor,
        rooms: floor.floorId === 1 ? floor1Rooms : []
      };
    })
  }));
  
  setBuildings(buildingsWithRooms);
  setLoading(false);
}, []);

  return { buildings, loading, error };
};