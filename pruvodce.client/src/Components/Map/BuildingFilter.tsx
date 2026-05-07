import { Icon } from '@iconify/react';
import style from '../../Styles/MapPage.module.css';

interface BuildingSelectProps {
  buildings: any[];
  currentBuilding: any;
  isOpen: boolean;
  onToggle: () => void;
  onChange: (buildingId: number) => void;
}

const BuildingSelect: React.FC<BuildingSelectProps> = ({
  buildings,
  currentBuilding,
  isOpen,
  onToggle,
  onChange,
}) => {
  return (
    <div className={style.buildingSelect}>
      <button className={style.buildingSelectButton} onClick={onToggle}>
        <Icon icon="lucide:map-pin" width="22" height="22" />

        <span className={style.buildingSelectText}>
          <strong>{currentBuilding.name}</strong>
          <span>{currentBuilding.address || 'Bez adresy'}</span>
        </span>

        <Icon icon="lucide:chevron-down" width="18" height="18" />
      </button>

      {isOpen && (
        <div className={style.buildingDropdown}>
          {buildings.map(building => (
            <button
              key={building.buildingId}
              className={style.buildingDropdownItem}
              onClick={() => onChange(building.buildingId)}
            >
              <strong>{building.name}</strong>
              <span>{building.address || 'Bez adresy'}</span>
            </button>
          ))}
        </div>
      )}
    </div>
  );
};

export default BuildingSelect;