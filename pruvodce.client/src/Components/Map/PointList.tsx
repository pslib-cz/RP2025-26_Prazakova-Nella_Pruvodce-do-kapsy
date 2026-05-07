import { Icon } from '@iconify/react';
import type { Point } from '../../Types/MapType';

import style from '../../Styles/MapPage.module.css';

import { getPointIconName } from '../../pointIconUtils';

interface PointListProps {
  points: Point[];
}

const fieldClassMap: Record<string, string> = {
  IT: 'fieldIt',
  EL: 'fieldEl',
  ST: 'fieldSt',
  TL: 'fieldTl',
  OD: 'fieldOd',
  TE: 'fieldTe',
};

const iconMap: Record<string, string> = {
  Talk: 'lucide:presentation',
  Hand: 'lucide:messages-square',
  Ucebna: 'lucide:door-open',
  Jine: 'lucide:ellipsis',
};

function getPointTitle(point: Point): string {
  const rawPoint = point as Point & {
    label?: string;
    name?: string;
    title?: string;
  };

  return rawPoint.label ?? rawPoint.name ?? rawPoint.title ?? 'Stanoviště';
}

function getPointSubtitle(point: Point): string {
  const rawPoint = point as Point & {
    description?: string;
    fieldName?: string;
    specialization?: {
      name?: string;
    };
  };

  return (
    rawPoint.description ??
    rawPoint.fieldName ??
    rawPoint.specialization?.name ??
    'Bez popisu'
  );
}

function getPointRoom(point: Point): string {
  const rawPoint = point as Point & {
    roomCode?: string;
    roomLabel?: string;
    roomId?: string;
  };

  return rawPoint.roomCode ?? rawPoint.roomLabel ?? rawPoint.roomId ?? '';
}

function getPointFieldType(point: Point): string {
  const rawPoint = point as Point & {
    fieldType?: string | number | null;
    field?: string | number | null;
    specialization?: {
      type?: string | number | null;
      Type?: string | number | null;
      fieldType?: string | number | null;
    } | null;
  };

  const value =
    rawPoint.fieldType ??
    rawPoint.field ??
    rawPoint.specialization?.type ??
    rawPoint.specialization?.Type ??
    rawPoint.specialization?.fieldType ??
    'default';

  if (typeof value === 'number') {
    const enumMap: Record<number, string> = {
      0: 'IT',
      1: 'EL',
      2: 'ST',
      3: 'TL',
      4: 'OD',
      5: 'TE',
    };

    return enumMap[value] ?? 'default';
  }

  return value;
}

function getPointIconType(point: Point): string {
  const rawPoint = point as Point & {
    iconType?: string;
    pointIcon?: string;
    icon?: string;
  };

  return rawPoint.iconType ?? rawPoint.pointIcon ?? rawPoint.icon ?? 'Jine';
}

const PointList: React.FC<PointListProps> = ({ points }) => {
  if (points.length === 0) {
    return (
      <div className={style.emptyState}>
        Žádná stanoviště neodpovídají filtru.
      </div>
    );
  }

  return (
    <div className={style.PointList}>
      {points.map(point => {
        const fieldType = getPointFieldType(point).toUpperCase();
        const fieldClass = fieldClassMap[fieldType] ?? 'fieldDefault';

        const icon = getPointIconName(point.icon);

        return (
          <button
            key={point.pointId}
            className={style.PointCard}
            type="button"
          >
            <span className={`${style.PointIcon} ${style[fieldClass]}`}>
              <Icon icon={icon} width="20" height="20" />
            </span>

            <span className={style.PointInfo}>
              <strong>{getPointTitle(point)}</strong>
              <small>{getPointSubtitle(point)}</small>
            </span>

            <span className={style.PointRoom}>
              {getPointRoom(point)}
            </span>
          </button>
        );
      })}
    </div>
  );
};

export default PointList;