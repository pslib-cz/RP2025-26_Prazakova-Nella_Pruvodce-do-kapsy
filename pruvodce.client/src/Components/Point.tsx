import { Icon } from '@iconify/react';
import type { Point } from '../Types/MapType';
import style from '../Styles/Point.module.css';

import { getPointIconFromPoint, getPointMapColorClass } from '../pointIconUtils';

interface InteractivePointProps {
  point: Point;
  x: number;
  y: number;
  zoomLevel: number;
  onClick: (point: Point) => void;
  isOtherFloor?: boolean;
}

function getPointScale(zoomLevel: number): number {
  if (zoomLevel <= 1) return 1;

  const scale = 1 / Math.sqrt(zoomLevel);
  return Math.max(0.45, Math.min(1, scale));
}

const InteractivePoint: React.FC<InteractivePointProps> = ({
  point,
  x,
  y,
  zoomLevel,
  onClick,
  isOtherFloor = false,
}) => {
  const typeClass = getPointMapColorClass(point, style);
  const iconName = getPointIconFromPoint(point);
  const pointScale = getPointScale(zoomLevel);

  return (
    <g
      className={`${style.point} ${typeClass} ${isOtherFloor ? style.otherFloor : ''}`}
      transform={`translate(${x} ${y}) scale(${pointScale})`}
      onClick={event => {
        event.stopPropagation();
        onClick(point);
      }}
    >
      <g className={style.pinInner}>
        <path
          className={style.pinBody}
          d="M 0 0 C -10 -13 -15 -22 -15 -31 C -15 -39 -10 -46 0 -46 C 10 -46 15 -39 15 -31 C 15 -22 10 -13 0 0 Z"
        />

        <circle className={style.pinCircle} cx="0" cy="-31" r="11" />

        <Icon
          icon={iconName}
          className={style.pinIcon}
          x={-7}
          y={-38}
          width={14}
          height={14}
        />
      </g>
    </g>
  );
};

export default InteractivePoint;