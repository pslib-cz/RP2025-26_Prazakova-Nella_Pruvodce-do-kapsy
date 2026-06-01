import BuildingSelect from './BuildingSelect';
import PointTypeFilters from './PointTypeFilters';
import FloorControls from './FloorControls';
import PointList from './PointList';

import type { BuildingData, FloorData, Point } from '../../Types/MapType';

import style from '../../Styles/MapPage.module.css';

type PointIcon = 'Talk' | 'Hand' | 'Ucebna' | 'Jine';

interface MapFiltersPanelProps {
  buildings: BuildingData[];
  currentBuilding: BuildingData;
  isDropdownOpen: boolean;
  onDropdownToggle: () => void;
  onBuildingChange: (buildingId: number) => void;

  floors: FloorData[];
  currentFloorId: number;
  onFloorChange: (floorId: number) => void;

  activeTypes: PointIcon[];
  onActiveTypesChange: React.Dispatch<React.SetStateAction<PointIcon[]>>;

  points: Point[];
  onPointSelect?: (point: Point) => void;
  hasActiveEvent: boolean;
  isEventLoading?: boolean;
  eventError?: string | null;

  hideBuildingSelect?: boolean;
  hideFloorSelect?: boolean;
}

const MapFiltersPanel: React.FC<MapFiltersPanelProps> = ({
  buildings,
  currentBuilding,
  isDropdownOpen,
  onDropdownToggle,
  onBuildingChange,
  floors,
  currentFloorId,
  onFloorChange,
  activeTypes,
  onActiveTypesChange,
  points,
  onPointSelect,
  hasActiveEvent,
  isEventLoading = false,
  eventError = null,
  hideBuildingSelect = false,
  hideFloorSelect = false,
}) => {
  return (
    <div className={style.panelInner}>
      {!hideBuildingSelect && (
        <section className={style.panelSection}>
          <h2 className={style.panelLabel}>Areál</h2>

          <BuildingSelect
            buildings={buildings}
            currentBuilding={currentBuilding}
            isOpen={isDropdownOpen}
            onToggle={onDropdownToggle}
            onChange={onBuildingChange}
          />
        </section>
      )}

      <section className={style.panelSection}>
        <h2 className={style.panelLabel}>Typ stanovišť</h2>

        <PointTypeFilters
          activeTypes={activeTypes}
          onChange={onActiveTypesChange}
        />
      </section>

      {!hideFloorSelect && (
        <section className={style.panelSection}>
          <h2 className={style.panelLabel}>Patro</h2>

          <FloorControls
            floors={floors}
            currentFloorId={currentFloorId}
            onFloorChange={onFloorChange}
            variant="panel"
          />
        </section>
      )}

      <section className={`${style.panelSection} ${style.PointSection}`}>
        <h2 className={style.panelLabel}>Stanoviště ({points.length})</h2>

        {isEventLoading && (
          <div className={style.emptyState}>Načítání stanovišť...</div>
        )}

        {!isEventLoading && eventError && (
          <div className={style.emptyState}>
            Stanoviště se nepodařilo načíst.
          </div>
        )}

        {!isEventLoading && !eventError && !hasActiveEvent && (
          <div className={style.emptyState}>
            Aktuálně není aktivní žádná akce
          </div>
        )}

        {!isEventLoading && !eventError && hasActiveEvent && (
          <PointList points={points} onPointSelect={onPointSelect}/>
        )}
      </section>
    </div>
  );
};

export default MapFiltersPanel;