import type { Point } from '../Types/MapType';

interface InteractivePointProps {
  point: Point;
  x: number;
  y: number;
  zoomLevel: number;
  onClick: (point: Point) => void;
}

const InteractivePoint: React.FC<InteractivePointProps> = ({
  point,
  x,
  y,
  zoomLevel,
  onClick
}) => {
  const radius = Math.max(5 / zoomLevel, 2.5);

  return (
    <g
      onClick={() => onClick(point)}
      style={{ cursor: 'pointer', pointerEvents: 'all' }}
    >
      <circle
        cx={x}
        cy={y}
        r={radius}
        fill="#2f6fed"
        stroke="white"
        strokeWidth={1 / zoomLevel}
      />

      {zoomLevel > 1.4 && (
        <text
          x={x}
          y={y - radius - 2 / zoomLevel}
          textAnchor="middle"
          style={{
            fontSize: `${10 / zoomLevel}px`,
            fill: '#1f2933',
            fontWeight: 700,
            pointerEvents: 'none',
            userSelect: 'none'
          }}
        >
          {point.label}
        </text>
      )}
    </g>
  );
};

export default InteractivePoint;