import style from '../../Styles/MapPage.module.css';

interface FloorControlsProps {
  floors: any[];
  currentFloorId: number;
  onFloorChange: (floorId: number) => void;
  variant?: 'floating' | 'panel';
  reverse?: boolean;
}

const FloorControls: React.FC<FloorControlsProps> = ({
  floors,
  currentFloorId,
  onFloorChange,
  variant = 'floating',
  reverse = false,
}) => {
  const orderedFloors = reverse ? [...floors].reverse() : floors;

  return (
    <div className={variant === 'panel' ? style.panelFloorControls : style.floatingFloorControls}>
      {orderedFloors.map(floor => (
        <button
          key={floor.floorId}
          onClick={() => onFloorChange(floor.floorId)}
          className={`${style.floorButton} ${currentFloorId === floor.floorId ? style.floorButtonActive : ''}`}
          title={floor.name}
        >
          {floor.floorNumber ?? floor.floorId}
        </button>
      ))}
    </div>
  );
};

export default FloorControls;