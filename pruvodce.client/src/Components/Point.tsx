import { Icon } from '@iconify/react';
import type { Point } from '../Types/MapType';
import style from '../Styles/Point.module.css';

interface InteractivePointProps {
  point: Point;
  x: number;
  y: number;
  zoomLevel: number;
  onClick: (point: Point) => void;
}

type FieldTypeKey = 'IT' | 'EL' | 'ST' | 'TL' | 'OD' | 'TE';

const fieldTypeByNumber: Record<number, FieldTypeKey> = {
  0: 'IT',
  1: 'EL',
  2: 'ST',
  3: 'TL',
  4: 'OD',
  5: 'TE'
};

const iconByName: Record<string, string> = {
  Computer: 'lucide:monitor',
  Code: 'lucide:code-2',
  Network: 'lucide:network',
  Electricity: 'lucide:zap',
  Machine: 'lucide:cog',
  Design: 'lucide:palette',
  Health: 'lucide:heart-pulse',
  Business: 'lucide:briefcase-business'
};

const iconByNumber: Record<number, string> = {
  0: 'lucide:monitor',
  1: 'lucide:code-2',
  2: 'lucide:network',
  3: 'lucide:zap',
  4: 'lucide:cog',
  5: 'lucide:palette',
  6: 'lucide:heart-pulse',
  7: 'lucide:briefcase-business'
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
  const specializationIcon = point.specialization?.icon;
  const pointIcon = point.icon;

  if (typeof specializationIcon === 'number') {
    return iconByNumber[specializationIcon] ?? 'lucide:map-pin';
  }

  if (typeof specializationIcon === 'string') {
    return iconByName[specializationIcon] ?? specializationIcon;
  }

  if (typeof pointIcon === 'string' && pointIcon.trim()) {
    return pointIcon;
  }

  return 'lucide:map-pin';
}

function getPointScale(zoomLevel: number): number {
  /*
    Při oddálení necháme pin větší.
    Při přiblížení ho zmenšíme, aby nezakrýval místnost.
  */
  if (zoomLevel <= 1) return 1;

  const scale = 1 / Math.sqrt(zoomLevel);
  return Math.max(0.45, Math.min(1, scale));
}

const InteractivePoint: React.FC<InteractivePointProps> = ({
  point,
  x,
  y,
  zoomLevel,
  onClick
}) => {
  const typeClass = getPointTypeClass(point);
  const iconName = getPointIcon(point);
  const pointScale = getPointScale(zoomLevel);

  return (
    <g
      className={`${style.point} ${typeClass}`}
      transform={`translate(${x} ${y}) scale(${pointScale})`}
      onClick={(event) => {
        event.stopPropagation();
        onClick(point);
      }}
    >
      <g className={style.pinInner}>
        <path
          className={style.pinBody}
          d="M 0 0 C -10 -13 -15 -22 -15 -31 C -15 -39 -10 -46 0 -46 C 10 -46 15 -39 15 -31 C 15 -22 10 -13 0 0 Z"
        />

        <circle className={style.pinCircle} cx="0" cy="-31" r="11"/>

        <Icon icon={iconName} className={style.pinIcon}
          x={-7} y={-38}
          width={14} height={14} />
      </g>
    </g>
  );
};

export default InteractivePoint;