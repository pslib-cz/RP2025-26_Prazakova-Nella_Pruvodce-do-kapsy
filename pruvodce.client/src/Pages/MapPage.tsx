import { useParams, useNavigate, Navigate } from 'react-router-dom';
import { useState, useEffect } from 'react';
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

  const currentBuilding = buildings.find(b => b.buildingId === Number(buildingId));
  const buildingFloors = currentBuilding?.floors || [];

  useEffect(() => {
    if (buildingFloors.length > 0) {
      setCurrentFloorId(buildingFloors[0].floorId);
    }
  }, [buildingId]);

  if (loading) return <div>Načítání mapy...</div>;
  if (!currentBuilding || buildingFloors.length === 0) return <Navigate to="/" />;


  if (!currentBuilding || buildingFloors.length === 0) {
    return <Navigate to="/" />;
  }

  return (
    <div className={style.pageWrapper}>
      <div className={style.mapUIContainer}>
        <div className={style.controlsWrapper}>
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
                    <div>
                      <span className={style.bName}>{building.name}</span>
                      <br />
                      <span className={style.bAddress}>
                        {building.address || 'Bez adresy'}
                      </span>
                    </div>
                  </button>
                ))}
              </div>
            )}
          </div>

          <button
            className={style.filterBtn}
            onClick={() => navigate('/settings')}
            title="Nastavení"
          >
            <Icon icon="lucide:sliders-horizontal" width="22" height="22" />
          </button>
        </div>

        <div className={style.mapContainer}>
          <div className={style.mapInnerContainer}>
            <InteractiveMap
              floors={buildingFloors}
              activeFloorId={currentFloorId}
              buildingId={Number(buildingId)}
              className="desktop-map"
            />
          </div>
        </div>

        <div className={style.floorControls}>
          {[...buildingFloors].reverse().map((floor) => (
            <button
              key={floor.floorId}
              onClick={() => setCurrentFloorId(floor.floorId)}
              className={`${style.floorButton} ${
                currentFloorId === floor.floorId ? style.active : ''
              }`}
              title={floor.name}
            >
              {floor.floorId || floor.name.charAt(0)}
            </button>
          ))}
        </div>
      </div>
    </div>
  );
};

export default MapPage;