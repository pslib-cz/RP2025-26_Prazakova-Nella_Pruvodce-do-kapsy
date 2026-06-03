import { useParams, useNavigate } from 'react-router-dom';
import { useState, useEffect, useMemo } from 'react';
import { Icon } from '@iconify/react';

import { useStaticData } from '../Hooks/useStaticData';
import { useActiveEventPoints } from '../Hooks/useActiveEventPoints';

import InteractiveMap from '../Components/InteractiveMap';
import BuildingSelect from '../Components/Map/BuildingSelect';
import MapFiltersPanel from '../Components/Map/MapFiltersPanel';
import FloorControls from '../Components/Map/FloorControls';

import PointDetailPanel from '../Components/Map/PointDetailPanel';

import type { Point } from '../Types/MapType';

import { mergePointsIntoRooms } from '../mapUtils';


import style from '../Styles/MapPage.module.css';
import '../Styles/MapItems.css';

type PointIcon = 'Talk' | 'Hand' | 'Ucebna' | 'Jine';

function getPointIconType(point: Point): PointIcon {
  const rawPoint = point as Point & {
    iconType?: PointIcon;
    pointIcon?: PointIcon;
    icon?: PointIcon;
  };

  return rawPoint.iconType ?? rawPoint.pointIcon ?? rawPoint.icon ?? 'Jine';
}

function useIsDesktop(): boolean {
  const [isDesktop, setIsDesktop] = useState(() => {
    if (typeof window === 'undefined') return true;
    return window.matchMedia('(min-width: 1024px)').matches;
  });

  useEffect(() => {
    if (typeof window === 'undefined') return;

    const mediaQuery = window.matchMedia('(min-width: 1024px)');

    const handleChange = () => {
      setIsDesktop(mediaQuery.matches);
    };

    handleChange();
    mediaQuery.addEventListener('change', handleChange);

    return () => {
      mediaQuery.removeEventListener('change', handleChange);
    };
  }, []);

  return isDesktop;
}

