import { Icon } from '@iconify/react';
import type { Point } from '../Types/MapType';
import style from '../Styles/Point.module.css';
import { getPointIconName } from '../pointIconUtils';

interface InteractivePointProps {
  point: Point;
  x: number;
  y: number;
  zoomLevel: number;
  onClick: (point: Point) => void;
}

type FieldTypeKey = 'IT' | 'EL' | 'ST' | 'TL' | 'OD' | 'TE';
type PointIconKey = 'Talk' | 'Hand' | 'Ucebna' | 'Jine';

const fieldTypeByNumber: Record<number, FieldTypeKey> = {
  0: 'IT',
  1: 'EL',
  2: 'ST',
  3: 'TL',
  4: 'OD',
  5: 'TE',
};

const pointIconByNumber: Record<number, PointIconKey> = {
  0: 'Talk',
  1: 'Hand',
  2: 'Ucebna',
  3: 'Jine',
};

const iconByPointType: Record<PointIconKey, string> = {
  Talk: 'lucide:presentation',
  Hand: 'lucide:hand',
  Ucebna: 'lucide:door-open',
  Jine: 'lucide:map-pin',
};

function getFieldTypeKey(type: unknown): FieldTypeKey | null {
  if (typeof type === 'number') {
    return fieldTypeByNumber[type] ?? null;
  }

  if (typeof type === 'string') {
    const normalized = type.trim().toUpperCase();

    const asNumber = Number(normalized);
    if (!Number.isNaN(asNumber) && asNumber in fieldTypeByNumber) {
      return fieldTypeByNumber[asNumber];
    }

    if (['IT', 'EL', 'ST', 'TL', 'OD', 'TE'].includes(normalized)) {
      return normalized as FieldTypeKey;
    }
  }

  return null;
}

function getPointIconKey(icon: unknown): PointIconKey {
  if (typeof icon === 'number') {
    return pointIconByNumber[icon] ?? 'Jine';
  }

  if (typeof icon === 'string') {
    const normalized = icon.trim();

    const asNumber = Number(normalized);
    if (!Number.isNaN(asNumber) && asNumber in pointIconByNumber) {
      return pointIconByNumber[asNumber];
    }

    if (normalized === 'Talk') return 'Talk';
    if (normalized === 'Hand') return 'Hand';
    if (normalized === 'Ucebna') return 'Ucebna';
    if (normalized === 'Jine') return 'Jine';
  }

  return 'Jine';
}

function getPointTypeClass(point: Point): string {
  const typeKey = getFieldTypeKey(point.specialization?.type);

  switch (typeKey) {
    case 'IT':
      return style.typeIT;
    case 'EL':
      return style.typeEL;
    case 'ST':
      return style.typeST;
    case 'TL':
      return style.typeTL;
    case 'OD':
      return style.typeOD;
    case 'TE':
      return style.typeTE;
    default:
      return style.typeDefault;
  }
}

function getPointIcon(point: Point): string {
  const iconKey = getPointIconKey(point.icon);
  return iconByPointType[iconKey];
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
}) => {
  const typeClass = getPointTypeClass(point);
  const iconName = getPointIconName(point.icon);
  const pointScale = getPointScale(zoomLevel);

  return (
    <g
      className={`${style.point} ${typeClass}`}
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