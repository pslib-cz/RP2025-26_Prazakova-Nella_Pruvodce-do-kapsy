import { useParams, useNavigate, Navigate } from 'react-router-dom';
import { useState, useMemo } from 'react';
import { useStaticData } from '../Hooks/useStaticData';
import InteractiveMap from '../Components/InteractiveMap';
import style from "../Styles/MapPage.module.css";
import { Icon } from '@iconify/react';

const MapPage: React.FC = () => {

  const { buildingId } = useParams<{ buildingId: string }>();
  const navigate = useNavigate();

  const { buildings, loading } = useStaticData();
  
  const [isDropdownOpen, setIsDropdownOpen] = useState(false);
  const [currentFloorId, setCurrentFloorId] = useState<number>(1);

  const currentBuilding = useMemo(
    () => buildings.find(b => b.buildingId === Number(buildingId)),
    [buildings, buildingId]
  );

  const buildingFloors = currentBuilding?.floors || [];

    console.log('buildings:', buildings);
  console.log('currentBuilding:', currentBuilding);
  console.log('buildingFloors:', buildingFloors);

  useMemo(() => {
    if (buildingFloors.length > 0 && currentFloorId === 1) {
      setCurrentFloorId(buildingFloors[0].floorId);
    }
  }, [buildingFloors]);

  if (loading) {
    return (
      <div className={style.mapUIContainer}>
        <div style={{ padding: '20px', textAlign: 'center' }}>
          Načítám mapy...
        </div>
      </div>
    );
  }

  // Budova neexistuje
    if (!currentBuilding || buildingFloors.length === 0) {
    return <Navigate to="/" />;
  }

  // === VRATITELNÁ ČÁST ===
  return (
    <div className={style.mapUIContainer}>
      {/* Building selector */}
      <div className={style.topControlsWrapper}>
        <div className={`${style.buildingSelector} ${isDropdownOpen ? style.expanded : ''}`}>
          <button
            className={style.mainSelectorBtn}
            onClick={() => setIsDropdownOpen(!isDropdownOpen)}
          >
            Změnit areál
          </button>

          {isDropdownOpen && (
            <div className={style.buildingList}>
              {buildings.map(building => (
                <button
                  key={building.buildingId}
                  className={style.buildingListItem}
                  onClick={() => {
                    setIsDropdownOpen(false);
                    navigate(`/map/${building.buildingId}`);
                  }}
                >
                  <span className={style.bName}>{building.name}</span>
                  <span className={style.bAddress}>
                    {building.address || 'Bez adresy'}
                  </span>
                </button>
              ))}
            </div>
          )}
        </div>

        <button
          className={style.filterBtn}
          onClick={() => navigate('/settings')}
        >
          <Icon icon="lucide:sliders-horizontal" width="22" height="22" />
        </button>
      </div>

      {/* Floor selector */}
      <div className={style.floorControls}>
        {[...buildingFloors].reverse().map((floor) => (
          <button
            key={floor.floorId}
            onClick={() => setCurrentFloorId(floor.floorId)}
            className={`${style.floorButton} ${
              currentFloorId === floor.floorId ? style.active : ''
            }`}
          >
            {floor.name.charAt(0)}.
          </button>
        ))}
      </div>

      {/* Mapa */}
      <InteractiveMap
        floors={buildingFloors}
        activeFloorId={currentFloorId}
        buildingId={Number(buildingId)}
      />
    </div>
  );
};

export default MapPage;