import { useEffect, useState } from 'react';
import type { BuildingData, RoomData } from '../Types/MapType';

interface StaticDataResult {
  buildings: BuildingData[];
  loading: boolean;
  error: string | null;
}

const apiBase = import.meta.env.VITE_API_URL ?? '';

function withBaseUrl(url: string): string {
  if (/^https?:\/\//i.test(url)) return url;
  if (!apiBase) return url;
  return `${apiBase}${url.startsWith('/') ? url : `/${url}`}`;
}

async function fetchJson<T>(url: string): Promise<T> {
  const response = await fetch(withBaseUrl(url));
  const contentType = response.headers.get('content-type') ?? '';

  if (!response.ok) {
    throw new Error(`${url} vrátilo chybu ${response.status}`);
  }

  if (!contentType.includes('application/json')) {
    const preview = (await response.text()).slice(0, 120);
    throw new Error(`${url} nevrátilo JSON. Začátek odpovědi: ${preview}`);
  }

  return response.json() as Promise<T>;
}

function normalizeRooms(rooms: RoomData[]): RoomData[] {
  return rooms.map(room => ({
    ...room,
    type: typeof room.type === 'string' ? Number(room.type) : room.type,
  }));
}

export function useStaticData(): StaticDataResult {
  const [buildings, setBuildings] = useState<BuildingData[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    let cancelled = false;

    async function loadData() {
      try {
        setLoading(true);
        setError(null);

        const mapData = await fetchJson<{ buildings: BuildingData[] }>('/data/map.json');

        const buildingsWithRooms = await Promise.all(
          mapData.buildings.map(async building => ({
            ...building,
            floors: await Promise.all(
              building.floors.map(async floor => {
                if (!floor.roomsUrl) {
                  console.warn('Patro nemá roomsUrl:', floor);

                  return {
                    ...floor,
                    rooms: normalizeRooms(floor.rooms ?? []),
                  };
                }

                const rooms = await fetchJson<RoomData[]>(floor.roomsUrl);

                return {
                  ...floor,
                  rooms: normalizeRooms(rooms),
                };
              })
            ),
          }))
        );

        if (!cancelled) {
          setBuildings(buildingsWithRooms);
        }
      } catch (err) {
        console.error(err);

        if (!cancelled) {
          setError(err instanceof Error ? err.message : 'Nepodařilo se načíst statická data.');
          setBuildings([]);
        }
      } finally {
        if (!cancelled) {
          setLoading(false);
        }
      }
    }

    loadData();

    return () => {
      cancelled = true;
    };
  }, []);

  return { buildings, loading, error };
}