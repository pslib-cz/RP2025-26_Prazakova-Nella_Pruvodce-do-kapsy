import { useParams, useNavigate, Navigate } from 'react-router-dom';
import { useState, useMemo } from 'react';
import { BUILDINGS } from '../Components/mapData';
import InteractiveMap from '../Components/InteractiveMap';
import style from "../Styles/MapPage.module.css";
import { Icon } from '@iconify/react';

const MapPage: React.FC = () => {
  const { buildingId } = useParams<{ buildingId: string }>();
  const navigate = useNavigate();

  const currentBuilding = useMemo(() => 
    BUILDINGS.find(b => b.buildingId === Number(buildingId)), 
    [buildingId]
  );

  const [isDropdownOpen, setIsDropdownOpen] = useState(false);

  const buildingFloors = currentBuilding?.floors || [];

  const [currentFloorId, setCurrentFloorId] = useState<number>(
    buildingFloors[0]?.floorId
  );

  if (!currentBuilding || buildingFloors.length === 0) {
    return <Navigate to="/" />;
  }

  return (
    <div className={style.mapUIContainer}>
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
            <button 
              className={style.buildingListItem}
              onClick={() => {
                localStorage.removeItem('preferredBuilding');
                setIsDropdownOpen(false);
                navigate('/map/1'); 
              }}
            >
              <span className={style.bName}>Masarykova</span>
              <span className={style.bAddress}>Masarykova 460/3</span>
            </button>
            
            <button 
              className={style.buildingListItem}
              onClick={() => {
                localStorage.removeItem('preferredBuilding');
                setIsDropdownOpen(false);
                navigate('/map/2');
              }}
            >
              <span className={style.bName}>Tyršova</span>
              <span className={style.bAddress}>Tyršova 82/1</span>
            </button>
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

      <div className={style.floorControls}>
        {[...buildingFloors].reverse().map((floor) => (
          <button 
            key={floor.floorId}
            onClick={() => setCurrentFloorId(floor.floorId)}
            className={`${style.floorButton} ${currentFloorId === floor.floorId ? style.active : ''}`}
          >
            {floor.name.charAt(0)}.
          </button>
        ))}
      </div>

      <InteractiveMap 
        floors={buildingFloors} 
        activeFloorId={currentFloorId} 
        buildingId={Number(buildingId)}
      />
    </div>
  );
};

export default MapPage;