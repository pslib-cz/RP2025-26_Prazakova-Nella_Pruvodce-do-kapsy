import { useEffect, useState } from 'react';
import type { Point } from '../Types/MapType';

interface ActiveEvent {
  eventId: number;
  name: string;
}

interface ActiveEventPointsResult {
  activeEvent: ActiveEvent | null;
  points: Point[];
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

export function useActiveEventPoints(): ActiveEventPointsResult {
  const [activeEvent, setActiveEvent] = useState<ActiveEvent | null>(null);
  const [points, setPoints] = useState<Point[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    let cancelled = false;

    async function loadData() {
      try {
        setLoading(true);
        setError(null);

        const loadedPoints = await fetchJson<Point[]>('/api/ReferenceData/points');

        if (cancelled) return;

        if (loadedPoints.length === 0) {
          setActiveEvent(null);
          setPoints([]);
          return;
        }
        setActiveEvent({
          eventId: 0,
          name: 'Aktivní akce',
        });

        setPoints(loadedPoints);
      } catch (err) {
        console.error(err);

        if (!cancelled) {
          setActiveEvent(null);
          setPoints([]);
          setError(err instanceof Error ? err.message : 'Stanoviště se nepodařilo načíst.');
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

  return { activeEvent, points, loading, error };
}