const MapPage: React.FC = () => {
  const { buildingId } = useParams<{ buildingId: string }>();
  const navigate = useNavigate();

  const isDesktop = useIsDesktop();
const [selectedPoint, setSelectedPoint] = useState<Point | null>(null);

  const buildingIdNumber = Number(buildingId);

  const {
    buildings: staticBuildings,
    loading: staticLoading,
    error: staticError,
  } = useStaticData();

  const {
    activeEvent,
    points: eventPoints,
    loading: eventLoading,
    error: eventError,
  } = useActiveEventPoints();

  const [isDropdownOpen, setIsDropdownOpen] = useState(false);
  const [isMobilePanelOpen, setIsMobilePanelOpen] = useState(false);
  const [currentFloorId, setCurrentFloorId] = useState<number | null>(null);

  const [activeTypes, setActiveTypes] = useState<PointIcon[]>([
    'Talk',
    'Hand',
    'Ucebna',
    'Jine',
  ]);

  const hasActiveEvent = Boolean(activeEvent);

  const visibleEventPoints = useMemo(() => {
    if (!hasActiveEvent) return [];

    return eventPoints.filter(point => {
      const pointIconType = getPointIconType(point);
      return activeTypes.includes(pointIconType);
    });
  }, [eventPoints, activeTypes, hasActiveEvent]);

  const buildings = useMemo(() => {
    return mergePointsIntoRooms(staticBuildings, visibleEventPoints);
  }, [staticBuildings, visibleEventPoints]);

  const currentBuilding = useMemo(() => {
    return buildings.find(building => building.buildingId === buildingIdNumber);
  }, [buildings, buildingIdNumber]);

  const buildingFloors = currentBuilding?.floors ?? [];

  useEffect(() => {
    if (buildingFloors.length === 0) return;

    setCurrentFloorId(previous => {
      const stillExists =
        previous != null &&
        buildingFloors.some(floor => floor.floorId === previous);

      return stillExists ? previous : buildingFloors[0].floorId;
    });
  }, [buildingFloors]);

  useEffect(() => {
  setSelectedPoint(null);
}, [buildingId, currentFloorId]);

  const allPoints = useMemo(() => {
  if (!hasActiveEvent) return [];

    return buildingFloors.flatMap(floor =>
      floor.rooms.flatMap(room =>
        (room.points ?? []).map(point => ({
          ...point,
          floorId: floor.floorId,
        }))
      )
    );
  }, [buildingFloors, hasActiveEvent]);

  const handleBuildingChange = (nextBuildingId: number) => {
    setIsDropdownOpen(false);
    setIsMobilePanelOpen(false);

    localStorage.setItem('preferredBuilding', nextBuildingId.toString());
    navigate(`/map/${nextBuildingId}`);
  };

  const handleFloorChange = (nextFloorId: number) => {
    setCurrentFloorId(nextFloorId);
  };

  if (staticLoading) {
    return <div>Načítání mapy...</div>;
  }

  if (staticError) {
    return <div>{staticError}</div>;
  }

  if (!Number.isFinite(buildingIdNumber)) {
    return <div>Neplatná budova.</div>;
  }

  if (!currentBuilding) {
    return <div>Budova nebyla nalezena.</div>;
  }

  if (buildingFloors.length === 0) {
    return <div>Budova nemá žádná patra.</div>;
  }

  if (currentFloorId == null) {
    return <div>Patro nebylo vybráno.</div>;
  }

  return (
    <div className={style.page}>
      <aside className={style.desktopPanel}>
        <MapFiltersPanel
          buildings={buildings}
          currentBuilding={currentBuilding}
          isDropdownOpen={isDropdownOpen}
          onDropdownToggle={() => setIsDropdownOpen(previous => !previous)}
          onBuildingChange={handleBuildingChange}
          floors={buildingFloors}
          currentFloorId={currentFloorId}
          onFloorChange={handleFloorChange}
          activeTypes={activeTypes}
          onActiveTypesChange={setActiveTypes}
          points={allPoints}
          onPointSelect={point => {
            setSelectedPoint(point);
            setIsMobilePanelOpen(false);
            setIsDropdownOpen(false);
          }}
          hasActiveEvent={hasActiveEvent}
          isEventLoading={eventLoading}
          eventError={eventError}
        />
      </aside>

      <main className={style.mapArea}>
        <div className={style.mobileTopControls}>
          <BuildingSelect
            buildings={buildings}
            currentBuilding={currentBuilding}
            isOpen={isDropdownOpen}
            onToggle={() => setIsDropdownOpen(previous => !previous)}
            onChange={handleBuildingChange}
          />

          <button
            type="button"
            className={style.mobileFilterButton}
            onClick={() => setIsMobilePanelOpen(true)}
            aria-label="Otevřít preference"
          >
            <Icon icon="lucide:sliders-horizontal" width="22" height="22" />
          </button>
        </div>

        <div className={style.mapCanvas}>
          <InteractiveMap
            floors={buildingFloors}
            activeFloorId={currentFloorId}
            points={allPoints}
            buildingId={buildingIdNumber}
            className="main-map"
            onPointSelect={point => {
              setSelectedPoint(point);
              setIsMobilePanelOpen(false);
              setIsDropdownOpen(false);
            }}
            onMapClick={() => {
              setSelectedPoint(null);
            }}
          />
        </div>

        <FloorControls
          floors={buildingFloors}
          currentFloorId={currentFloorId}
          onFloorChange={handleFloorChange}
          reverse
        />
      </main>

      {isMobilePanelOpen && (
        <button
          type="button"
          className={style.mobileOverlay}
          onClick={() => setIsMobilePanelOpen(false)}
          aria-label="Zavřít preference"
        />
      )}

      <section
        className={`${style.mobileSheet} ${
          isMobilePanelOpen ? style.mobileSheetOpen : ''
        }`}
        aria-hidden={!isMobilePanelOpen}
      >
        <button
          type="button"
          className={style.mobileSheetClose}
          onClick={() => setIsMobilePanelOpen(false)}
          aria-label="Zavřít preference"
        >
          <Icon icon="lucide:x" width="18" height="18" />
        </button>

        <div className={style.mobileSheetHandle} />

        <MapFiltersPanel
          buildings={buildings}
          currentBuilding={currentBuilding}
          isDropdownOpen={false}
          onDropdownToggle={() => {}}
          onBuildingChange={handleBuildingChange}
          floors={buildingFloors}
          currentFloorId={currentFloorId}
          onFloorChange={handleFloorChange}
          activeTypes={activeTypes}
          onActiveTypesChange={setActiveTypes}
          points={allPoints}
          onPointSelect={point => {
            setSelectedPoint(point);
            setIsMobilePanelOpen(false);
          }}
          hasActiveEvent={hasActiveEvent}
          isEventLoading={eventLoading}
          eventError={eventError}
          hideBuildingSelect
          hideFloorSelect
        />
        
      </section>
      <PointDetailPanel
          point={selectedPoint}
          isDesktop={isDesktop}
          onClose={() => setSelectedPoint(null)}
        />
    </div>
  );
};

export default MapPage;