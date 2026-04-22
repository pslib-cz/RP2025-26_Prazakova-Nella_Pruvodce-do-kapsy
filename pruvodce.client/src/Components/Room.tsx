import React from 'react';

interface Coords {
  x: number;
  y: number;
}

interface RoomProps {
  id: string;
  pathData: string;
  label: string;
  labelCoords: Coords;
  isVisible: boolean;
  onClick: (id: string) => void;
}

const Room: React.FC<RoomProps> = ({ 
  id, 
  pathData, 
  label, 
  labelCoords, 
  isVisible, 
  onClick 
}) => {
  return (
    <g 
      id={id} 
      onClick={() => onClick(id)} 
      style={{ cursor: 'pointer', pointerEvents: 'all' }}
    >
      <path
        d={pathData}
        fill="transparent"
        stroke="black"
        strokeWidth="1"
        className="room-path"
        style={{ vectorEffect: 'non-scaling-stroke' }}
      />
      
      {isVisible && (
        <text
          x={labelCoords.x}
          y={labelCoords.y}
          fill="red"
          fontSize="16"
          fontWeight="bold"
          textAnchor="middle"
          style={{ userSelect: 'none', pointerEvents: 'none' }}
        >
          {label}
        </text>
      )}
    </g>
  );
};

export default Room;